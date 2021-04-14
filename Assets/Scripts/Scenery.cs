using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public sealed class Scenery
{
    public List<PlaceableObject> Objects;
    
    //internal instance
    static Scenery _instance;
    static readonly object padlock = new object();
    //THis is where it instantiates itself
    public static Scenery Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new Scenery(); }
                return _instance;
            }
        }
    }

    public Scenery()
    {
        Objects = new List<PlaceableObject>();
    }

    public void PlaceObjects()
    {
        foreach (PlaceableObject PO in Objects)
        {
            PO.PlaceObject();
        }
    }

    public void RemoveObjects()
    {
        foreach (PlaceableObject PO in Objects)
        {
            PO.RemoveGameObject();
        }
        Objects.Clear();
    }

}

//********************************************************************************

public abstract class PlaceableObject : System.IDisposable
{
    public string prefab { get; set; }
    public string name { get; set; }
    public GameObject gameObject;
    private Camera FPCam;
    public Vector3 Pos;
    public Vector3 Scale;
    public Quaternion Rot;
    private ToolboxController Toolbox;
    public static PlaceableObject Current;
    public bool Chargeable { get; set; }
    public ToolOption Opt { get; set; }
    Material mtlClickCollider;
    public bool RotateTop { get; set; }
    public event GizmoEventHandler OnGizmoMoved;

    //contructor
    public PlaceableObject()
    {
        mtlClickCollider = (Material)Resources.Load("Prefabs/Gizmos/Materials/ClickCollider");
    }

    public void Init()
    {
        
    }

    public void PlaceObject()
    {
        if (gameObject == null)
        {
            UnityEngine.Object objPrefab = Resources.Load(prefab);
            if(objPrefab == null) objPrefab = Resources.Load("Prefabs/Scenery/MissingModel");
            gameObject = (GameObject)Object.Instantiate(objPrefab, Vector3.zero, Quaternion.identity);
            //name = prefab;
            gameObject.name = name;
            foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
            {
                CO.PlaceableObject = this;  //This gets removed OnDestroy
            }
        }

        gameObject.transform.position = Pos;
        gameObject.transform.localScale = Scale;
        gameObject.transform.rotation = Rot;
        if (PlayerManager.Type != "BuilderPlayer") DisableClickColliders();

        aSceneryComponent sc = gameObject.GetComponentInChildren<aSceneryComponent>();
        if (sc != null) sc.Init(this);

    }


    public void PlaceObject(Vector3 Pos, Quaternion Rot, Vector3 scale)
    {
        this.Pos = Pos;
        this.Rot = Rot;
        Scale = scale;
        if (gameObject == null)
        {
            UnityEngine.Object objPrefab = Resources.Load(prefab);
            if (objPrefab == null) objPrefab = Resources.Load("Prefabs/Scenery/MissingModel");
            gameObject = (GameObject)Object.Instantiate(objPrefab, Vector3.zero, Quaternion.identity);
            gameObject.transform.localScale = scale;
            gameObject.name = name;
            foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
            {
                CO.PlaceableObject = this;
            }

        }
        gameObject.transform.position = this.Pos;
        TopRotator tr = gameObject.GetComponent<TopRotator>();
        try{ tr.RotateTop(this.Rot);  }
        catch { gameObject.transform.rotation = this.Rot; }
        if (PlayerManager.Type != "BuilderPlayer") DisableClickColliders();

        aSceneryComponent ss = gameObject.GetComponentInChildren<aSceneryComponent>();
        if (ss != null) ss.Init(this);

    }

    public void PlaceObject(Vector3 Pos, Vector3 LookAt, Vector3 scale)
    {
        this.Pos = Pos;
        if (gameObject == null)
        {
            UnityEngine.Object objPrefab = Resources.Load(prefab);
            if (objPrefab == null) objPrefab = Resources.Load("Prefabs/Scenery/MissingModel");
            gameObject = (GameObject)Object.Instantiate(objPrefab, Vector3.zero, Quaternion.identity);
            gameObject.name = name;
            foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
            { CO.PlaceableObject = this; }
        }
        gameObject.transform.position = this.Pos;
        gameObject.transform.LookAt(LookAt);
        Rot = gameObject.transform.rotation;
        Scale = scale;
        if (PlayerManager.Type != "BuilderPlayer") { DisableClickColliders(); }

        aSceneryComponent ss = gameObject.GetComponentInChildren<aSceneryComponent>();
        if (ss != null) ss.Init(this);
    }

    public void Select()
    {
        GameObject Canvas = GameObject.Find("BuilderGUICanvas(Clone)");
        //If you've clicked on a UI element then you didn't mean to click the terrain
        for (int i = 0; i < Canvas.transform.childCount; i++)
        {
            if (Canvas.transform.GetChild(i).name == "RR") break;   //because RR covers the whole screen
            RectTransform RT = Canvas.transform.GetChild(i).GetComponent<RectTransform>();
            Vector3[] worldCorners = new Vector3[4];
            RT.GetWorldCorners(worldCorners);
            if (Input.mousePosition.x >= worldCorners[0].x && Input.mousePosition.x < worldCorners[2].x
                                    && Input.mousePosition.y >= worldCorners[0].y && Input.mousePosition.y < worldCorners[2].y)
            {
                return;
            }
        }
        foreach (PlaceableObject p in Scenery.Instance.Objects)
        {
            p.HideAllGizmos();
            p.DeselectClickColliders();
        }
        try { BezCtrlPt.Current.goRdMkr.GetComponent<RoadMarker>().Gizmo.SetActive(false); }
        catch { }
        Toolbox = Canvas.GetComponent<ToolboxController>();
        Toolbox.SetToolToggle("Scenery", false);
        ShowGizmo(Toolbox.Gizmo);
        Current = this;
        SelectClickColliders();
    }

    public void ShowGizmo(string Gz)
    {
        HideAllGizmos();
        if (PlayerManager.Type != "BuilderPlayer") return;
        UnityEngine.Object objGizmo = Resources.Load("Prefabs/Gizmos/Gizmo" + Gz);
        GameObject Gizmo = (GameObject)Object.Instantiate(objGizmo, Vector3.zero, Quaternion.identity);
        Gizmo.name = "Gizmo" + Gz;
        Gizmo.transform.SetParent(gameObject.transform);
        if (Gz == "Rotate" || Gz == "Scale") Gizmo.transform.rotation = gameObject.transform.rotation;
        if (Gz == "Scale") Gizmo.transform.localScale = VectGeom.Reciprocal(gameObject.transform.localScale);
        aGizmo G = Gizmo.GetComponent<aGizmo>();
        G.OnGizmoMoved += G_OnGizmoMoved;
        G.AttachedTransform = gameObject.transform;
        G.AttachedObject = this;


        //this version puts the gizmo wherever you clicked on the collider 
        RaycastHit Hit;
        FPCam = GameObject.Find("BuilderCamera").GetComponent<Camera>();
        Ray R = FPCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(R, out Hit) && Hit.collider.transform.parent == gameObject.transform)  //if clicked on this object
        {
            //put the gizmo where you clicked
            Gizmo.transform.position = Hit.point;
        }
        else
        {
            Collider TopC = gameObject.GetComponentsInChildren<Collider>().OrderBy(c => c.enabled == false ? -1000 : c.transform.position.y + c.bounds.max.y).LastOrDefault();
            Gizmo.transform.position = TopC.transform.position + new Vector3(0, TopC.bounds.extents.y, 0);
        }
        if (Gz == "Move" || Gz == "Scale") { Gizmo.transform.position += Vector3.up * 2; }
        G.SetHoverPos(Gizmo.transform.position - gameObject.transform.position);

    }
    
    private void G_OnGizmoMoved()
    {
        // Forwards the event to the SceneryComponent
        Debug.Log("Placeable sez GizMov");
        if (OnGizmoMoved != null) OnGizmoMoved();
    }

    public void HideAllGizmos()
    {
        aGizmo G = gameObject.GetComponentInChildren<aGizmo>();
        if (G == null) { return; }
        G.OnGizmoMoved -= G_OnGizmoMoved;
        G.transform.SetParent(null);
        GameObject.Destroy(G.gameObject);
    }

    public void DisableClickColliders()
    {
        foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
        {
            CO.enabled = false;
            CO.GetComponent<Collider>().enabled = false;
            CO.GetComponent<Renderer>().enabled = false;
        }
    }
    public void EnableClickColliders()
    {
        foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
        {
            CO.enabled = true;
            CO.Init();
            CO.GetComponent<Collider>().enabled = true;
            CO.GetComponent<Renderer>().enabled = true;
        }
    }

    public void SelectClickColliders()
    {
        foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
        {
            CO.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Prefabs/Gizmos/Materials/ClickColliderSelected");
        }
    }

    public void DeselectClickColliders()
    {
        foreach (ClickableObject CO in gameObject.transform.GetComponentsInChildren<ClickableObject>())
        {
            CO.GetComponent<Renderer>().sharedMaterial = mtlClickCollider;
        }
    }

    public void Delete()
    {
        Scenery.Instance.Objects.Remove(this);
        RemoveGameObject();
    }

    public void RemoveGameObject()
    {
        GameObject.Destroy(gameObject);
    }

    public void Dispose()
    {
        Dispose(true);
        System.GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool b)
    {
        HideAllGizmos();
        RemoveGameObject();
        System.GC.Collect();
        Toolbox = null;
        mtlClickCollider = null;
    }

}



public class StartingLine : PlaceableObject
{

    public StartingLine(string Name)
    {
        name = Name;
        prefab = "Prefabs/StartingLine";
    }

}
public class SceneryObject : PlaceableObject
{
    public SceneryObject(ToolOption opt)
    {
        prefab = "Prefabs/Scenery/" + opt.Name;
        name = opt.Name;
        Opt = opt;
    }

    public SceneryObject(string Name)
    {
        name = Name;
        prefab = "Prefabs/Scenery/" + Name;
    }

}



