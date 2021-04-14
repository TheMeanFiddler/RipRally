using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using NatCorder.Examples;
using System.IO;



public class Replayer : MonoBehaviour
{
    public List<iVehicleManager> ReplayerVehicleManagers { get; set; }
    public iVehicleManager PlayerCarManager { get; set; }
    public int CurrFrame { get; set; }
    public string State { get; set; }
    GameObject ReplayCamController;
    ReplayCamControllerFactory CCF;
    private iReplayerGameObjectController GameObjectController;
    public iRecrdng Recording { get; set; }
    public int StartFrame { get; set; }
    public int EndFrame { get; set; }
    private BoxCollider2D cldrTimeline;
    private BoxCollider2D cldrStartFrame;
    private BoxCollider2D cldrEndFrame;
    private Transform trControls;
    private RectTransform Timeline;
    private RectTransform CurrFrameMkr;
    private RectTransform StartFrameMkr;
    private RectTransform EndFrameMkr;
    private float TimelineFramesPerPx;
    private int FrameCount;
    private byte SloMoCounter = 0;
    GameObject pnlSave;
    GameObject pnlBuyCameras;
    GameObject btnShare;
    GameObject pnlDirector;
    GameObject pnlCamTimeline;
    bool ReopenDrivingMenuOnDestroy = true;
    Canvas _canvas;
    ScreenRecorder _screenRec;
    string ReplayFilename;

    void Awake()
    {
        _screenRec = GetComponentInChildren<ScreenRecorder>();
        _screenRec.OnStopRec += _replayCam_OnStopRec;
    }

    void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
        Recording = Recrdng.Current;
        State = "Stopped";
        StartFrame = 0;
        CurrFrame = Recording.FrameCount - 1;
        FrameCount = Recording.FrameCount;
        EndFrame = Recording.FrameCount - 1;
        Recording.EndFrame = EndFrame;
        //Cars are already paused when you hit pause button. The race is paused too
        CreateReplayerVehicleManagersFromLiveOnes();
        try
        {
            CamSelector.Instance.ActiveCam.GetComponent<CamController_Follow_Swing>().enabled = false;
        }
        catch { };
        try
        {
            CamSelector.Instance.ActiveCam.GetComponent<CamController_Follow_Fly>().enabled = false;
        }
        catch { };
        CamSelector.Instance.ActiveCam.GetComponentInChildren<Camera>().enabled = false;

        ReplayCamController = new GameObject();
        ReplayCamController.name = "ReplayCamController";
        CCF = ReplayCamController.AddComponent<ReplayCamControllerFactory>();
        RaceRecorder.current.State = "Paused";
        GameObjectController = new ReplayerGameObjectController(ReplayerVehicleManagers, this);
        if (PlayerManager.Type == "Replayer") { RewToStart(); }
        trControls = transform.Find("Controls");
        CurrFrameMkr = (RectTransform)transform.Find("Controls/Timeline/btnCurrFrame");
        StartFrameMkr = (RectTransform)transform.Find("Controls/Timeline/btnStartFrame");
        EndFrameMkr = (RectTransform)transform.Find("Controls/Timeline/btnEndFrame");
        Timeline = (RectTransform)transform.Find("Controls/Timeline");
        cldrTimeline = Timeline.GetComponent<BoxCollider2D>();
        cldrStartFrame = StartFrameMkr.GetComponent<BoxCollider2D>();
        cldrEndFrame = EndFrameMkr.GetComponent<BoxCollider2D>();
        TimelineFramesPerPx = (float)(Recording.FrameCount) / (Screen.width/2);
        pnlSave = transform.Find("pnlSave").gameObject;
        pnlBuyCameras = transform.Find("pnlBuyCameras").gameObject;
        //RectTransform rtb = pnlBuyCameras.GetComponent<RectTransform>();
        //rtb.anchoredPosition = new Vector2(0, -400);
        btnShare = transform.Find("Controls/btnShareVideo").gameObject;
        btnShare.SetActive(false);
        pnlDirector = transform.transform.Find("pnlCameraDirector").gameObject;
        pnlCamTimeline = transform.transform.Find("pnlCamTimeline").gameObject;
        //pnlDirector.SetActive(false);
        PlayerManager.Type = "Replayer";

        //If they've not saved any replays show the tutorial
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        if (dir.GetFiles("*.rcd").Length < 3)
        {
            GameObject.Instantiate(Resources.Load("Prefabs/pnlTutorialReplayer"), transform.parent);
        }
        
    }

    private void CreateReplayerVehicleManagersFromLiveOnes()
    {
        //Now make a deep copy of the VehicleManagers and hide the drivingplay Vehicles
        ReplayerVehicleManagers = new List<iVehicleManager>();
        foreach (iVehicleManager VM in DrivingPlayManager.Current.VehicleManagers)
        {
            iVehicleManager RVM = new VehicleManager(VM.VId, CarManagerType.Replayer, VM.Vehicle, VM.Color);
            ReplayerVehicleManagers.Add(RVM);
            if (VM == DrivingPlayManager.Current.PlayerCarManager) this.PlayerCarManager = RVM;
            RVM.SetPos(VM.goCar.transform.position);
            RVM.SetRot(VM.goCar.transform.rotation);
            //RigidBody is frozen when created
            VM.goCar.SetActive(false);
        }
        DrivingPlayManager.Current.SendPlayerInstantiateEvent(PlayerCarManager.goCar.transform);
    }

    #region Timeline Control
    float _touchx;
    int DraggingStartMarker;
    int DraggingEndMarker;
    float DragOffset;
    void Update()
    {
        _touchx = -100;
        foreach(Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                if (cldrStartFrame.OverlapPoint(t.position))
                {
                    DraggingStartMarker = t.fingerId;
                    DragOffset = t.position.x - StartFrameMkr.position.x;
                }
                else if (cldrEndFrame.OverlapPoint(t.position))
                {
                    DraggingEndMarker = t.fingerId;
                    DragOffset = t.position.x - EndFrameMkr.position.x;
                }
                else { DraggingStartMarker = -1; DraggingEndMarker = -1; }
            }
            if (t.phase == TouchPhase.Moved)
            {
                if (DraggingStartMarker == t.fingerId)
                {
                    float newPos = t.position.x - DragOffset;
                    if (newPos < EndFrameMkr.position.x - 10 && newPos <= CurrFrameMkr.position.x)
                        StartFrameMkr.position = new Vector3(newPos, StartFrameMkr.position.y, 0);
                }
                if (DraggingEndMarker == t.fingerId)
                {
                    float newPos = t.position.x - DragOffset;
                    if (newPos > StartFrameMkr.position.x + 10 && newPos >= CurrFrameMkr.position.x)
                        EndFrameMkr.position = new Vector3(newPos, EndFrameMkr.position.y, 0);
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                if (DraggingStartMarker == t.fingerId)
                {
                    DraggingStartMarker = -1;
                    DraggingEndMarker = -1;
                    StartFrame = Mathf.RoundToInt((StartFrameMkr.position.x - Screen.width / 4) * TimelineFramesPerPx);
                    Recrdng.Current.StartFrame = StartFrame;
                }
                else if (DraggingEndMarker == t.fingerId)
                {
                    DraggingStartMarker = -1;
                    DraggingEndMarker = -1;
                    EndFrame = Mathf.RoundToInt((EndFrameMkr.position.x - Screen.width / 4) * TimelineFramesPerPx);
                    Recrdng.Current.EndFrame = EndFrame;
                }
                else if (cldrTimeline.OverlapPoint(t.position))
                {
                    _touchx = t.position.x - Screen.width / 4;
                    CurrFrame = Mathf.RoundToInt(_touchx * TimelineFramesPerPx);
                    CCF.ChooseFollow();
                }
            }
        }
#if UNITY_EDITOR
        if (cldrStartFrame.bounds.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0))
        {
            DraggingStartMarker = 100;
            DragOffset = Input.mousePosition.x - StartFrameMkr.position.x;
        }

        if (cldrEndFrame.bounds.Contains(Input.mousePosition) && Input.GetMouseButtonDown(0))
        {
            DraggingEndMarker = 100;
            DragOffset = Input.mousePosition.x - EndFrameMkr.position.x;
        }

        if (DraggingStartMarker == 100 )
        {
            float newPos = Input.mousePosition.x - DragOffset;
            if(newPos < EndFrameMkr.position.x - 10 && newPos <= CurrFrameMkr.position.x)
            StartFrameMkr.position = new Vector3(newPos, StartFrameMkr.position.y, 0);
        }
        if (DraggingEndMarker == 100 )
        {
            float newPos = Input.mousePosition.x - DragOffset;
            if(newPos > StartFrameMkr.position.x + 10 && newPos >= CurrFrameMkr.position.x)
            EndFrameMkr.position = new Vector3(newPos, EndFrameMkr.position.y, 0);
        }

        if (DraggingStartMarker == 100 && Input.GetMouseButtonUp(0))
        {
            DraggingStartMarker = -1;
            StartFrame = Mathf.RoundToInt((StartFrameMkr.position.x - Screen.width / 4) * TimelineFramesPerPx);
            Recrdng.Current.StartFrame = StartFrame;
        }
        else if (DraggingEndMarker == 100 && Input.GetMouseButtonUp(0))
        {
            DraggingEndMarker = -1;
            EndFrame = Mathf.RoundToInt((EndFrameMkr.position.x - Screen.width / 4) * TimelineFramesPerPx);
            Recrdng.Current.EndFrame = EndFrame;
        }
        else if (cldrTimeline.bounds.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            _touchx = Input.mousePosition.x - Screen.width / 4;
            CurrFrame = Mathf.RoundToInt(_touchx * TimelineFramesPerPx);
            CCF.ChooseFollow();
        }
#endif            

    }
    #endregion

    public int GetFrameFromxPos(int xPos)
    {
        return Mathf.RoundToInt((xPos - Timeline.rect.xMin) * (float)Recording.FrameCount/Timeline.rect.width);
    }

    public float GetxPosFromFrame(int frame)
    {
        return frame * Timeline.rect.width / Recording.FrameCount + Timeline.rect.xMin;
    }

    #region Transport Buttons
    public void Play(bool IsOn)
    {
        if (IsOn)
        {
            DeleteReplayFile();
            State = "Playing";
            Recording.CurrFrame = CurrFrame;
            //EndFrame = Recording.FrameCount - 1;
            pnlBuyCameras.SetActive(false);
            btnShare.SetActive(false);
            //pnlDirector.SetActive(true);
            pnlCamTimeline.SetActive(false);
            _screenRec.StartRecording();
        }
        else
        {
            State = "Stopped";
            pnlBuyCameras.SetActive(true);
            btnShare.SetActive(true);
            //pnlDirector.SetActive(false);
            pnlCamTimeline.SetActive(true);
            _screenRec.StopRecording();
            CCF.ChooseFollow();
        }
    }

    public void RewToStart()
    {
        State = "Stopped";
        CurrFrame = StartFrame;
        Recording.CurrFrame = CurrFrame;
        Road.Instance.RemoveRuts();
        RespawnCars();
        DrivingPlayManager.Current.FreezeCars();
        iVehicleManager CM = PlayerCarManager;
        DrivingPlayManager.Current.SendPlayerInstantiateEvent(CM.goCar.transform);
        CCF.Car = CM.goCar.transform;
        GameObjectController.DrawFrame(Recording.Data[CurrFrame]);
        CCF.ChooseFollow();
        try {pnlBuyCameras.SetActive(true); } catch { }
        //_replayCam.StopRecording();
    }



    public void PlaySloMo(bool IsOn)
    {
        if (IsOn)
        {
            CurrFrame = Recording.CurrFrame;
            State = "PlayingSloMo";
            //EndFrame = Recording.FrameCount - 1;
            pnlBuyCameras.SetActive(false);
            btnShare.SetActive(false);
            //pnlDirector.SetActive(true);
        }
        else
        {
            State = "Stopped";
            pnlBuyCameras.SetActive(true);
            btnShare.SetActive(true);
            pnlDirector.SetActive(false);
            CCF.ChooseFollow();
        }
    }


    public void Rewind(bool IsOn)
    {
        if (IsOn)
        {
            State = "Rewind";
            CurrFrame = Recording.CurrFrame;
            CCF.ChooseFollow();
            pnlBuyCameras.SetActive(false);
            btnShare.SetActive(false);
            //pnlDirector.SetActive(true);
        }
    }

    public void Reverse(bool IsOn)
    {
        if (IsOn)
        {
            State = "Reverse";
            CurrFrame = Recording.CurrFrame;
            CCF.ChooseFollow();
            pnlBuyCameras.SetActive(false);
            btnShare.SetActive(false);
            //pnlDirector.SetActive(true);
        }
    }

    public void Freeze(bool IsOn)
    {
        if (IsOn)
        {
            State = "Stopped";
            foreach (iVehicleManager vm in ReplayerVehicleManagers)
            {
                vm.DmgCtlr.FreezeDetatchedParts();
            }
        }
        else
        {
            State = "Playing";
            foreach (iVehicleManager vm in ReplayerVehicleManagers)
            {
                vm.DmgCtlr.UnfreezeDetatchedParts();
            }
        }
    }

    #endregion

    public void Save()
    {
        pnlSave.SetActive(true);
        pnlBuyCameras.SetActive(false);
        btnShare.SetActive(false);
        pnlSave.GetComponentInChildren<RecordingPanel>().Init(Game.current);
        pnlSave.transform.SetParent(this.transform.parent);
        ReopenDrivingMenuOnDestroy = false;
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (State == "Playing")
        {
            GameObjectController.DrawFrame(Recording.Data[CurrFrame]);
            if (CurrFrame == EndFrame) State = "Stopped";
            else CurrFrame++;
            Recording.CurrFrame = CurrFrame;
        }

        if (State == "PlayingSloMo")
        {
            if (SloMoCounter == 0)
            {
                SloMoCounter = 3;
                GameObjectController.DrawFrame(Recording.Data[CurrFrame]);
                if (CurrFrame == EndFrame) State = "Stopped";
                else CurrFrame++;
                Recording.CurrFrame = CurrFrame;
            }
            SloMoCounter--; 
        }

        if (State == "Reverse")
        {
            GameObjectController.DrawFrame(Recording.Data[CurrFrame]);
            if (CurrFrame == 0) State = "Stopped"; else CurrFrame--;
            Recording.CurrFrame = CurrFrame;
        }

        if (State == "Rewind")
        {
            GameObjectController.DrawFrame(Recording.Data[CurrFrame]);
            if (CurrFrame < 3) { CurrFrame = 0; State = "Stopped"; } else CurrFrame -= 3;
            Recording.CurrFrame = CurrFrame;
        }
        CurrFrameMkr.anchoredPosition = new Vector2((float)CurrFrame * 400 / FrameCount, -7);

        if (State == "Stopped")
        {
            //Everyplay.StopRecording();
            //if(Megacool.Instance._IsRecording) Megacool.Instance.StopRecording();
            //if (Recording.CurrFrame > 10)
            //{
                //btnShare.SetActive(true);
            //    Everyplay.SetMetadata("Date", System.DateTime.Now);
            //}
        }
    }

    public void SetScreenRecCam(Camera cam)
    {
        _screenRec.SetCamera(cam);
    }


    /// <summary>
    /// Sends a message to the CamControllerFactory
    /// </summary>
    /// <param name="cam"></param>
    public void ChooseCam(int cam)
    {
        CCF.ChooseCam(cam);
    }

    public int CamDrop(Vector2 anchPos, int CamId)
    {
        if (anchPos.x > Timeline.rect.xMin && anchPos.x < Timeline.rect.xMax)
        {
            int F = GetFrameFromxPos(Mathf.RoundToInt(anchPos.x));
            Recrdng.Current.Data[F][0].Events.Add(new RecEvent { Type = RecEventType.CamChange, Data = CamId.ToString() });
            return F;
        }
        else return -1;
    }

    public void DeleteCam(int F)
    {
        Debug.Log("Deleting event at " + F);
        RecEvent DeleteRecEvent = Recrdng.Current.Data[F][0].Events.First(e => e.Type == RecEventType.CamChange);
        Recrdng.Current.Data[F][0].Events.Remove(DeleteRecEvent);
    }

    private void RespawnCars()
    {
        foreach (iVehicleManager RVM in ReplayerVehicleManagers)
        {
            RVM.DestroyVehicle();
            RVM.CreateVehicle("Vehicle" + RVM.VId, CarManagerType.Replayer, RVM.Vehicle, RVM.Color);
            //RigidBody is frozen when created
        }
    }

    public void btnBuyCameras_Click()
    {
        UnityEngine.Object objShopPanel = Resources.Load("Prefabs/pnlShop");
        GameObject pnlShop = (GameObject)GameObject.Instantiate(objShopPanel, new Vector2(500, 500), Quaternion.identity, _canvas.transform);
        pnlShop.transform.localScale = Vector3.one;
        pnlShop.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        pnlShop.GetComponent<ShopPanel>().SwitchToggle("Camera");
    }

    private void _replayCam_OnStopRec(string RecFilename)
    {
        ReplayFilename = RecFilename;
    }

    private void DeleteReplayFile()
    {
        string foldr;
#if UNITY_EDITOR
        foldr = Directory.GetCurrentDirectory();
#else
        foldr = Application.persistentDataPath;
#endif
        DirectoryInfo dir = new DirectoryInfo(foldr);
        FileInfo[] info = dir.GetFiles("*.mp4");
        foreach (FileInfo f in info)
        {
            File.Delete(f.FullName);
            Debug.Log("Replay file deleted");
        }
    }

    public void PreviewVideo()
    {
        trControls.gameObject.SetActive(false);
        pnlBuyCameras.gameObject.SetActive(false);
        btnShare.gameObject.SetActive(false);
        _screenRec.PreviewVideo();
    }

    public void ShareVideo()
    {
        _screenRec.NativeShare();
        ClosePreviewVideo();
    }

    public void ClosePreviewVideo()
    {
        _screenRec.ClosePreview();
        trControls.gameObject.SetActive(true);
        pnlBuyCameras.gameObject.SetActive(true);
        btnShare.gameObject.SetActive(true);
    }

    public void Close()
    {
            CCF.Controller.Reset();
            Destroy(this.gameObject);
            PlayerManager.Type = "CarPlayer";
            PlayerCarManager.Gps.SegTime = Time.time;
    }

    void OnDestroy()
    {
        _screenRec.StopRecording();
        DeleteReplayFile();
        _screenRec.OnStopRec -= _replayCam_OnStopRec;
        foreach (VehicleManager rvm in ReplayerVehicleManagers)
        {
            rvm.DestroyVehicle();
        }
        Destroy(CCF);
        iVehicleManager CM = DrivingPlayManager.Current.PlayerCarManager;
        Destroy(ReplayCamController);
        if(ReopenDrivingMenuOnDestroy && CM.goCar != null)
        {
            DrivingPlayManager.Current.UnfreezeCars();
            CamSelector.Instance.ActiveCam.GetComponentInChildren<Camera>().enabled = true;
            CamController_Follow_Swing CamFol = CamSelector.Instance.ActiveCam.GetComponent<CamController_Follow_Swing>();
            CamFol.target = CM.goCar;
            CamFol.enabled = true;
            RaceRecorder.current.State = "Recording";
            DrivingPlayManager.Current.UnpauseCars();
            DrivingMenuController dmc = _canvas.GetComponent<DrivingMenuController>();
            if (dmc == null) { Debug.LogError("dmc is null"); Debug.Break(); }
            dmc.ShowPanel(true);    //Keeps crashing here not any more cos fixed line 44
        }
    }
}

public interface iReplayerGameObjectController
{
    void DrawFrame(List<RecFrameData> FrameData);
}

public class ReplayerGameObjectController : iReplayerGameObjectController
{
    private IEnumerable<RecordableVehicle> Recbles;
    private Replayer _replayer;

    public ReplayerGameObjectController(List<iVehicleManager> rvms, Replayer replayer)
    {
        Recbles = rvms.Select(vm => new RecordableVehicle(vm));
        _replayer = replayer;
    }

    public void DrawFrame(List<RecFrameData> FrameData)
    {
        foreach (RecFrameData TS in FrameData)
        {
            RecordableVehicle rv = (RecordableVehicle)Recbles.Where(v => v.VId == TS.VId).FirstOrDefault();
            rv.Transform.position = TS.Pos;
            rv.Transform.rotation = TS.Rot;
            ReplayerInput ri = (ReplayerInput)(rv.InputManager);
            rv.WCFL.steerAngle = TS.XMovement;
            rv.WCFR.steerAngle = TS.XMovement;
            //ri.zMovement = TS.ZMovement;
            rv.WCFL.angularVelocity = TS.FAV;
            rv.WCRL.angularVelocity = TS.RAV;
            rv.WCFR.angularVelocity = TS.FAV;
            rv.WCRR.angularVelocity = TS.RAV;
            rv.WCFL.springLengthInst = (float)TS.FLSprL / 100;
            rv.WCFR.springLengthInst = (float)TS.FRSprL / 100;
            rv.WCRL.springLengthInst = (float)TS.RLSprL / 100;
            rv.WCRR.springLengthInst = (float)TS.RRSprL / 100;
            rv.WCFL.isGrounded = TS.FLGnd;
            rv.WCFR.isGrounded = TS.FRGnd;
            rv.WCRL.isGrounded = TS.RLGnd;
            rv.WCRR.isGrounded = TS.RRGnd;
            if (TS.Events != null)
            {
                foreach (RecEvent e in TS.Events)
                {
                    if (e.Type == RecEventType.JointBreak)
                    {
                        rv.DMC.BreakOffPart(e.Data.ToString());
                    }

                    if (e.Type == RecEventType.Crunch)
                    {
                        rv.DMC.CrunchPart(e.Data.ToString(), e.Data2.ToString());
                    }

                    if (e.Type == RecEventType.AddHinge)
                    {
                        rv.DMC.AddHingeToPart(e.Data.ToString(), e.Data2.ToString());
                    }
                    if (e.Type == RecEventType.Scratch)
                    {
                        rv.DMC.ScratchPart(e.Data.ToString());
                    }
                    if(e.Type == RecEventType.CamChange)
                    {
                        _replayer.ChooseCam(System.Convert.ToInt32(e.Data));
                    }
                }
            }
        }
    }
}

public enum RecEventType:byte
{
    Crunch = 0,
    JointBreak = 1,
    AddHinge = 2,
    Scratch = 3,
    CamChange = 4,
}

