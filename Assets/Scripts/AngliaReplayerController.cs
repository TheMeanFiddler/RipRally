using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AngliaReplayerController : MonoBehaviour, iVehicleController
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
    private ParticleSystem peSprayFL;
    private ParticleSystem peSprayFR;
    private ParticleSystem peDustRR;
    private ParticleSystem peDustRL;
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

        peSprayFL = transform.Find("WheelColliders/WCRL/SprayFL").GetComponent<ParticleSystem>();
        peSprayFR = transform.Find("WheelColliders/WCRR/SprayFR").GetComponent<ParticleSystem>();
        peDustRL = transform.Find("WheelColliders/WCRL/DustFL").GetComponent<ParticleSystem>();
        peDustRR = transform.Find("WheelColliders/WCRR/DustFR").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emRL = peDustRL.emission;
        emRL.rate = 160f;
        ParticleSystem.EmissionModule emRR = peDustRR.emission;
        emRR.rate = 160f;

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
        if (peDustRL.isPaused) return;
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
                if (groundedRL && WCFL.angularVelocity != 0)
                {
                    peSprayFL.Play();
                    peSprayFL.startSpeed = -8;
                    // THis was for the dust - peSprayFL.startSpeed = -Mathf.Abs(ForwardSlipFL) / 2;
                    peSprayFL.emissionRate = 150;
                }
                else
                {
                    peSprayFL.Stop();
                    peSprayFL.startSpeed = 0;
                }

                //SprayFR.transform.localRotation = Quaternion.Euler(45, (ForwardSlipRR >= 0 ? 0 : 180), 0);
                if (groundedRR && WCRR.angularVelocity != 0)
                {
                    peSprayFR.Play();
                    peSprayFR.startSpeed = -8;
                    peSprayFR.emissionRate = 150;
                }
                else
                {
                    peSprayFR.Stop();
                    peSprayFR.startSpeed = 0;
                }
            }
            else { peSprayFR.Stop(); peSprayFL.Stop(); peSprayFR.startSpeed = 0; peSprayFL.startSpeed = 0; }
        }
        catch (Exception e) { Debug.Log(e.ToString()); }

        try
        {
            peDustRL.Stop();
            peDustRR.Stop();

            if (RoadMat == "DirtyRoad")
            {
                if (groundedRL)
                {
                    peDustRL.Play();
                }
                if (groundedRR)
                {
                    peDustRR.Play();
                }
                peDustRL.transform.localPosition = new Vector3(0, -0.4f, -WCRL.forwardFriction.slip / 6);
                peDustRR.transform.localPosition = new Vector3(0, -0.4f, -WCRR.forwardFriction.slip / 6);


            }
            else
            {
                peDustRL.Stop();
                peDustRR.Stop();
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
