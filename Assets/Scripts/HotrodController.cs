using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;




public class HotrodController : VehicleController
{
    DamageController _damageController;
    private int SegIdx;
    private ParticleSystem psSprayLFwd;
    private ParticleSystem psSprayRFwd;
    private Transform _trSkidMarks;
    private int RutLeftNodeCount = 0;
    private int RutRightNodeCount = 0;
    private GameObject goRutLeft;
    private FlatLineRenderer RutLeft;
    private GameObject goRutRight;
    private FlatLineRenderer RutRight;
    private UnityEngine.Object objSkidMk;
    private GameObject goSkidMkLeft;
    private FlatLineRenderer SkidMkLeft;
    private GameObject goSkidMkRight;
    private FlatLineRenderer SkidMkRight;
    private bool WasInAir = false;
    private bool IsRuttingRL = false;
    public bool WasRuttingRL { get; set; }
    private bool IsRuttingRR = false;
    public bool WasRuttingRR { get; set; }
    private bool IsSkiddingRL = false;
    public bool WasSkiddingRL { get; set; }
    private bool IsSkiddingRR = false;
    public bool WasSkiddingRR { get; set; }
    List<GameObject> SkidMarks;
    private AudioSource SkidAudioSource;
    float ClutchStartTime = 0;
    private float _prevEngineTorque;

    PhysicMaterial StickyCarBodyPhysicsMaterial;
    PhysicMaterial CarBodyPhysicsMaterial;
    private bool Braking = false;
    private float FCoef;    //remember the default Fwd and Side Force COeffs cos we tinker with them
    private float SCoef;


    public override void Awake()
    {
        base.Awake();
        _damageController = GetComponent<DamageController>();
        _trSkidMarks = GameObject.Find("Skidmarks").transform;
        SkidAudioSource = GetComponent<AudioSource>();
        EngineAudioSource = transform.Find("Sounds/Engine").GetComponent<AudioSource>();
        CoughAudioSource = transform.Find("Sounds/Cough").GetComponent<AudioSource>();
        ClutchAudioSource = transform.Find("Sounds/ClutchDown").GetComponent<AudioSource>();
        EngineAudioSource.mute = true;
        CoughAudioSource.mute = true;
        SkidAudioSource.volume = Settings.Instance.SFXVolume;
        EngineAudioSource.volume = Settings.Instance.SFXVolume;
        ClutchAudioSource.volume = Settings.Instance.SFXVolume;
        CoughAudioSource.volume = Settings.Instance.SFXVolume;
        FLWheel = transform.Find("car/FLWheel").gameObject;
        FRWheel = transform.Find("car/FRWheel").gameObject;
        WasRuttingRL = false;
        WasRuttingRR = false;
        RutMatrl = (Material)Resources.Load("Prefabs/Materials/WheelRutGrey");
        SkidMatrl = (Material)Resources.Load("Prefabs/Materials/SkidMark");

        psSprayL = transform.Find("WheelColliders/WCRL/SprayFL").GetComponent<ParticleSystem>();
        psSprayR = transform.Find("WheelColliders/WCRR/SprayFR").GetComponent<ParticleSystem>();
        psSprayLFwd = transform.Find("WheelColliders/WCRL/SprayFLFwd").GetComponent<ParticleSystem>();
        psSprayRFwd = transform.Find("WheelColliders/WCRR/SprayFRFwd").GetComponent<ParticleSystem>();
        psDustL = transform.Find("WheelColliders/WCRL/DustFL").GetComponent<ParticleSystem>();
        psDustR = transform.Find("WheelColliders/WCRR/DustFR").GetComponent<ParticleSystem>();
        peSprayL = psSprayL.emission;
        peSprayR = psSprayR.emission;
        pmSprayL = psSprayL.main;
        pmSprayR = psSprayR.main;
        try
        {
            _fLRimRenderer = transform.Find("car/FLWheel/FLRim").GetComponent<Renderer>();
            _fRRimRenderer = transform.Find("car/FRWheel/FRRim").GetComponent<Renderer>();
            _rLRimRenderer = transform.Find("car/RLWheel/RLRim").GetComponent<Renderer>();
            _rRRimRenderer = transform.Find("car/RRWheel/RRRim").GetComponent<Renderer>();
            _fRRimSpinRenderer = transform.Find("car/FRWheel/FRRimSpin").GetComponent<Renderer>();
            _rLRimSpinRenderer = transform.Find("car/RLWheel/RLRimSpin").GetComponent<Renderer>();
            _fLRimSpinRenderer = transform.Find("car/FLWheel/FLRimSpin").GetComponent<Renderer>();
        }
        catch { _rimSpin = false; }

        StickyCarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/StickyCarBodyPhysicsMaterial");
        CarBodyPhysicsMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/CarBodyPhysicsMaterial");

        RLWheel = transform.Find("car/RLWheel").gameObject;
        RRWheel = transform.Find("car/RRWheel").gameObject;
        RRWheel.transform.localScale = Vector3.one;
        WCFL = transform.Find("WheelColliders/WCFL").GetComponent<WheelController>();
        WCFR = transform.Find("WheelColliders/WCFR").GetComponent<WheelController>();
        WCRL = transform.Find("WheelColliders/WCRL").GetComponent<WheelController>();
        WCRR = transform.Find("WheelColliders/WCRR").GetComponent<WheelController>();
        _rb = GetComponent<Rigidbody>(); 
        _rb.centerOfMass = new Vector3(0, 0.2f, 0.52f);

        motorForce = 2000f;
        _maxBrakeForce = 3500f;
        steerForce = 25f;
        AntiRollForce = 0f;
        FCoef = WCRL.fFriction.forceCoefficient;
        SCoef = WCRL.sideFriction.forceCoefficient;
        Keyframe[] kfs;
        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 0f, 0.3f),
            new Keyframe(0.08f, 0.14f, 2.7f, 2.7f),
            new Keyframe(0.12f, 0.3f, 3.3f, 3.3f),
            new Keyframe(0.15f, 0.2f, 0f, 0f)
            /*
            new Keyframe(0f, 0f, 0f, 0.1f),
            new Keyframe(0.1f, 0.1f, 2.25f, 2.25f),
            new Keyframe(0.17f, 0.27f, 3.3f, 3.3f),
            new Keyframe(0.19f, 0.2f, 0f, 0f)
            */
        };
        frontFrictTarmac = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(0.5f, 0.45f, 0.03f, 0.03f),
            new Keyframe(1f, 0.8f, 0.3f, 0.3f),
            new Keyframe(1.4f, 0.45f, 0f, 0f)
            /*
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(0.7f, 0.3f, 0.03f, 0.03f),
            new Keyframe(1.2f, 0.55f, 0.3f, 0.3f),
            new Keyframe(1.7f, 0.45f, 0f, 0f)
            */
};
        rearFrictTarmac = new AnimationCurve(kfs);

        kfs = new Keyframe[3]
        {
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(2.25f, 0.5f, 0.3f, 0.3f),
            new Keyframe(3f, 0.3f, 0f, 0f)
        };
        frontFrictDirtyRoad = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(1.25f, 0.25f, 0.03f, 0.03f),
            new Keyframe(2f, 0.35f, 0.3f, 0.3f),
            new Keyframe(3f, 0.2f, 0f, 0f)
};
        rearFrictDirtyRoad = new AnimationCurve(kfs);

        kfs = new Keyframe[3]
        {
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(2.25f, 0.5f, 0.3f, 0.3f),
            new Keyframe(3f, 0.3f, 0f, 0f)
        };
        frontFrictDirt = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(1.25f, 0.25f, 0.03f, 0.03f),
            new Keyframe(2f, 0.35f, 0.3f, 0.3f),
            new Keyframe(3f, 0.2f, 0f, 0f)
};
        rearFrictDirt = new AnimationCurve(kfs);


        /*
        //this is for testing
        WCFL.forwardFriction.frictionCurve = frontFrictTarmac;
        WCFR.forwardFriction.frictionCurve = frontFrictTarmac;
        WCRL.forwardFriction.frictionCurve = rearFrictTarmac;
        WCRR.forwardFriction.frictionCurve = rearFrictTarmac;
        */

    }

    // Update is called once per frame
    //FixedUpdate is called for every physics calculation
    void FixedUpdate()
    {


        GetInputFromInputManager();

        //The rest of this method controls the car

        WCFL.brakeTorque = BrakeForce;
        WCFR.brakeTorque = BrakeForce;
        WCRL.brakeTorque = BrakeForce;
        WCRR.brakeTorque = BrakeForce;

        WCRL.motorTorque = v;
        WCRR.motorTorque = v;

        if (WCRL.rpm < -300)
        {
            WCRL.motorTorque = Mathf.Clamp(WCRL.motorTorque, 0, 10000);
            WCRR.motorTorque = Mathf.Clamp(WCRR.motorTorque, 0, 10000);
        }

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

        //Adapt the tyre slip according to the road type


        bool GravelSimulation = false;
        if (Gps != null)
        {
            UnityEngine.Profiling.Profiler.BeginSample("AdaptSlip");
            RoadMat = Gps.RoadMat;
            switch (RoadMat)
            {
                case "Tarmac":
                case "Washboard":
                    frontFrict = frontFrictTarmac;
                    rearFrict = rearFrictTarmac;
                    break;
                case "DirtyRoad":
                    frontFrict = frontFrictDirtyRoad;
                    rearFrict = rearFrictDirtyRoad;
                    //GravelSimulation = true;
                    break;
                default:
                    frontFrict = frontFrictDirt;
                    rearFrict = rearFrictDirt;
                    GravelSimulation = true;
                    break;
            }
            SkidThresh = rearFrict.keys[1].time;

            UnityEngine.Profiling.Profiler.EndSample();
            UnityEngine.Profiling.Profiler.BeginSample("ReverseSteer");
            //For Rear wheel drive we give a little extra torque when reverse steering in a skid
            Vector3 _vel = _rb.velocity;
            float _rearSlideSlip = Vector3.Angle(_vel, transform.forward);
            if (WCRL.angularVelocity > 0)
            {
                //Easy cornering - The GPS can manage the reverse steering
                float BendAnglePerSec = 0;
                UnityEngine.Profiling.Profiler.BeginSample("CalcNewF");
                if (Gps.CurrBend != null)
                {
                    BendAnglePerSec = Gps.CurrBend.AnglePerSeg * Gps.SegsPerSec;
                    float CorrectionAnglePerSec = (BendAnglePerSec - _rb.angularVelocity.y * 57.3f);
                    if (Mathf.Sign(CorrectionAnglePerSec) != Mathf.Sign(BendAnglePerSec))     //only apply the correction if oversteering
                    { h += CorrectionAnglePerSec; }
                    h = Mathf.Clamp(h, -40, 40);
                }

                WCRL.fFriction.forceCoefficient = FCoef;
                WCRR.fFriction.forceCoefficient = FCoef;
                WCRL.sideFriction.forceCoefficient = SCoef;
                WCRR.sideFriction.forceCoefficient = SCoef;

                Vector3 cross = Vector3.Cross(_vel, transform.forward);
                if (Mathf.Abs(h) < 3 || Mathf.Sign(cross.y) != Mathf.Sign(h))
                {
                    WCRL.motorTorque *= (1 + Mathf.Abs(_rearSlideSlip) / 90f);
                    WCRR.motorTorque *= (1 + Mathf.Abs(_rearSlideSlip) / 90f);
                    WCRL.fFriction.forceCoefficient *= (1 + Mathf.Abs(_rearSlideSlip) / 90f);
                    WCRR.fFriction.forceCoefficient *= (1 + Mathf.Abs(_rearSlideSlip) / 90f);
                }
            }   //going forwrds

            //spinout
            //if (_rearSlideSlip > 90) { WCRL.sFriction.forceCoefficient = 0.2f; WCRR.sFriction.forceCoefficient = 0.2f; }
        }
        else  //if gps is null
        {
            frontFrict = frontFrictDirt;
            rearFrict = rearFrictDirt;
            RoadMat = "Dirt";
        }

        if (frontFrict == frontFrictTarmac && _rb.velocity.sqrMagnitude > 4) h = h / 2;
        WCFL.steerAngle = h;
        WCFR.steerAngle = h;


        //comment out this block for roadmat testing
        WCFL.forwardFriction.frictionCurve = frontFrict;
        WCFR.forwardFriction.frictionCurve = frontFrict;
        WCRL.forwardFriction.frictionCurve = rearFrict;
        WCRR.forwardFriction.frictionCurve = rearFrict;
        WCFL.GravelSimulation = GravelSimulation;
        WCFR.GravelSimulation = GravelSimulation;
        WCRL.GravelSimulation = GravelSimulation;
        WCRR.GravelSimulation = GravelSimulation;


        #region Anti-Roll bar Region
        UnityEngine.Profiling.Profiler.BeginSample("AntRollbar");
        float travelL;
        float travelR;
        float AntiRoll = AntiRollForce * GetComponent<Rigidbody>().velocity.magnitude;

        bool groundedFL = WCFL.GetGroundHit(out hitFL);

        //float ContactAngle;
        //WheelFrictionCurve Fric;
        //Fric = WCFL.forwardFriction;
        if (groundedFL)
        {
            travelL = 1 - WCFL.springCompression; // (-WCFL.transform.InverseTransformPoint(hitFL.point).y - WCFL.radius) / WCFL.springLength;
        }
        else
        {
            travelL = 1.0f;
        }

        bool groundedFR = WCFR.GetGroundHit(out hitFR);
        if (groundedFR)
        {
            travelR = 1 - WCFR.springCompression; // (-WCFR.transform.InverseTransformPoint(hitFR.point).y - WCFR.radius) / WCFR.springLength;
        }
        else
        {
            travelR = 1.0f;
        }

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

        bool groundedRL = WCRL.GetGroundHit(out hitRL);
        bool groundedRR = WCRR.GetGroundHit(out hitRR);

        float ForwardSlipRL = hitRL.forwardSlip;
        if (groundedRL)
        {
            travelL = (-WCRL.transform.InverseTransformPoint(hitRL.point).y - WCRL.radius) / WCRL.springLength;
        }
        else
        {
            travelL = 1.0f;
        }
        if (WCRL.SlipVectorMagnitude > SkidThresh*2.25f && RoadMat == "Tarmac")
        { IsSkiddingRL = true; }
        else { IsSkiddingRL = false; }

        float ForwardSlipRR = hitRR.forwardSlip;
        if (groundedRR)
        {
            travelR = (-WCRR.transform.InverseTransformPoint(hitRR.point).y - WCRR.radius) / WCRR.springLength;
        }
        else
        {
            travelR = 1.0f;
        }

        if (WCRR.SlipVectorMagnitude > SkidThresh*2.25f && RoadMat == "Tarmac")
            IsSkiddingRR = true;
        else IsSkiddingRR = false;

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
        UnityEngine.Profiling.Profiler.EndSample();
        #endregion

        #region Spray and Smoke Particles Region
        //Spray dirt
        try
        {
            if (RoadMat == "Dirt" || RoadMat == "Air")
            {
                if (groundedRL && ForwardSlipRL != 0)
                {
                    if (WCRL.motorTorque > 0)
                    {
                        psSprayLFwd.Stop();
                        psSprayL.Play();
                        var vel = psSprayL.velocityOverLifetime;
                        vel.x = WCRL.slipVectorNorm.y * Mathf.Sign(WCRL.motorTorque) * 3;
                        vel.y = 2;
                        vel.z = WCRL.slipVectorNorm.x * Mathf.Abs(WCRL.SlipVectorMagnitude) * 3.5f;
                        peSprayL.rateOverTime = Mathf.Clamp(WCRL.SlipVectorMagnitude * 50, 0, 50);
                    }
                    else   //goimg backwards
                    {
                        psSprayL.Stop();
                        psSprayLFwd.Play();
                        var vel = psSprayLFwd.velocityOverLifetime;
                        vel.x = WCRL.slipVectorNorm.y * Mathf.Sign(WCRL.motorTorque) * 3;
                        vel.y = Mathf.Abs(WCRL.SlipVectorMagnitude);
                        vel.z = WCRL.slipVectorNorm.x * Mathf.Abs(WCRL.SlipVectorMagnitude) * 3.5f;
                        //peSprayFLFwd.emissionRate = Mathf.Clamp(WCRL.SlipVectorMagnitude * 30, 0, 30);
                        ParticleSystem.EmissionModule e = psSprayLFwd.emission;
                        e.rateOverTime = Mathf.Clamp(WCRL.SlipVectorMagnitude * 30, 0, 30);
                    }
                }
                else
                {
                    psSprayLFwd.Stop();
                    psSprayL.Stop();
                }

                if (groundedRR && ForwardSlipRR != 0)
                {
                    if (WCRL.motorTorque > 0)
                    {
                        psSprayRFwd.Stop();
                        psSprayR.Play();
                        var vel = psSprayR.velocityOverLifetime;
                        vel.x = WCRR.slipVectorNorm.y * Mathf.Sign(WCRR.motorTorque) * 4;
                        vel.y = 2;
                        vel.z = WCRR.slipVectorNorm.x * Mathf.Abs(WCRR.SlipVectorMagnitude) * 5;
                        peSprayR.rateOverTime = Mathf.Clamp(WCRR.SlipVectorMagnitude * 50, 0, 50);
                    }
                    else
                    {
                        psSprayR.Stop();
                        psSprayRFwd.Play();
                        var vel = psSprayRFwd.velocityOverLifetime;
                        vel.x = WCRR.slipVectorNorm.y * Mathf.Sign(WCRR.motorTorque) * 4;
                        vel.y = 2;
                        vel.z = WCRR.slipVectorNorm.x * Mathf.Abs(WCRR.SlipVectorMagnitude) * 5;
                        //peSprayFRFwd.emissionRate = Mathf.Clamp(WCRR.SlipVectorMagnitude * 50, 0, 50);
                        ParticleSystem.EmissionModule e = psSprayRFwd.emission;
                        e.rateOverTime = Mathf.Clamp(WCRR.SlipVectorMagnitude * 30, 0, 30);
                    }
                }
                else
                {
                    psSprayRFwd.Stop();
                    psSprayR.Stop();
                }
            }
            else { psSprayR.Stop(); psSprayRFwd.Stop(); psSprayL.Stop(); psSprayLFwd.Stop(); }
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
                float SlipRL = Mathf.Clamp(WCRL.SlipVectorMagnitude, 0, 2f);
                float SlipRR = Mathf.Clamp(WCRR.SlipVectorMagnitude, 0, 2f);
                peDustL.rateOverTime = SlipRL * 80f;
                peDustR.rateOverTime = SlipRR * 80f;
                psDustL.transform.localPosition = new Vector3(0, -0.4f, -WCRL.forwardFriction.slip / 6);
                psDustR.transform.localPosition = new Vector3(0, -0.4f, -WCRR.forwardFriction.slip / 6);


            }
            else
            {
                psDustL.Stop();
                psDustR.Stop();
            }

        }
        catch { }
        #endregion

        #region Wheel Ruts and Skidmarks Region
        //Make Wheel ruts

        if ((RoadMat == "Dirt" || RoadMat == "DirtyRoad") && groundedRL && WCRL.SlipVectorMagnitude > SkidThresh/2) IsRuttingRL = true; else IsRuttingRL = false;
        //Debug.Log(IsRuttingRL.ToString() + WasRuttingRL.ToString());
        if (IsRuttingRL)
        {
            if (!WasRuttingRL)
            {
                //create a new rut
                RutLeft = Road.Instance.NextSkidMk(RutLeft);
                if (RutLeft != null)
                {
                    RutLeft.SetMaterial(RutMatrl);
                    RutLeft.Width = 0.233f;
                    RutLeftNodeCount = 0;
                }
            }
            if (RutLeft != null)
            {
                RutLeft.AddNode(hitRL.point + Vector3.up * 0.05f);
            }
        }
        WasRuttingRL = IsRuttingRL;

        if ((RoadMat == "Dirt" || RoadMat == "DirtyRoad") && groundedRR && WCRR.SlipVectorMagnitude > SkidThresh/2) IsRuttingRR = true; else IsRuttingRR = false;

        if (IsRuttingRR)
        {
            if (!WasRuttingRR)
            {
                //create a new rut
                RutRight = Road.Instance.NextSkidMk(RutRight);
                if (RutRight != null)
                {
                    RutRight.SetMaterial(RutMatrl);
                    RutRight.Width = 0.233f;
                    RutRightNodeCount = 0;
                }
            }
            if (RutRight != null)
            {
                RutRight.AddNode(hitRR.point + Vector3.up * 0.05f);
            }
        }
        WasRuttingRR = IsRuttingRR;


        //Skidmarks

        if (IsSkiddingRL)
        {
            if (!WasSkiddingRL)
            {
                //create a new rut
                RutLeft = Road.Instance.NextSkidMk(RutLeft);
                if (RutLeft != null)
                {
                    RutLeft.SetMaterial(SkidMatrl);
                    RutLeft.Width = 0.233f;
                    RutLeftNodeCount = 0;
                }
            }
            if (RutLeft != null)
            {
                RutLeft.AddNode(hitRL.point + Vector3.up * 0.05f);
            }
        }
        WasSkiddingRL = IsSkiddingRL;
        
        if (IsSkiddingRR)
        {
            if (!WasSkiddingRR)
            {
                //create a new rut
                RutRight = Road.Instance.NextSkidMk(RutRight);
                if (RutRight != null)
                {
                    RutRight.SetMaterial(SkidMatrl);
                    RutRight.Width = 0.233f;
                    RutRightNodeCount = 0;
                }
            }
            if (RutRight != null)
            {
                RutRight.AddNode(hitRR.point + Vector3.up * 0.05f);
            }
        }

        WasSkiddingRR = IsSkiddingRR;
        if (EndSkidmarks)
        {
            WasSkiddingRL = false; WasSkiddingRR = false; WasRuttingRL = false; WasRuttingRR = false; EndSkidmarks = false;
        }

        #endregion

        //Skid Sounds
        if (IsSkiddingRL || IsSkiddingRR)
        { if (!SkidAudioSource.isPlaying) SkidAudioSource.Play(); }
        else SkidAudioSource.Stop();
    }

    void OnCollisionStay(Collision coll)
{
    foreach (ContactPoint cp in coll.contacts)
    {
        try
        {
            if (cp.thisCollider.sharedMaterial.name == "CarBodyPhysicsMaterial" && cp.normal.y > 0.5f)
            {
                    //Sticky has 0.6 friction instead of 0 and o.3 bounce instead of 0
                cp.thisCollider.sharedMaterial = StickyCarBodyPhysicsMaterial;
            }
        }
        catch { }
        float bcx = cp.thisCollider.bounds.center.x;
        if (Mathf.Abs(bcx) > 5 && Vector3.Angle(transform.up, Vector3.up) > 90 && _rb.angularVelocity.sqrMagnitude < 3)
        {
            _rb.AddRelativeTorque(Vector3.forward * 10 * bcx, ForceMode.Force);
        }
    }

}


public new virtual void GetInputFromInputManager()
{
    if (InputManager == null) return;
    //Accel and Brake
    v = InputManager.ZMovement() * motorForce;
    BrakeForce = InputManager.BrakeForce * _maxBrakeForce;
    if (BrakeForce > 0) Braking = true;
        //STEERING
    h = InputManager.XMovement();
    // / (1 + GetComponent<Rigidbody>().velocity.magnitude / 20); //The last number: bigger means sharper turns at high speed
    h = Mathf.Clamp(h, -40, 40);
}


void Update()
{
        if (Gps != null)
        {
            if (_gpsTimer == 0)
            {
                Gps.UpdateSegIdx();
                _gpsTimer = 2;
            }
            _gpsTimer--;
        }

        //Engine Sound


        if (_prevEngineTorque > 0 && WCRL.motorTorque == 0)
    {
        EngineAudioSource.Stop();
        ClutchAudioSource.Play();
        ClutchStartTime = Time.time;
    }
    if (_prevEngineTorque == 0 && WCRL.motorTorque == 0 && Time.time > ClutchStartTime + 1f && EngineAudioSource.isPlaying == false)
    {
        EngineAudioSource.pitch = 0.2f;
        EngineAudioSource.Play();
    }
    if (Mathf.Abs(WCRL.motorTorque) > 0)
    {
        ClutchAudioSource.Stop();
        float _pitch;
        float rpm;
        float Wheelrpm = Mathf.Abs((WCFL.rpm < WCFR.rpm) ? WCFL.rpm : WCFR.rpm);
        if (Wheelrpm > 1499) Wheelrpm = 1499;
        if (EngineAudioSource.isPlaying == false) EngineAudioSource.Play();
        if (Wheelrpm < 300)
        {
            rpm = -Wheelrpm;
            _pitch = rpm / 600 - 0.5f;
        }
        else
        {
            rpm = -(Wheelrpm - 300) % 300;
            _pitch = (rpm / 600 - 0.5f);

        }
        EngineAudioSource.pitch = _pitch * 1.5f;
    }
    CoughAudioSource.mute = (WCRL.motorTorque != 0);
    _prevEngineTorque = WCRL.motorTorque;

}

    public override void StartEngine()
    {
        EngineAudioSource.mute = false;
        CoughAudioSource.mute = false;
    }

}




public class CarMenuHotrodController : HotrodController
{

    void Start()
    {
        gameObject.AddComponent<CarMenuCarInput>();
        InputManager = gameObject.GetComponent<CarMenuCarInput>();
        GetComponentInChildren<FPCamController>().enabled = false;
        //NetworkManager.Instance.OnRPCPositionReceived += new RPCPositionReceivedEventHandler(_nwm_PositionReceived);
        ClutchAudioSource.mute = true;
    }


}

