using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ReplayCamControllerFactory : MonoBehaviour
{
    private GameObject goCam1;
    public Transform trCam1;
    private Camera cam1;
    private Camera ActiveCam;
    private Camera InactiveCam;
    public Transform Car;
    public iReplayCamController Controller { get; set; }
    private Replayer _replayer;
    private bool _fadeInProgess = false;
    List<RecFrameData> PlayerFrames = new List<RecFrameData>();
    ShopItem[] PurchasedCameras;
    List<ShopItem> RandomCameras;
    ToggleGroup _camToggleGroup;
    List<Toggle> Tgls = new List<Toggle>();
    int _currentCamId = 16;
    List<CarStop> CarStops = new List<CarStop>();
    CarStop _nextCarStop = new CarStop();

    void Awake()
    {
        goCam1 = new GameObject();
        goCam1.name = "ReplayCam1";
        trCam1 = goCam1.transform;
        trCam1.SetParent(this.transform);
        cam1 = goCam1.AddComponent<Camera>();
        cam1.cullingMask = ~(1 << 5);
        _replayer = GameObject.Find("pnlReplayer(Clone)").GetComponent<Replayer>();
        Car = _replayer.PlayerCarManager.goCar.transform;
        cam1.farClipPlane = 100;
        cam1.nearClipPlane = 0.3f;
        cam1.useOcclusionCulling = true;
        cam1.clearFlags = CameraClearFlags.SolidColor;
        cam1.backgroundColor = new Color(0.69f, 0.69f, 0.69f, 1f);

        GetPurchasedCameras();

        PlayerFrames = (from d
                    in Recrdng.Current.Data
                        select d[Recrdng.Current.PlayerCarId]).ToList();

        /*
        List<CarStopFrame> CarStopFrames = new List<CarStopFrame>();
        if (PurchasedCameras.Count(c => c.Name == "Stop Cam") > 0)
        {
            //Find where the car changes direction suddenly
            for (int i = 100; i < Recrdng.Current.FrameCount - 150; i++)
            {
                Vector3 velocity = PlayerFrames[i - 14].Pos - PlayerFrames[i - 50].Pos;
                Vector2 velocity2d = new Vector2(velocity.x, velocity.z);
                Vector3 nextVelocity = PlayerFrames[i].Pos - PlayerFrames[i - 1].Pos;
                Vector2 nextVelocity2d = new Vector2(nextVelocity.x, nextVelocity.z);
                //This version works out the the component of nextvel2d that as in the same direction as vel2d
                float angle = Vector2.Angle(nextVelocity2d, velocity2d);
                float relativeSpeed = nextVelocity.magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
                float velocityDelta = (nextVelocity - velocity).sqrMagnitude;
                //Debug.Log("vel" + velocity.sqrMagnitude.ToString() + " diff" + (nextVelocity - velocity).sqrMagnitude.ToString());
                //if (velocity.sqrMagnitude>nextVelocity.sqrMagnitude && velocityDelta > 0.04f)
                if (velocity2d.sqrMagnitude > 25 && relativeSpeed < 0.03f && nextVelocity.sqrMagnitude < 1)
                {
                    CarStopFrame csf = new CarStopFrame();
                    csf.Frame = i;
                    csf.VelocityDelta = velocityDelta;
                    csf.RelativeSpeed = relativeSpeed;
                    CarStopFrames.Add(csf);
                }
            }
        }


        CarStop _carStop = new CarStop(); //start with a dummy carstop
        _carStop.StartFrame = 0;
        _carStop.EndFrame = 0;
        CarStopFrame _smallestRel = new CarStopFrame();
        _smallestRel.RelativeSpeed = 1000;
        //Move through the CarStopFrames and find the one where the velocity changes the most
        foreach (CarStopFrame f in CarStopFrames)
        {

            if (f.Frame <= _carStop.EndFrame + 100)    // There is no distict gap since the last CarStopFrame
                _carStop.EndFrame = f.Frame;       //this keeps getting incremented until there is a gap
            else
            {   //there was a gap so finish off last CarStop
                if (_carStop.StartFrame != 0)    //if its not the dummy one then finish it off
                {
                    AddCarStop(_carStop);
                }
                _carStop = new CarStop();
                _carStop.StartFrame = f.Frame - 100;
                _carStop.EndFrame = f.Frame;
            }
        }
        //Finish off the last CarStop
        if (_carStop.StartFrame > 0)
        {
            AddCarStop(_carStop);
        }
        */
        //set the counter ready for when it's running
        Init();
    }

    private void Start()
    {
        DrawCamChooseFrames();
    }

    #region AddCarStop
    /*
        private void AddCarStop(CarStop _carStop)
        {
            //Find the Position where it changes velocity
            _carStop.StopPos = PlayerFrames[_carStop.EndFrame].Pos;
            //Add on a 4m vector in front of the car
            Vector3 dir = (_carStop.StopPos - PlayerFrames[_carStop.EndFrame - 14].Pos);
            dir.Normalize();
            _carStop.CamPos = _carStop.StopPos + dir * 4f;
            _carStop.LookPos = PlayerFrames[_carStop.EndFrame - 70].Pos;
            _carStop.CamPos = PlayerFrames[_carStop.EndFrame].Pos + (PlayerFrames[_carStop.EndFrame].Pos - PlayerFrames[_carStop.EndFrame - 100].Pos).normalized * 1.5f;
            _carStop.CamPos = new Vector3(_carStop.CamPos.x, PlayerFrames[_carStop.EndFrame].Pos.y, _carStop.CamPos.z);
            //If theres something in the way, move the cam up incrementally
            for (float vOffset = 0; vOffset < 10; vOffset += 0.5f)
            {
                _carStop.CamPos += vOffset * Vector3.up;
                if (Physics.Raycast(_carStop.StopPos + Vector3.up, _carStop.CamPos - _carStop.StopPos - Vector3.up, 4))
                {
                    //Debug.Log("Raycast hit at " + _carStop.StopPos.ToString() + " => moved camera up by " + vOffset.ToString());
                }
                else
                {
                    if (vOffset == 0 && Random.Range(0, 2) == 0) _carStop.CamPos += Vector3.up;
                    break;
                }
            }
            //Calculate the tripodHeight
            RaycastHit hit;
            if (Physics.Raycast(_carStop.CamPos, Vector3.down, out hit))
            {
                _carStop.TripodHeight = Vector3.Distance(hit.point, _carStop.CamPos);
            }

            CarStops.Add(_carStop);
        }
        */
    #endregion

    public void GetPurchasedCameras()
    {
        PurchasedCameras = (from i in Shop.Items join p in UserDataManager.Instance.Data.Purchases on i.Id equals p select i).Where(i => i.Type == ShopItemType.Camera).ToArray();
        RandomCameras = PurchasedCameras.ToList();
        UnityEngine.Object objCamDirector = Resources.Load("Prefabs/tglCamDirector");
        Transform trDirector = _replayer.transform.Find("pnlCameraDirector");
        _camToggleGroup = trDirector.GetComponent<ToggleGroup>();
        foreach (ShopItem c in RandomCameras)
        {
            GameObject goCamTgl = (GameObject)GameObject.Instantiate(objCamDirector, trDirector);
            Toggle tgl = goCamTgl.GetComponent<Toggle>();
            Tgls.Add(tgl);
            tgl.group = _camToggleGroup;
            goCamTgl.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ToolIcons/CamType" + c.Name);
            goCamTgl.GetComponent<ReplayCamDragDrop>().Replayer = _replayer;
            goCamTgl.GetComponent<ReplayCamDragDrop>().CamId = c.Id;
            tgl.onValueChanged.AddListener(delegate { ChooseToggle(c.Id, tgl.isOn); });
        }
    }
    /// <summary>
    /// Draws the lttle draggable clapperboards that are stored as CamChange events in the recrding
    /// </summary>
    void DrawCamChooseFrames()
    {
        Vector2 _prevPos = new Vector2(-1000,-40);
        var CamEvtFrames = PlayerFrames.Where(fr => fr.Events.Count(e => e.Type == RecEventType.CamChange) > 0);
        foreach (RecFrameData EF in CamEvtFrames)
        {
            string CamId = EF.Events.First(e => e.Type == RecEventType.CamChange).Data.ToString();
            int _intCamId = System.Convert.ToInt32(CamId);
            GameObject goDragImg = new GameObject("DragImg");
            string _camName = Shop.Items.First(i=>i.Id == _intCamId).Name;
            goDragImg.AddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ToolIcons/CamType" + _camName);
            ReplayCamDragDrop rcdd = goDragImg.AddComponent<ReplayCamDragDrop>();
            rcdd.Frame = PlayerFrames.IndexOf(EF);
            rcdd.Replayer = _replayer;
            rcdd.CamId = _intCamId;
            RectTransform trDragImg = (RectTransform)goDragImg.transform;
            trDragImg.localScale = Vector3.one * 0.25f;
            trDragImg.SetParent(_replayer.transform.Find("pnlCamTimeline"),false);
            float xPos = _replayer.GetxPosFromFrame(rcdd.Frame);
            float yPos = -40;
            if (xPos - _prevPos.x < 40) yPos = _prevPos.y == -40 ? -80 : -40;
            trDragImg.anchoredPosition = new Vector2(xPos, yPos);
            _prevPos = trDragImg.anchoredPosition;
            GameObject goArrow = new GameObject("Arrow");
            Image imgArrow = goArrow.AddComponent<Image>();
            RectTransform trArrow = (RectTransform)goArrow.transform;
            trArrow.SetParent(trDragImg, false);
            trArrow.pivot = new Vector2(0.5f, 0);
            trArrow.anchorMin = new Vector2(0.5f, 1); trArrow.anchorMax = new Vector2(0.5f, 1);
            trArrow.sizeDelta = new Vector2(5, -trDragImg.localPosition.y * 4);
            imgArrow.color = Color.yellow;
        }
    }

    public void Init()
    {
        ActiveCam = cam1;
        goCam1.SetActive(true);
        Controller = new CamController_Follow(ActiveCam.transform, Car, _replayer);
        Controller.Init(false);
        _replayer.SetScreenRecCam(cam1);
    }

    public void ChooseRandomCam()
    {
        int NewCamId = 0;
        _camToggleGroup.SetAllTogglesOff();
        int CamShopItemId;
        do
        {
            CamShopItemId = Random.Range(0, RandomCameras.Count);
            NewCamId = PurchasedCameras[CamShopItemId].Id;
        } while (NewCamId == _currentCamId && RandomCameras.Count != 1);
        ChooseCam(NewCamId);
        Tgls[CamShopItemId].Select(); Tgls[CamShopItemId].isOn = true;
    }

    public void ChooseCam(int NewCamId)
    {
        _currentCamId = NewCamId;
        Controller.Reset();

        switch (NewCamId)
        {
            case 16:
                //Debug.Log("Follow" + ActiveCam.name);
                Controller = new CamController_Follow(ActiveCam.transform, Car, _replayer);
                break;
            case 17:
                //Debug.Log("Wheelarch" + ActiveCam.name);
                Controller = new CamController_Wheelarch(ActiveCam.transform, Car, _replayer);
                break;
            case 18:
                //Debug.Log("Fish" + ActiveCam.name);
                Controller = new CamController_Driver(ActiveCam.transform, Car, _replayer);
                break;
            case 19:
                //Debug.Log("Zoom" + ActiveCam.name);
                Controller = new CamController_Zoom(ActiveCam.transform, Car, _replayer);
                break;
            case 20:
                //Debug.Log("Copter" + ActiveCam.name);
                Controller = new CamController_Drone(ActiveCam.transform, Car, _replayer);
                break;
            case 21:
                //Debug.Log("Chase" + ActiveCam.name);
                Controller = new CamController_RoadRunner(ActiveCam.transform, Car, _replayer, PlayerFrames);
                break;
            case 22:
                //Debug.Log("Hedgehog" + ActiveCam.name);
                Controller = new CamController_Hedgehog(ActiveCam.transform, Car, _replayer);
                break;
            case 23:
                //Debug.Log("Hedgehog" + ActiveCam.name);
                Controller = new CamController_Side(ActiveCam.transform, Car, _replayer);
                break;
            case 34:
                //Debug.Log("Ice" + ActiveCam.name);
                Controller = new CamController_Ice(ActiveCam.transform, Car, _replayer);
                break;
        }
        Controller.Init(false); //No Lerp cos we never coded it
        _nextCarStop = CarStops.FirstOrDefault(t => t.StartFrame > Recrdng.Current.CurrFrame);
    }
    /// <summary>
    /// Resets the current controller and starts the CamController_Follow
    /// </summary>
    public void ChooseFollow()
    {
        Controller.Reset();
        Controller = new CamController_Follow(ActiveCam.transform, Car, _replayer);
        Controller.Init(false);
    }

    /// <summary>
    /// This calls ChooseCam only when toggle = true
    /// </summary>
    void ChooseToggle(int CamId, bool IsOn)
    {
        if (IsOn) ChooseCam(CamId);
    }
    // Update is called once per frame
    void Update()
    {
        Controller.Update();
        if (_nextCarStop.StartFrame != 0 && (_replayer.State == "Playing" || _replayer.State == "PlayingSloMo"))
        {
            if (Recrdng.Current.CurrFrame > _nextCarStop.StartFrame)
            {
                if (_nextCarStop.EndFrame > Recrdng.Current.CurrFrame && Random.Range(0, 2) > 1)
                {
                    Controller.Reset();
                    Controller = new CamController_Stop(ActiveCam.transform, Car, _replayer, _nextCarStop);
                    Controller.Init(false);
                }
                _nextCarStop = CarStops.FirstOrDefault(s => s.StartFrame > _nextCarStop.EndFrame);
            }
            else if (Controller.OutOfBounds())
            {
                ChooseRandomCam();
            }
        }
        else
        {
            if (Controller.OutOfBounds())
            {
                ChooseRandomCam();
            }
        }
    }

    void OnDestroy()
    {
        Controller = null;
        Destroy(goCam1);
    }


}

public interface iReplayCamController
{
    void Init(bool LerpToStart);
    void Update();
    void Reset();
    bool OutOfBounds();
}

public class CamController_Zoom : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    int ClosestFrame;
    int FirstVisibleFrame = Recrdng.Current.CurrFrame;
    Vector3 _firstVisiblePos;
    Replayer _replayer;
    public CamController_Zoom(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        ClosestFrame = Recrdng.Current.FrameCount - 1;
        if (_replayer.State == "Playing")
        {
            ClosestFrame = Recrdng.Current.CurrFrame + 250;
            if (ClosestFrame >= Recrdng.Current.FrameCount) ClosestFrame = Recrdng.Current.FrameCount - 1;

        }
        if (_replayer.State == "PlayingSloMo")
        {
            ClosestFrame = Recrdng.Current.CurrFrame + 100;
            if (ClosestFrame >= Recrdng.Current.FrameCount) ClosestFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            ClosestFrame = Recrdng.Current.CurrFrame - 250;
            if (ClosestFrame < 0) ClosestFrame = 0;
        }
        List<RecFrameData> FrameData = Recrdng.Current.Data[ClosestFrame];
        Vector3 ClosestCarPos = FrameData[Recrdng.Current.PlayerCarId].Pos;
        Vector3 Offset = Random.onUnitSphere * 3;
        Offset = new Vector3(Offset.x, Mathf.Abs(Offset.y), Offset.z);
        _trCam.position = ClosestCarPos + Offset; // + Vector3.up * 2;
        int TerrainLayerMask= (1 << 10);
        _firstVisiblePos = Recrdng.Current.Data[FirstVisibleFrame][Recrdng.Current.PlayerCarId].Pos;
        while (Physics.Raycast(_trCam.position, _firstVisiblePos - _trCam.position, Vector3.Distance(_trCam.position, _firstVisiblePos), TerrainLayerMask))
        {
            FirstVisibleFrame += 3;
            _firstVisiblePos = Recrdng.Current.Data[FirstVisibleFrame][Recrdng.Current.PlayerCarId].Pos;
        }
    }
    public void Update()
    {
        if(Recrdng.Current.CurrFrame<FirstVisibleFrame)
            _trCam.LookAt(_firstVisiblePos);
        else
            _trCam.LookAt(_car.position);
        float d = Vector3.Distance(_trCam.position, _car.transform.position);
        _trCam.GetComponent<Camera>().fieldOfView = Mathf.Clamp(250 / d, 7, 90);
        _trCam.GetComponent<Camera>().farClipPlane = d + 10;
    }

    public bool OutOfBounds()
    {
        bool rtn = false;
        RaycastHit hit;
        string HitCollidername = "Vehicle0";
        if (_replayer.State == "Playing" || _replayer.State == "PlayingSloMo")
        {
            if (Recrdng.Current.CurrFrame > ClosestFrame)
            {
                if (Physics.Raycast(_trCam.position, _car.position - _trCam.position, out hit, Vector3.Distance(_trCam.position, _car.position)))
                {
                    HitCollidername = hit.collider.name;
                }
                if (Vector3.Distance(_trCam.position, _car.transform.position) > 50 || !(HitCollidername.StartsWith("V") || HitCollidername.Contains("Fence")))

                    rtn = true;
            }
        }
        if (_replayer.State == "Rewind" || _replayer.State == "Reverse")
        {
            if (Recrdng.Current.CurrFrame < ClosestFrame)
            {
                if (Physics.Raycast(_trCam.position, _car.position - _trCam.position, out hit, Vector3.Distance(_trCam.position, _car.position)))
                {
                    HitCollidername = hit.collider.name;
                }
                if (Vector3.Distance(_trCam.position, _car.transform.position) > 50 || !(HitCollidername.Contains("Car") || HitCollidername.Contains("Fence")))
                    rtn = true;
            }
        }
        return rtn;
    }

    public void Reset() {
        _trCam.GetComponent<Camera>().farClipPlane = 100;

    }
}

public class CamController_Driver : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    int FutureFrame;
    Replayer _replayer;
    public CamController_Driver(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        _trCam.parent = _car;
        switch (Main.Instance.Vehicle)
        {
            case "Anglia":
                _trCam.localPosition = new Vector3(0.35f, 1.2f, 0);
                break;
            case "Car":
                _trCam.localPosition = new Vector3(0.35f, 1.2f, 0.52f);
                break;
            case "Hotrod":
                _trCam.localPosition = new Vector3(-0.35f, 1.25f, 0f);
                break;
            case "Porsche":
                _trCam.localPosition = new Vector3(-0.35f, 1.1f, 0);
                break;
            default:   _trCam.localPosition =  Vector3.zero;
                break;
        }
        
        //porsche = new Vector3(-0.35f, 1.1f, 0);
        //slocus = new Vector3(-0.35f, 1.2f, 0.52f);
        //Hotrod = new Vector3(-0.35f, 1.25f, 0.1f);
        _trCam.localRotation = Quaternion.Euler(2.3f, 0, 0);
        _trCam.GetComponent<Camera>().nearClipPlane = 0.1f;

        FutureFrame = Recrdng.Current.FrameCount - 1;
        if (_replayer.State == "Playing")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 150;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 50;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = Recrdng.Current.CurrFrame - 150;
            if (FutureFrame < 0) FutureFrame = 0;
        }
    }

    public void Update()
    {

    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if (Recrdng.Current.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/
        return false;
    }

    public void Reset() { _trCam.parent = null; _trCam.GetComponent<Camera>().nearClipPlane = 0.3f; }

}
public class CamController_Wheelarch : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    int FutureFrame;
    Replayer _replayer;
    public CamController_Wheelarch(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        _trCam.parent = _car;
        _trCam.localPosition = Vector3.right * 1.5f + Vector3.up * 0.5f;
        _trCam.localRotation = Quaternion.identity;

        FutureFrame = Recrdng.Current.FrameCount - 1;
        if (_replayer.State == "Playing")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 150;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 50;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = Recrdng.Current.CurrFrame - 150;
            if (FutureFrame < 0) FutureFrame = 0;
        }
    }

    public void Update()
    {
        
    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if (Recrdng.Current.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/
        return false;
    }

    public void Reset() { _trCam.parent = null; _trCam.GetComponent<Camera>().nearClipPlane = 0.3f; }
}
public class CamController_Follow : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    Vector3 RelPos;
    int FutureFrame;
    Replayer _replayer;
    string HitCollidername;
    RaycastHit hit;
    

    public CamController_Follow(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        FutureFrame = _replayer.CurrFrame + 1;    //if replayer stopped
        if (_replayer.State == "Playing")
        {
            FutureFrame = _replayer.CurrFrame + 200;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = _replayer.CurrFrame - 200;
            if (FutureFrame < 0) FutureFrame = 0;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = _replayer.CurrFrame + 66;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }

        Vector3 StartPos = -_car.forward * 10f + Vector3.up * 10f;
        _trCam.position = _car.position + StartPos;
        _trCam.LookAt(_car.position);
        _trCam.GetComponent<Camera>().fieldOfView = 60;
        RelPos = StartPos;
    }
    public void Update()
    {
        try
        {
            _trCam.position = _car.position + RelPos;
            _trCam.LookAt(_car.position);
            RelPos = _trCam.position - _car.position;
        }
        catch { }
    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if ((_replayer.State == "Reverse" || _replayer.State == "Rewind") && _replayer.CurrFrame <= FutureFrame) rtn = true;
        if ((_replayer.State == "Playing" || _replayer.State == "PlayingSloMo") && _replayer.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/

        return false;
    }

    public void Reset() { }
}
public class CamController_Rotating : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    float camRotationSpeed;
    private bool _nearlyOutOfBounds = false;
    Vector3 RelPos;
    int FutureFrame;
    Replayer _replayer;
    string HitCollidername;
    RaycastHit hit;
    

    public CamController_Rotating(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;

    }
    public void Init(bool LerpToStart)
    {
        FutureFrame = Recrdng.Current.CurrFrame + 1;    //if replayer stopped
        if (_replayer.State == "Playing")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 200;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = Recrdng.Current.CurrFrame - 200;
            if (FutureFrame < 0) FutureFrame = 0;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 66;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }

        camRotationSpeed = Random.Range(0.3f, 0.6f);
        Vector3 RandomPos = Random.onUnitSphere * Random.Range(5f, 12f);
        RandomPos = new Vector3(RandomPos.x, Mathf.Abs(RandomPos.y), RandomPos.z);
        _trCam.position = _car.position + RandomPos;
        _trCam.LookAt(_car.position);
        _trCam.GetComponent<Camera>().fieldOfView = 60;
        RelPos = RandomPos;
    }
    public void Update()
    {
        try
        {
            _trCam.position = _car.position + RelPos;
            try
            {
                if (Physics.Raycast(_trCam.position, _car.position - _trCam.position, out hit, Vector3.Distance(_trCam.position, _car.position)))
                {
                    HitCollidername = hit.collider.name;
                }
                if (HitCollidername.Contains("Terrain"))
                {
                    camRotationSpeed = -camRotationSpeed;
                }
            }
            catch { }
            _trCam.RotateAround(_car.position, Vector3.up, camRotationSpeed);
            _trCam.LookAt(_car.position);
            RelPos = _trCam.position - _car.position;
        }
        catch { }
    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if ((_replayer.State == "Reverse" || _replayer.State == "Rewind") && Recrdng.Current.CurrFrame <= FutureFrame) rtn = true;
        if ((_replayer.State == "Playing" || _replayer.State == "PlayingSloMo") && Recrdng.Current.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/

        return false;
    }

    public void Reset() { }
}
public class CamController_Hedgehog : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    Vector3 CamStartPos;
    Vector3 CamEndPos;
    Replayer _replayer;
    private bool _nearlyOutOfBounds = false;
    int FutureFrame;
    int EndFrame;
    

    public CamController_Hedgehog(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        if (_replayer.State == "Playing" || _replayer.State == "PlayingSloMo")
        { FutureFrame = Recrdng.Current.CurrFrame + 200; EndFrame = FutureFrame + 50; }
        if (_replayer.State == "Rewind" || _replayer.State == "Reverse")
        { FutureFrame = Recrdng.Current.CurrFrame - 200; EndFrame = FutureFrame - 50; }
        FutureFrame = Mathf.Clamp(FutureFrame, 0, Recrdng.Current.FrameCount - 1);
        EndFrame = Mathf.Clamp(EndFrame, 0, Recrdng.Current.FrameCount - 1);
        List<RecFrameData> FrameData = Recrdng.Current.Data[FutureFrame];
        CamStartPos = FrameData[Recrdng.Current.PlayerCarId].Pos;
        Ray _r = new Ray(CamStartPos + Vector3.up * 0.2f, Vector3.down);
        int LayerMask = ~((1 << 8) + (1 << 10) + (1 << 13));
        RaycastHit hit;
        if (Physics.Raycast(_r, out hit, 2, LayerMask))
        {
            if (hit.collider.name.StartsWith("Road"))
            {
                XSec xs = Road.Instance.XSecs[System.Convert.ToInt16(hit.collider.name.Substring(7))];
                if (Vector3.Distance(CamStartPos, xs.KerbR) > Vector3.Distance(CamStartPos, xs.KerbL))
                {
                    CamEndPos = CamStartPos + (xs.MidLeft - CamStartPos).normalized * 2f + Vector3.up * 0.2f;
                    CamStartPos = xs.KerbR + Vector3.up * 0.2f;
                }
                else
                {
                    CamEndPos = CamStartPos + (xs.MidRight - CamStartPos).normalized * 2f + Vector3.up * 0.2f;
                    CamStartPos = xs.KerbL + Vector3.up * 0.2f;
                }
            }
            else
            { CamEndPos = CamStartPos + Vector3.right * 2; }
        }
        else
        { CamEndPos = CamStartPos + Vector3.right * 2; }
    }
    public void Update()
    {
        try
        {
            float frac = 1 - (float)(Mathf.Abs(Recrdng.Current.CurrFrame - FutureFrame)) / 200f;
            _trCam.position = Vector3.Lerp(CamStartPos, CamEndPos, frac);
            _trCam.LookAt(_car.position);
            _trCam.GetComponent<Camera>().fieldOfView = 60;
        }
        catch { }
    }

    public bool OutOfBounds()
    {
        bool rtn = false;
        if (Recrdng.Current.CurrFrame >= EndFrame) { rtn = true; }
        return rtn;
    }

    public void Reset() { }
}
public class CamController_RoadRunner : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    int FutureFrame;
    SpringJoint j;
    Vector3 _prevCarPos;
    Vector3 _followPos;
    Vector3 _prevFollowPos;
    Replayer _replayer;
    Vector3 _targetPos;
    Vector3 _lerpStartPos;
    Vector3 _currPos;
    float _lerpDist;
    List<RecFrameData> _playerFrames;


    public CamController_RoadRunner(Transform Cam, Transform Car, Replayer rep, List<RecFrameData> PlayerFrames)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
        _playerFrames = PlayerFrames;


    }
    public void Init(bool LerpToStart)
    {
        FutureFrame = Recrdng.Current.FrameCount - 1;
        //if (_replayer.State == "Playing")
        //{
        //    FutureFrame = Recrdng.Current.CurrFrame + 550;
        //    if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        //}
        if (_replayer.State == "Playing")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 500;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 250;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse")
        {
            FutureFrame = Recrdng.Current.CurrFrame - 550;
            if (FutureFrame < 0) FutureFrame = 0;
        }
        Vector3 RandomPos = _car.forward * -3f;
        _prevCarPos = _playerFrames[Recrdng.Current.CurrFrame - 1].Pos;
        Vector3 CarVel = _car.position - _prevCarPos;
        RandomPos = new Vector3(RandomPos.x, 1, RandomPos.z);
        _trCam.position = _car.position + RandomPos;
        _trCam.LookAt(_car.position);
        _trCam.GetComponent<Camera>().fieldOfView = 60;
        j = _trCam.gameObject.AddComponent<SpringJoint>();
        j.damper = 0.9f;
        j.maxDistance = 5;
        j.connectedBody = _car.GetComponent<Rigidbody>();
        j.GetComponent<Rigidbody>().mass = 10;
        _trCam.GetComponent<Rigidbody>().isKinematic = false;
        _trCam.GetComponent<Rigidbody>().velocity = -RandomPos + CarVel*3;
        CapsuleCollider cc = j.gameObject.AddComponent<CapsuleCollider>();
        cc.radius = 0.5f;
        cc.height = 0;
        cc.center = new Vector3(0, 0.4f, 0);
        cc.sharedMaterial = new PhysicMaterial();
        cc.sharedMaterial.dynamicFriction = 0;
        cc.sharedMaterial.staticFriction = 0;
        cc.sharedMaterial.bounciness = 0;
        cc.sharedMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        //j.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        //j.GetComponent<Rigidbody>().useGravity = true;
        Road.Instance.CreateRoofCollider();
    }
    public void Update()
    {
        if (Physics.Raycast(_trCam.position, _car.position - _trCam.position, Vector3.Distance(_trCam.position, _car.position), (1 << 10)))
            _trCam.Translate(Vector3.up * 0.2f);
       _trCam.LookAt(_car.position);
            //RelPos = _trCam.position - _car.position;
    }

    //bug two goTempRooves

    public bool OutOfBounds()
    {
        RaycastHit hit;
        if (Physics.Raycast(_trCam.position, Vector3.up, out hit, 3))
        {
            if (!hit.collider.name.StartsWith("goT"))
            { Debug.Log(hit.collider.name); return true; }
        }
        else
        {
            return true;
        }
        /*bool rtn = false;
        if (Recrdng.Current.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/
        return false;
    }

    public void Reset()
    {
        GameObject.Destroy(j.GetComponent<CapsuleCollider>());
        GameObject.Destroy(j);
        _trCam.GetComponent<Rigidbody>().isKinematic = true;
        j = null;
        Road.Instance.RemoveRoofCollider();
    }
}

public class CamController_Drone : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    Vector3 RelPos;
    int FutureFrame;
    Replayer _replayer;
    SpringJoint j;
    

    public CamController_Drone(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        float _maxY = 10;
        FutureFrame = Recrdng.Current.FrameCount - 1;    //if replayer stopped
        if (_replayer.State == "Playing")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 500;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
            List<List<RecFrameData>> Span = Recrdng.Current.Data.GetRange(Recrdng.Current.CurrFrame, FutureFrame - Recrdng.Current.CurrFrame);
            var SortedSpan = from d
                     in Span
                             orderby d[Recrdng.Current.PlayerCarId].Pos.y
                             select d;
            try { _maxY = SortedSpan.LastOrDefault()[Recrdng.Current.PlayerCarId].Pos.y; } catch { }
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = Recrdng.Current.CurrFrame + 200;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
            List<List<RecFrameData>> Span = Recrdng.Current.Data.GetRange(Recrdng.Current.CurrFrame, FutureFrame - Recrdng.Current.CurrFrame);
            var SortedSpan = from d
                     in Span
                             orderby d[Recrdng.Current.PlayerCarId].Pos.y
                             select d;
            try { _maxY = SortedSpan.LastOrDefault()[Recrdng.Current.PlayerCarId].Pos.y; } catch { }
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = Recrdng.Current.CurrFrame - 500;
            if (FutureFrame < 0) FutureFrame = 0;
        }

        Vector3 RandomPos = Random.onUnitSphere * Random.Range(0f, 10f);
        RandomPos = new Vector3(RandomPos.x, _maxY - _car.position.y + 3, RandomPos.z);
        _trCam.position = _car.position + RandomPos;
        _trCam.LookAt(_car.position);
        _trCam.GetComponent<Camera>().fieldOfView = 60;
        RelPos = RandomPos;
        j = _trCam.gameObject.AddComponent<SpringJoint>();
        j.damper = 0.7f;
        j.maxDistance = 7;
        j.connectedBody = _car.GetComponent<Rigidbody>();
        j.GetComponent<Rigidbody>().mass = 10;
        _trCam.GetComponent<Rigidbody>().isKinematic = false;
        CapsuleCollider cc = j.gameObject.AddComponent<CapsuleCollider>();
        cc.radius = 3;
        cc.height = 0;
        cc.sharedMaterial = new PhysicMaterial();
        cc.sharedMaterial.dynamicFriction = 0;
        cc.sharedMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        j.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        j.GetComponent<Rigidbody>().useGravity = false;
    }
    public void Update()
    {
        try
        {
            //_trCam.position = _car.position + RelPos;
            _trCam.LookAt(_car.position);
            //RelPos = _trCam.position - _car.position;
        }
        catch { }
    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if (Recrdng.Current.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/

        return false;
    }

    public void Reset()
    {
        GameObject.Destroy(j.GetComponent<CapsuleCollider>());
        GameObject.Destroy(j);
        _trCam.GetComponent<Rigidbody>().isKinematic = true;
        j = null;
    }
}

public class CamController_Side : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    private bool _nearlyOutOfBounds = false;
    Vector3 RelPos;
    int FutureFrame;
    Replayer _replayer;
    string HitCollidername;
    RaycastHit hit;


    public CamController_Side(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }
    public void Init(bool LerpToStart)
    {
        FutureFrame = _replayer.CurrFrame + 1;    //if replayer stopped
        if (_replayer.State == "Playing")
        {
            FutureFrame = _replayer.CurrFrame + 200;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }
        if (_replayer.State == "Reverse" || _replayer.State == "Rewind")
        {
            FutureFrame = _replayer.CurrFrame - 200;
            if (FutureFrame < 0) FutureFrame = 0;
        }
        if (_replayer.State == "PlayingSloMo")
        {
            FutureFrame = _replayer.CurrFrame + 66;
            if (FutureFrame >= Recrdng.Current.FrameCount) FutureFrame = Recrdng.Current.FrameCount - 1;
        }

        Vector3 StartPos = -_car.right * 10f + Vector3.up * 3f;
        _trCam.position = _car.position + StartPos;
        _trCam.LookAt(_car.position);
        _trCam.GetComponent<Camera>().fieldOfView = 60;
        RelPos = StartPos;
    }
    public void Update()
    {
        try
        {
            _trCam.position = _car.position + RelPos;
            _trCam.LookAt(_car.position);
            RelPos = _trCam.position - _car.position;
        }
        catch { }
    }

    public bool OutOfBounds()
    {
        /*bool rtn = false;
        if ((_replayer.State == "Reverse" || _replayer.State == "Rewind") && _replayer.CurrFrame <= FutureFrame) rtn = true;
        if ((_replayer.State == "Playing" || _replayer.State == "PlayingSloMo") && _replayer.CurrFrame >= FutureFrame)
        {
            rtn = true;
        }*/

        return false;
    }

    public void Reset() { }
}

#region StopCam - Not used
public class CamController_Stop : iReplayCamController
{
    Transform _car;
    BoxCollider _collCar;
    Transform _trCam;
    Vector3 FutureCarPos;
    Vector3 CamPos;
    Replayer _replayer;
    private bool _nearlyOutOfBounds = false;
    int FutureFrame;
    int EndFrame;
    GameObject _goCamBox;
    Rigidbody _rbCamBox;
    GameObject _tripod;
    Transform _ccf;
    CarStop _stp;
    public CamController_Stop(Transform Cam, Transform Car, Replayer rep, CarStop stp)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
        _stp = stp;
        FutureFrame = stp.EndFrame;
        _ccf = _trCam.parent;
        _goCamBox = new GameObject();
        _goCamBox.transform.position = stp.CamPos;
        _goCamBox.transform.localRotation = Quaternion.identity;
        _trCam.position = stp.CamPos;
        _trCam.SetParent(_goCamBox.transform);
        _trCam.LookAt(stp.LookPos);
        _goCamBox.layer = 14;
        BoxCollider c = _goCamBox.AddComponent<BoxCollider>();
        c.center = Vector3.zero;
        c.size = new Vector3(0.1f, stp.TripodHeight * 2, 0.1f);
        _rbCamBox = _goCamBox.AddComponent<Rigidbody>();
        _rbCamBox.isKinematic = true;
        _tripod = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _tripod.transform.localScale = new Vector3(1, 0.2f, 1);
        _tripod.transform.position = stp.CamPos - (stp.TripodHeight - 0.052f) * Vector3.up;
        _tripod.GetComponent<MeshRenderer>().enabled = false;
        _collCar = _car.gameObject.AddComponent<BoxCollider>();
        _collCar.center = Vector3.up * 0.8f;
        _collCar.size = new Vector3(2, 1.2f, 4);

        Debug.Log("StopJit constructor TripodHeight = " + stp.TripodHeight.ToString() + " " + Cam.name);
    }
    public void Init(bool LerpToStart)
    {
        Debug.Log("StopJit Init");
        if (_replayer.State == "Playing" || _replayer.State == "PlayingSloMo")
            EndFrame = FutureFrame + 50;
        if (_replayer.State == "Rewind" || _replayer.State == "Reverse")
        { FutureFrame = Recrdng.Current.CurrFrame - 100; EndFrame = FutureFrame - 50; }
        FutureFrame = Mathf.Clamp(FutureFrame, 0, Recrdng.Current.FrameCount - 1);
        EndFrame = Mathf.Clamp(EndFrame, 0, Recrdng.Current.FrameCount - 1);
        _trCam.GetComponent<Camera>().fieldOfView = 90;
        _trCam.GetComponent<Camera>().nearClipPlane = 0.1f;
    }
    public void Update()
    {
        if ((_car.position - _trCam.position).sqrMagnitude < 4 && _rbCamBox.isKinematic)
        {
            _rbCamBox.isKinematic = false;
            //float speed = Vector3.Distance(_car.position, _stp.StopPos);
            //Debug.Log("Camhitspeed " + speed.ToString());
            //_rbCamBox.AddForce((_trCam.position - _car.position).normalized*speed);
            EndFrame += 50;
        }
    }

    public bool OutOfBounds()
    {
        bool rtn = false;
        if (Recrdng.Current.CurrFrame >= EndFrame) { rtn = true; }
        return rtn;
    }

    public void Reset()
    {
        _trCam.SetParent(_ccf);
        GameObject.Destroy(_goCamBox);
        GameObject.Destroy(_tripod);
        GameObject.Destroy(_collCar);
        _trCam.GetComponent<Camera>().nearClipPlane = 0.3f;
        _trCam.GetComponent<Camera>().fieldOfView = 60;
    }
}

public struct CarStop
{
    public string CamType;
    public int StartFrame;
    public int EndFrame;
    public Vector3 CamPos;
    public Vector3 StopPos;
    public Vector3 LookPos;
    public float TripodHeight;
}

public struct CarStopFrame
{
    public int Frame;
    public float VelocityDelta;
    public float RelativeSpeed;
}
#endregion

public class CamController_Ice : iReplayCamController
{
    Transform _car;
    Transform _trCam;
    float camRotationSpeed;
    Replayer _replayer;
    string HitCollidername;
    int FrameCount = 0;
    RaycastHit hit;
    Vector3 RotatePos;
    Vector3 RotateAxis;
    Transform trCyl;
    Rigidbody rbCyl;
    bool CylLanded = false;
    RaycastHit hx, hmx, hz, hmz;
    

    public CamController_Ice(Transform Cam, Transform Car, Replayer rep)
    {
        _car = Car;
        _trCam = Cam;
        _replayer = rep;
    }

    public void Init(bool LerpToStart)
    {
        _replayer.Freeze(true);
        camRotationSpeed = 40f;
        //_trCam.GetComponent<Camera>().fieldOfView = 60;
        int CarsInView = 0;
        Vector3 Sum = Vector3.zero;
        foreach(RecFrameData d in Recrdng.Current.Data[Recrdng.Current.CurrFrame])
        {
            if ((_car.position - d.Pos).sqrMagnitude < 100)
            { CarsInView++; Sum += d.Pos; }
        }
        RotatePos = Sum / CarsInView;
        /*
        //Find out roughly what is underneath the RotatePos so we know what angle to set the cylinder at
        if(Physics.Raycast(RotatePos + Vector3.up*5 + Vector3.right*10, Vector3.down, out hx)
            && Physics.Raycast(RotatePos + Vector3.up * 5 + Vector3.left * 10, Vector3.down, out hmx)
            && Physics.Raycast(RotatePos + Vector3.up * 5 + Vector3.forward * 10, Vector3.down, out hz)
            && Physics.Raycast(RotatePos + Vector3.up * 5 + Vector3.back * 10, Vector3.down, out hmz))
            {
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = hx.point;
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = hmx.point;
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = hz.point;
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = hmz.point;
            List<Vector3> pts = new List<Vector3> { hz.point, hx.point, hmz.point, hmx.point };
            pts.Remove(pts.OrderBy(p => p.y).First());
            Plane pln = new Plane(pts[0], pts[1], pts[2]);
            RotateAxis = pln.normal;
        }
        */
        //RotatePos = new Vector3(RotatePos.x, _car.position.y+10, RotatePos.z);
        trCyl = ((GameObject)GameObject.Instantiate(Resources.Load("Prefabs/FallingCylinder"))).transform;
        trCyl.position = RotatePos;
        trCyl.rotation = Quaternion.LookRotation(Vector3.forward, RotateAxis);
        _trCam.parent = trCyl;
        //FallingCollider Cylinder has a radius 10m
    }
    public void Update()
    {
         trCyl.Rotate(Vector3.up, camRotationSpeed * Time.deltaTime, Space.Self);
        _trCam.LookAt(_car.position);
        FrameCount++;

    }

    public bool OutOfBounds()
    {
        bool rtn = false;
        if (FrameCount > 50)
        {
            rtn = true;
        }
        return rtn;
    }

    public void Reset() { _trCam.parent = null; _replayer.Freeze(false); GameObject.Destroy(trCyl.gameObject); }
}