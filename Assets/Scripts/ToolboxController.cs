using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class ToolboxController : MonoBehaviour
{
    List<ToolOption> PurchasedRoadTools;
    List<ToolOption> PurchasedSceneryTools;
    List<ToolOption> PurchasedFenceTools;
    private String _side;
    public string SelectedTool = "RoadSectn";
    public ToolOption SceneryOpt;
    public string Gizmo = "Move";
    private GameObject ToolOptionPanel;
    private RectTransform rtToolOptionPanel;
    private RectTransform rtToolOptionPanelMask;
    private GameObject SceneryEditPanel;
    private GameObject RoadPanel;
    private Transform ToolOptionContentPanel;
    private GameObject InsertButton;
    private GameObject DeleteRoadSectionButton;
    private GameObject SceneryDeleteButton;
    private UnityEngine.Object btnTool50;
    private bool OptionPanelJustOpened;

    void Start()
    {
        //if the MacId of the game doesnt match that of the machine the BuilderController has Disabled the toolbox so this bit will fail anyway so skip it
        if (GameData.current.MacId != SystemInfo.deviceUniqueIdentifier) return;
        ToolOptionPanel = GameObject.Find("ToolOptionPanel");
        rtToolOptionPanel = ToolOptionPanel.GetComponent<RectTransform>();
        rtToolOptionPanelMask = ToolOptionPanel.transform.Find("Scrollview/Mask").GetComponent<RectTransform>();
        ToolOptionPanel.SetActive(false);
        ToolOptionContentPanel = ToolOptionPanel.transform.Find("Scrollview/Mask/ToolOptionContentPanel");
        RoadPanel = GameObject.Find("RoadPanel");
        SceneryEditPanel = GameObject.Find("SceneryEditPanel");
        InsertButton = GameObject.Find("InsertButton");
        DeleteRoadSectionButton = GameObject.Find("DeleteButton");
        SceneryDeleteButton = GameObject.Find("SceneryDeleteButton");
        SceneryDeleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteCurrentScenery());
        GameObject.Find("btnSceneryCopy").GetComponent<Button>().onClick.AddListener(() => CopyCurrentScenery());
        InsertButton.SetActive(false);
        SceneryEditPanel.SetActive(false);
        SceneryOpt = new ToolOption { Type = ToolType.Scenery, Name = "Tree", Cost = 50 };
        LoadToolOptions();
        btnTool50 = Resources.Load("Prefabs/ToolboxButtons/btnTool50");
    }

    public void LoadToolOptions()
    {
        var PurchasedRoadItems = (from i in Shop.Items join p in UserDataManager.Instance.Data.Purchases on i.Id equals p select i).Where(i => i.Type == ShopItemType.Road);
        PurchasedRoadTools = new List<ToolOption>();
        foreach (var i in PurchasedRoadItems)
        {
            PurchasedRoadTools.Add((ToolOption)i);
        }
        var PurchasedSceneryItems = (from i in Shop.Items join p in UserDataManager.Instance.Data.Purchases on i.Id equals p select i).Where(i => i.Type == ShopItemType.Scenery);
        PurchasedSceneryTools = new List<ToolOption>();
        foreach (var i in PurchasedSceneryItems)
        {
            PurchasedSceneryTools.Add((ToolOption)i);
        }
        var PurchasedFenceItems = (from i in Shop.Items join p in UserDataManager.Instance.Data.Purchases on i.Id equals p select i).Where(i => i.Type == ShopItemType.Fence);
        PurchasedFenceTools = new List<ToolOption>();
        foreach (var i in PurchasedFenceItems)
        {
            PurchasedFenceTools.Add((ToolOption)i);
        }
    }

    private void DeleteCurrentScenery()
    {
        PlaceableObject.Current.Delete();
        PlaceableObject.Current = null;
        Game.current.Dirty = true;
    }

    private void CopyCurrentScenery()
    {
        SceneryObject SObj = new SceneryObject(PlaceableObject.Current.Opt);
        SObj.PlaceObject(PlaceableObject.Current.Pos + Vector3.right*5, PlaceableObject.Current.Rot, PlaceableObject.Current.Scale);
        SObj.Chargeable = true;
        Scenery.Instance.Objects.Add(SObj);
        SObj.Select();
        Game.current.Dirty = true;
    }

    //This is set when you click on a scenery object
    public void SetToolToggle(string Tool, bool ShowToolOptions = true)
    {
        GameObject.Find("ToolboxPanel").transform.Find("btn" + Tool).GetComponent<Toggle>().isOn = true;
        ToolOptionPanel.SetActive(ShowToolOptions);
    }

    public void SelectTool(string tool)
    {
        if (tool == "RoadSectn")
        {
            SelectedTool = tool;
            ToolOptionPanel.SetActive(false);
            SceneryEditPanel.SetActive(false);
            //InsertButton.SetActive(true);
            DeleteRoadSectionButton.SetActive(true);
            RoadPanel.SetActive(true);
        }
        if (tool == "LFence" || tool == "RFence")
        {
            _side = tool.Substring(0, 1);
            ToolOptionPanel.SetActive(true);
            rtToolOptionPanel.sizeDelta = new Vector2(202, 50);
            ToolOptionContentPanel.GetComponent<GridLayoutGroup>().constraintCount = 4;

            ClearToolOptions();
            foreach (ToolOption opt in PurchasedFenceTools)
            {
                AddOptionButton(opt);
            }
            AddBuyButton("Fence");
            StartCoroutine(ExpandToolOptionPanel(4));
        }
        if (tool == "Road")
        {
            Debug.Log("ROadOptionTrue");
            ToolOptionPanel.SetActive(true);
            rtToolOptionPanel.sizeDelta = new Vector2(202, 50);
            ToolOptionContentPanel.GetComponent<GridLayoutGroup>().constraintCount = 4;

            ClearToolOptions();
            foreach (ToolOption opt in PurchasedRoadTools)
            {
                AddOptionButton(opt);
            }
            AddBuyButton("Road");
            StartCoroutine(ExpandToolOptionPanel(4));
        }
        if (tool == "Scenery")
        {
            SelectedTool = tool;
            ToolOptionPanel.SetActive(true);
            rtToolOptionPanel.sizeDelta = new Vector2(302, 50);
            ToolOptionContentPanel.GetComponent<GridLayoutGroup>().constraintCount = 6;

            ClearToolOptions();
            foreach (ToolOption opt in PurchasedSceneryTools)
            {
                AddOptionButton(opt);
            }
            AddBuyButton("Scenery");
            SceneryEditPanel.SetActive(true);
            InsertButton.SetActive(false);
            DeleteRoadSectionButton.SetActive(false);
            RoadPanel.SetActive(false);
            StartCoroutine(ExpandToolOptionPanel(6));
        }

        if (tool == "StartingLine")
        {
            SelectedTool = tool;
            ToolOptionPanel.SetActive(false);
            SceneryEditPanel.SetActive(false);
            InsertButton.SetActive(false);
            RoadPanel.SetActive(false);
        }

        if (tool == "GizmoMove")
        {
            ChangeToolOption(new ToolOption { Type = ToolType.Gizmo, Name = "Move", Cost = 0 });
        }

        if (tool == "GizmoRotate")
        {
            ChangeToolOption(new ToolOption { Type = ToolType.Gizmo, Name = "Rotate", Cost = 0 });
        }

        if (tool == "GizmoScale")
        {
            ChangeToolOption(new ToolOption { Type = ToolType.Gizmo, Name = "Scale", Cost = 0 });
        }
    }

    private void ClearToolOptions()
    {
        while (ToolOptionContentPanel.childCount > 0)
        {
            Transform child = ToolOptionContentPanel.GetChild(0);
            try { child.GetComponent<Toggle>().onValueChanged.RemoveAllListeners(); }
            catch { }
            try { child.GetComponent<Button>().onClick.RemoveAllListeners(); }
            catch { }
            child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }
    }

    internal void ShowInsertButton(bool Show)
    {
        InsertButton.SetActive(Show);
    }

    void AddOptionButton(ToolOption opt)
    {
        //UnityEngine.Object btnPrefab = Resources.Load("Prefabs/ToolboxButtons/btn" + opt.Name);
        GameObject btn = Instantiate(btnTool50) as GameObject;
        btn.transform.SetParent(ToolOptionContentPanel);
        btn.transform.localScale = Vector3.one;
        if (opt.Image !=null)
        { btn.GetComponentsInChildren<Image>()[0].sprite = opt.Image; }
        btn.name = "btn" + opt.Name;
        Toggle tog = btn.GetComponent<Toggle>();
        tog.group = ToolOptionContentPanel.GetComponent<ToggleGroup>();
        tog.onValueChanged.AddListener(delegate { ChangeToolOption(opt); });
        tog.onValueChanged.AddListener(delegate { ChangeMainButtonImage((tog.isOn), opt); });
    }

    public void AddBuyButton(String type)
    {
        int _gridWidth = ToolOptionContentPanel.GetComponent<GridLayoutGroup>().constraintCount;
        Debug.Log("BeforeAddBuyButton ChildCount = " + ToolOptionContentPanel.childCount.ToString());
        int SpareCols = _gridWidth - (ToolOptionContentPanel.childCount) % _gridWidth;
        if (SpareCols == _gridWidth) { SpareCols = 0; }
        for (int i = 0; i < SpareCols; i++)
        {
            UnityEngine.Object _spc = Resources.Load("Prefabs/ToolboxButtons/Spacer50");
            GameObject _goSpc = Instantiate(_spc) as GameObject;
            _goSpc.transform.SetParent(ToolOptionContentPanel);
            _goSpc.transform.localScale = Vector3.one;
        }
        UnityEngine.Object btnPrefab = Resources.Load("Prefabs/ToolboxButtons/btnBuy");
        GameObject btnBuy = Instantiate(btnPrefab) as GameObject;
        btnBuy.transform.SetParent(ToolOptionContentPanel);
        btnBuy.transform.localScale = Vector3.one;
        btnBuy.name = "btnBuy";
        Button btn = btnBuy.GetComponent<Button>();
        btn.onClick.AddListener(delegate { BuyToys(type); });
    }


    IEnumerator ExpandToolOptionPanel(int ColCount)
    {
        rtToolOptionPanelMask.sizeDelta = Vector2.up * -50;
        int Rows = (ToolOptionContentPanel.childCount - 1) / ColCount;
        while (rtToolOptionPanelMask.sizeDelta.y < Rows * 50)
        {
            rtToolOptionPanelMask.sizeDelta = new Vector2(rtToolOptionPanelMask.sizeDelta.x, rtToolOptionPanelMask.sizeDelta.y + 8);
            yield return null;
        }
        yield break;
    }

    public void BuyToys(String type)
    {
        ToolOptionPanel.SetActive(false);
        Transform canv = GameObject.FindObjectOfType<Canvas>().transform;
        UnityEngine.Object objShopPanel = Resources.Load("Prefabs/pnlShop");
        GameObject pnlShop = (GameObject)GameObject.Instantiate(objShopPanel,new Vector2(500,500), Quaternion.identity, canv);
        pnlShop.transform.localScale = Vector3.one;
        pnlShop.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        pnlShop.GetComponent<ShopPanel>().SwitchToggle(type);
    }

    public void ChangeMainButtonImage(bool IsOn, ToolOption opt)
    {
        if (IsOn)
        {
            if (opt.Type == ToolType.Fence)
            {
                Sprite SrcSprite = GameObject.Find("btn" + opt.Name).GetComponentInChildren<Image>().sprite;
                Sprite S = Instantiate(SrcSprite) as Sprite;
                GameObject.Find("btn" + _side + opt.Type).GetComponentInChildren<Image>().sprite = S;
            }
            if (opt.Type == ToolType.Road)
            {
                Sprite SrcSprite = GameObject.Find("btn" + opt.Name).GetComponentInChildren<Image>().sprite;
                Sprite S = Instantiate(SrcSprite) as Sprite;
                GameObject.Find("btn" + opt.Type).GetComponentInChildren<Image>().sprite = S;
                Debug.Log("OptTOol = Road");
            }
            if (opt.Type == ToolType.RoadSectn)
            {
                Sprite SrcSprite = GameObject.Find("btn" + opt.Name).GetComponentInChildren<Image>().sprite;
                Sprite S = Instantiate(SrcSprite) as Sprite;
                GameObject.Find("btn" + opt.Type).GetComponentsInChildren<Image>()[0].sprite = S;
                Debug.Log("OptTOol = RoadSctn");
            }
            if (opt.Type == ToolType.Scenery)
            {
                GameObject.Find("btn" + opt.Type).GetComponentsInChildren<Image>()[0].sprite = opt.Image;
            }
            ToolOptionPanel.SetActive(false);
        }
        
        
    }

    //Here's how we raise an event
    //declare the delegate with its returntype and parameters;
    public delegate void ToolOptionChangedEventHandler(GameObject sender, ToolOptionChangedEventArgs e);
    //define the event that will be fired. 
    //At the consuming end you will see it as a lightning bolt in intellisense
    //and you will use ToolOptionChanged += SomeMethod;
    public event ToolOptionChangedEventHandler ToolOptionChanged;       //this event consumed by the RoadBUilder

    //This method loads the args with the correct stuff
    private void ChangeToolOption(ToolOption opt)
    {
        if (opt.Type == ToolType.Gizmo) Gizmo = opt.Name;
        if (opt.Type == ToolType.Scenery)
        {
            SceneryOpt = opt;
        }
        ToolOptionChangedEventArgs args = new ToolOptionChangedEventArgs();
        args.Opt = opt;
        args.Side = _side;
        OnToolOptionChanged(args);  //Passed to the roadbuilder so it can re-draw the road and fences

    }
    //This fires the event. It is virtual because derived classes can override it
    protected virtual void OnToolOptionChanged(ToolOptionChangedEventArgs e)
    {
        if (ToolOptionChanged != null)
        {
            ToolOptionChanged(this.gameObject, e);
            //ToolOptionChanged 
        }
    }

    public void ShowRoadToolOptionForSelectedSection(ShopItemType typ, string to, string Side = "N")
    {
        var ShopItm = Shop.Items.FirstOrDefault(i => i.Type == typ && i.Name == to);
        ToolOption opt = (ToolOption)ShopItm;
        if(opt.Type==ToolType.Road)
        RoadPanel.transform.Find("btnRoad").GetComponentsInChildren<Image>()[0].sprite = opt.Image;
        if (opt.Type == ToolType.Fence && Side == "L")
            RoadPanel.transform.Find("btnLFence").GetComponentsInChildren<Image>()[0].sprite = opt.Image;
        if (opt.Type == ToolType.Fence && Side == "R")
            RoadPanel.transform.Find("btnRFence").GetComponentsInChildren<Image>()[0].sprite = opt.Image;
    }

    public enum ToolType { Fence, Road, RoadSectn, Scenery, Gizmo}
}


public class ToolOptionChangedEventArgs : EventArgs
{
    public ToolOption Opt { get; set; }
    public string Side { get; set; }
}

public class ToolOption
{
    public ToolboxController.ToolType Type { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
    public Sprite Image { get; set; }
}



