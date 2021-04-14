using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


class BumpMenu: MonoBehaviour, IMenuListView
{
    protected iMain mainscript;
    protected List<MenuDataItem> data;
    public int ItemCount { get; set; }
    protected List<IMenuItem> MenuItems = new List<IMenuItem>();
    public IMenuItem SelectedItem { get; set; }
    public IMenuItem PrevItem { get; set; }
    public IMenuItem NextItem { get; set; }

    bool _dragging = false;
    bool _mouseUp = false;

    Vector3 MouseDownPos;
    Vector3 PrevMousePos;
    float PrevMouseX;
    protected bool _preparingBump = false;
    protected bool _finishingBump = false;
    protected bool _bumpFwd;
    bool Snapped = true;



    public virtual void Init()
    {
        MenuItems.Clear();
    }

    public virtual void GetData()
    {}

    public virtual void PopulateMenu<T>() where T : IMenuItem, new()
    {
        ItemCount = data.Count;
        for (int Idx = 0; Idx < data.Count; Idx++)
        {
            IMenuItem _mi = new T();
            MenuDataItem mdi = data[Idx];
            //SavedGame.ScrollIdx = Idx;
            MenuItems.Add(_mi);
            //Initialize the item
            _mi.ContainingMenu = this;
            _mi.CreateGameObjects(mdi.Name, mdi.Type);
            _mi.OuterGameObject.name = "Menu_" + mdi.Name;
            _mi.Initialize();
            Transform trSavedItem = _mi.OuterGameObject.transform;
            trSavedItem.SetParent(this.transform);
            _mi.Id = Idx;
            if (_mi.Id == 0)
            {
                trSavedItem.localPosition = Vector3.up * 3;
                trSavedItem.Rotate(10, -60, 0);
            } else
                trSavedItem.localPosition = Vector3.right * (10 + _mi.Id*5);
                trSavedItem.Rotate(0, -90, 0);
            
        }

    }

    void OnMouseDown()
    {
        PrevMouseX = Input.mousePosition.x;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.name == "Cylinder")
            {
                MouseDownPos = new Vector3(hit.point.x, 0, hit.point.z);
            }
        }
        _dragging = true;
    }

    void OnMouseUp()
    {
        _dragging = false;
        _mouseUp = true;
        if (Input.mousePosition.x - PrevMouseX > 20 && PrevItem != null) {
            _bumpFwd = false;
            CarMenuItem I = (CarMenuItem)PrevItem;
            I.BumpInto((CarMenuItem)SelectedItem, false);
            Select(PrevItem);
        }
        if (Input.mousePosition.x - PrevMouseX < -20)
        {
            if (NextItem != null)
            {
                _bumpFwd = true;
                CarMenuItem I = (CarMenuItem)NextItem;
                I.BumpInto((CarMenuItem)SelectedItem, true);
                Select(NextItem);
            }
            else
                GameObject.Find("MenuCanvas").transform.Find("btnBuyCar").gameObject.SetActive(true);
        }
        Snapped = false;
    }

    void Update()
    {

        if (_dragging)
        {
            Drag( Input.mousePosition - PrevMousePos);
        }

        //Pick this up next frame to see how fast the mouse has moved
        PrevMousePos = Input.mousePosition;


        //stop when you get to the end

        //stop when you get to the beginning

    }

    public virtual void Drag(Vector3 ScreenDirection)
    {
    }

    public void SnapTo(int Idx)
    {
        Snapped = true;
    }


    public void Select(IMenuItem itm)
    {
        foreach (IMenuItem i in MenuItems)
        {
            if (i == itm) {
                i.Select();
                SelectedItem = itm;
                if (i.Id < ItemCount-1) NextItem = MenuItems[i.Id + 1]; else NextItem = null;
                if (i.Id > 0) PrevItem = MenuItems[i.Id - 1]; else PrevItem = null;
            }
            else i.Deselect();
       }

    }

    public void Delete(int GameId)
    {

    }

}



