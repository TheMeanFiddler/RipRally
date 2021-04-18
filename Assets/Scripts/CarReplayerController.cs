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
    private ParticleSystem peSprayFL;
    private ParticleSystem peSprayFR;
    private ParticleSystem peDustFR;
    private ParticleSystem peDustFL;
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

        peSprayFL = transform.Find("WheelColliders/WCFL/SprayFL").GetComponent<ParticleSystem>();
        peSprayFR = transform.Find("WheelColliders/WCFR/SprayFR").GetComponent<ParticleSystem>();
        peDustFL = transform.Find("WheelColliders/WCFL/DustFL").GetComponent<ParticleSystem>();
        peDustFR = transform.Find("WheelColliders/WCFR/DustFR").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emRL = peDustFL.emission;
        emRL.rateOverTime = 160f;
        ParticleSystem.EmissionModule emRR = peDustFR.emission;
        emRR.rateOverTime = 160f;

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
        if (peSprayFL.isPaused) return;
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
                    peSprayFL.Play();
                    peSprayFL.startSpeed = 8;
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                    peSprayFL.emissionRate = 100;
                }
                else
                {
                    peSprayFL.Stop();
                }

                //SprayFR.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRR >= 0 ? 0 : 180), 0);
                if (groundedFR && WCFR.angularVelocity != 0)
                {
                    peSprayFR.Play();
                    peSprayFR.startSpeed = 8;
                    peSprayFR.emissionRate = 100;
                }
                else
                {
                    peSprayFR.Stop();
                    //peSprayFR.startSpeed = 0;
                }
            }
            else { peSprayFR.Stop(); peSprayFL.Stop(); peSprayFR.startSpeed = 0; peSprayFL.startSpeed = 0; }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        try
        {
            peDustFL.Stop();
            peDustFR.Stop();

            if (RoadMat == "DirtyRoad")
            {
                if (groundedFL)
                {
                    peDustFL.Play();
                }
                if (groundedFR)
                {
                    peDustFR.Play();
                }
                peDustFL.transform.localPosition = new Vector3(0, -0.4f, -WCFL.forwardFriction.slip / 6);
                peDustFR.transform.localPosition = new Vector3(0, -0.4f, -WCFR.forwardFriction.slip / 6);


            }
            else
            {
                peDustFL.Stop();
                peDustFR.Stop();
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
