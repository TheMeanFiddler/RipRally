using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

//keeps the list of CarManagers
//Has three play functions (PlayOffline, PlayAsServer and PlayAsClient)
//each PlayFunction sets up the CarManagers
//                  creates the Car Gameobjects
//                  Sets up the Race
//                  Sets up the RaceRecorder
//When the scene is loaded it assigns the InputControllers and starts the RaceRecorder.

public delegate void PlayerInstantiateEventHandler(Transform trPlayer); // Consumed by PlayerTracker component

public class DrivingPlayManager: IDisposable
{
    public static DrivingPlayManager Current;
    public List<iVehicleManager> VehicleManagers { get; set; }
    public iVehicleManager PlayerCarManager { get; set; }
    public iVehicleManager GhostCarManager { get; set; }
    public iRaceRecorder Recorder { get; set; }
    private iMain _MainScript;
    private GameObject DrivingGUICanvas;
    private GameObject InputGUI;
    private int SelectedGameID;
    public bool playingOffline { get; set; }
    public event PlayerInstantiateEventHandler OnPlayerInstantate;

    public DrivingPlayManager()
    {
        _MainScript = Main.Instance;
        VehicleManagers = new List<iVehicleManager>();
        _MainScript.OnFixedUpdate += MainScript_OnFixedUpdate;
        AddSkidMarks();
    }

    public DrivingPlayManager(iMain MainScript, int _selectedGameId)
    {
        VehicleManagers = new List<iVehicleManager>();
        _MainScript = MainScript;
        SelectedGameID = _selectedGameId;
        MainScript.OnFixedUpdate += MainScript_OnFixedUpdate;
        MainScript.OnLevelLoaded += MainScript_OnLevelLoaded;
        AddSkidMarks();
        //_sql = new SQLHelper();
        //_sql.RunSQL("Delete From tblInput WHERE Event > 0");
    }
    
    void AddSkidMarks()
    {
        
        for (int sm = 0; sm < 20; sm++)
        {
            GameObject gosm = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/SkidMk"));
            gosm.name = "SkdMk" + sm.ToString();
            Road.Instance.SkidMks.Enqueue(gosm.GetComponent<FlatLineRenderer>());
        }
    }

    void RemoveSkidMarks()
    {
        while (Road.Instance.SkidMks.Count > 0)
        {
            FlatLineRenderer flr = Road.Instance.SkidMks.Dequeue();
            GameObject.Destroy(flr.gameObject);
        }
    }

    void MainScript_OnLevelLoaded(int Level)
    {
        _MainScript.OnLevelLoaded -= MainScript_OnLevelLoaded;
    }

    public void MainScript_OnFixedUpdate()
    {
        if (PlayerCarManager != null)
        {
            if (playingOffline)
            {
                if (Race.Current.Started)
                {
                    if (PlayerCarManager.VehicleController.InputManager != null)
                    {
                        Race.Current.Update();
                        RaceRecorder.current.RecFrame();
                    }
                }
            }
        }
    }


    public void PlayOffline(string Vehicle, string color)
    {
        List<String> AvailableColors = new List<String> { "Red", "Pink", "Purp", "Black", "Blue", "Yellow", "Green" };
        VehicleManagers.Clear();
        playingOffline = true;
        if(GameData.current.Scene=="Desert")
            RenderSettings.fog = false;
        else
            RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.015f;
        //if (Race.Current != null) Debug.Log("Need to RaceCurrentDispose"); Havent found a problem here yet
        Race.Current = new Race();
        Race.Current.Laps = _MainScript.Laps;

        PlayerCarManager = new VehicleManager(0, CarManagerType.Player, Vehicle, color);
        VehicleManagers.Add(PlayerCarManager);
        AvailableColors.Remove(color);
        if(!GameObject.Find("DrivingGUICanvas(Clone)"))
        DrivingGUICanvas = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/DrivingGUICanvas"), new Vector3(354f, 183f, 0), Quaternion.identity);
        Transform trCanvas = DrivingGUICanvas.transform;
        trCanvas.Find("pnlDrivingMenu/Speedo").GetComponent<SpeedoController>().Init();
        InputGUI = GameObject.Find("ControlCanvas");
        SendPlayerInstantiateEvent(PlayerCarManager.goCar.transform); //this event is consumed by scenery that Tracks the player car like the Spectator and the pheasant

        for (int Opp = 0; Opp < _MainScript.OpponentCount; Opp++)
        {
            string _veh = "Car";
            if (_MainScript.RandomVehicles == false) { _veh = Vehicle; }
            else
            {
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0: break;
                    case 1: _veh = "Hotrod"; break;
                    case 2: _veh = "Anglia"; break;
                }
            }
            int Rnd = UnityEngine.Random.Range(0, AvailableColors.Count);
            String _color = AvailableColors[Rnd];
            iVehicleManager AICarManager;
            AICarManager = new VehicleManager((byte)(Opp+1), CarManagerType.AI, _veh, _color);
            AvailableColors.Remove(_color);
            VehicleManagers.Add(AICarManager);

        }
        if (_MainScript.Ghost)
        {
            GhostCarManager = new VehicleManager(99, CarManagerType.Ghost, "Ghost", "G");
            //if(File.Exists(Application.persistentDataPath + "/" + Game.current.Filename + ".gst"))
        }

        Race.Current.ArrangeGrid();
        CamSelector.Instance.Init();
        RaceRecorder.current = new RaceRecorder(VehicleManagers);
        Recrdng.Current = new Recrdng();
        RaceRecorder.current.State = "Recording";


    }

    /// <summary>
    /// Creates the VehicleManagers ready to drive. The actual vehicles are hidden and then cloned as CarManagerType.Replayer
    /// </summary>
    /// <param name="Vehicle"></param>
    /// <param name="color"></param>
    public void CreateLiveRacersForReplayerToHide()
    {
        Debug.Log("DrivingPlayManager Create Hidden Racers For Replay");
        VehicleManagers.Clear();
        playingOffline = true;
        if(GameData.current.Scene=="Desert")
            RenderSettings.fog = false;
        else
            RenderSettings.fog = true;
        RenderSettings.fogDensity = 0.015f;

        Race.Current = new Race();
        Race.Current.Laps = 3;

        //OK we have to create these but the vehicle get hidden and then cloned as CarManagerType.Replayer
        PlayerCarManager = new VehicleManager(0, CarManagerType.Player, Main.Instance.Vehicles[0].Vehicle, Main.Instance.Vehicles[0].Color);
        VehicleManagers.Add(PlayerCarManager);
        if(!GameObject.Find("DrivingGUICanvas(Clone)"))
        DrivingGUICanvas = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/DrivingGUICanvas"), new Vector3(354f, 183f, 0), Quaternion.identity);
        DrivingGUICanvas.GetComponent<DrivingMenuController>().ShowPanel(false);
        for (int Opp = 1; Opp < Main.Instance.Vehicles.Count; Opp++)
        {
            iVehicleManager AICarManager;
            AICarManager = new VehicleManager((byte)Opp, CarManagerType.AI, Main.Instance.Vehicles[Opp].Vehicle, Main.Instance.Vehicles[Opp].Color);
            VehicleManagers.Add(AICarManager);
        }

        Race.Current.ArrangeGrid();
        CamSelector.Instance.Init();
        RaceRecorder.current = new RaceRecorder(VehicleManagers);
        Recrdng.Current.Recordables = VehicleManagers.Select(vm => new RecordableVehicle(vm)).ToList();
        RaceRecorder.current.State = "Stopped";
        PauseCars();
    }

    public void PlayAsServer(string Vehicle, string color)
    {

    }

    public void ShowReplayPanel()
    {
        Transform Canvas = DrivingGUICanvas.transform;
        UnityEngine.Object TransptPrefab = Resources.Load("Prefabs/pnlReplayer");
        GameObject Tr = GameObject.Instantiate(TransptPrefab) as GameObject;
        Tr.transform.SetParent(Canvas, false);
        Tr.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        Tr.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        Tr.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Tr.transform.localScale = Vector2.one;
    }


    public void FreezeCars()
    {
        foreach (iVehicleManager CM in VehicleManagers)
        {
            CM.Gps.RecoveryAllowed = false;
            FreezeCar(CM);
        }
    }

    public void FreezeCar(iVehicleManager cm)
    {
        cm.goCar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
    }

    public void UnfreezeCars()
    {
        foreach (iVehicleManager CM in VehicleManagers)
        {
            CM.Gps.RecoveryAllowed = true;
            CM.goCar.SetActive(true);
            CM.goCar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    public void PauseCars()
    {
        Race.Current.Pause();
        Recrdng.Current.Pause();
        foreach (iVehicleManager VM in VehicleManagers)
        {
            VM.PauseCar();
            FreezeCar(VM);
        }
    }

    public void UnpauseCars()
    {
        Race.Current.Resume();
        Recrdng.Current.Resume();
        foreach (iVehicleManager VM in VehicleManagers)
        {
            VM.goCar.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            VM.UnpauseCar();
        }
        SendPlayerInstantiateEvent(PlayerCarManager.goCar.transform);
    }

    public void RespawnCars()
    {
        foreach(iRacer r in Race.Current.Racers)
        {
            r.Dispose();
        }
        Race.Current.Racers.Clear();
        //RaceRecorder.current.Recordables.Clear();
        foreach (iVehicleManager VM in VehicleManagers)
        {
            VM.DestroyVehicle();
            VM.CreateVehicle("Vehicle" + VM.VId.ToString(), VM.ManagerType, VM.Vehicle, VM.Color);
        }
        SendPlayerInstantiateEvent(PlayerCarManager.goCar.transform);
        if (!GameObject.Find("DrivingGUICanvas(Clone)"))
        DrivingGUICanvas = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/DrivingGUICanvas"), new Vector3(354f, 183f, 0), Quaternion.identity);
        Transform trCanvas = DrivingGUICanvas.transform;
        trCanvas.Find("pnlDrivingMenu/Speedo").GetComponent<SpeedoController>().Init();
        Race.Current.AirBonus = 0;
        Race.Current.DriftBonus = 0;
        Race.Current.HogBonus = 0;
        Race.Current.ArrangeGrid();
        Recrdng.Current.Data.Clear();
        Recrdng.Current = null;
        Recrdng.Current = new Recrdng();
    }

    public void ShowDrivingGUI(bool show)
    {
        DrivingGUICanvas.SetActive(show);
    }

    public void ShowInputGUI(bool show)
    {
        InputGUI.SetActive(show);
    }

    public void SendPlayerInstantiateEvent(Transform trplyr)
    {
        if (OnPlayerInstantate != null)
        { OnPlayerInstantate(trplyr); } //this event is consumed by scenery that tracks the player like the Spectator and the pheasant
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        RemoveSkidMarks();
        GameObject.Destroy(GameObject.Find("DrivingGUICanvas(Clone)"));
        _MainScript.OnUpdate -= MainScript_OnFixedUpdate;
        _MainScript.OnLevelLoaded -= MainScript_OnLevelLoaded;  //should have removed itself when it ran
        Race.Current.Started = false;
        Recrdng.Current.Data.Clear();
        Recrdng.Current = null;
        Race.Current.Dispose();
        Race.Current = null;
        RaceRecorder.current = null;
        PlayerCarManager = null;
        GhostCarManager = null;
        System.GC.Collect();
        //_sql.Dispose();
    }
}

