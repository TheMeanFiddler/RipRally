using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RoadMarker : MonoBehaviour
{
    public int Index;
    public RoadBuilder RoadBuilder;
    private BuilderController _bc;
    public Vector3 DroppedPosition;
    private Camera FPCam;
    public BezCtrlPt CtrlPt {get; set;}
    public bool Selected {get;set;}
    public bool Released = false;
    public GameObject Gizmo;
    public GameObject GizmoBankR;
    public GameObject GizmoBankL;

    void Awake()
    {
        RoadBuilder = GameObject.Find("BuilderPlayer(Clone)").GetComponent<RoadBuilder>();
        _bc = RoadBuilder.GetComponent<BuilderController>();
        DroppedPosition = transform.position;
        Gizmo = transform.Find("RoadMarkerGizmo").gameObject;
        Gizmo.SetActive(false);
        GizmoBankR = transform.Find("GizmoBankR").gameObject;
        GizmoBankR.SetActive(false);
        GizmoBankL = transform.Find("GizmoBankL").gameObject;
        GizmoBankL.SetActive(false);
    }

    void OnMouseUp()
    {
        if (Index == 1 && Road.Instance.IsCircular == false)
        {
            RoadBuilder.JoinSection(DroppedPosition);
        }
        else
        Select();
    }

 

    public void DropMarker()
    {
        if (!MouseIsOverUIElement())
        {
            if(Index==1 && Road.Instance.IsCircular == false)
            {
                RoadBuilder.JoinSection(DroppedPosition);
            }
            else if (Index ==1 && Road.Instance.IsCircular)
            {
                RoadBuilder.DropLoopMarker(this);
                Select();
            }
            else
            {
                RoadBuilder.DropMarker(this);
                Select();
            }
            
            Game.current.Dirty = true;
        }
    }

    public void Select()
    {
        try
        {
            PlaceableObject.Current.HideAllGizmos();
            PlaceableObject.Current.EnableClickColliders();
        }
        catch (System.Exception e) { }
        Material[] Mats;
        Material Mat;
        //Deselect the current roadmarker
        if (BezCtrlPt.Current != null)
        {
            if (BezCtrlPt.Current.goRdMkr != null)
            {
                Mats = BezCtrlPt.Current.goRdMkr.GetComponentInChildren<MeshRenderer>().sharedMaterials;
                Mat = (Material)Resources.Load("Prefabs/Materials/Orange", typeof(Material));
                Mats[1] = Mat;
                BezCtrlPt.Current.goRdMkr.GetComponentInChildren<MeshRenderer>().sharedMaterials = Mats;
                BezCtrlPt.Current.goRdMkr.GetComponent<RoadMarker>().Gizmo.SetActive(false);
                BezCtrlPt.Current.goRdMkr.GetComponent<RoadMarker>().GizmoBankL.SetActive(false);
                BezCtrlPt.Current.goRdMkr.GetComponent<RoadMarker>().GizmoBankR.SetActive(false);
                BezCtrlPt.Current.goRdMkr.GetComponent<RoadMarker>().Selected = false;
            }
        }
        Selected = true;
        CtrlPt.Select();    //sets th Current CtrlPt and Current Line;
        Mats = BezCtrlPt.Current.goRdMkr.GetComponentInChildren<MeshRenderer>().sharedMaterials;
        Mat = (Material)Resources.Load("Prefabs/Materials/BrightGreen", typeof(Material));
        Mats[1] = Mat;
        GetComponentInChildren<MeshRenderer>().sharedMaterials = Mats;
        Gizmo.SetActive(true);
        Gizmo.transform.rotation = Quaternion.identity;
        if (CtrlPt.SegStartIdx < Road.Instance.XSecs.Count)     //put this in because the GizmoBank was failing on the last CtrlPt
        {
            GizmoBankR.SetActive(true);
            GizmoBankR.GetComponent<GizmoBank>().Init();
            GizmoBankL.SetActive(true);
            GizmoBankL.GetComponent<GizmoBank>().Init();
            _bc.LerpTowards(BezCtrlPt.Current.Pos + Vector3.up * 10);
        }
        ToolboxController tbc = GameObject.Find("BuilderGUICanvas(Clone)").GetComponent<ToolboxController>();
        tbc.SetToolToggle("RoadSectn", false);
        tbc.ShowInsertButton(true);
        if(Road.Instance.Sectns[CtrlPt.CtrlPtId].RoadMaterial!=null)
        tbc.ShowRoadToolOptionForSelectedSection(ShopItemType.Road, Road.Instance.Sectns[CtrlPt.CtrlPtId].RoadMaterial);
        tbc.ShowRoadToolOptionForSelectedSection(ShopItemType.Fence, Road.Instance.Sectns[CtrlPt.CtrlPtId].LFenceType,"L");
        tbc.ShowRoadToolOptionForSelectedSection(ShopItemType.Fence, Road.Instance.Sectns[CtrlPt.CtrlPtId].RFenceType, "R");
    }
    /// <summary>
    /// If you've clicked on a UI element then you didn't mean to click the road marker
    /// </summary>
    public bool MouseIsOverUIElement()      //todo move this to somewhere more generally accessible
    {
        //
        bool rtn = false;
        GameObject Canvas = GameObject.Find("BuilderGUICanvas(Clone)");
        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            if (Canvas.transform.GetChild(i).name == "RR") break;   //because RR covers the whole screen
            RectTransform RT = Canvas.transform.GetChild(i).GetComponent<RectTransform>();
            Vector3[] worldCorners = new Vector3[4];
            RT.GetWorldCorners(worldCorners);
            if (Input.mousePosition.x >= worldCorners[0].x && Input.mousePosition.x < worldCorners[2].x
                                    && Input.mousePosition.y >= worldCorners[0].y && Input.mousePosition.y < worldCorners[2].y)
            {
                rtn = true;
            }
        }
        return rtn;
    }
}

public class JumpMarker : RoadMarker
{
    public float Height;
}