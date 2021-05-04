using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Declare the event delegate
public delegate void MainUpdateHandler();
//Declare the event delegate
public delegate void MainFixedUpdateHandler();
//Declare the event delegate
public delegate void MainLevelWasLoadedHandler(int Level);

public interface iMain
{
    event MainUpdateHandler OnUpdate;
    event MainFixedUpdateHandler OnFixedUpdate;
    event MainLevelWasLoadedHandler OnLevelLoaded;
    int SelectedGameID { get; set; }
    string SelectedScene { get; set; }
    string Vehicle { get; set; }
    bool RandomVehicles { get; set; }
    bool Ghost { get; set; }
    string Color { get; set; }
    List<RecordableVehicleSerial> Vehicles { get; set; }
    string LastSelectedCam { get; set; }
    int OpponentCount { get; set; }
    int Laps { get; set; }
    void ShowCoins();
    bool CreateTrackCoroutineFinished { get; set; }
    }

public class Main : Singleton<Main>, iMain
{

    //Declare the event
    public event MainUpdateHandler OnUpdate;
    public event MainFixedUpdateHandler OnFixedUpdate;
    //Declare the event
    public event MainLevelWasLoadedHandler OnLevelLoaded;

    public int SelectedGameID { get; set; }
    public string SelectedScene { get; set; }
    public bool GameLoadedFromFileNeedsDecoding { get; set; }
    private string _vehicle = "Car";
    public string Vehicle { get { return _vehicle; } set { _vehicle = value; } }
    public bool RandomVehicles { get; set; }
    public bool Ghost { get; set; }
    private string _color = "Red";
    public string Color { get { return _color; } set { _color = value; } }
    public List<RecordableVehicleSerial> Vehicles { get; set; }
    public string LastSelectedCam { get; set; }
    public int OpponentCount { get; set; }
    public int Laps { get; set; }
    public AsyncOperation _gao;
    public AsyncOperation mao;
    public bool ReplayButtonNeedsClickingOnceDrivingGUIMenuIsOpen { get; set; }
    UnityEngine.Object objPopupCanvas;
    public bool CreateTrackCoroutineFinished { get; set; }
    public bool GameSceneLoaded { get; set; }
    bool LoadMenuSceneRunning;
    GameObject goTipCanvas;
    MusicPlayer music;

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
        Settings.Instance.LoadFromFile();
        UserDataManager.Instance.LoadFromFile();
        SceneManager.activeSceneChanged += SceneChange;
        objPopupCanvas = Resources.Load("Prefabs/PopupCanvas");
        Vehicles = new List<RecordableVehicleSerial>();
        Shop.FillItems();
        Laps = 3;
        if (Settings.Instance.TutorialIntroHide == false)
        {
            GameObject _canvas = GameObject.Find("MenuCanvas");
            GameObject Tut = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlTutorialIntro1"), new Vector2(500, 500), Quaternion.identity, _canvas.transform);
            Tut.transform.localScale = Vector3.one;
            Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        music = MusicPlayer.Instance;
    }

    /*
    void Start()
    {   //MegaCool Initialisation
        // Receiver's callback for when a share has been opened
        Megacool.OnReceivedShareOpened += (MegacoolEvent megacoolEvent) => {
            // Check if this is a new install
            if (megacoolEvent.FirstSession)
            {
                // Receiver installed the app for the first time
                Debug.Log("This device installed the app.");
            }
        };
        // Sender's callback for when a share has been opened
        Megacool.OnSentShareOpened += (MegacoolEvent megacoolEvent) => {
            // Check if this is a new install
            if (megacoolEvent.FirstSession)
            {
                // Receiver installed the app for the first time
                Debug.Log("Your friend installed the app. Here's your reward!");
            }
        };
        // Initialize the Megacool SDK
        Megacool.Debug = true;
        Megacool.Instance.Start();
        
    }*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (SceneManager.GetActiveScene().name == "TrackSelector") { Application.Quit(); } else GotoHome();
        if (OnUpdate != null)
            OnUpdate();
    }

    void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate();
    }

    void SceneChange(Scene OldScene, Scene NewScene)
    {
        if (OldScene.name == null) return;
        Debug.Log("Main SceneChange from " + OldScene.name + " to " + NewScene.name);
        if (NewScene.name == "SceneSelector") return;
        int Level = NewScene.buildIndex;
        if (Level < 5) return;
        if (OnLevelLoaded != null)
            OnLevelLoaded(Level);

        if (PlayerManager.Type == "CarPlayer")
        {
            if (GameLoadedFromFileNeedsDecoding)
            {
                SaveLoadModel.DecodeGameScene();
                GameLoadedFromFileNeedsDecoding = false;
            }
            PopupTip();
            StartCoroutine(CreateTrackAndRacers());
            
        }

        if (PlayerManager.Type == "Replayer")
        {
            if (GameLoadedFromFileNeedsDecoding)
            { SaveLoadModel.DecodeGameScene(); GameLoadedFromFileNeedsDecoding = false; }
            PopupTip();
            StartCoroutine(CreateTrackAndRacers());
        }

        if (PlayerManager.Type == "BuilderPlayer")
        {
            if (SelectedGameID == -1)
            {
                Game.current = new Game();
                GameData.current = new GameData();
                GameData.current.Scene = NewScene.name;
                SaveLoadModel.Save();
                SaveLoadModel.LoadTerrainBackup(NewScene.name);
                TerrainController.Instance.Init();
                //Clear all the bez points and add the 2 empty ones.
                //We need these, because cos although the Constructor calls Init(), its a singleton class so the constructor only gets called at the beginning
                BezierLine.Instance.Init();

                //Clear all the road sections and add the empty Road Section
                //We need these, because cos although the Constructor calls Init(), its a singleton class so the constructor only gets called at the beginning
                Road.Instance.Init();

                //Clear all the PlaceableObjects
                Scenery.Instance.RemoveObjects();

            }
            else
            {
                SaveLoadModel.LoadGameData(SelectedGameID);
                if (GameLoadedFromFileNeedsDecoding)
                {
                    SaveLoadModel.DecodeGameScene();
                    GameLoadedFromFileNeedsDecoding = false;
                }
            }
            PopupTip();
            StartCoroutine(BPMPlayOfflineCoroutine());//
        }
    }

    IEnumerator BPMPlayOfflineCoroutine()
    {
        CreateTrackCoroutineFinished = false;
        UnityEngine.UI.Text txtProgress = goTipCanvas.transform.Find("txtProgress").GetComponent<UnityEngine.UI.Text>();
        txtProgress.text = "Excavating";
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        TerrainController.Instance.Init();
        txtProgress.text = "Calculating Road";
        yield return new WaitForEndOfFrame();
        Road.Instance.CreateGameObjects();
        txtProgress.text = "Building Road";
        yield return new WaitForEndOfFrame();
        Road.Instance.RenderRoad();
        txtProgress.text = "Adding Scenery";
        yield return new WaitForEndOfFrame();
        Scenery.Instance.PlaceObjects();
        CreateTrackCoroutineFinished = true;
        BuildingPlayManager BPM = new BuildingPlayManager(this, SelectedGameID);//
        BuildingPlayManager.Current = BPM;//
        BPM.PlayOffline();
        Destroy(goTipCanvas);
        yield return 0;
    }

    internal void StartLoadingGameScene(string v)
    {
        StartCoroutine(LoadGameScene(v));
    }

    internal void StartLoadingMenuScene(string v)
    {
        if(LoadMenuSceneRunning==false)
        StartCoroutine(LoadMenuScene(v));
    }

    IEnumerator LoadGameScene(string SceneName)
    {
        _gao = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        _gao.allowSceneActivation = false;
        _gao.priority = 2;
        while (!_gao.isDone)
        {
            if (_gao.progress == 0.9f)
                _gao.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();
        }
        try { GameObject.Find("EventSystem").SetActive(true); } catch { }
        foreach (GameObject rgo in SceneManager.GetSceneByName(SceneName).GetRootGameObjects())
        {
            if (rgo.tag != "Terrain" && rgo.name != "EventSystem" && rgo.name != "EventSystemCreator")
            {
                rgo.SetActive(false);
            }
            if (rgo.tag == "Terrain")
            {
                rgo.GetComponent<Terrain>().enabled = false;
            }
        }
        Debug.Log(SceneName + " Objects Deactivated");
        GameSceneLoaded = true;
        if (PlayerManager.Type == "BuilderPlayer")
        {
            ActivateGameScene();
        }
        yield return 0;

    }

    IEnumerator LoadMenuScene(string SceneName)
    {
        LoadMenuSceneRunning = true;
        GameObject evsys = GameObject.Find("EventSystem");
        string PrevScene = SceneManager.GetActiveScene().name;
        mao = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        mao.allowSceneActivation = false;
        mao.priority = 1;
        while (!mao.isDone)
        {
            if (mao.progress == 0.9f)
                mao.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.MoveGameObjectToScene(evsys, SceneManager.GetSceneByName(SceneName));
        try { SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName)); } catch { }
        SceneManager.UnloadSceneAsync(PrevScene);
        LoadMenuSceneRunning = false;
        yield return null;
    }

    internal void ActivateGameScene()
    {
        GameObject evsys = GameObject.Find("EventSystem");
        foreach (GameObject rgo in SceneManager.GetSceneByName(SelectedScene).GetRootGameObjects())
        {
            if (rgo.tag != "Terrain")
            { rgo.SetActive(true); }

            if (rgo.tag == "Terrain")
            {
                rgo.GetComponent<Terrain>().enabled = true;
            }
        }
        SceneManager.MoveGameObjectToScene(evsys, SceneManager.GetSceneByName(SelectedScene));
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SelectedScene));
    }

    IEnumerator CreateTrackAndRacers()   //moved from DrivingPlayManager constructor
    {
        CreateTrackCoroutineFinished = false;
        UnityEngine.UI.Text txtProgress = goTipCanvas.transform.Find("txtProgress").GetComponent<UnityEngine.UI.Text>();
        txtProgress.text = "Excavating";
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        TerrainController.Instance.Init();
        txtProgress.text = "Calculating Road";
        yield return new WaitForEndOfFrame();
        Road.Instance.CreateGameObjects();
        txtProgress.text = "Building Road";
        yield return new WaitForEndOfFrame();
        Road.Instance.RenderRoad();
        txtProgress.text = "Adding Scenery";
        yield return new WaitForEndOfFrame();
        Scenery.Instance.PlaceObjects();
        CreateTrackCoroutineFinished = true;
        DrivingPlayManager DPM = new DrivingPlayManager(this, SelectedGameID);
        DrivingPlayManager.Current = DPM;
        yield return new WaitForEndOfFrame(); //Put this in so the spectators would find the player
        Destroy(goTipCanvas);
        if (PlayerManager.Type == "CarPlayer")
        {
            DPM.PlayOffline(this.Vehicle, this.Color);
            Race.Current.ReadySteady();
        }
        if (PlayerManager.Type == "Replayer")
        {
            DPM.CreateLiveRacersForReplayerToHide();
            DPM.ShowReplayPanel();
        }
        yield return 0;
    }

    public void GotoHome()
    {
        if (PlayerManager.Type == "CarPlayer")
        {
            if (DrivingPlayManager.Current != null)
            {
                DrivingPlayManager.Current.PlayerCarManager = null;
                DrivingPlayManager.Current.VehicleManagers.Clear();
                DrivingPlayManager.Current.Dispose();
                DrivingPlayManager.Current = null;
            }
            if (Race.Current != null)
            { Race.Current.Dispose(); }
            Race.Current = null;
            RaceRecorder.current = null;
        }
        else
        {
            BuildingPlayManager.Current = null;
        }

        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Machine"))
        {
            GameObject.Destroy(p);
        }
        try
        {
            Camera _followCam = GameObject.Find("FollowCamInner").GetComponent<Camera>();
            _followCam.enabled = true;
        }
        catch { }
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
           GameObject.Destroy(p);
        }

        SceneManager.LoadScene("TrackSelector");
    }

    public void PopupMsg(string msg)
    {
        GameObject goCanvas = (GameObject)GameObject.Instantiate(objPopupCanvas);
        goCanvas.GetComponent<PopupMsg>().Init(msg, new Color(1,1,0), false);
    }

    public void PopupTip()
    {
        goTipCanvas = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TipCanvas"));
    }

    public void PopupMsg(string msg, Color col)
    {
        GameObject goCanvas = (GameObject)GameObject.Instantiate(objPopupCanvas);
        goCanvas.GetComponent<PopupMsg>().Init(msg, col, false);
    }

    public void ShowCoins()
    {
        try
        {
            GameObject go = GameObject.Find("txtCoins");
            GameObject.Find("txtCoins").GetComponent<Text>().text = UserDataManager.Instance.Data.Coins.ToString();
        }
        catch { }
    }

    /*void ShowReplayPanel()
    {
        Transform Canvas = GameObject.FindObjectOfType<Canvas>().transform;
        UnityEngine.Object TransptPrefab = Resources.Load("Prefabs/pnlReplayer");
        GameObject Tr = GameObject.Instantiate(TransptPrefab, Vector3.zero, Quaternion.identity, Canvas) as GameObject;
        Tr.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        Tr.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        Tr.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Tr.transform.localScale = Vector2.one;
    }*/

    public void PlaceMarker(Vector3 pos)
    {
        GameObject rtn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rtn.GetComponent<Collider>().enabled = false;
        rtn.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        rtn.transform.position = pos;
    }
}

/* THings copied from 5.6
 * VehicleManager
 * Main
 * AssetLoader
 * RoadSegment
 * WheelController
 * CarPlayer
 * CarDamageController
 * CarController
 * CarMenuItem - formatting
 * OpponentAI - formatting
 * TransportPanelView
 * ToolboxController - formatting
 * SaveLoadModel
 * SavedGameMenu
 * RoadBuilder
 * InputManager
 * DrivingMenuController
 * CameraSwing - comment
 * BuilderController
 * RaceRecorder

    */
