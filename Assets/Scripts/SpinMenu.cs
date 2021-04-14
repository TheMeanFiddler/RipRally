using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public interface IMenuDataItem { }

public interface IMenuListView
{
    void GetData();
    void PopulateMenu<T>() where T : IMenuItem, new();
    void Select(IMenuItem Itm);
    void Delete(int GameId);
}

class SpinMenu: MonoBehaviour, IMenuListView
{
    protected iMain mainscript;
    protected List<IMenuDataItem> data;
    public int ItemCount { get; set; }
    protected List<IMenuItem> MenuItems = new List<IMenuItem>();
    public IMenuItem SelectedItem { get; set; }
    protected bool ExtraItemAtStart { get; set; }
    protected List<float> Angles = new List<float>(); //Keeps a list of all the items rotations starting with NewTrack at 0
    bool _dragging = false;
    bool _mouseUp = false;
    float CylinderAngleOnMouseDown;
    float MouseDownAngle;
    float PrevMouseX;
    bool Lerping = false;
    float LerpStartTime;
    Quaternion LerpStartQuat;
    Quaternion LerpEndQuat;
    float TargetAngle;
    bool Snapped = true;
    bool RotFwd = false;
    public float Spacing=20;
    public bool SpinLocked { get; set; }

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        MenuItems.Clear();
        Angles.Clear();
    }

    public virtual void GetData()
    {}

    public virtual void PopulateMenu<T>() where T : IMenuItem, new()
    {
        int InvIdx;
        ItemCount = data.Count;
        for (int Idx = data.Count-1; Idx >= 0; Idx--)
        {
            InvIdx = data.Count - Idx;
            IMenuDataItem mdi = data[Idx];
            IMenuItem SavedItm = new T();
            SavedItm.Populate(mdi); //Sets the name, Id and IsMine from the data
            SavedItm.ScrollIdx = InvIdx;
            //if (ExtraItemAtStart) SavedItm.ScrollIdx++;
            MenuItems.Add(SavedItm);
            //Initialize the item
            SavedItm.ContainingMenu = this;
                                                        //SavedItm.SetName(mdi.Name);
            SavedItm.Data = mdi;
            
            Transform trSavedItem = SavedItm.OuterGameObject.transform;
            trSavedItem.SetParent(this.transform);
            Quaternion _q = Quaternion.AngleAxis(Spacing * InvIdx, Vector3.down);
            Angles.Add(_q.eulerAngles.y);
            trSavedItem.localPosition = _q * Vector3.back * 900;
            //SavedGameImg.transform.SetParent(CntPnl);
            trSavedItem.localScale = Vector3.one*3;
        }

    }

    void OnMouseDown()
    {
        if (SpinLocked) return;
        PrevMouseX = Input.mousePosition.x;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.name == gameObject.name)
            {
                Vector2 MDP2D = new Vector2(hit.point.x, hit.point.z);
                MouseDownAngle = Vector2.Angle(MDP2D, -Vector2.up);
                if (Vector3.Cross(MDP2D, Vector2.up).z > 0)
                    MouseDownAngle = -MouseDownAngle;
                CylinderAngleOnMouseDown = transform.rotation.eulerAngles.y;
            }
        }
        _dragging = true;
    }

    void OnMouseUp()
    {
        _dragging = false;
        _mouseUp = true;
        Snapped = false;
    }
    void Update()
    {
        float CylAngle = transform.rotation.eulerAngles.y;
        int ClosestScrollIdx = (int)(CylAngle / Spacing + 0.5f);
        if (ClosestScrollIdx > ItemCount+5) ClosestScrollIdx = 0;   //5 is a big number - otherwise it jumps back to the new track
        else
        if (ClosestScrollIdx > ItemCount) ClosestScrollIdx = ItemCount;
        
        if (!Lerping) TargetAngle = 360 - Angles[ClosestScrollIdx];
        if (_dragging)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                if (hit.collider.name == gameObject.name)
                {
                    transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
                    transform.Rotate(Vector3.up, CylinderAngleOnMouseDown + 180);
                    transform.Rotate(Vector3.up, -MouseDownAngle);
                }
            }
        }
        if (_mouseUp)
        {
                float v = (Input.mousePosition.x - PrevMouseX) * -0.02f;
                GetComponent<Rigidbody>().angularVelocity = new Vector3(0,v,0);    
            //rigidbody.AddTorque(0, v, 0);
                Snapped = false;
                _mouseUp = false;
        }

        //Pick this up next frame to see how fast the mouse has moved
        PrevMouseX = Input.mousePosition.x;

        //spin each image as it rotates
        for (int s = 0; s < ItemCount+1; s++)
        {
            float AngleToFwd = (Angles[s] + CylAngle) % 360;
            if (AngleToFwd > 180) AngleToFwd = AngleToFwd - 360;
            float SpinAngle = (AngleToFwd * 4.5f);
            //if (s % 2 == 1) A += 90;
            MenuItems[s].OuterGameObject.transform.rotation = Quaternion.Euler(30, SpinAngle , 0);
        }

        //stop when you get to the end
        if (transform.rotation.eulerAngles.y > 360-Angles[ItemCount] && transform.rotation.eulerAngles.y + 180 < 360-Spacing/2)
        {
            transform.rotation = Quaternion.Euler(0, 360-Angles[ItemCount], 0);
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Select(MenuItems[MenuItems.Count-1]);
            Snapped = true;
            Lerping = false;
        }

        //stop when you get to the beginning
        if (transform.rotation.eulerAngles.y + 180 + Spacing/2 < 0)
        {
            Debug.Log("GotToStart");
            transform.rotation = Quaternion.Euler(0, 180, 0);
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        //If its freely spinning and slowing down check which is the nearest item and start lerping towards it
        if (Lerping == false && GetComponent<Rigidbody>().angularVelocity.magnitude < 0.2f && Snapped == false)
        {
            if (TargetAngle < 0) TargetAngle += 360;
            LerpStartQuat = transform.rotation;
            LerpEndQuat = Quaternion.Euler(0, TargetAngle, 0);
            LerpStartTime = Time.time;
            Lerping = true;
            //Debug.Log("ClosestScrollIdx = " + ClosestScrollIdx);
            Select(MenuItems.FirstOrDefault(m=>m.ScrollIdx==ClosestScrollIdx));
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (Lerping)
        {
            //do the lerp
            transform.rotation = Quaternion.Slerp(LerpStartQuat, LerpEndQuat, (Time.time-LerpStartTime)*7f);
            //got there
            float diff = transform.rotation.eulerAngles.y - LerpEndQuat.eulerAngles.y;
            if(Mathf.Abs(diff)<=1)
            {
                transform.rotation = LerpEndQuat;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                Lerping = false;
                Snapped = true;
            }
            
        }
    }

    public void SnapTo(int Idx)
    {
        int ScrollIdx = MenuItems[Idx].ScrollIdx;
        transform.rotation=Quaternion.Euler(0,(ScrollIdx) *Spacing,0);
        Select(MenuItems[Idx]);
        Snapped = true;
        Lerping = false;
    }


    public virtual void Select(IMenuItem itm)
    {
        if (itm.Deleted) { SelectedItem = null; return; }
        foreach (IMenuItem i in MenuItems)
        {
            if (i == itm) { i.Select(); SelectedItem = itm; }
            else if (!i.Deleted) i.Deselect();
       }

    }

    public void Delete(int GameId)
    {
        throw new System.NotImplementedException();
    }

}

public class MenuDataItem : IMenuDataItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsMine { get; set; }
}

