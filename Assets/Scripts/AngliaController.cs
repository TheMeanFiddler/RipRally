using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;




public class AngliaController : VehicleController
{
    DamageController _damageController;
    private int SegIdx;
    private ParticleSystem psSprayLFwd;
    private ParticleSystem psSprayRFwd;
    UnityEngine.Object SkidPrefab;
    private Transform _trSkidMarks;
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
    private bool WasInAir = false;
    private bool IsRuttingFL = false;
    private bool IsRuttingFR = false;
    private bool IsSkiddingRL = false;
    public bool WasSkiddingRL { get; set; }
    private bool IsSkiddingRR = false;
    public bool WasSkiddingRR { get; set; }
    protected AudioSource SkidAudioSource;
    protected AudioSource RevAudioSource;
    protected AudioSource AccelAudioSource;
    protected AudioSource DecelAudioSource;
    float RevStartTime = 0;
    float DecelStartTime = 0;
    private float _prevEngineTorque;
    private float _prevRPM;
    PhysicMaterial StickyCarBodyPhysicsMaterial;
    PhysicMaterial CarBodyPhysicsMaterial;
    private bool Braking = false;
    private float BrakeForce;
    private float FCoef;    //remember the default Fwd and Side Force COeffs cos we tinker with them
    private float SCoef;


    public override void Awake()
    {
        base.Awake();
        _damageController = GetComponent<DamageController>();
        SkidPrefab = Resources.Load("Prefabs/SkidmarkPrefab");
        _trSkidMarks = GameObject.Find("Skidmarks").transform;
        SkidAudioSource = transform.Find("Sounds/Skid1").GetComponent<AudioSource>();
        IdleAudioSource = transform.Find("Sounds/Idle").GetComponent<AudioSource>();
        RevAudioSource = transform.Find("Sounds/Rev").GetComponent<AudioSource>();
        AccelAudioSource = transform.Find("Sounds/Accel").GetComponent<AudioSource>();
        DecelAudioSource = transform.Find("Sounds/Decel").GetComponent<AudioSource>();
        IdleAudioSource.mute = true;
        RevAudioSource.mute = true;
        AccelAudioSource.mute = true;
        DecelAudioSource.mute = true;
        IdleAudioSource.volume = Settings.Instance.SFXVolume;
        RevAudioSource.volume = Settings.Instance.SFXVolume*0.66f;
        AccelAudioSource.volume = Settings.Instance.SFXVolume;
        DecelAudioSource.volume = Settings.Instance.SFXVolume;
        SkidAudioSource.volume = Settings.Instance.SFXVolume;
        FLWheel = transform.Find("car/FLWheel").gameObject;
        FRWheel = transform.Find("car/FRWheel").gameObject;
        WasRuttingFL = false;
        WasRuttingFR = false;
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
        _rb.centerOfMass = new Vector3(0, 0.45f, 0.42f);

        motorForce = 1500f;
        _maxBrakeForce = 2000f;
        AntiRollForce = 40f;
        FCoef = WCRL.fFriction.forceCoefficient;
        SCoef = WCRL.sideFriction.forceCoefficient;
        Keyframe[] kfs;
        kfs = new Keyframe[4]
        {
            new Keyframe(0f, 0f, 0f, 0.1f),
            new Keyframe(0.1f, 0.1f, 2.25f, 2.25f),
            new Keyframe(0.17f, 0.27f, 3.3f, 3.3f),
            new Keyframe(0.19f, 0.2f, 0f, 0f)
        };
        frontFrictTarmac = new AnimationCurve(kfs);

        kfs = new Keyframe[4]
{
            new Keyframe(0f, 0f, 0.7f, 0.7f),
            new Keyframe(0.7f, 0.4f, 0.03f, 0.03f),
            new Keyframe(1.2f, 0.7f, 0.3f, 0.3f),
            new Keyframe(1.7f, 0.4f, 0f, 0f)
};
        rearFrictTarmac = new AnimationCurve(kfs);
        SkidThresh = rearFrictTarmac.keys[1].time;

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


        
        //this is for testing
        /*
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


            
            Vector3 _vel = _rb.velocity;
            float _rearSlideSlip = Vector3.Angle(_vel, transform.forward);
            if (WCRL.angularVelocity > 0)
            {
                //Easy cornering - The GPS can manage the reverse steering
                float BendAnglePerSec = 0;
                if (Gps.CurrBend != null)
                {
                    BendAnglePerSec = Gps.CurrBend.AnglePerSeg * Gps.SegsPerSec;
                    float CorrectionAnglePerSec = (BendAnglePerSec - _rb.angularVelocity.y * 57.3f);
                    if (Mathf.Sign(CorrectionAnglePerSec) != Mathf.Sign(BendAnglePerSec))     //only apply the correction if oversteering
                    { h += CorrectionAnglePerSec; }
                    h = Mathf.Clamp(h, -40, 40);
                }

                //For Rear wheel drive we give a little extra torque when reverse steering in a skid
                Vector3 cross = Vector3.Cross(_vel, transform.forward);
                WCRL.fFriction.forceCoefficient = FCoef;
                WCRR.fFriction.forceCoefficient = FCoef;
                WCRL.sideFriction.forceCoefficient = SCoef;
                WCRR.sideFriction.forceCoefficient = SCoef;
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

        if (frontFrict == frontFrictTarmac) h = h / (1f + GetComponent<Rigidbody>().velocity.magnitude / 35); //The last number: bigger means sharper turns at high speed
        WCFL.steerAngle = h;
        WCFR.steerAngle = h;
        
        //For testing comment out this block
        WCFL.forwardFriction.frictionCurve = frontFrict;
        WCFR.forwardFriction.frictionCurve = frontFrict;
        WCRL.forwardFriction.frictionCurve = rearFrict;
        WCRR.forwardFriction.frictionCurve = rearFrict;
        
        WCFL.GravelSimulation = GravelSimulation;
        WCFR.GravelSimulation = GravelSimulation;
        WCRL.GravelSimulation = GravelSimulation;
        WCRR.GravelSimulation = GravelSimulation;
        
        #region Anti-Roll bar Region

        float travelL;
        float travelR;
        float AntiRoll = AntiRollForce * GetComponent<Rigidbody>().velocity.magnitude;

        bool groundedFL = WCFL.GetGroundHit(out hitFL);

        //float ContactAngle;
        //WheelFrictionCurve Fric;
        //Fric = WCFL.forwardFriction;
        if (groundedFL)
        {
            travelL = WCFL.springCompression-1;
        }
        else
        {
            travelL = 1.0f;
        }

        bool groundedFR = WCFR.GetGroundHit(out hitFR);
        if (groundedFR)
        {
            travelR = WCFR.springCompression-1;
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
        float SidewaysSlipRL = hitRL.sidewaysSlip;
        if (groundedRL)
        {
            travelL = WCRL.springCompression-1;
        }
        else
        {
            travelL = 1.0f;
        }
        if (WCRL.SlipVectorMagnitude > SkidThresh && RoadMat == "Tarmac")
        { IsSkiddingRL = true; }
        else { IsSkiddingRL = false;}

        float ForwardSlipRR = hitRR.forwardSlip;
        float SidewaysSlipRR = hitRR.sidewaysSlip;

        if (groundedRR)
        {
            travelR = WCRR.springCompression-1;
        }
        else
        {
            travelR = 1.0f;
        }

        if (WCRR.SlipVectorMagnitude > SkidThresh && RoadMat == "Tarmac")
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

        #endregion

        #region Spray and Smoke Particles Region
        //Spray dirt
        try
        {
            if (RoadMat == "Dirt" || RoadMat == "Air")
            {
                if (groundedRL && ForwardSlipRL !=0)
                {
                    if (WCRL.motorTorque > 0)
                    {
                        psSprayLFwd.Stop();
                        psSprayL.Play();
                        var vel = psSprayL.velocityOverLifetime;
                        vel.x = WCRL.slipVectorNorm.y * Mathf.Sign(WCRL.motorTorque) * 7;
                        vel.y = 2;
                        vel.z = WCRL.slipVectorNorm.x * Mathf.Abs(WCRL.SlipVectorMagnitude) * 7;
                        peSprayL.rateOverTime = Mathf.Clamp(WCRL.SlipVectorMagnitude * 50, 0, 50);
                    }
                    else   //goimg backwards
                    {
                        psSprayL.Stop();
                        psSprayLFwd.Play();
                        var vel = psSprayLFwd.velocityOverLifetime;
                        vel.x = WCRL.slipVectorNorm.y * Mathf.Sign(WCRL.motorTorque) * 7;
                        vel.y = 2;
                        vel.z = WCRL.slipVectorNorm.x * Mathf.Abs(WCRL.SlipVectorMagnitude) * 7;
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
                        vel.x = WCRR.slipVectorNorm.y * Mathf.Sign(WCRR.motorTorque) * 7;
                        vel.y = 2;
                        vel.z = WCRR.slipVectorNorm.x * Mathf.Abs(WCRR.SlipVectorMagnitude) * 7;
                        peSprayR.rateOverTime = Mathf.Clamp(WCRR.SlipVectorMagnitude * 50, 0, 50);
                    }
                    else
                    {
                        psSprayR.Stop();
                        psSprayRFwd.Play();
                        var vel = psSprayRFwd.velocityOverLifetime;
                        vel.x = WCRR.slipVectorNorm.y * Mathf.Sign(WCRR.motorTorque) * 7;
                        vel.y = 2;
                        vel.z = WCRR.slipVectorNorm.x * Mathf.Abs(WCRR.SlipVectorMagnitude) * 7;
                        ParticleSystem.EmissionModule e = psSprayRFwd.emission;
                        e.rateOverTime = Mathf.Clamp(WCRR.SlipVectorMagnitude * 30, 0, 30);
                    }


                }
                else
                {
                    psSprayRFwd.Stop();
                    psSprayR.Stop();
                }
                //Fred.wizard@btinternet.com
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
                ParticleSystem.EmissionModule emRL = psDustL.emission;
                peDustL.rateOverTime = SlipRL * 80f;
                peDustR.rateOverTime = SlipRR * 80f;
                psDustL.transform.localPosition = new Vector3(0, -0.4f, -WCRL.forwardFriction.slip/6);
                psDustR.transform.localPosition = new Vector3(0, -0.4f, -WCRR.forwardFriction.slip/6);
                

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
        try
        {

            if ((RoadMat == "Dirt" || RoadMat == "DirtyRoad") && groundedRL && ((Math.Abs(ForwardSlipRL) > 1f || Math.Abs(SidewaysSlipRL) > 0.1f) && WCRL.springCompression < 0.6f)) IsRuttingFL = true; else IsRuttingFL = false;

            if (IsRuttingFL)
            {

                if (!WasRuttingFL)
                {
                    //create a new rut
                    if (RutLeft != null) Destroy(RutLeft);
                    if (RutLeftNodeCount < 20) { GameObject.Destroy(goRutLeft); Road.Instance.RutCount--; }
                    goRutLeft = new GameObject("RutLeft");
                    goRutLeft.transform.SetParent(_trSkidMarks);
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
                    RutLeft.AddNode(hitRL.point + Vector3.up * 0.05f);
                }
            }
            WasRuttingFL = IsRuttingFL;

            if ((RoadMat == "Dirt" || RoadMat == "DirtyRoad") && groundedRR && ((Math.Abs(ForwardSlipRR) > 1f || Math.Abs(SidewaysSlipRR) > 0.1f) && WCRR.springCompression<0.6f)) IsRuttingFR = true; else IsRuttingFR = false;
            if (IsRuttingFR)
            {
                if (!WasRuttingFR)
                {
                    //create a new rut
                    if (RutRightNodeCount < 20) { GameObject.Destroy(goRutRight); Road.Instance.RutCount--; }
                    goRutRight = new GameObject("RutRight");
                    goRutRight.transform.SetParent(_trSkidMarks);
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
                    RutRight.AddNode(hitRR.point + Vector3.up * 0.05f);
                }
            }
            WasRuttingFR = IsRuttingFR;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

        //Skid Sounds
        if (IsSkiddingRL || IsSkiddingRR)
        { if (!SkidAudioSource.isPlaying) SkidAudioSource.Play(); }
        else SkidAudioSource.Stop();

        //Skidmarks

        if (IsSkiddingRL)
        {
            if (!WasSkiddingRL)
            {
                //create a new rut
                if (SkidMkLeft != null) { SkidMkLeft.enabled = false; Destroy(SkidMkLeft); }
                if (SkidMkLeftNodeCount < 50) { GameObject.Destroy(goSkidMkLeft); Road.Instance.RutCount--; }
                goSkidMkLeft = new GameObject("SkidMkLeft");
                goSkidMkLeft.transform.SetParent(_trSkidMarks);
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
            SkidMkLeft.AddNode(hitRL.point + Vector3.up * 0.05f);
        }
        WasSkiddingRL = IsSkiddingRL;
        if (IsSkiddingRR)
        {
            if (!WasSkiddingRR)
            {
                //create a new rut
                if (SkidMkRight != null) Destroy(SkidMkRight);
                if (SkidMkRightNodeCount < 50) { GameObject.DestroyImmediate(goSkidMkRight); Road.Instance.RutCount--; }
                goSkidMkRight = new GameObject("SkidMkRight");
                goSkidMkRight.transform.SetParent(_trSkidMarks);
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
            SkidMkRight.AddNode(hitRR.point + Vector3.up * 0.05f);
        }
        WasSkiddingRR = IsSkiddingRR;
        if (EndSkidmarks)
        {
            WasSkiddingRL = false; WasSkiddingRR = false; WasRuttingFL = false; WasRuttingFR = false; EndSkidmarks = false;
        }
        #endregion
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
            float bcx = cp.thisCollider.bounds.center.x;
            if (Mathf.Abs(bcx)> 5 && Vector3.Angle(transform.up, Vector3.up) > 90 && _rb.angularVelocity.sqrMagnitude<3)
            {
                _rb.AddRelativeTorque(Vector3.forward *10*bcx, ForceMode.Force);
            }
        }

    }

    public virtual void GetInputFromInputManager()
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
        if (_gpsTimer == 0)
        {
            try { Gps.UpdateSegIdx(); }
            catch { }
            _gpsTimer = 2;
        }
        _gpsTimer--;

        //Engine Sound
        if (WCRL.motorTorque > 0 && WCRL.brakeTorque==0)
        {
            if (IdleAudioSource.isPlaying || DecelAudioSource.isPlaying)
            { IdleAudioSource.Stop(); DecelAudioSource.Stop();
                    RevAudioSource.Play(); RevStartTime = Time.time;
            }

            if (RevStartTime != 0 && Time.time > RevStartTime + 1.5f)
            {
                AccelAudioSource.timeSamples = UnityEngine.Random.Range(0, AccelAudioSource.clip.samples);
                AccelAudioSource.Play();
                RevStartTime = 0;
            }
        }
        else
        {
            if (AccelAudioSource.isPlaying || RevAudioSource.isPlaying)
            { AccelAudioSource.Stop(); RevAudioSource.Stop();
              DecelAudioSource.Play(); DecelStartTime = Time.time; }
            if (DecelStartTime != 0 && Time.time > DecelStartTime + 3f)
            {
                IdleAudioSource.Play();
                DecelStartTime = 0;
            }
        }

            _prevEngineTorque = WCRL.motorTorque;
        }
        /*
        if (_prevEngineTorque > 0 && WCRL.motorTorque == 0)
        {
            //EngineAudioSource.Stop();
            //ClutchAudioSource.Play();
            ClutchStartTime = Time.time;
        }
        //Idle
        if (_prevEngineTorque == 0 && WCRL.motorTorque == 0 && IdleAudioSource.isPlaying == false && Time.time > ClutchStartTime + 1f && EngineAudioSource.isPlaying == false)
        {
            EngineAudioSource.Play();
        }
        if (Mathf.Abs(WCRL.motorTorque) < 1000000)
        {
            ClutchAudioSource.Stop();
            //IdleAudioSource.Stop();
            float _pitch;
            if (EngineAudioSource.isPlaying == true) {
                _pitch = Mathf.Clamp(WCRL.rpm / 800f,0.5f,1f);
                EngineAudioSource.pitch = _pitch;// * 0.1f + EngineAudioSource.pitch * 0.9f;
                float StartTIme = UnityEngine.Random.Range(0, EngineAudioSource.clip.length - 1f);
                EngineAudioSource.time = StartTIme;
                float EndTIme;
                if (EngineAudioSource.pitch < 0) EndTIme = UnityEngine.Random.Range(StartTIme, 0);
                else EndTIme = UnityEngine.Random.Range(StartTIme, EngineAudioSource.clip.length);
                EngineAudioSource.Play();
                EngineAudioSource.SetScheduledEndTime(AudioSettings.dspTime + EndTIme);  }
            /*
            if (Wheelrpm < 300)
            {
                rpm = -Wheelrpm;
                _pitch = rpm / 600 - 0.5f;
                Gear = 1;
            }
            else
            {
                rpm = -(Wheelrpm - 300) % 300;
                Gear = (int)Wheelrpm / 300 + 1;
                _pitch = (rpm / 600 - 0.5f);

            }
//            EngineAudioSource.pitch = _pitch*1.5f;

        }
        CoughAudioSource.mute = (WCRL.motorTorque != 0);
        _prevEngineTorque = WCRL.motorTorque;
        _prevRPM = WCRL.rpm;
*/


    public override void StartEngine()
    {
        IdleAudioSource.mute = false;
        RevAudioSource.mute = false;
        AccelAudioSource.mute = false;
        DecelAudioSource.mute = false;
        IdleAudioSource.Play();
    }

}




public class CarMenuAngliaController : AngliaController
{

    void Start()
    {
        gameObject.AddComponent<CarMenuCarInput>();
        InputManager = gameObject.GetComponent<CarMenuCarInput>();
        GetComponentInChildren<FPCamController>().enabled = false;
    }


}

