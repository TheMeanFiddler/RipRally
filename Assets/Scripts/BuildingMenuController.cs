using System;
using UnityEngine;
using UnityEngine.UI;

class BuildingMenuController : MonoBehaviour
{
    public GameObject goMenuPanel;
    Button DriveButton;
    Button HomeButton;
    Toggle MenuToggle;
    GameObject GUICanvas;
    Button DeleteButton;
    Button InsertButton;
    Button TestButton;
    RoadBuilder _roadBuilder;

    void Start()
    {
        goMenuPanel.SetActive(false);
        HomeButton = transform.Find("MenuPanel/HomeButton").GetComponent<Button>();
        DriveButton = transform.Find("MenuPanel/DriveButton").GetComponent<Button>();
        MenuToggle = transform.Find("MenuToggle").GetComponent<Toggle>();
        DeleteButton = transform.Find("DeleteButton").GetComponent<Button>();
        InsertButton = transform.Find("InsertButton").GetComponent<Button>();

        _roadBuilder = GameObject.Find("BuilderPlayer(Clone)").GetComponent<RoadBuilder>();
        HomeButton.onClick.AddListener(() => GotoHome());
        DeleteButton.onClick.AddListener(() => _roadBuilder.DeleteRoadSectn());
        InsertButton.onClick.AddListener(() => _roadBuilder.InsertSection());
        //if (GameData.current.Filename==null) { DriveButton.interactable = false; }  //Until you save the track, the drive button hasn't worked out the fence colliders or something

        //Stop them editing other peoples tracks
        if (GameData.current != null && GameData.current.MacId != SystemInfo.deviceUniqueIdentifier)
        {
            foreach (Transform c in transform)
            {
                if (c.name != "MenuToggle" && c.name != "DPad(Clone)")
                    c.gameObject.SetActive(false);
            }
        }
    }

    private void GotoHome()
    {
        if (Game.current.Dirty) _roadBuilder.TakePlanViewScreenshot();
        Main.Instance.GotoHome();
    }

    public void ShowMenuPanel(bool Show)
    {
        goMenuPanel.SetActive(Show);
    }

    public void Save()
    {
        _roadBuilder.TakePlanViewScreenshot();
        ShowMenuPanel(false); //dont need cos TakePlanViewScreenshot hides the canvas
        UnityEngine.Object objdlgSave = Resources.Load("Prefabs/dlgSave");
        GameObject goSave = (GameObject)GameObject.Instantiate(objdlgSave, Vector3.zero, Quaternion.identity, this.transform);
        goSave.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void TestDrive()
    {
        PlayerManager.Type = "CarPlayer";
        Camera _followCam = GameObject.Find("FollowCamInner").GetComponent<Camera>();
        _followCam.enabled = true;
        BuildingPlayManager.Current = null;
        Destroy(GameObject.Find("BuilderPlayer(Clone)"));
        Destroy(this.gameObject);
        Road.Instance.CalculateBends();
        DrivingPlayManager DPM = new DrivingPlayManager();
        DrivingPlayManager.Current = DPM;
        DPM.PlayOffline(Main.Instance.Vehicle, Main.Instance.Color);
        CamSelector.Instance.SelectCam("FollowCamSwing");
        BezierLine.Instance.DeleteAllRoadMarkers();
        BezierLine.Instance.EraseLine();
        foreach (PlaceableObject PO in Scenery.Instance.Objects)
        {
                PO.DisableClickColliders();
                PO.HideAllGizmos();
        }

        foreach (RoadSectn RS in Road.Instance.Sectns)
        {
            RS.BuildFenceColliderTriangles();
            foreach (iRoadSeg s in RS.Segments)
            {
                s.CreateFenceColliderVerts();
            }
            if (RS.goSectn != null)
            {
                MeshCollider[] C = RS.goSectn.GetComponents<MeshCollider>();
                foreach(MeshCollider mc in C)
                {
                    Destroy(mc);
                }
            }
            RS.CreateFenceColliders();
        }
        Race.Current.ReadySteady();
    }


        void OnDestroy()
    {
        DeleteButton.onClick.RemoveAllListeners();
        InsertButton.onClick.RemoveAllListeners();
        //TestButton.onClick.RemoveAllListeners();
        HomeButton.onClick.RemoveAllListeners();
    }
}



