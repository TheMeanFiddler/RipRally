using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// Controls some of the menu button functions
/// </summary>
public class DrivingMenuController : MonoBehaviour
{
    public GameObject goMenuPanel;
    public GameObject MenuToggle;
    public GameObject goDrivingMenu;
    GameObject goControlCanvas;


    void Start()
    {
        goMenuPanel.SetActive(false);
        goDrivingMenu.transform.Find("MenuPanel/HomeButton").GetComponent<Button>().onClick.AddListener(() => Main.Instance.GotoHome());
        if (Main.Instance.ReplayButtonNeedsClickingOnceDrivingGUIMenuIsOpen)
        {
            Main.Instance.ReplayButtonNeedsClickingOnceDrivingGUIMenuIsOpen = false;
            StartCoroutine(ClickReplayButtonNextFrame());
        }
        //Button TestButton = goDrivingMenu.transform.Find("MenuPanel/TestButton").GetComponent<Button>();
        //TestButton.onClick.AddListener(() => DrivingTests.Instance.TestAll());
        goControlCanvas = GameObject.Find("ControlCanvas");
    }

    public void TestButtonClick()
    {
        DrivingTests.Instance.MaterialTest();
    }

    IEnumerator ClickReplayButtonNextFrame()
    {
        yield return new WaitForEndOfFrame();
        ReplayButtonClick();
        Debug.Log("Clicked own replay button");
        foreach(Rigidbody rb in FindObjectsOfType<Rigidbody>()) { if(rb.gameObject.activeInHierarchy)Debug.Log(rb.name); }
        yield break;
    }

    public void ShowMenuPanel(bool Show)
    {
        goMenuPanel.SetActive(Show);
    }

    public void RestartRace()
    {
        MenuToggle.GetComponent<Toggle>().isOn = false;
        DrivingPlayManager.Current.RespawnCars();
        Race.Current.ReadySteady();
    }

    public void AdjustTrack()
    {
        PlayerManager.Type = "BuilderPlayer";
        foreach (iVehicleManager VM in DrivingPlayManager.Current.VehicleManagers)
        {
            VM.DestroyVehicle();
        }
        DrivingPlayManager.Current.PlayerCarManager = null;
        DrivingPlayManager.Current.VehicleManagers.Clear();
        DrivingPlayManager.Current.Dispose(); //also gets rid of Race current and RaceRecorder and Recrdng.Current
        DrivingPlayManager.Current = null;

        ShowMenuPanel(false);

        BuildingPlayManager BPM = new BuildingPlayManager();
        BuildingPlayManager.Current = BPM;
        BuildingPlayManager.Current.PlayOffline();
        foreach (PlaceableObject PO in Scenery.Instance.Objects)
        {
            PO.EnableClickColliders();
        }
    }
    public void CamButtonClick()
    {
        CamSelector.Instance.SwapCam();
    }

    public void ReplayButtonClick()
    {
        Debug.Log("ReplaBUttonCLick");
        DrivingPlayManager.Current.PauseCars();
        Transform Canvas = this.transform;
        UnityEngine.Object TransptPrefab = Resources.Load("Prefabs/pnlReplayer");
        GameObject Tr = GameObject.Instantiate(TransptPrefab) as GameObject;
        Tr.transform.SetParent(Canvas, false);
        Tr.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        Tr.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        Tr.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        Tr.transform.localScale = Vector2.one;
        ShowPanel(false);
     }

    public void ShowPanel(bool show)
    {
        goDrivingMenu.SetActive(show);
        if(goControlCanvas!=null)goControlCanvas.SetActive(show);
    }
 
}
