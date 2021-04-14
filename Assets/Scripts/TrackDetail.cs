using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NatShareU;

public class TrackDetail: MonoBehaviour
{
    Game _game;
    InputField ipName;
    Text txtBestLap;
    Transform RecPanel;

    public void Init(Game g)
    {
        _game = g;
        
        transform.Find("btnBack").GetComponent<Button>().onClick.AddListener(delegate { ClosePanel(); });
        ipName = transform.Find("txtName").GetComponent<InputField>();
        ipName.text = g.Filename;
        ipName.onEndEdit.AddListener(delegate { Rename(); });
        txtBestLap = transform.Find("pnlStats/txtBestLap").GetComponent<Text>();
        if(g.BestLap!=null)
            txtBestLap.text = HMS((float)g.BestLap);
        GetComponentInChildren<RecordingPanel>().Init(_game);
        if (GameData.current.Img!=null)
        {
            Texture2D img = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            img.LoadRawTextureData(GameData.current.Img);
            img.Apply();
            Sprite s = Sprite.Create(img, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
            transform.Find("pnlScreenshot").GetComponent<Image>().sprite = s;
        }
    }
    public void Init(GameData gd)
    {
        transform.Find("btnBack").GetComponent<Button>().onClick.AddListener(delegate { ClosePanel(); });
        ipName = transform.Find("txtName").GetComponent<InputField>();
        ipName.text = gd.Filename;
    }

    private void Rename()
    {
        SaveLoadModel.Rename(_game.GameId, ipName.text);
        try
        {
            SavedGameMenu sgm = GameObject.Find("SpinMenu").GetComponent<SavedGameMenu>();
            IMenuItem itm = sgm.SelectedItem;
            GameObject goItm = itm.OuterGameObject;
            goItm.GetComponentInChildren<Text>().text = ipName.text;
        }
        catch { }
    }

    public void ResetBestLap()
    {
        Game.current.BestLap = null;
        transform.Find("pnlStats/txtBestLap").GetComponent<Text>().text = "none";
    }

    public void ReplayRecordingNotUsed(string Filename)
    {
        Recrdng.Current = new Recrdng(Filename);
        Main.Instance.OpponentCount = Recrdng.Current.Data[0].Count-1;
        if (SceneManager.GetActiveScene().buildIndex < 5)
        {
            PlayerManager.Type = "Replayer";
            SaveLoadModel.LoadGameData(_game.GameId);
            SceneManager.LoadScene("Canyon");
            //rest of the procedure happens in Main.SceneChange - if (PlayerManager.Type == "Replayer")
        }
    }

    public void ShareTrack()
    {
        NatShare.Share(Application.persistentDataPath + "/" + _game.Filename, OnShare);
    }

    void OnShare()
    {
        Main.Instance.PopupMsg("Thank you for sharing");
    }

    public void DeleteTrack()
    {
        SaveLoadModel.Delete(_game.GameId);
        SavedGameMenu sgm = GameObject.Find("SpinMenu").GetComponent<SavedGameMenu>();
        IMenuItem itm = sgm.SelectedItem;
        GameObject goItm = itm.OuterGameObject;
        goItm.SetActive(false);
        ClosePanel();
    }

    void DeleteRecording(GameObject goRcd, string Recname)
    {
        if (File.Exists(Application.persistentDataPath + "/" + Recname))
        {
            File.Delete(Application.persistentDataPath + "/" + Recname);
            goRcd.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        transform.Find("btnBack").GetComponent<Button>().onClick.RemoveAllListeners();
        Destroy(gameObject);
    }

    private string HMS(float FloatTime)
    {
        float minutes = Mathf.Floor(FloatTime / 60);
        float seconds = FloatTime % 60;
        seconds = Mathf.Round(seconds * 100f) / 100f;
        string strMins = minutes.ToString();
        string strSecs = seconds.ToString("00.00");

        if (minutes < 10) { strMins = "0" + strMins; }
        return strMins + ":" + strSecs;
    }
}

