using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Text;

public interface iRace : IDisposable
{
    int Laps { get; set; }
    float StartTime { get; set; }
    int LapRecBonus { get; set; }
    int DriftBonus { get; set; }
    int AirBonus { get; set; }
    int HogBonus { get; set; }
    List<iRacer> Racers { get; set; }
    iRacer PlayerRacer { get; set; }
    List<LapStat> LapStats { get; set; }
    bool Started { get; set; }
    void ArrangeGrid();
    void ReadySteady();
    void Update();
    void Pause();
    void Resume();
}

public interface iRacer
{
    GameObject goRacer { get; set; }
    iVehicleManager VehicleManager { get; set; }
    bool isMachine { get; set; }
    int Progrss { get; set; }
    void GetProgrss();
    void SetPos(Vector3 pos);
    void SetRot(Quaternion rot);
    void RemoveListeners();
    void Dispose();
}

public class Race : iRace
{
    public static Race Current;
    public int Laps { get; set; }
    public float StartTime { get; set; }
    public int LapRecBonus { get; set; }
    public int DriftBonus { get; set; }
    public int AirBonus { get; set; }
    public int HogBonus { get; set; }
    public List<iRacer> Racers { get; set; }
    public iRacer PlayerRacer { get; set; }
    private bool Paused = false;
    public bool Started { get; set; }
    private Text txtTimer;
    private Text txtLap;
    private GameObject goStartingLine;
    private GameObject goChequeredFlag;
    public List<LapStat> LapStats { get; set; }

    public Race()
    {
        Racers = new List<iRacer>();
        Started = false;
    }

    public void ArrangeGrid()
    {
        System.Random rnd = new System.Random();
        List<iRacer> GridOrder = Racers.OrderBy(item => rnd.Next()).ToList<iRacer>();
        goStartingLine = GameObject.Find("StartingLine");
        Transform SL = goStartingLine.transform;
        goChequeredFlag = SL.Find("ChequeredFlag").gameObject;
        goChequeredFlag.SetActive(false);
        int Pos = 0;
        foreach (iRacer r in GridOrder)
        {
            int row = Pos % 3;
            int col = Pos / 3;
            Pos++;
            r.SetPos(SL.position + SL.rotation * (Vector3.left * (-4 + 4 * row) + Vector3.back * 6 * col + Vector3.up * 0.5f));
            r.SetRot(SL.rotation);
            r.goRacer.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        Road.Instance.RemoveRuts();
        if (Main.Instance.Ghost && PlayerManager.Type != "Replayer")
            DrivingPlayManager.Current.GhostCarManager.VehicleController.Init();
        LapStats = new List<LapStat>();
        PlayerRacer = DrivingPlayManager.Current.PlayerCarManager.Racer;
    }


    public void ReadySteady()
    {
        Started = false;    //becuase we might have got here by the menu restart
        foreach (Racer r in Racers)
        {
            r.GetOpposition();
            r.Lap = 1;
            r.Progrss = 0;
            r.PrevProgrss = 0;
            r.TotProgrss = 0;
            r.VehicleManager.VehicleController.EndSkidmarks = true;
            r.goRacer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        goStartingLine.GetComponent<RaceStarter>().enabled = true;
        goStartingLine.GetComponent<RaceStarter>().Init();
    }

    public void Go()
    {
        goStartingLine.GetComponent<RaceStarter>().enabled = false;
        foreach (Racer r in Racers)
        {
            r.LapStartTime = Time.time;
            r.goRacer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            r.VehicleManager.Gps.RecoveryAllowed = true;
        }
        Started = true;
        StartTime = Time.time;
        if (Main.Instance.Ghost)
        {
            GhostController _ghostController = (GhostController)DrivingPlayManager.Current.GhostCarManager.VehicleController;
            _ghostController.Go();
        }
    }

    public void Update()
    {
        if (Paused) return;
        if (!Started) return;
        if (txtTimer == null) { txtTimer = GameObject.Find("txtTimer").GetComponent<Text>(); }
        foreach (Racer r in Racers)
        {
            r.GetProgrss();
        }
        float TotalTime = Time.time - StartTime;
        txtTimer.text = GenFunc.HMS(TotalTime);
    }

    public void Pause()
    {
        Paused = true;
    }
    public void Resume()
    {
        Paused = false;
    }

    public void ShowChequeredFlag(bool show)
    {
        goChequeredFlag.SetActive(show);
    }

    public void Finish()
    {
        Started = false;
        float TotTime = Time.time - StartTime;
        foreach (iVehicleManager vm in DrivingPlayManager.Current.VehicleManagers)
        {
            vm.Gps.RecoveryAllowed = false;
        }
        goStartingLine.GetComponentInChildren<ParticleSystem>().Play();
        GameObject goPnlFinish = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/HiScoresCanvas"), Vector3.zero, Quaternion.identity);
        goPnlFinish.GetComponentInChildren<Canvas>().worldCamera = GameObject.Find("FollowCamInner").GetComponent<Camera>();
        goPnlFinish.GetComponentInChildren<Canvas>().planeDistance = 1.5f;
        goPnlFinish.GetComponentInChildren<HiScoresPanel>().Init(TotTime);

    }

    internal void StartEngines()
    {
        foreach (Racer r in Racers)
        {
            r.VehicleManager.VehicleController.StartEngine();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        PlayerRacer = null;
        foreach (iRacer r in Racers)
        {
            r.Dispose();
        }
        Racers.Clear();
        System.GC.Collect();
    }
}

public struct LapStat
{
    public int stFr, finFr;
    public float time;
}

public class Racer : iRacer, IDisposable
{
    public GameObject goRacer { get; set; }
    private iRace _Race;
    private int RoadSegCount;
    private GPS _gps;
    private GPS _playerGps;
    private iRacer _playerRacer;
    private DamageController _damCtrl;
    MusicPlayer music;
    public int Lap { get; set; }
    public float LapStartTime { get; set; }
    int LapStartFrame;
    public int PrevProgrss;
    public int Progrss { get; set; }
    public int TotProgrss;
    public bool isMachine { get; set; }
    private List<iRacer> PlayerOpposition;
    private List<iRacer> Opposition;
    public iVehicleManager VehicleManager { get; set; }
    private iVehicleController _vehicleController;
    private float _defaultMotorForce;
    private Text txtLap;
    private Scorer _scorer;


    public Racer(iVehicleManager vm, DamageController dc, iRace Race, bool IsMachine)
    {
        isMachine = IsMachine;
        _Race = Race;
        VehicleManager = vm;
        _damCtrl = dc;
        goRacer = vm.goCar;
        _vehicleController = vm.VehicleController;
        _defaultMotorForce = _vehicleController.motorForce;
        _gps = _vehicleController.Gps;
        if (IsMachine)
        {
            _playerGps = DrivingPlayManager.Current.PlayerCarManager.Gps;
            _playerRacer = DrivingPlayManager.Current.PlayerCarManager.Racer;
        }
        else
        {
            _scorer = new Scorer(vm, _gps);
            music = MusicPlayer.Instance;
        }
        
        RoadSegCount = Road.Instance.Segments.Count;
        if (goRacer.name == "Vehicle1") _vehicleController.motorForce *= 1.1f;   //This car is more powerful. It jumps backward and comes up behind you
        _damCtrl.OnCollisionExitEvent += StartHogTimer;
    }

    float _hogBonusTime = 0;
    private void StartHogTimer()
    {
        _hogBonusTime = Time.time + 3;
    }


    public void GetProgrss()
    {
        Progrss = _gps.CurrSegIdx - Road.Instance.StartingLineSegIdx;
        if (Progrss < 1) Progrss = Progrss + RoadSegCount; // -Road.Instance.StartingLineSegIdx;
        if (Progrss > PrevProgrss + RoadSegCount - 100) Progrss = PrevProgrss; //Rolling backwards over the starting line - ignore
        OnSegmentEnter();
        PrevProgrss = Progrss;
        if (_hogBonusTime != 0)
        {
            if (Time.time > _hogBonusTime)
            {
                _hogBonusTime = 0;
                if (_playerGps.IsOnRoad)
                {
                    if (!_gps.IsOnRoad)
                    {
                        Main.Instance.PopupMsg("Road Rage Points\n$20", Color.red);
                        Race.Current.HogBonus += 20;
                    }
                    else if (_playerRacer.Progrss - Progrss > 12)
                    {
                        int pts = (_playerRacer.Progrss - Progrss) / 6;
                        Race.Current.HogBonus += pts;
                        Main.Instance.PopupMsg("Road Hog Points\n$" + pts.ToString(), Color.red);
                    }
                }
            }
        }
    }

    private bool ChFlag = false;

    private void OnSegmentEnter()
    {
        if (Progrss < 20 && PrevProgrss > RoadSegCount - 20)
        {
            Lap++;
            if (!isMachine)
            {
                float LapTime = Time.time - LapStartTime;
                _Race.LapStats.Add(new LapStat { stFr = LapStartFrame, finFr = Recrdng.Current.FrameCount - 1, time = LapTime });
                if (Game.current.BestLap == null || LapTime < Game.current.BestLap)
                {
                    int LapScore = RoadSegCount / 50;
                    Race.Current.LapRecBonus += LapScore;
                    StringBuilder s = new StringBuilder("New Lap Record\n$");
                    s.Append(LapScore.ToString());
                    Main.Instance.PopupMsg(s.ToString(), Color.green);
                    Game.current.BestLap = LapTime;
                    SaveLoadModel.SaveSavedGames();
                }
                if (Main.Instance.Ghost)
                {
                    GhostController gc = (GhostController)DrivingPlayManager.Current.GhostCarManager.VehicleController;
                    gc.Frame = 0;
                }
                LapStartTime = Time.time;
                LapStartFrame = Recrdng.Current.FrameCount;
            }
        }
        TotProgrss = Progrss + (Lap - 1) * RoadSegCount;

        //keeping the AI cars close to the player
        if (isMachine)
        {
            foreach (Racer pr in PlayerOpposition)
            {
                if (_hogBonusTime == 0 && pr.TotProgrss > TotProgrss + 20 || pr.TotProgrss < TotProgrss - 301)
                //Player is too far ahead or really far behind
                {
                    JumpTo(pr);
                }
                if (pr.TotProgrss < TotProgrss - 10)
                //AICar is a bit ahead of the player slow down
                {
                    _vehicleController.motorForce = _defaultMotorForce * 0.4f;
                }
                else
                { _vehicleController.motorForce = _defaultMotorForce; }
            }
        }
        else
        {
            if (txtLap == null) { txtLap = GameObject.Find("txtLap").GetComponent<Text>(); }
            float LapTime = Time.time - LapStartTime;
            if (LapTime > 10) txtLap.text = GenFunc.HMS(LapTime);
            if (Lap == Race.Current.Laps + 1) Race.Current.Finish();
            if (TotProgrss > RoadSegCount * Race.Current.Laps - 100 && ChFlag == false)
            {
                Transform trStart = GameObject.Find("StartingLine").transform;
                foreach (ParticleSystem ps in trStart.GetComponentsInChildren<ParticleSystem>()) { ps.Play(); }
                Race.Current.ShowChequeredFlag(true);
                CamSelector.Instance.SelectCam("Simple");
                ChFlag = true;
            }
            var s = _gps.Speed;
            if (s > 4) { music.SetMix(1); }
            else { music.SetMix(0); }

        }
    }

    public void GetOpposition()
    {
        PlayerOpposition = new List<iRacer>();
        PlayerOpposition = _Race.Racers.Where(r => r != this && r.isMachine == false).ToList<iRacer>();
        Opposition = _Race.Racers.Where(r => r != this).ToList<iRacer>();
    }

    public void JumpTo(Racer r)
    {
        int JumpTotSeg;
        if (!goRacer.name.EndsWith("1")) { JumpTotSeg = r.TotProgrss + 120; }
        else { JumpTotSeg = r.TotProgrss - 7; }
        if (JumpTotSeg < 0) JumpTotSeg = 0;
        int JumpSeg = JumpTotSeg % RoadSegCount;
        if (JumpSeg < 0) JumpSeg = 0;
        if (JumpSeg > RoadSegCount - 3) JumpSeg -= (RoadSegCount + 3);
        if (Opposition.Where(Opp => Mathf.Abs(Opp.Progrss - JumpSeg) < 7).Count() != 0)
        {
            //Debug.Log("Fail");
            return;
        }
        try
        {
            goRacer.transform.position = Road.Instance.XSecs[JumpSeg].MidPt;
            goRacer.transform.LookAt(Road.Instance.XSecs[JumpSeg + 2].MidPt);
            goRacer.GetComponent<Rigidbody>().velocity = (Road.Instance.XSecs[JumpSeg + 2].MidPt - Road.Instance.XSecs[JumpSeg].MidPt).normalized * r.goRacer.GetComponent<Rigidbody>().velocity.magnitude;
            Progrss = JumpSeg;
            Lap = r.Lap; //bugbugbug if it jumps over the starting line it skips a lap
        }
        catch (System.Exception e)
        {
            Debug.Log("Cant Jump to " + JumpSeg + " when RoadSegCount = " + RoadSegCount);
        }
        iVehicleController VC = VehicleManager.VehicleController;
        VC.EndSkidmarks = true;
        PrevProgrss = Progrss;

    }

    public void SetPos(Vector3 pos)
    {
        goRacer.transform.position = pos;
    }

    public void SetRot(Quaternion rot)
    {
        goRacer.transform.rotation = rot;
    }

    class Scorer
    {
        GameObject _goVeh;
        GPS _gps;
        float _takeOffStartTime = 0;
        float _landStartTime = 0;
        int _takeOffSegIdx = 0;
        float _landAngVel;
        float _driftStartTime = 0;
        int _driftSegIdx = 0;
        bool _drifting = false;
        int pts;


        public Scorer(iVehicleManager vm, GPS gps)
        {
            _goVeh = vm.goCar;
            _gps = gps;
            _gps.OnDrift += Drift;
            _gps.OnLand += Land;
            _gps.OnTakeOff += TakeOff;
            _gps.OnRecovery += Recovery;
            _gps.OnDriftEnd += DriftEnd;
            _gps.OnDriftFail += DriftFail;
        }

        public void Drift(GPSEventArgs args)
        {
            _driftStartTime = Time.time;
            _driftSegIdx = args.SegIdx;

        }

        public void DriftEnd(GPSEventArgs args)
        {
            float _driftTime = Time.time - _driftStartTime;
            int _driftSegs = args.SegIdx - _driftSegIdx;
            pts = _driftSegs / 10;
            if (pts > 0)
            {
                Main.Instance.PopupMsg("Drift Points \n$" + (pts).ToString());
                Race.Current.DriftBonus += pts;
            }
        }

        public void DriftFail(GPSEventArgs args)
        {
            if (args.SegIdx - _driftSegIdx > 4)
            { Main.Instance.PopupMsg("Drift Fail"); }
        }

        public void TakeOff(GPSEventArgs args)
        {
            _takeOffStartTime = Time.time;
            _takeOffSegIdx = args.SegIdx;
        }

        public void Land(GPSEventArgs args)
        {
            float _airTime = Time.time - _takeOffStartTime;
            _airTime = Mathf.Round(_airTime * 100) / 100;
            _landStartTime = Time.time;
            _landAngVel = _goVeh.GetComponent<Rigidbody>().angularVelocity.magnitude;
            //Main.Instance.PopupMsg(Mathf.RoundToInt(Angl).ToString());
        }

        public void Recovery(GPSEventArgs args)
        {
            float _airTime = _landStartTime - _takeOffStartTime;
            if (_airTime < 1) { return; }   //not a real jump
            float _recoveryTime = Time.time - _landStartTime;
            int _recoverySegIdx = args.SegIdx;
            float _recoverySegsPerSec = (float)(_recoverySegIdx - _takeOffSegIdx) / (Time.time - _takeOffStartTime);
            if (_recoverySegsPerSec > 40) { return; }   //landed on the wrong bit of road
            //float _spin = _landAngVel; //a good spin is 2.5
            pts = (int)((1 + _landAngVel) * _recoverySegsPerSec / 5);
            if (pts > 0 && pts < 200)
            {
                Race.Current.AirBonus += pts;
                Main.Instance.PopupMsg("Air Points\n$" + pts.ToString(), new Color(0f, 0.7f, 1f));
            }
        }

        public void DriftRecovery(GPSEventArgs args)
        {
            //Not sure if used
            _drifting = false;
            return;
        }
        /*
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                protected virtual void Dispose(bool b)
                {
                    _goVeh = null;
                    _gps = null;
                }
                */
    }

    public void RemoveListeners()
    {
        if (!isMachine)
        {
            _gps.OnTakeOff -= _scorer.TakeOff;
            _gps.OnLand -= _scorer.Land;
            _gps.OnRecovery -= _scorer.Recovery;
            _gps.OnDrift -= _scorer.Drift;
            _gps.OnDriftEnd -= _scorer.DriftRecovery;
        }
        _damCtrl.OnCollisionExitEvent -= StartHogTimer;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        RemoveListeners();
        _Race = null;
        VehicleManager = null;
        goRacer = null;
        _vehicleController = null;
        _damCtrl = null;
        _gps = null;
        _playerGps = null;
        _playerRacer = null;
        _scorer = null;
        if (PlayerOpposition != null)
        {
            PlayerOpposition.Clear();
            Opposition.Clear();
        }
    }
}

public class GPSEventArgs : EventArgs
{
    public int SegIdx { get; set; }
}

public static class GenFunc
{
    public static string HMS(float FloatTime)
    {
        float minutes = Mathf.Floor(FloatTime / 60);
        float seconds = FloatTime % 60;
        seconds = Mathf.Round(seconds * 100f) / 100f;
        string strMins = minutes.ToString();
        string strSecs = seconds.ToString("00.00");

        if (minutes < 10) { strMins = "0" + strMins; }
        return strMins + ":" + strSecs;
    }

    public static string OrdinalIndicator(int rank)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(rank);
        switch (rank % 100)
        {
            case 11:
            case 12:
            case 13:
                sb.Append("th"); break;
        }

        switch (rank % 10)
        {
            case 1:
                sb.Append("st"); break;
            case 2:
                sb.Append("nd"); break;
            case 3:
                sb.Append("rd"); break;
            default:
                sb.Append("th"); break;
        }

        return sb.ToString();
    }
}

