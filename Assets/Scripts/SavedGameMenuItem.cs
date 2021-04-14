using UnityEngine;
using UnityEngine.UI;



public interface IMenuItem
{
    int Id { get; set; }
    int ScrollIdx { get; set; }
    void CreateGameObjects(string N, string Typ);
    bool Permanent { get; set; }
    void Populate(IMenuDataItem Data);
    IMenuDataItem Data { get; set; }
    IMenuListView ContainingMenu { get; set; }
    GameObject OuterGameObject { get; }
    void Initialize();
    void Select();
    void Deselect();
    void SetType(string type);
    string Type { get; set; }
    bool Selected { get; set; }
    bool Deleted { get; set; }

}

public abstract class CanvasMenuItem : IMenuItem
{
    protected int _id;
    public int ScrollIdx { get; set; }
    protected string _name;
    public virtual int Id { get { return _id; } set { _id = value; } }
    public bool Permanent { get; set; }
    public IMenuDataItem Data { get; set; }
    public IMenuListView ContainingMenu { get; set; }
    public GameObject OuterGameObject { get; set; }
    public void Initialize()
    { }
    public virtual void CreateGameObjects(string N, string Typ) { _name = N; }
    public virtual void SetType(string type) { }
    public virtual string Type{ get; set; }
    public virtual void Populate(IMenuDataItem Data) { }
    public virtual void Select() { }
    public virtual void Deselect() { }
    public bool Selected { get; set; }
    public bool Deleted { get; set; }
}


public class SavedGameMenuItem : CanvasMenuItem
{
    GameObject Canvas;
    GameObject Img;
    Text TextField;
    GameObject goRenameInputField;
    Button btnSave;
    iMain mainscript;
    bool _editable;
    Game _game;

    //Overrides Id from the base class
    public override int Id { get { return base._id; } set { _id = value;} }

    //Constructor
    public SavedGameMenuItem()
    {
        Deleted = false;
        mainscript = Main.Instance; // goMain.GetComponent<Main>();
        Object objSavedGameCanvas = Resources.Load("Prefabs/SavedGameCanvas");
        Object objSavedGameImg = Resources.Load("Prefabs/SavedGameImage");
        Canvas = (GameObject)GameObject.Instantiate(objSavedGameCanvas);
        OuterGameObject = Canvas;
        Img = (GameObject)GameObject.Instantiate(objSavedGameImg);
        Img.transform.SetParent(Canvas.transform);
        //goRenameInputField = GameObject.Find("RenameInputField");
        btnSave = Img.transform.Find("btnSave").gameObject.GetComponent<Button>();
        Button.ButtonClickedEvent Delegt = btnSave.onClick;
        Delegt.AddListener(delegate { Save(); });
        Img.transform.Find("btnDetail").GetComponent<Button>().onClick.AddListener(delegate { ShowDetailPanel(); });
        //Switch off the text editor and buttons cos we dont want to rename or save this
        Img.GetComponentInChildren<TextEditStarter>().enabled = false;
        Img.transform.Find("btnSave").gameObject.SetActive(false);
        Canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    public override void Populate(IMenuDataItem data)
    {
        Data = data;
        Game mdi = (Game)data;
        //SetName(mdi.GameId.ToString() + ". " + mdi.Filename);
        CreateGameObjects(mdi.Filename, "");
        Id = mdi.GameId;
        if (!mdi.IsMine) Type = "NotMine";
        _game = mdi;
    }


    public override void CreateGameObjects(string N, string typ)
    {
        Img.GetComponentsInChildren<Text>()[0].text = N;
    }

    public override void SetType(string type)
    {
        if (type == "NotMine")
        {
            Img.GetComponent<Image>().color = Color.grey;
        }
    }

    public override string Type
    {
        get { return null; }
        set
        {
            if (value == "NotMine")
            {
                Img.GetComponent<Image>().color = Color.grey;
            }
        }

    }

    public void EnableEditing(bool enabl)
    {
        if(Permanent) enabl=false;  //we dont want to enable the new game
        Canvas.GetComponent<GraphicRaycaster>().enabled = enabl;
        Img.transform.Find("btnDetail").gameObject.SetActive(enabl);
        EnableSaveButton(false);
        //The save button gets enabled by the SavedGameMenu 
    }

    public void EnableSaveButton(bool enabl)
    {
        Img.transform.Find("btnSave").gameObject.SetActive(enabl);
    }


    public void ShowDetailPanel()
    {
        UnityEngine.Object objTrackPanel = Resources.Load("Prefabs/pnlTrackDetail");
        GameObject goTrackPanel = (GameObject)GameObject.Instantiate(objTrackPanel, Vector3.zero, Quaternion.identity, GameObject.Find("MenuCanvas").transform);
        goTrackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        SaveLoadModel.LoadGameData(_game.GameId);
        goTrackPanel.GetComponent<TrackDetail>().Init(_game);
    }



    public override void Select()
    {
        if (!Selected)
        {
            EnableEditing(true);
            Selected = true;
        }

    }

    public override void Deselect()
    {
        if (Selected)
        {
            EnableEditing(false);
            Selected = false;
        }
    }


    private void Save()
    {
        UnityEngine.Object objdlgSave = Resources.Load("Prefabs/dlgSave");
        GameObject goSave = (GameObject)GameObject.Instantiate(objdlgSave, Vector3.zero, Quaternion.identity, GameObject.Find("MenuCanvas").transform);
        goSave.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        goSave.GetComponent<dlgSave>().MenuItemToSave = this;
    }

    public void Delete()
    {
        SaveLoadModel.Delete(Id);
        OuterGameObject.SetActive(false);
        Deleted = true;
    }

}

