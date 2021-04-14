using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;




public class ModelTController : MonoBehaviour, iVehicleController
{
    public GPS Gps { get; set; }
    public iInputManager InputManager { get; set; }
    GameObject Player;
    protected GameObject goCar;
    GameObject FLWheel;
    GameObject FRWheel;
    GameObject RLWheel;
    GameObject RRWheel;
    GameObject FLRim;
    GameObject FRRim;
    GameObject RLRim;
    GameObject FRSteering;
    GameObject FLSteering;
    UnityEngine.Object SkidPrefab;
    Transform Skidmarks;
    public float SkidThresh { get; set; }
    public float motorForce { get; set; }
    public float steerForce { get; set; }
    public float AntiRollForce { get; set; }
    private int SegIdx;
    private WheelCollider WCFL;
    private WheelCollider WCFR;
    private WheelCollider WCRL;
    private WheelCollider WCRR;
    private GameObject SprayFL;
    private ParticleSystem peSprayFL;
    private GameObject SprayFR;
    private ParticleSystem peSprayFR;
    private Transform _trSkidMarks;
    private int RutLeftNodeCount = 0;
    private int RutRightNodeCount = 0;
    private GameObject goRutLeft;
    private LineRenderer RutLeft;
    private GameObject goRutRight;
    private LineRenderer RutRight;
    private int SkidMkLeftNodeCount = 0;
    private int SkidMkRightNodeCount = 0;
    private GameObject goSkidMkLeft;
    private FlatLineRenderer SkidMkLeft;
    private GameObject goSkidMkRight;
    private FlatLineRenderer SkidMkRight;
    WheelHit hitFL = new WheelHit();
    WheelHit hitFR = new WheelHit();
    WheelHit hitRL = new WheelHit();
    WheelHit hitRR = new WheelHit();
    private bool WasInAir = false;
    private bool IsRuttingFL = false;
    public bool WasRuttingFL { get; set; }
    private bool IsRuttingFR = false;
    public bool WasRuttingFR { get; set; }
    private bool IsSkiddingFL = false;
    public bool WasSkiddingFL { get; set; }
    private bool IsSkiddingRL = false;
    public bool WasSkiddingRL { get; set; }
    private bool IsSkiddingFR = false;
    public bool WasSkiddingFR { get; set; }
    private AudioSource SkidAudioSource;
    protected AudioSource EngineAudioSource;
    private AudioSource CoughAudioSource;
    private AudioSource ClutchAudioSource;
    private AudioSource IdleAudioSource;
    Material RutMatrl;
    Material SkidMatrl;
    public bool EndSkidmarks { get; set; }
    protected float v;
    private float h;
    private bool Braking = false;

    public virtual void Init() { } //this is used by the car player controller to add the input manager and the speedo

    void Awake()
    {
        //DontDestroyOnLoad(this);

        //if (Network.isClient || Network.isServer)
        //{
        //    if (!networkView.isMine)
        //    {
        //        enabled = false;
        //    }
        //}
        Player = this.gameObject;
        goCar = this.transform.Find("car").gameObject;
        SkidPrefab = Resources.Load("Prefabs/SkidmarkPrefab");
        _trSkidMarks = GameObject.Find("Skidmarks").transform;
        SkidAudioSource = GetComponent<AudioSource>();
        Transform Eng = transform.Find("Engine");
        EngineAudioSource = transform.Find("Engine").GetComponent<AudioSource>();
        CoughAudioSource = Eng.Find("Cough").GetComponent<AudioSource>();
        ClutchAudioSource = Eng.Find("ClutchDown").GetComponent<AudioSource>();
        IdleAudioSource = Eng.Find("Idle").GetComponent<AudioSource>();
        EngineAudioSource.mute = true;
        CoughAudioSource.mute = true;
        IdleAudioSource.mute = true;
        FLWheel = transform.Find("car/FLSteering/FLWheel").gameObject;
        FRWheel = transform.Find("car/FRSteering/FRWheel").gameObject;
        WasRuttingFL = false;
        WasRuttingFR = false;
        RutMatrl = (Material)Resources.Load("Prefabs/Materials/WheelRutGrey");
        SkidMatrl = (Material)Resources.Load("Prefabs/Materials/SkidMark");
        try
        {
            SprayFL = transform.Find("WheelColliders/WCFL/SprayFL").gameObject;
            peSprayFL = SprayFL.GetComponent<ParticleSystem>();
            SprayFR = transform.Find("WheelColliders/WCFR/SprayFR").gameObject;
            peSprayFR = SprayFR.GetComponent<ParticleSystem>();
            FLRim = transform.Find("car/FLSteering/FLWheel/FLRim").gameObject;
            FRRim = transform.Find("car/FRSteering/FRWheel/FRRim").gameObject;
            RLRim = transform.Find("car/RLWheel/RLRim").gameObject;
        }
        catch{ }

        FRSteering = transform.Find("car/FRSteering").gameObject;
        FLSteering = transform.Find("car/FLSteering").gameObject;
        RLWheel = transform.Find("car/RLWheel").gameObject;
        RRWheel = transform.Find("car/RRWheel").gameObject;
        WCFL = transform.Find("WheelColliders/WCFL").GetComponent<WheelCollider>();
        WCFR = transform.Find("WheelColliders/WCFR").GetComponent<WheelCollider>();
        WCRL = transform.Find("WheelColliders/WCRL").GetComponent<WheelCollider>();
        WCRR = transform.Find("WheelColliders/WCRR").GetComponent<WheelCollider>();
        GetComponent<Rigidbody>().centerOfMass += new Vector3(0, -0.1f, 0.5f);
        SkidThresh = 8.0f;
        motorForce = 600f;
        steerForce = 25f;
        AntiRollForce = 500f;
    }


    // Update is called once per frame
    //FixedUpdate is called for every physics calculation
    void FixedUpdate()
    {
        try { Gps.UpdateSegIdx(); }
        catch{ }
        GetInputFromInputManager();

        //The rest of this method controls the car
        WCRL.motorTorque = v;
        WCRR.motorTorque = v;
        //FLWheel.transform.Rotate(WCFR.rpm / 10, 0, 0, Space.Self);
        //FRWheel.transform.Rotate(WCFR.rpm / 10, 0, 0, Space.Self);
        //RRWheel.transform.Rotate(WCRR.rpm / 10, 0, 0, Space.Self);
        //RLWheel.transform.Rotate(WCRL.rpm / 10, 0, 0, Space.Self);

        WCFL.steerAngle = h;
        WCFR.steerAngle = h;
        Quaternion qSteer = Quaternion.Euler(new Vector3(0, 180 + h * 2f, 0));
        FRSteering.transform.localRotation = qSteer;
        FLSteering.transform.localRotation = qSteer;

        //Adapt the tyre slip according to the road type
        string RoadMat = "";
        WheelFrictionCurve sFriction = WCRL.sidewaysFriction;
        WheelFrictionCurve fFriction = WCRL.forwardFriction;
        if (Gps != null)
        {
            if (Gps.IsOnRoad)
            {
                SegIdx = Gps.CurrSegIdx;
                RoadMat = Road.Instance.Segments[SegIdx].roadMaterial;
                if (RoadMat == "Dirt")
                {
                    fFriction.stiffness = 0.1f;
                    sFriction.stiffness = 0.01f;//hit.collider.material.staticFriction;
                }
                else
                {
                    fFriction.stiffness = 0.5f;
                    sFriction.stiffness = 0.04f;
                }
            }
            else
            {
                fFriction.stiffness = 0.1f;
                sFriction.stiffness = 0.01f;
                RoadMat = "Dirt";
            }
        }
        WCFL.forwardFriction = fFriction;
        WCFR.forwardFriction = fFriction;
        WCFL.sidewaysFriction = sFriction;
        WCFR.sidewaysFriction = sFriction;
        WCRL.sidewaysFriction = sFriction;
        WCRR.sidewaysFriction = sFriction;

        float travelL;
        float travelR;
        float AntiRoll = AntiRollForce * GetComponent<Rigidbody>().velocity.magnitude;

        bool groundedFL = WCFL.GetGroundHit(out hitFL);
        float ForwardSlipFL = hitFL.forwardSlip;
        float SidewaysSlipFL = hitFL.sidewaysSlip;
        //float ContactAngle;
        //WheelFrictionCurve Fric;
        //Fric = WCFL.forwardFriction;
        if (groundedFL)
        {
            travelL = (-WCFL.transform.InverseTransformPoint(hitFL.point).y - WCFL.radius) / WCFL.suspensionDistance;
        }
        else
        {
            travelL = 1.0f;
        }

        //ContactAngle = Vector3.Angle (Vector3.up,WCFL.transform.InverseTransformPoint(hit.point))*2*Mathf.PI/360;
        //Fric.stiffness= Mathf.Sin (ContactAngle);
        bool groundedFR = WCFR.GetGroundHit(out hitFR);
        float ForwardSlipFR = hitFR.forwardSlip;
        float SidewaysSlipFR = hitFR.sidewaysSlip;
        if (groundedFR)
        {
            travelR = (-WCFR.transform.InverseTransformPoint(hitFR.point).y - WCFR.radius) / WCFR.suspensionDistance;

            FRWheel.transform.Rotate((WCFR.rpm) / 10 - ForwardSlipFR * 40, 0, 0, Space.Self);
        }
        else
        {
            travelR = 1.0f;
            FRWheel.transform.Rotate((WCFR.rpm) / 10, 0, 0, Space.Self);
        }

        float SkidR = Mathf.Abs(SidewaysSlipFR) * (0.7f - travelR);
        if (Braking)
        {
            SkidR += 18.0f * Mathf.Abs(ForwardSlipFR);
        }
        else
        {
            SkidR += 18.0f * Mathf.Abs(ForwardSlipFR);
        }
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
        AntiRoll = 50f * GetComponent<Rigidbody>().velocity.magnitude;
        bool groundedRL = WCRL.GetGroundHit(out hitRL);
        float ForwardSlipRL = hitRL.forwardSlip;
        float SidewaysSlipRL = hitRL.sidewaysSlip;
        if (groundedRL)
        {
            travelL = (-WCRL.transform.InverseTransformPoint(hitRL.point).y - WCRL.radius) / WCRL.suspensionDistance;
            RLWheel.transform.Rotate((WCRL.rpm) / 10 - ForwardSlipRL * 40, 0, 0, Space.Self);
        }
        else
        {
            travelL = 1.0f;
            RLWheel.transform.Rotate((WCRL.rpm) / 10, 0, 0, Space.Self);
        }
        float SkidL = Mathf.Abs(SidewaysSlipRL) * (0.7f - travelL);
        if (Braking)
        {
            SkidL += 18.0f * Mathf.Abs(ForwardSlipRL);
        }
        else
        {
            SkidL += 18.0f * Mathf.Abs(ForwardSlipRL);
        }
        if (SkidL > SkidThresh && RoadMat == "Tarmac")
            IsSkiddingRL = true;
        else IsSkiddingRL = false;

        bool groundedRR = WCRR.GetGroundHit(out hitRR);
        float ForwardSlipRR = hitRR.forwardSlip;
        float SidewaysSlipRR = hitRR.sidewaysSlip;
        if (groundedRR)
        {
            travelR = (-WCRR.transform.InverseTransformPoint(hitRR.point).y - WCRR.radius) / WCRR.suspensionDistance;
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

        //Make Wheel ruts
        try
        {
            if (RoadMat == "Dirt" && groundedFL && !hitFL.collider.name.Contains("Car") && hitFL.force > 3200 && Road.Instance.RutCount < 150) IsRuttingFL = true; else IsRuttingFL = false;
            if (EndSkidmarks)
                RutLeft = null;
            if (IsRuttingFL)
            {


                if (!WasRuttingFL)
                {
                    //create a new rut
                    if (RutLeftNodeCount < 50) { GameObject.DestroyImmediate(goRutLeft); Road.Instance.RutCount--; }
                    goRutLeft = new GameObject("RutLeft");
                    goRutLeft.tag = "Rut";
                    Road.Instance.RutCount++;
                    RutLeft = goRutLeft.AddComponent<LineRenderer>();
                    RutLeft.material = RutMatrl;
                    RutLeft.SetWidth(0.2F, 0.2F);
                    RutLeftNodeCount = 0;
                }

                RutLeftNodeCount++;
                RutLeft.SetVertexCount(RutLeftNodeCount);
                RutLeft.SetPosition(RutLeftNodeCount - 1, hitFL.point);
            }
            WasRuttingFL = IsRuttingFL;

            if (RoadMat == "Dirt" && groundedFR && !hitFR.collider.name.Contains("Car") && hitFR.force>3000 && Road.Instance.RutCount<150) IsRuttingFR = true; else IsRuttingFR = false;

            if (EndSkidmarks) RutRight = null;
            if (IsRuttingFR)
            {
                if (!WasRuttingFR)
                {
                    //create a new rut
                    if (RutRightNodeCount < 50) { GameObject.DestroyImmediate(goRutRight); Road.Instance.RutCount--; }
                    goRutRight = new GameObject("RutRight");
                    goRutRight.tag = "Rut";
                    Road.Instance.RutCount++;
                    RutRight = goRutRight.AddComponent<LineRenderer>();
                    RutRight.material = RutMatrl;
                    RutRight.SetWidth(0.2F, 0.2F);
                    RutRightNodeCount = 0;
                }
                RutRightNodeCount++;
                RutRight.SetVertexCount(RutRightNodeCount);
                RutRight.SetPosition(RutRightNodeCount - 1, hitFR.point);
            }
            WasRuttingFR = IsRuttingFR;
        }
        catch (Exception e) {Debug.Log(e.ToString()); }


        //Spray dirt
        try
        {
            if (RoadMat == "Dirt")
            {
                Quaternion qSprayL = Quaternion.Euler(new Vector3(90 - Mathf.Sign(ForwardSlipFL) * 45, 180 + h * 1.5f, 0));
                SprayFL.transform.localRotation = qSprayL;
                if (groundedFL && ((ForwardSlipFL < 0 && h > 0) || (ForwardSlipFL > 0 && h < 0)) && Mathf.Abs(WCFL.rpm) > 0)
                {
                    peSprayFL.Play();
                    peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) * 15;
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                    peSprayFL.emissionRate = (Mathf.Abs(ForwardSlipFL) + Mathf.Abs(SidewaysSlipFL)) * 300;
                }
                else
                {
                    peSprayFL.Stop();
                }

                Quaternion qSprayR = Quaternion.Euler(new Vector3(90 - Mathf.Sign(ForwardSlipFR) * 45, 180 + h * 1.5f, 0));
                SprayFR.transform.localRotation = qSprayR;
                if (groundedFR && ((ForwardSlipFR < 0 && h < 0) || (ForwardSlipFR > 0 && h > 0)) && Mathf.Abs(WCFR.rpm) > 0)
                {
                    peSprayFR.Play();
                    peSprayFR.startSpeed = -Mathf.Abs(ForwardSlipFR) * 15;
                    peSprayFR.emissionRate = (Mathf.Abs(ForwardSlipFR) + Mathf.Abs(SidewaysSlipFR)) * 300;
                }
                else
                {
                    peSprayFR.Stop();
                }
            }
            else { peSprayFR.Stop(); peSprayFL.Stop();}
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        //Skidmarks
        if (EndSkidmarks) { SkidMkLeft = null; SkidMkRight = null; WasSkiddingRL = false; WasSkiddingFR = false;}
        if (IsSkiddingRL)
        {
            if (!WasSkiddingRL)
            {
                //create a new rut
                if (SkidMkLeft != null) SkidMkLeft.enabled = false;
                if (SkidMkLeftNodeCount < 50) { GameObject.DestroyImmediate(goSkidMkLeft); Road.Instance.RutCount--; }
                goSkidMkLeft = new GameObject("SkidMkLeft");
                goSkidMkLeft.tag = "Rut";
                Road.Instance.RutCount++;
                SkidMkLeft = goSkidMkLeft.AddComponent<FlatLineRenderer>();
                SkidMkLeft.Init();
                SkidMkLeft.SetMaterial(SkidMatrl);
                SkidMkLeft.Width=0.1f;
                SkidMkLeftNodeCount = 0;
            }
            SkidMkLeftNodeCount++;
            SkidMkLeft.AddNode(hitRL.point + Vector3.up * 0.05f);
        }
        WasSkiddingRL = IsSkiddingRL;
        if (IsSkiddingFR)
        {
            if (!WasSkiddingFR)
            {
                //create a new rut
                if (SkidMkRightNodeCount < 50) { GameObject.DestroyImmediate(goSkidMkRight); Road.Instance.RutCount--; }
                goSkidMkRight = new GameObject("SkidMkRight");
                goSkidMkRight.tag = "Rut";
                Road.Instance.RutCount++;
                SkidMkRight = goSkidMkRight.AddComponent<FlatLineRenderer>();
                SkidMkRight.Init();
                SkidMkRight.SetMaterial(SkidMatrl);
                SkidMkRight.Width = 0.1f;
                SkidMkRightNodeCount = 0;
            }
            SkidMkRightNodeCount++;
            SkidMkRight.AddNode(hitFR.point + Vector3.up * 0.05f);
        }
        WasSkiddingFR = IsSkiddingFR;
        EndSkidmarks = false;

        //Braking
        if (Braking)
        {
            WCFL.brakeTorque = 700;
            WCFR.brakeTorque = 700;
            WCRL.brakeTorque = 700;
            WCRR.brakeTorque = 700;
        }
        else
        {
            WCFL.brakeTorque = 0;
            WCFR.brakeTorque = 0;
            WCRL.brakeTorque = 0;
            WCRR.brakeTorque = 0;
        }
    }

    public virtual void GetInputFromInputManager()
    {
        //Accel and Brake
        if (GetComponent<Rigidbody>().velocity.magnitude < 30) v = InputManager.ZMovement() * motorForce; else v = 0;
        if (InputManager.Brake()) { Braking = true; } else { Braking = false; }
        //STEERING
        if (InputManager.ToString().Contains("AIInput"))
        {
            h = InputManager.XMovement() / 2;
        }
        else
        {
            h = InputManager.XMovement() * 25 / (1 + GetComponent<Rigidbody>().velocity.magnitude / 20); //The last number: bigger means sharper turns at high speed
        }
        if (h > 0 && h > 40) { h = 40; }
        if (h < -40) { h = -40; }
    }


    void Update()
    {

        //Make the graphic wheels move with the suspension
        RaycastHit hit;

        if (Physics.Raycast(WCFL.transform.position, -WCFL.transform.up, out hit, WCFL.suspensionDistance + WCFL.radius))
        {
            FLWheel.transform.position = hit.point + WCFL.transform.up * WCFL.radius;
        }
        else
        {
            FLWheel.transform.position = WCFL.transform.position - (WCFL.transform.up * WCFL.suspensionDistance);
        }

        if (Physics.Raycast(WCFR.transform.position, -WCFR.transform.up, out hit, WCFR.suspensionDistance + WCFR.radius))
        {
            FRWheel.transform.position = hit.point + WCFR.transform.up * WCFR.radius;
        }
        else
        {
            FRWheel.transform.position = WCFR.transform.position - (WCFR.transform.up * WCFR.suspensionDistance);
        }

        if (Physics.Raycast(WCRL.transform.position, -WCRL.transform.up, out hit, WCRL.suspensionDistance + WCRL.radius))
        {
            RLWheel.transform.position = hit.point + WCRL.transform.up * WCRL.radius;
        }
        else
        {
            RLWheel.transform.position = WCRL.transform.position - (WCRL.transform.up * WCRL.suspensionDistance);
        }

        if (Physics.Raycast(WCRR.transform.position, -WCRR.transform.up, out hit, WCRR.suspensionDistance + WCRR.radius))
        {
            RRWheel.transform.position = hit.point + WCRR.transform.up * WCRR.radius;
        }
        else
        {
            RRWheel.transform.position = WCRR.transform.position - (WCRR.transform.up * WCRR.suspensionDistance);
        }
    }


    public void StartEngine() { }
}




public class CarMenuModelTController : ModelTController
{

    void Start()
    {
        gameObject.AddComponent<CarMenuCarInput>();
        InputManager = gameObject.GetComponent<CarMenuCarInput>();
        //NetworkManager.Instance.OnRPCPositionReceived += new RPCPositionReceivedEventHandler(_nwm_PositionReceived);
    }


}

