using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class VehicleController : MonoBehaviour, iVehicleController
{
    public GPS Gps { get; set; }
    private byte _gpsTimer = 0;
    public iInputManager InputManager { get; set; }
    protected GameObject goCar;
    protected Rigidbody _rb;
    protected GameObject FLWheel;
    protected GameObject FRWheel;
    protected GameObject RLWheel;
    protected GameObject RRWheel;
    Renderer _fLRimRenderer;
    Renderer _fLRimSpinRenderer;
    Renderer _fRRimRenderer;
    Renderer _fRRimSpinRenderer;
    Renderer _rLRimRenderer;
    Renderer _rLRimSpinRenderer;
    Renderer _rRRimRenderer;
    Renderer _rRRimSpinRenderer;
    bool _rimSpin = true;
    UnityEngine.Object SkidPrefab;
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
    private int RutLeftNodeCount = 0;
    private int RutRightNodeCount = 0;
    private GameObject goRutLeft;
    private FlatLineRenderer RutLeft;
    private GameObject goRutRight;
    private FlatLineRenderer RutRight;
    private int SkidMkLeftNodeCount = 0;
    private int SkidMkRightNodeCount = 0;
    private GameObject goSkidMkLeft;
    private FlatLineRenderer SkidMkLeft;
    private GameObject goSkidMkRight;
    private FlatLineRenderer SkidMkRight;
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
    bool GravelSimulation = false;
    bool WashboardSimulation = false;
    private Queue<float> LSlips = new Queue<float>();
    private Queue<float> RSlips = new Queue<float>();
    private float LSlipsum = 0;
    private float RSlipsum = 0;
    private Queue<float> SlipQueue = new Queue<float>();
    private float DelayedSlip = 0f;
    private int SlipDelayTimer = 50;
    protected float v;
    private float h;
    private float _prevh;
    private float _maxBrakeForce;
    private float BrakeForce;
    Text txtTrace;
    Text txtTrace2;
    public string TestRoadMat { get; set; }
    List<InputStruct> Inputs = new List<InputStruct>();

    public virtual void Init() { } //this is used by the car player controller to add the input manager and the speedo

    void Awake()
    {
        goCar = this.transform.Find("car").gameObject;
        SkidPrefab = Resources.Load("Prefabs/SkidmarkPrefab");
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


        //Used for testing
        /*
        WCFL.forwardFriction.frictionCurve = frontFrictDirtyRoad;
        WCFR.forwardFriction.frictionCurve = frontFrictDirtyRoad;
        WCRL.forwardFriction.frictionCurve = rearFrictDirtyRoad;
        WCRR.forwardFriction.frictionCurve = rearFrictDirtyRoad;
        WCFL.fFriction.slipCoefficient = 1.1f;
        WCFR.fFriction.slipCoefficient = 1.1f;
        */
    }
    void Start()
    {
        txtTrace = GameObject.Find("txtTrace").GetComponent<Text>();
        txtTrace2 = GameObject.Find("txtTrace2").GetComponent<Text>();
    }
    // Update is called once per frame
    //FixedUpdate is called for every physics calculation
    void FixedUpdate()
    {

        GetInputFromInputManager();

        //The rest of this method controls the car
        WCFL.motorTorque = v;
        WCFR.motorTorque = v;

        UnityEngine.Profiling.Profiler.BeginSample("RimSpin");
        if (_rimSpin)
        {
            if (Mathf.Abs(WCFL.rpm) > 300)
            {
                _fLRimSpinRenderer.enabled = true;
                _fLRimRenderer.enabled = false;
            }
            else
            {
                _fLRimRenderer.enabled = true;
                _fLRimSpinRenderer.enabled = false;
            }
            if (Mathf.Abs(WCRL.rpm) > 300)
            {
                _rLRimSpinRenderer.enabled = true;
                _rLRimRenderer.enabled = false;
            }
            else
            {
                _rLRimRenderer.enabled = true;
                _rLRimSpinRenderer.enabled = false;
            }
            if (Mathf.Abs(WCFR.rpm) > 300)
            {
                _fRRimRenderer.enabled = false;
                _fRRimSpinRenderer.enabled = true;
            }
            else
            {
                _fRRimRenderer.enabled = true;
                _fRRimSpinRenderer.enabled = false;
            }
            if (Mathf.Abs(WCRR.rpm) > 300)
            {
                _rRRimRenderer.enabled = false;
                _rRRimSpinRenderer.enabled = true;
            }
            else
            {
                _rRRimRenderer.enabled = true;
                _rRRimSpinRenderer.enabled = false;
            }
        }
        UnityEngine.Profiling.Profiler.EndSample();



        //Adapt the tyre slip according to the road type

        if (Gps != null)
        {
            RoadMat = Gps.RoadMat;
            if (TestRoadMat != null) RoadMat = TestRoadMat;
            switch (RoadMat)
            {
                case "Tarmac":
                    frontFrict = frontFrictTarmac;
                    rearFrict = rearFrictTarmac;
                    GravelSimulation = false;
                    WashboardSimulation = false;
                    break;
                case "Washboard":
                    frontFrict = frontFrictTarmac;
                    rearFrict = rearFrictTarmac;
                    GravelSimulation = false;
                    WashboardSimulation = true;
                    break;
                case "DirtyRoad":
                    frontFrict = frontFrictDirtyRoad;
                    rearFrict = rearFrictDirtyRoad;
                    GravelSimulation = false;
                    WashboardSimulation = false;
                    break;
                default:
                    frontFrict = frontFrictDirt;
                    rearFrict = rearFrictDirt;
                    GravelSimulation = true;
                    WashboardSimulation = false;
                    break;
            }


            //For pendulum turns - if the angular velocity is big, reduce the rear side slip coeff:
            //PendulumSlipCoeff should be between 1 and 0.7
            //Vector3 _desiredVector = Road.Instance.XSecs[Gps.SegIdx + 20].MidPt - Road.Instance.XSecs[Gps.SegIdx + 10].MidPt;
            float PendulumForceCoeff;
            float TotSlip = 0;
            float Divisor = 0;
            if (WCFL.isGrounded)
            {
                float s = Mathf.Abs(WCRL.sideFriction.slip);
                LSlips.Enqueue(s);
                LSlipsum += s;
                if (LSlips.Count > 50)
                {
                    LSlipsum -= LSlips.Dequeue();
                    Divisor = 50;
                }
                TotSlip = LSlipsum;
            }
            if (WCFR.isGrounded)
            {
                float rs = Mathf.Abs(WCRR.sideFriction.slip);
                RSlips.Enqueue(rs);
                RSlipsum += rs;
                if (RSlips.Count > 50)
                {
                    RSlipsum -= RSlips.Dequeue();
                    Divisor += 50;
                }
                TotSlip += RSlipsum;
            }
            float MovingAvgSlip;
            if (Divisor == 0) MovingAvgSlip = 0.7f;
            else MovingAvgSlip = TotSlip / Divisor;
            //MovingAvgSlip stays around 0.01-0.02 and goes up to 0.2 when it breaks away. For a severe skid goes up to 0.7
            //Each Fixedframe it's changing by 0.002 to 0.009
            //So we can force it to increase fast and drop back slowly
            SlipQueue.Enqueue(MovingAvgSlip);
            if (SlipQueue.Count > 50)
            {
                DelayedSlip = SlipQueue.Dequeue();
            }
            else DelayedSlip = MovingAvgSlip;
            if (DelayedSlip < MovingAvgSlip) DelayedSlip = MovingAvgSlip;

            
            if (Mathf.Abs(h - _prevh) > 10)
            {
                SlipDelayTimer = 0;
            }
            if (SlipDelayTimer < 50)
            {
                DelayedSlip = 0.5f;
                SlipDelayTimer++;
            }
            _prevh = h;

            PendulumForceCoeff = Mathf.Clamp(1 - DelayedSlip * SlipEffect, 0.4f, 1f);
            WCRL.sideFriction.forceCoefficient = _rearslipCoeff * PendulumForceCoeff;  //normally forceCoeff = 5; Bigger = more turning force
            WCRR.sideFriction.forceCoefficient = _rearslipCoeff * PendulumForceCoeff;

            //Easy cornering - The GPS can manage the reverse steering
            float BendAnglePerSec = 0;
            if (Gps.CurrBend != null)
            {
                BendAnglePerSec = Gps.CurrBend.RacelineAnglePerSeg * Gps.SegsPerSec;
                float CorrectionAnglePerSec = (BendAnglePerSec - _rb.angularVelocity.y * 57.3f);
                if (Mathf.Sign(CorrectionAnglePerSec) != Mathf.Sign(BendAnglePerSec))     //only apply the correction if oversteering
                { h += CorrectionAnglePerSec; }
                h = Mathf.Clamp(h, -40, 40);
            }
            //Feather the throttle if were understeering
            if (WCFL.sideFriction.slip > frontFrict.keys[1].time || WCFR.sideFriction.slip > frontFrict.keys[1].time) { WCFL.motorTorque *= 0.7f; WCFL.motorTorque *= 0.7f; }
            //txtTrace.text = "FSlip=" + WCFL.SlipVectorMagnitude;
            //txtTrace.color = (WCFL.SlipVectorMagnitude > frontFrict.keys[1].time)? Color.red: Color.yellow;
            //txtTrace2.text = "RSlip" + WCRL.SlipVectorMagnitude;
            //txtTrace2.color = (WCRL.SlipVectorMagnitude > rearFrict.keys[1].time) ? Color.red : Color.yellow;
        }
        else
        {
            frontFrict = frontFrictDirt;
            rearFrict = rearFrictDirt;
            RoadMat = "Dirt";
            GravelSimulation = true;
            WashboardSimulation = false;
        }

        //Comment out for testing

        WCFL.forwardFriction.frictionCurve = frontFrict;
        WCFR.forwardFriction.frictionCurve = frontFrict;
        WCRL.forwardFriction.frictionCurve = rearFrict;
        WCRR.forwardFriction.frictionCurve = rearFrict;
        WCFL.GravelSimulation = GravelSimulation;
        WCFR.GravelSimulation = GravelSimulation;
        WCRL.GravelSimulation = GravelSimulation;
        WCRR.GravelSimulation = GravelSimulation;
        WCFL.WashboardSimulation = WashboardSimulation;
        WCFR.WashboardSimulation = WashboardSimulation;
        WCRL.WashboardSimulation = WashboardSimulation;
        WCRR.WashboardSimulation = WashboardSimulation;

        WCFL.steerAngle = h;
        WCFR.steerAngle = h;

        //Limit max speed in reverse
        if (WCFL.rpm < -300)
        {
            WCFL.motorTorque = Mathf.Clamp(WCFL.motorTorque, 0, 10000);
            WCFR.motorTorque = Mathf.Clamp(WCFR.motorTorque, 0, 10000);
        }


        float travelL;
        float travelR;
        float AntiRoll = AntiRollForce * GetComponent<Rigidbody>().velocity.magnitude;

        bool groundedFL = WCFL.GetGroundHit(out hitFL);
        float ForwardSlipFL = hitFL.forwardSlip;
        float SidewaysSlipFL = hitFL.sidewaysSlip;

        if (groundedFL)
        {
            travelL = (-WCFL.transform.InverseTransformPoint(hitFL.point).y - WCFL.radius) / WCFL.springLength;
            //if (hitFL.force > 150000 && FLRim != null)
            //{
            //    GameObject.Destroy(FLRim);
            //}


        }
        else
        {
            travelL = 1.0f;
        }
        float SkidL = Mathf.Abs(SidewaysSlipFL) + Mathf.Abs(ForwardSlipFL);

        if (SkidL > SkidThresh && RoadMat == "Tarmac")
        { IsSkiddingFL = true; }
        else { IsSkiddingFL = false; SkidAudioSource.Stop(); }
        bool groundedFR = WCFR.GetGroundHit(out hitFR);
        float ForwardSlipFR = hitFR.forwardSlip;
        float SidewaysSlipFR = hitFR.sidewaysSlip;
        if (groundedFR)
        {
            travelR = (-WCFR.transform.InverseTransformPoint(hitFR.point).y - WCFR.radius) / WCFR.springLength;
        }
        else
        {
            travelR = 1.0f;
        }
        float SkidR = Mathf.Abs(SidewaysSlipFR) + Mathf.Abs(ForwardSlipFR);

        if (SkidR > SkidThresh && RoadMat == "Tarmac")
            IsSkiddingFR = true;
        else IsSkiddingFR = false;

        //ContactAngle = Vector3.Angle (Vector3.up,WCFR.transform.InverseTransformPoint(hit.point))*2*Mathf.PI/360;
        //Fric.stiffness= Mathf.Sin (ContactAngle);
        var antiRollForce = (travelL - travelR) * AntiRoll;
        if (groundedFL)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WCFL.transform.up * antiRollForce, WCFR.transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(WCFL.transform.up * -antiRollForce, WCFL.transform.position);
        }
        if (groundedFR)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WCFR.transform.up * -antiRollForce, WCFL.transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(WCFR.transform.up * antiRollForce, WCFR.transform.position);
        }

        //BACK WHEEL ANTI-ROLL BAR
        //took this out dont know what it was for?
        //AntiRoll = 50f * GetComponent<Rigidbody>().velocity.magnitude;
        bool groundedRL = WCRL.GetGroundHit(out hitRL);
        float ForwardSlipRL = hitRL.forwardSlip;
        float SidewaysSlipRL = hitRL.sidewaysSlip;
        if (groundedRL)
        {
            travelL = (-WCRL.transform.InverseTransformPoint(hitRL.point).y - WCRL.radius) / WCRL.springLength;
        }
        else
        {
            travelL = 1.0f;
        }
        bool groundedRR = WCRR.GetGroundHit(out hitRR);
        float ForwardSlipRR = hitRR.forwardSlip;
        float SidewaysSlipRR = hitRR.sidewaysSlip;
        if (groundedRR)
        {
            travelR = (-WCRR.transform.InverseTransformPoint(hitRR.point).y - WCRR.radius) / WCRR.springLength;
        }
        else
        {
            travelR = 1.0f;
        }
        antiRollForce = (travelL - travelR) * AntiRoll;
        if (groundedRL)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WCRL.transform.up * antiRollForce, WCRR.transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(WCRL.transform.up * -antiRollForce, WCRL.transform.position);
        }
        if (groundedRR)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(WCRR.transform.up * -antiRollForce, WCRL.transform.position);
            GetComponent<Rigidbody>().AddForceAtPosition(WCRR.transform.up * antiRollForce, WCRR.transform.position);
        }
#region Spray and Smoke Particles Region
        //Spray dirt
        try
        {
            if (RoadMat == "Dirt")
            {
                //+ve ForwardSlip means spraying backwards  
                //+ve SteerAngle = Right
                psSprayL.transform.localRotation = Quaternion.Euler(45, WCFL.steerAngle + (ForwardSlipFL >= 0 ? 0 : 180), 0);
                if (groundedFL && ((ForwardSlipFL < 0 && h < -0.01f) || (ForwardSlipFL > 0 && h > 0.01f)) && Mathf.Abs(WCFL.rpm) > 0)
                {
                    psSprayL.Play();
                    psSprayL.startSpeed = -Mathf.Clamp((Mathf.Abs(ForwardSlipFL) + Mathf.Abs(SidewaysSlipFL)) * 14, 0, 7);
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                    psSprayL.emissionRate = (Mathf.Abs(ForwardSlipFL) + Mathf.Abs(SidewaysSlipFL)) * 100;
                }
                else
                {
                    psSprayL.Stop();
                    psSprayL.startSpeed = 0;
                }

                psSprayR.transform.localRotation = Quaternion.Euler(45, WCFR.steerAngle + (ForwardSlipFR >= 0 ? 0 : 180), 0);
                if (groundedFR && ((ForwardSlipFR < 0 && h > 0.01f) || (ForwardSlipFR > 0 && h < -0.01f)) && Mathf.Abs(WCFR.rpm) > 0)
                {
                    psSprayR.Play();
                    psSprayR.startSpeed = -Mathf.Clamp((Mathf.Abs(ForwardSlipFR) + Mathf.Abs(SidewaysSlipFR)) * 14, 0, 7);
                    psSprayR.emissionRate = (Mathf.Abs(ForwardSlipFR) + Mathf.Abs(SidewaysSlipFR)) * 100;
                }
                else
                {
                    psSprayR.Stop();
                    psSprayR.startSpeed = 0;
                }
            }
            else { psSprayR.Stop(); psSprayL.Stop(); psSprayR.startSpeed = 0; psSprayL.startSpeed = 0; }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        //Spray dust on DirtyRoad
        try
        {
            psDustL.Stop();
            psDustR.Stop();

            if (RoadMat == "DirtyRoad")
            {
                psDustL.Play();
                psDustR.Play();
                float SlipFL = Mathf.Clamp(WCFL.SlipVectorMagnitude, 0, 0.1f);
                float SlipFR = Mathf.Clamp(WCFR.SlipVectorMagnitude, 0, 0.1f);
                ParticleSystem.EmissionModule emRL = psDustL.emission;
                emRL.rate = SlipFL * 500f;
                ParticleSystem.EmissionModule emRR = psDustR.emission;
                emRR.rate = SlipFR * 500f;
                psDustL.transform.localPosition = new Vector3(0, -0.4f, -WCFL.forwardFriction.slip / 6);
                psDustR.transform.localPosition = new Vector3(0, -0.4f, -WCFR.forwardFriction.slip / 6);


            }
            else
            {
                psDustL.Stop();
                psDustR.Stop();
            }
        }
        catch { }
#endregion

#region Wheel ruts and Skid marks
        //Make Wheel ruts
        try
        {
            if ((RoadMat == "DirtyRoad" && groundedFL && (Math.Abs(ForwardSlipFL) > 1 || Math.Abs(SidewaysSlipFL) > 1 || hitFL.force > 3000))
                || (RoadMat == "Dirt" && groundedFL && (Math.Abs(ForwardSlipFL) > 0.8f || Math.Abs(SidewaysSlipFL) > 0.5f || hitFL.force > 3000))) IsRuttingFL = true;
            else IsRuttingFL = false;

            if (IsRuttingFL)
            {
                if (!WasRuttingFL)
                {
                    //create a new rut
                    if (RutLeft != null) Destroy(RutLeft);
                    if (RutLeftNodeCount < 20) { GameObject.Destroy(goRutLeft); Road.Instance.RutCount--; }
                    goRutLeft = new GameObject("RutLeft");
                    goRutLeft.isStatic = true;
                    goRutLeft.tag = "Rut";
                    Road.Instance.RutCount++;
                    RutLeft = goRutLeft.AddComponent<FlatLineRenderer>();
                    RutLeft.Init();
                    RutLeft.SetMaterial(RutMatrl);
                    RutLeft.Width = 0.233f;
                    RutLeftNodeCount = 0;
                }
                if (RutLeft != null)
                {
                    RutLeftNodeCount++;
                    RutLeft.AddNode(hitFL.point + Vector3.up * 0.05f);
                }
            }
            WasRuttingFL = IsRuttingFL;

            if ((RoadMat == "DirtyRoad" && groundedFR && (Math.Abs(ForwardSlipFR) > 1 || Math.Abs(SidewaysSlipFR) > 1 || hitFR.force > 3000))
                || (RoadMat == "Dirt" && groundedFR && (Math.Abs(ForwardSlipFR) > 0.6f || Math.Abs(SidewaysSlipFR) > 0.6f || hitFR.force > 3000))) IsRuttingFR = true;
            else IsRuttingFR = false;
            if (IsRuttingFR)
            {
                if (!WasRuttingFR)
                {
                    //create a new rut
                    if (RutRightNodeCount < 20) { GameObject.Destroy(goRutRight); Road.Instance.RutCount--; }
                    goRutRight = new GameObject("RutRight");
                    goRutRight.isStatic = true;
                    goRutRight.tag = "Rut";
                    Road.Instance.RutCount++;
                    RutRight = goRutRight.AddComponent<FlatLineRenderer>();
                    RutRight.Init();
                    RutRight.SetMaterial(RutMatrl);
                    RutRight.Width = 0.233f;
                    RutRightNodeCount = 0;
                }
                if (RutRight != null)
                {
                    RutRightNodeCount++;
                    RutRight.AddNode(hitFR.point + Vector3.up * 0.05f);
                }
            }
            WasRuttingFR = IsRuttingFR;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        //Skid Sounds
        if (IsSkiddingFL || IsSkiddingFR)
        { if (!SkidAudioSource.isPlaying) SkidAudioSource.Play(); }
        else SkidAudioSource.Stop();

        //Skidmarks

        if (IsSkiddingFL)
        {
            if (!WasSkiddingFL)
            {
                //create a new rut
                if (SkidMkLeft != null) { SkidMkLeft.enabled = false; Destroy(SkidMkLeft); }
                if (SkidMkLeftNodeCount < 50) { GameObject.Destroy(goSkidMkLeft); Road.Instance.RutCount--; }
                goSkidMkLeft = new GameObject("SkidMkLeft");
                goSkidMkLeft.isStatic = true;
                goSkidMkLeft.tag = "Rut";
                Road.Instance.RutCount++;
                SkidMkLeft = goSkidMkLeft.AddComponent<FlatLineRenderer>();
                SkidMkLeft.Init();
                SkidMkLeft.SetMaterial(SkidMatrl);
                SkidMkLeft.Width = 0.15f;
                SkidMkLeftNodeCount = 0;
            }
            SkidMkLeftNodeCount++;
            SkidMkLeft.AddNode(hitFL.point + Vector3.up * 0.05f);
        }
        WasSkiddingFL = IsSkiddingFL;
        if (IsSkiddingFR)
        {
            if (!WasSkiddingFR)
            {
                //create a new rut
                if (SkidMkRight != null) Destroy(SkidMkRight);
                if (SkidMkRightNodeCount < 50) { GameObject.DestroyImmediate(goSkidMkRight); Road.Instance.RutCount--; }
                goSkidMkRight = new GameObject("SkidMkRight");
                goSkidMkRight.isStatic = true;
                goSkidMkRight.tag = "Rut";
                Road.Instance.RutCount++;
                SkidMkRight = goSkidMkRight.AddComponent<FlatLineRenderer>();
                SkidMkRight.Init();
                SkidMkRight.SetMaterial(SkidMatrl);
                SkidMkRight.Width = 0.15f;
                SkidMkRightNodeCount = 0;
            }
            SkidMkRightNodeCount++;
            SkidMkRight.AddNode(hitFR.point + Vector3.up * 0.05f);
        }
        WasSkiddingFR = IsSkiddingFR;
        if (EndSkidmarks)
        {
            WasSkiddingFL = false; WasSkiddingFR = false; WasRuttingFL = false; WasRuttingFR = false; EndSkidmarks = false;
        }

#endregion

        WCFL.brakeTorque = BrakeForce;
        WCFR.brakeTorque = BrakeForce;
        WCRL.brakeTorque = BrakeForce;
        WCRR.brakeTorque = BrakeForce;

    }

    void OnCollisionStay(Collision coll)
    {
        foreach (ContactPoint cp in coll.contacts)
        {
            try
            {
                if (cp.thisCollider.sharedMaterial.name == "CarBodyPhysicsMaterial" && cp.normal.y > 0.5f)
                {
                    cp.thisCollider.sharedMaterial = StickyCarBodyPhysicsMaterial;
                }
            }
            catch { }
        }
        //Todo: Should I put the relative torque on like Hotrod and Anglia?
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






