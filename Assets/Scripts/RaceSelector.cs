using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RaceSelector : MonoBehaviour, iGameStarter {
    //Declare the event
    //iGameStarterController GSPC;
    iMain mainScript;
    public Text numLaps;
    public Text numMult;

    void Start()
    {
        mainScript = Main.Instance;
        //GSPC = new GameStarterController(this, mainScript);
            ToggleGroup tg = GameObject.Find("pnlOpposition").GetComponent<ToggleGroup>();
            foreach (Toggle t in tg.GetComponentsInChildren<Toggle>())
            {
                if (t.name == mainScript.OpponentCount.ToString()) t.isOn = true; else t.isOn = false;
            }
        //Find how many segments
        int XSecCount;
        if (GameData.current.RdS.XSecs != null)
            XSecCount = GameData.current.RdS.XSecs.Length;
        else
            XSecCount = Road.Instance.XSecs.Count;
        numLaps.text = mainScript.Laps.ToString();
        float Mult = (float)XSecCount * (float)mainScript.Laps / 6000f;
        float MultRounded = Mathf.RoundToInt(Mult * 10) / 10f;
        numMult.text = "X " + MultRounded.ToString();
        GameObject.Find("RandomVehicles").GetComponent<Toggle>().isOn = mainScript.RandomVehicles;
        GameObject.Find("Ghost").GetComponent<Toggle>().isOn = mainScript.Ghost;
    }


    public void SelectOpponents(int OpponentCount)
    {
        mainScript.OpponentCount = OpponentCount;
    }

    public void RandomVehicles(bool IsOn)
    {
        mainScript.RandomVehicles = IsOn;
    }

    public void Ghost(bool IsOn)
    {
        mainScript.Ghost = IsOn;
    }

    public void PlayOffline()
    {
        StartCoroutine(PlayOfflineCoroutine()); //Made into coroutine just in case the GameScene hasnt loaded yet
    }

    IEnumerator PlayOfflineCoroutine()
    {
        yield return new WaitUntil(()=>Main.Instance.GameSceneLoaded);
        Main.Instance.ActivateGameScene();
        yield return 0;
    }


    public void ShowSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void ShowWallet()
    {
        Object objPnl = Resources.Load("Prefabs/pnlGetCoins");
        GameObject goPnl = (GameObject)GameObject.Instantiate(objPnl, Vector3.zero, Quaternion.identity,this.transform);
        goPnl.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void LapsInc()
    {
        if (mainScript.Laps < 10)
            mainScript.Laps++;
        numLaps.text = mainScript.Laps.ToString();

        int XSecCount = GameData.current.RdS.XSecs.Length;
        float Mult = (float)XSecCount * (float)mainScript.Laps / 6000f;
        float MultRounded = Mathf.RoundToInt(Mult * 10) / 10f;
        numMult.text = "X " + MultRounded.ToString();
    }

    public void LapsDec()
    {
        if(mainScript.Laps > 1)
            mainScript.Laps--;
        numLaps.text = mainScript.Laps.ToString();

        int XSecCount = GameData.current.RdS.XSecs.Length;
        float Mult = (float)XSecCount * (float)mainScript.Laps / 6000f;
        float MultRounded = Mathf.RoundToInt(Mult * 10) / 10f;
        numMult.text = "X " + MultRounded.ToString();
    }

    void Destroy()
    {
    }


}
