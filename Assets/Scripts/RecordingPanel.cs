using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class RecordingPanel : MonoBehaviour
{
    Transform RecPanel;
    Game _game;
    public AsyncOperation _gao;


    public void Init(Game game)
    {
        _game = game;
        GameObject gotxtFilename = GameObject.Find("InputField");
        if (gotxtFilename != null) {
            string ThisRecname = Recrdng.Current.Filename.Substring(_game.Filename.Length + 1);
            ThisRecname = ThisRecname.Substring(0, ThisRecname.Length - 4);
            gotxtFilename.GetComponent<InputField>().text = ThisRecname;
        }

        #region Show Filesize
        Transform trFileSize = transform.Find("txtFileSize");
        if (trFileSize != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Size of of File ");
            int FileSize = Mathf.CeilToInt((Recrdng.Current.EndFrame - Recrdng.Current.StartFrame) * Recrdng.Current.Data[0].Count/7.8f);
            sb.Append(FileSize.ToString());
            sb.Append("KB");
            if(FileSize>5000)
            {
                sb.Append("\n\nConsider saving a smaller section. Press cancel and slide the red start and finish markers along the timeline.");
            }
            trFileSize.GetComponent<Text>().text = sb.ToString();

        }
        #endregion

        #region Fill Recording Panel
        RecPanel = transform.Find("Scrollview/RcdContentPanel");
        Object objRcd = Resources.Load("Prefabs/pnlRcd");
        //Find all the recordings of this track
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles(_game.Filename + "-*.rcd");
        int RecNo = 0;
        foreach (FileInfo f in info)
        {
            string Recname = f.Name.Substring(_game.Filename.Length + 1);
            Recname = Recname.Substring(0, Recname.Length - 4);
            string Filename = f.Name;
            GameObject _goPnlRcd = (GameObject)GameObject.Instantiate(objRcd, Vector3.zero, Quaternion.identity, RecPanel);
            _goPnlRcd.transform.Find("txtRcd").GetComponent<Text>().text = Recname;
            _goPnlRcd.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -RecNo * 24);
            _goPnlRcd.transform.Find("btnPlay").GetComponent<Button>().onClick.AddListener(delegate
            {
                ReplayRecording(Filename);
            });
            _goPnlRcd.transform.Find("btnDelete").GetComponent<Button>().onClick.AddListener(delegate { DeleteRecording(_goPnlRcd, Filename); });
            RecNo++;
        }
        RecPanel.GetComponent<RectTransform>().offsetMin = new Vector2(0, -RecNo * 24);
        #endregion
    }

    public void ReplayRecording(string Filename)
    {
        Recrdng.Current = new Recrdng(Filename);
        Main.Instance.OpponentCount = Recrdng.Current.Data[0].Count - 1;
        if (SceneManager.GetActiveScene().buildIndex < 5)     //It was called from the TrackDetail Panel
        {
            PlayerManager.Type = "Replayer";
            Main.Instance.GameSceneLoaded = false;
            Time.fixedDeltaTime = 1; //Wow this makes a huge diff because While the scene is loading the Physics.Processing is running 50 times a second
            Main.Instance.StartLoadingGameScene(GameData.current.Scene); //This is a coroutine
            StartCoroutine(ReplayCoroutine());
            //rest of the procedure happens in Main.SceneChange - if (PlayerManager.Type == "Replayer")
        }
        else   //it was called from the Replayer Save Panel
        {
            //The recording panel is attached to the DrivingGUICanvas and we have just loaded a different recording 
            //so we have to destroy all the live vehicles
            PlayerManager.Type = "Replayer";
            foreach (iVehicleManager VM in DrivingPlayManager.Current.VehicleManagers)
            {
                VM.DestroyVehicle();
            }
            //the replayer vehicles are already destroyed because we destroyed the transport panel and the replayer when we opened the save panel
            DrivingPlayManager.Current.CreateLiveRacersForReplayerToHide();    //Creates the live cars
            //Main.Instance.ReplayButtonNeedsClickingOnceDrivingGUIMenuIsOpen = true;
            DrivingPlayManager.Current.ShowReplayPanel();
            Destroy(this.gameObject);
        }
    }

    IEnumerator ReplayCoroutine() //We need this so we can wait for the game scene to load
    {
        yield return new WaitUntil(()=>Main.Instance.GameSceneLoaded);
        Main.Instance.ActivateGameScene();  //once its activated it will run the CreateTrack Coroutine
        Time.fixedDeltaTime = 0.02f;
        yield return 0;
    }

    void DeleteRecording(GameObject goRcd, string Recname)
    {
        if (File.Exists(Application.persistentDataPath + "/" + Recname))
        {
            File.Delete(Application.persistentDataPath + "/" + Recname);
            goRcd.SetActive(false);
        }
    }

    public void Save_Click()
    {
        string name = GameObject.Find("txtSaveName").GetComponent<Text>().text;
        Recrdng.Current.SaveAs(name);
        DrivingPlayManager.Current.ShowReplayPanel();
        Destroy(this.gameObject);
    }

    public void Cancel_Click()
    {
        DrivingPlayManager.Current.ShowReplayPanel();
        Destroy(this.gameObject);
    }
}

