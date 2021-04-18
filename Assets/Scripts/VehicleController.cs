using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class VehicleController : MonoBehaviour, iVehicleController
{
    public GPS Gps { get; set; }
    protected byte _gpsTimer = 0;
    public iInputManager InputManager { get; set; }
    protected GameObject goCar;
    protected Rigidbody _rb;
    protected GameObject FLWheel;
    protected GameObject FRWheel;
    protected GameObject RLWheel;
    protected GameObject RRWheel;
    protected Renderer _fLRimRenderer;
    protected Renderer _fLRimSpinRenderer;
    protected Renderer _fRRimRenderer;
    protected Renderer _fRRimSpinRenderer;
    protected Renderer _rLRimRenderer;
    protected Renderer _rLRimSpinRenderer;
    protected Renderer _rRRimRenderer;
    protected Renderer _rRRimSpinRenderer;
    bool _rimSpin = true;
    Transform Skidmarks;
    public float SkidThresh { get; set; }
    public float motorForce { get; set; }
    protected float steerForce { get; set; }
    public float AntiRollForce { get; set; }
    protected string RoadMat = "";
    protected AnimationCurve frontFrict;
    protected AnimationCurve rearFrict;
    protected AnimationCurve frontFrictTarmac;
    protected AnimationCurve rearFrictTarmac;
    protected AnimationCurve frontFrictDirtyRoad;
    protected AnimationCurve rearFrictDirtyRoad;
    protected AnimationCurve frontFrictDirt;
    protected AnimationCurve rearFrictDirt;
    protected WheelController WCFL;
    protected WheelController WCFR;
    protected WheelController WCRL;
    protected WheelController WCRR;
    protected ParticleSystem psSprayL;
    protected ParticleSystem psSprayR;
    protected ParticleSystem psDustL;
    protected ParticleSystem psDustR;

    protected WheelController.WheelHit hitFL = new WheelController.WheelHit();
    protected WheelController.WheelHit hitFR = new WheelController.WheelHit();
    protected WheelController.WheelHit hitRL = new WheelController.WheelHit();
    protected WheelController.WheelHit hitRR = new WheelController.WheelHit();
    public bool WasRuttingFL { get; set; }
    public bool WasRuttingFR { get; set; }
    public bool WasSkiddingFL { get; set; }
    public bool WasSkiddingFR { get; set; }
    private AudioSource SkidAudioSource;
    protected AudioSource EngineAudioSource;
    protected AudioSource CoughAudioSource;
    private AudioSource ClutchAudioSource;
    protected AudioSource IdleAudioSource;
    float ClutchStartTime = 0;
    private float _prevEngineTorque;
    Material RutMatrl;
    Material SkidMatrl;
    public bool EndSkidmarks { get; set; }
    PhysicMaterial StickyCarBodyPhysicsMaterial;
    PhysicMaterial CarBodyPhysicsMaterial;
    float _rearslipCoeff;
    public float SlipEffect = 3f;
    protected float v;
    protected float h;
    protected float _maxBrakeForce;
    private float BrakeForce;
    protected Text txtTrace;
    protected Text txtTrace2;
    public string TestRoadMat { get; set; }

    public virtual void Init() { } //this is used by the car player controller to add the input manager and the speedo

    public virtual void Awake()
    {
        goCar = this.transform.Find("car").gameObject;

        StickyCarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/StickyCarBodyPhysicsMaterial");
        CarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/CarBodyPhysicsMaterial");


    }
    void Start()
    {
        txtTrace = GameObject.Find("txtTrace").GetComponent<Text>();
        txtTrace2 = GameObject.Find("txtTrace2").GetComponent<Text>();
    }

    public virtual void GetInputFromInputManager()
    {
        if (InputManager == null) return;
        //Accel and Brake
        v = InputManager.ZMovement() * motorForce;
        BrakeForce = InputManager.BrakeForce * _maxBrakeForce;
        //STEERING
        h = InputManager.XMovement();
        h = Mathf.Clamp(h, -40, 40);
    }

    public void StartEngine()
    {
        EngineAudioSource.mute = false;
        IdleAudioSource.mute = false;
        CoughAudioSource.mute = false;
    }

    protected virtual void OnDestroy()
    {
        Gps = null;
        goCar = null;
    }


}
