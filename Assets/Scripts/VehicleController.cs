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
    public float steerForce { get; set; }
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
    private int SegIdx;
    protected WheelController WCFL;
    protected WheelController WCFR;
    protected WheelController WCRL;
    protected WheelController WCRR;
    protected ParticleSystem psSprayL;
    protected ParticleSystem psSprayR;
    protected ParticleSystem psDustL;
    protected ParticleSystem psDustR;

    WheelController.WheelHit hitFL = new WheelController.WheelHit();
    WheelController.WheelHit hitFR = new WheelController.WheelHit();
    WheelController.WheelHit hitRL = new WheelController.WheelHit();
    WheelController.WheelHit hitRR = new WheelController.WheelHit();
    private bool WasInAir = false;
    private bool IsRuttingFL = false;
    public bool WasRuttingFL { get; set; }
    private bool IsRuttingFR = false;
    public bool WasRuttingFR { get; set; }
    private bool IsSkiddingFL = false;
    public bool WasSkiddingFL { get; set; }
    private bool IsSkiddingFR = false;
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
    List<InputStruct> Inputs = new List<InputStruct>();

    public virtual void Init() { } //this is used by the car player controller to add the input manager and the speedo

    void Awake()
    {
        goCar = this.transform.Find("car").gameObject;
        SkidAudioSource = GetComponent<AudioSource>();
        Transform Eng = transform.Find("Engine");
        EngineAudioSource = transform.Find("Engine").GetComponent<AudioSource>();
        CoughAudioSource = Eng.Find("Cough").GetComponent<AudioSource>();
        ClutchAudioSource = Eng.Find("ClutchDown").GetComponent<AudioSource>();
        IdleAudioSource = Eng.Find("Idle").GetComponent<AudioSource>();
        EngineAudioSource.mute = true;
        CoughAudioSource.mute = true;
        IdleAudioSource.mute = true;
        FLWheel = transform.Find("car/FLWheel").gameObject;
        FRWheel = transform.Find("car/FRWheel").gameObject;
        WasRuttingFL = false;
        WasRuttingFR = false;
        RutMatrl = (Material)Resources.Load("Prefabs/Materials/WheelRutGrey");
        SkidMatrl = (Material)Resources.Load("Prefabs/Materials/SkidMark");

        try
        {
            _fLRimRenderer = transform.Find("car/FLWheel/FLRim").GetComponent<Renderer>();
            _fRRimRenderer = transform.Find("car/FRWheel/FRRim").GetComponent<Renderer>();
            _rLRimRenderer = transform.Find("car/RLWheel/RLRim").GetComponent<Renderer>();
            _rRRimRenderer = transform.Find("car/RRWheel/RRRim").GetComponent<Renderer>();
            _fRRimSpinRenderer = transform.Find("car/FRWheel/FRRimSpin").GetComponent<Renderer>();
            _rLRimSpinRenderer = transform.Find("car/RLWheel/RLRimSpin").GetComponent<Renderer>();
            _fLRimSpinRenderer = transform.Find("car/FLWheel/FLRimSpin").GetComponent<Renderer>();
            _rRRimSpinRenderer = transform.Find("car/RLWheel/RRRimSpin").GetComponent<Renderer>();
        }
        catch { _rimSpin = false; }


        StickyCarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/StickyCarBodyPhysicsMaterial");
        CarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/CarBodyPhysicsMaterial");

        RLWheel = transform.Find("car/RLWheel").gameObject;
        RRWheel = transform.Find("car/RRWheel").gameObject;
        WCFL = transform.Find("WheelColliders/WCFL").GetComponent<WheelController>();
        WCFR = transform.Find("WheelColliders/WCFR").GetComponent<WheelController>();
        WCRL = transform.Find("WheelColliders/WCRL").GetComponent<WheelController>();
        WCRR = transform.Find("WheelColliders/WCRR").GetComponent<WheelController>();
        _rearslipCoeff = WCRR.sideFriction.forceCoefficient;
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass = new Vector3(0, 0.65f, 0.4f);

        SkidThresh = 0.6f;
        motorForce = 1800f;
        _maxBrakeForce = 1400f;
        steerForce = 25f;
        AntiRollForce = 160f;
        Keyframe[] kfs;
        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 2.3f, 2.3f),
            new Keyframe(0.3f, 1.4f, 0f, 0f),
            new Keyframe(0.4f, 0.9f, 0f, 0f),
            new Keyframe(1f, 1.2f, 0f, 0f)
        };
        frontFrictTarmac = new AnimationCurve(kfs);
        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 2.3f, 2.3f),
            new Keyframe(0.2f, 1.2f, 0f, 0f),
            new Keyframe(0.3f, 0.7f, 0f, 0f),
            new Keyframe(2f, 0.7f, 0f, 0f)
};
        rearFrictTarmac = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 2.0f, 2.0f),
            new Keyframe(0.2f, 0.7f, 0f, 0f),
            new Keyframe(0.3f, 0.5f, 0f, 0f),
            new Keyframe(1f, 0.6f, 0f, 0f)
        };
        frontFrictDirtyRoad = new AnimationCurve(kfs);
        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 2.0f, 2.0f),
            new Keyframe(0.15f, 0.7f, 0f, 0f),
            new Keyframe(0.2f, 0.5f, 0f, 0f),
            new Keyframe(1f, 0.5f, 0f, 0f)
};
        rearFrictDirtyRoad = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 2.0f, 2.0f),
            new Keyframe(0.2f, 0.7f, 0f, 0f),
            new Keyframe(0.3f, 0.3f, 0f, 0f),
            new Keyframe(1f, 1.1f, 0f, 0f)

        };
        frontFrictDirt = new AnimationCurve(kfs);
        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 2.1f, 2.1f),
            new Keyframe(0.15f, 0.5f, 0f, 0f),
            new Keyframe(0.2f, 0.3f, 0f, 0f),
            new Keyframe(1f, 0.4f, 0f, 0f)

        };
        rearFrictDirt = new AnimationCurve(kfs);



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


    void Update()
    {

        if (_gpsTimer == 0)
        {
            try { Gps.UpdateSegIdx(); }
            catch { }
            _gpsTimer = 2;
        }
        _gpsTimer--;

        //Engine Sound

        if (_prevEngineTorque > 0 && WCFL.motorTorque == 0)
        {
            EngineAudioSource.Stop();
            ClutchAudioSource.Play();
            ClutchStartTime = Time.time;
        }
        if (_prevEngineTorque == 0 && WCFL.motorTorque == 0 && IdleAudioSource.isPlaying == false && Time.time > ClutchStartTime + 1f && EngineAudioSource.isPlaying == false)
        {
            EngineAudioSource.pitch = 0.2f;
            EngineAudioSource.Play();
        }
        if (WCFL.motorTorque > 0)
        {
            ClutchAudioSource.Stop();
            //IdleAudioSource.Stop();
            float _pitch;
            float rpm;
            //int Gear;
            float Wheelrpm = Mathf.Abs((WCFL.rpm < WCFR.rpm) ? WCFL.rpm : WCFR.rpm);
            if (Wheelrpm > 1499) Wheelrpm = 1499;
            if (EngineAudioSource.isPlaying == false) EngineAudioSource.Play();
            if (Wheelrpm < 300)
            {
                rpm = -Wheelrpm;
                _pitch = rpm / 600 - 0.5f;
                //Gear = 1;
            }
            else
            {
                rpm = -(Wheelrpm - 400) % 400;
                //Gear = (int)Wheelrpm / 400 + 1;
                _pitch = (rpm / 600 - 0.5f);

            }
            EngineAudioSource.pitch = _pitch * 1.5f;
        }
        CoughAudioSource.mute = (WCFL.motorTorque != 0);
        _prevEngineTorque = WCFL.motorTorque;

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






