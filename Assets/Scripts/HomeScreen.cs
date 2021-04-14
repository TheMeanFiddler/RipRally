using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Declare the event delegate
public delegate void PlayOfflineClickHandler();
public delegate void StartServerClickHandler();
public delegate void ConnectToServerClickHandler(string IPAddress);

public interface iGameStarter
{
    void PlayOffline();
}

public class HomeScreen : MonoBehaviour, iGameStarter {
    //Declare the event
    iMain mainScript;
    bool ChooseRaceClicked = false;

    void Start()
    {
        Physics.gravity = Vector3.down * 9.8f;
        mainScript = Main.Instance;
        ChooseRaceClicked = false;
    }

    public void ChooseRace()
    {
        if (ChooseRaceClicked) return;
        ChooseRaceClicked = true;
        Main.Instance.GameSceneLoaded = false;
        Main.Instance.StartLoadingGameScene(GameData.current.Scene);
        Main.Instance.StartLoadingMenuScene("RaceSelector");
    }

    public void SelectOpponents(int OpponentCount)
    {
        mainScript.OpponentCount = OpponentCount;
    }

   public void PlayOffline()
    {
        /*
        Main.Instance.ActivateGameScene();
        SceneManager.UnloadSceneAsync("RaceSelector");
        Debug.Log("Activated Canyon");
        Race.Current.ReadySteady();
        */
    }

    public void ChangePlayerColor(string color)
    {
        Main.Instance.Color = color;
    }

    public void ShowSettings()
    {
        Object objPnl = Resources.Load("Prefabs/pnlSettings");
        GameObject goPnl = (GameObject)GameObject.Instantiate(objPnl, Vector3.zero, Quaternion.identity, this.transform);
        goPnl.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ShowWallet()
    {
        Object objPnl = Resources.Load("Prefabs/pnlGetCoins");
        GameObject goPnl = (GameObject)GameObject.Instantiate(objPnl, Vector3.zero, Quaternion.identity,this.transform);
        goPnl.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void ShowShop(string Type)
    {
        if (Type == "Car")
        {
            UnityEngine.Object objShopPanel = Resources.Load("Prefabs/pnlShop");
            GameObject pnlShop = (GameObject)GameObject.Instantiate(objShopPanel, new Vector2(500, 500), Quaternion.identity, GameObject.Find("MenuCanvas").transform);
            pnlShop.transform.localScale = Vector3.one;
            pnlShop.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            pnlShop.GetComponent<ShopPanel>().SwitchToggle("Car");
        }
    }

    void Destroy()
    {
    }


}
