using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class HotrodReplayerController : MonoBehaviour, iVehicleController
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
    WheelController.WheelHit hitRL = new WheelController.WheelHit();
    WheelController.WheelHit hitRR = new WheelController.WheelHit();
    private byte _gpsTimer = 0;
    private string RoadMat;
    private ParticleSystem psSprayL;
    private ParticleSystem psSprayR;
    private ParticleSystem psDustR;
    private ParticleSystem psDustL;
    private ParticleSystem.MainModule pmSprayL;
    private ParticleSystem.MainModule pmSprayR;
    private ParticleSystem.MainModule pmDustR;
    private ParticleSystem.MainModule pmDustL;
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

        psSprayL = transform.Find("WheelColliders/WCRL/SprayFL").GetComponent<ParticleSystem>();
        psSprayR = transform.Find("WheelColliders/WCRR/SprayFR").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emSprayRL = psSprayL.emission;
        emSprayRL.rateOverTime = 160f;
        ParticleSystem.EmissionModule emSprayRR = psSprayR.emission;
        emSprayRL.rateOverTime = 160f;
        psDustL = transform.Find("WheelColliders/WCRL/DustFL").GetComponent<ParticleSystem>();
        psDustR = transform.Find("WheelColliders/WCRR/DustFR").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emRL = psDustL.emission;
        emRL.rateOverTime = 160f;
        ParticleSystem.EmissionModule emRR = psDustR.emission;
        emRR.rateOverTime = 160f;
        pmSprayL = psSprayL.main;
        pmSprayR = psSprayR.main;
        pmDustL = psDustL.main;
        pmDustR = psDustR.main;

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
        if (psDustL.isPaused) return;
        RoadMat = Gps.RoadMat;
        bool groundedRL = WCRL.isGrounded;
        bool groundedRR = WCRR.isGrounded;

        //Spray dirt
        try
        {
            if (RoadMat == "Dirt")
            {
                //+ve ForwardSlip means spraying backwards  
                //+ve SteerAngle = Right
                //SprayFL.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRL >= 0 ? 0 : 180), 0);
                if (groundedRL && WCRL.angularVelocity != 0)
                {
                    psSprayL.Play();
                    pmSprayL.startSpeed = -8;
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                }
                else
                {
                    psSprayL.Stop();
                    pmSprayL.startSpeed = 0;
                }

                //SprayFR.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRR >= 0 ? 0 : 180), 0);
                if (groundedRR && WCRR.angularVelocity != 0)
                {
                    psSprayR.Play();
                    pmSprayR.startSpeed = -8;
                }
                else
                {
                    psSprayR.Stop();
                    pmSprayR.startSpeed = 0;
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
                if (groundedRL)
                {
                    psDustL.Play();
                }
                if (groundedRR)
                {
                    psDustR.Play();
                }
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
    }

    public void StartEngine()
    {
        throw new NotImplementedException();
    }

    void Destroy()
    {
        Gps = null;
        goCar = null;
    }
}
