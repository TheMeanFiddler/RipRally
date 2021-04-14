using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CarManagerType { AI, Player, ClientSideClient, ServerSideClient, Slave, Replayer, Ghost }
//ON THE CLIENT SIDE:
//ClientSideClient sends its input to the RPC and receives its position from the RPC
//Slave receives its input and position from the server
//ON THE SERVER SIDE
//ServerSideClient Receives input from the RPC and sends its position to the RPC
//AI and Player send their input and postion to the RPC

public interface iVehicleManager
{
    GameObject goCar { get; set; }
    string Color { get; set; }
    iVehicleController VehicleController { get; set; }
    iRacer Racer { get; set; }
    GPS Gps { get; set; }
    DamageController DmgCtlr { get; set; }
    CarManagerType ManagerType { get; set; }
    string Vehicle { get; set; }
    void SetPos(Vector3 pos);
    void SetRot(Quaternion rot);
    void SetGameObjectName(string n);
    void Repair();
    void CreateVehicle(string name, CarManagerType Type, string Vehicle, string color);
    void DestroyVehicle();
    byte VId { get; set; }
    void PauseCar();
    void UnpauseCar();
    void Recover();
    void DontRecover();
}



public class VehicleManager : iVehicleManager
{
    public GameObject goCar { get; set; }
    public GPS Gps { get; set; }
    public string Color { get; set; }
    public iVehicleController VehicleController { get; set; }
    public CarManagerType ManagerType { get; set; }
    public string Vehicle { get; set; }
    public iRacer Racer { get; set; }
    public DamageController DmgCtlr { get; set; }
    public byte VId { get; set; }
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    Vector3 _pausedPos;
    Quaternion _pausedRot;
    Vector3 _pausedVel;
    Vector3 _pausedAngVel;

    //Constructor
    public VehicleManager(byte vId, CarManagerType Type, string vehicle, string color)  //can be "Player" or "AI" or "ClientSideClient" or "ServerSideClient" or "Slave"
    {
        VId = vId;
        ManagerType = Type;
        Vehicle = vehicle;
        Color = color;

        CreateVehicle("Vehicle"+vId.ToString(), Type, vehicle, color);
    }

    public void CreateVehicle(string name, CarManagerType Type, string Vehicle, string color)
    {

        if (Type == CarManagerType.Player)
        {
            UnityEngine.Object Plyr = Resources.Load("Prefabs/" + Vehicle + "Player");
            goCar = (GameObject)GameObject.Instantiate(Plyr, new Vector3(0, 12.0f, 0), Quaternion.identity);
            goCar.tag = "Player";
            goCar.name = name;

            if (Vehicle == "Car")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<CarDamageController>();
                VehicleController = goCar.AddComponent<CarPlayerController>();
            }
            if (Vehicle == "Hotrod")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<HotrodDamageController>();
                VehicleController = goCar.AddComponent<HotrodPlayerController>();
            }
            if (Vehicle == "Anglia")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<AngliaDamageController>();
                VehicleController = goCar.AddComponent<AngliaPlayerController>();
            }
            if (Vehicle == "ModelT")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<ModelTDamageController>();
                VehicleController = goCar.AddComponent<ModelTPlayerController>();
            }
            if (Vehicle == "Porsche")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<PorscheDamageController>();
                VehicleController = goCar.AddComponent<PorschePlayerController>();
            }
            DmgCtlr.SetColor(Vehicle + "_" + color, true);
            DmgCtlr.AttachHingesAndJoints();
            VehicleController.Init();
            DmgCtlr.Gps = VehicleController.Gps;
            Racer = new Racer(this, DmgCtlr, Race.Current, false);
            Race.Current.Racers.Add(Racer);
      }

        if (Type == CarManagerType.AI)
        {
            UnityEngine.Object Plyr = Resources.Load("Prefabs/" + Vehicle + "Player");
            goCar = (GameObject)GameObject.Instantiate(Plyr, new Vector3(0, 12.0f, 0), Quaternion.identity);
            goCar.tag = "Machine";
            goCar.name = name;

            if (Vehicle == "Car")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<CarDamageController>();
                VehicleController = goCar.AddComponent<MachineCarController>();
            }
            if (Vehicle == "Hotrod")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<HotrodDamageController>();
                VehicleController = goCar.AddComponent<HotrodAIController>();
            }
            if (Vehicle == "Anglia")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<AngliaDamageController>();
                VehicleController = goCar.AddComponent<AngliaAIController>();
            }
            if (Vehicle == "ModelT")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<ModelTDamageController>();
                VehicleController = goCar.AddComponent<ModelTAIController>();
            }
            if (Vehicle == "Porsche")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<PorscheDamageController>();
                VehicleController = goCar.AddComponent<PorscheAIController>();
            }
            DmgCtlr.AttachHingesAndJoints();
            DmgCtlr.SetColor(Vehicle + "_" + color);
            VehicleController.Init();
            //Todo: Whee's the GPS?
            Racer = new Racer(this, DmgCtlr, Race.Current, true);
            Race.Current.Racers.Add(Racer);
        }

        if (Type == CarManagerType.Replayer)
        {
            UnityEngine.Object Plyr = Resources.Load("Prefabs/" + Vehicle + "Player");
            goCar = (GameObject)GameObject.Instantiate(Plyr, new Vector3(0, 12.0f, 0), Quaternion.identity);
            goCar.tag = "Player";
            goCar.name = name;
            if (Vehicle == "Car")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<CarDamageController>();
                VehicleController = goCar.AddComponent<CarReplayerController>();
            }
            if (Vehicle == "ModelT")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<ModelTDamageController>();
                VehicleController = goCar.AddComponent<ModelTReplayerController>();
            }
            if (Vehicle == "Hotrod")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<HotrodDamageController>();
                VehicleController = goCar.AddComponent<HotrodReplayerController>();
            }
            if (Vehicle == "Anglia")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<AngliaDamageController>();
                VehicleController = goCar.AddComponent<AngliaReplayerController>();
            }
            if (Vehicle == "Porsche")
            {
                DmgCtlr = (DamageController)goCar.AddComponent<PorscheDamageController>();
                VehicleController = goCar.AddComponent<PorscheReplayerController>();
            }
            DmgCtlr.SetColor(Vehicle + "_" + color, true);
            DmgCtlr.AttachHingesAndJoints();
            VehicleController.Init(); //Creates a new GPS, Creates a new ReplayerInput
            goCar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

        }

        if (Type == CarManagerType.Ghost)
        {
            UnityEngine.Object Plyr = Resources.Load("Prefabs/GhostPlayer");
            goCar = (GameObject)GameObject.Instantiate(Plyr, new Vector3(0, 12.0f, 0), Quaternion.identity);
            goCar.name = name;

            VehicleController = goCar.AddComponent<GhostController>();
            VehicleController.Init();
        }
        else  //everything but the ghost has a Gps
        {
            DmgCtlr.VId = VId;
            Gps = VehicleController.Gps;
        }
    }

    public void SetPos(Vector3 pos)
    {
        goCar.transform.position = pos;
    }

    public void SetRot(Quaternion rot)
    {
        goCar.transform.rotation = rot;
    }

    public void SetGameObjectName(string n)
    {
        goCar.name = n;
    }

    public void Repair()
    {
        DmgCtlr.RepairAll();
    }

    public void PauseCar()
    {
        _pausedPos = goCar.transform.position;
        _pausedRot = goCar.transform.rotation;
        _pausedVel = goCar.GetComponent<Rigidbody>().velocity;
        _pausedAngVel = goCar.GetComponent<Rigidbody>().angularVelocity;
    }

    public void UnpauseCar()
    {
        goCar.transform.position = _pausedPos;
        goCar.transform.rotation = _pausedRot;
        goCar.GetComponent<Rigidbody>().velocity = _pausedVel;
        goCar.GetComponent<Rigidbody>().angularVelocity = _pausedAngVel;
    }

    public void DestroyVehicle()
    {
        if (ManagerType == CarManagerType.AI || ManagerType == CarManagerType.Player || ManagerType == CarManagerType.Replayer)
        {
            VehicleController.InputManager.Dispose();
            VehicleController.InputManager = null;
            VehicleController.Gps = null;
            DmgCtlr.Gps = null;
            DmgCtlr = null;
            Gps = null;
            Racer = null;
            GameObject.Destroy(goCar);
            goCar = null;
        }
    }

    public void Recover()
    {
        int RecoverySegIdx = Gps.CurrSegIdx;
        bool MovedBack = false;
        while (Road.Instance.Segments[RecoverySegIdx].roadMaterial == "Air")
        {
            MovedBack = true;
            RecoverySegIdx -= 1;
            if (RecoverySegIdx < 0) RecoverySegIdx = Road.Instance.Segments.Count - 1;
        }
        if (MovedBack) {
            RecoverySegIdx -= 40;
            if (RecoverySegIdx < 0) RecoverySegIdx = Road.Instance.Segments.Count + RecoverySegIdx;
        }
        goCar.transform.position = Road.Instance.XSecs[RecoverySegIdx].MidPt + Vector3.up;

           
        goCar.transform.rotation = Quaternion.LookRotation(Road.Instance.XSecs[Gps.CurrSegIdx].Forward);
        Gps.RecoveryAllowed = true;
    }

    public void DontRecover()
    {
        Gps.SegTime = Time.time;
        Gps.RecoveryAllowed = true;
    }
}


