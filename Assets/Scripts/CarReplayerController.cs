using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CarReplayerController : MonoBehaviour, iVehicleController
{

    public GPS Gps { get; set; }
    public iInputManager InputManager { get; set; }
    public float motorForce { get; set; }
    public bool EndSkidmarks { get; set; }
    protected GameObject goCar;
    protected WheelController WCFL;
    protected WheelController WCFR;
    protected WheelController WCRL;
    protected WheelController WCRR;
    WheelController.WheelHit hitFL = new WheelController.WheelHit();
    WheelController.WheelHit hitFR = new WheelController.WheelHit();
    private byte _gpsTimer = 0;
    private string RoadMat;
    private ParticleSystem psSprayL;
    private ParticleSystem psSprayR;
    private ParticleSystem psDustR;
    private ParticleSystem psDustL;
    private ParticleSystem.EmissionModule peSprayL;
    private ParticleSystem.EmissionModule peSprayR;
    private ParticleSystem.EmissionModule peDustL;
    private ParticleSystem.EmissionModule peDustR;
    protected ParticleSystem.MainModule pmSprayL;
    protected ParticleSystem.MainModule pmSprayR;
    protected ParticleSystem.MainModule pmDustL;
    protected ParticleSystem.MainModule pmDustR;
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

    public void Init()
    {
        goCar = this.transform.Find("car").gameObject;
        Gps = new GPS(this.gameObject);
        InputManager = new ReplayerInput();

        WCFL = transform.Find("WheelColliders/WCFL").GetComponent<WheelController>();
        WCFR = transform.Find("WheelColliders/WCFR").GetComponent<WheelController>();
        WCRL = transform.Find("WheelColliders/WCRL").GetComponent<WheelController>();
        WCRR = transform.Find("WheelColliders/WCRR").GetComponent<WheelController>();

        psSprayL = transform.Find("WheelColliders/WCFL/SprayFL").GetComponent<ParticleSystem>();
        psSprayR = transform.Find("WheelColliders/WCFR/SprayFR").GetComponent<ParticleSystem>();
        psDustL = transform.Find("WheelColliders/WCFL/DustFL").GetComponent<ParticleSystem>();
        psDustR = transform.Find("WheelColliders/WCFR/DustFR").GetComponent<ParticleSystem>();
        peSprayL = psSprayL.emission;
        peSprayL = psSprayL.emission;
        peSprayR = psSprayR.emission;
        peDustL = psDustL.emission;
        peDustR = psDustR.emission;
        pmSprayL = psSprayL.main;
        pmSprayR = psSprayR.main;
        pmDustL = psDustL.main;
        pmDustR = psDustR.main;
        peDustL.rateOverTime = 160f;
        peDustR.rateOverTime = 160f;

        WCFL.Replayer = true;
        WCFR.Replayer = true;
        WCRL.Replayer = true;
        WCRR.Replayer = true;

        //Destroy all colliders
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            Destroy(c);
        }
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

        #region Spray and Smoke Particles Region
        if (psSprayL.isPaused) return;
        RoadMat = Gps.RoadMat;
        bool groundedFL = WCFL.isGrounded;
        bool groundedFR = WCFR.isGrounded;

        //Spray dirt
        try
        {
            if (RoadMat == "Dirt")
            {
                //+ve ForwardSlip means spraying backwards  
                //+ve SteerAngle = Right
                //SprayFL.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRL >= 0 ? 0 : 180), 0);
                if (groundedFL && WCFL.angularVelocity != 0)
                {
                    psSprayL.Play();
                    pmSprayL.startSpeed = 8;
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                    peSprayL.rateOverTime = 100;
                }
                else
                {
                    psSprayL.Stop();
                }

                //SprayFR.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRR >= 0 ? 0 : 180), 0);
                if (groundedFR && WCFR.angularVelocity != 0)
                {
                    psSprayR.Play();
                    pmSprayR.startSpeed = 8;
                    peSprayR.rateOverTime = 100;
                }
                else
                {
                    psSprayR.Stop();
                    //peSprayFR.startSpeed = 0;
                }
            }
            else { psSprayR.Stop(); psSprayL.Stop(); pmSprayR.startSpeed = 0; pmSprayL.startSpeed = 0; }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        try
        {
            psDustL.Stop();
            psDustR.Stop();

            if (RoadMat == "DirtyRoad")
            {
                if (groundedFL)
                {
                    psDustL.Play();
                }
                if (groundedFR)
                {
                    psDustR.Play();
                }
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
    }

    public void StartEngine()
    {
        throw new NotImplementedException();
    }

        


}
