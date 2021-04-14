using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public sealed class CamSelector {
    public string name;
    private static CamSelector _instance;
    static readonly object padlock = new object();
	private GameObject FollowCam;
    private Camera FollowCamCamera;
    private CamController_Follow_Fly FollowFlyScript;
    private CamController_Follow_Swing FollowSwingScript;
    private CamController_Follow_Simple FollowSimpleScript;
    public GameObject FPCam;
    public string CamType = "FollowCamSwing";
	public GameObject ActiveCam;

    //THis is where it instantiates itself
    public static CamSelector Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) {
                    _instance = new CamSelector();
                    _instance.CamType = Settings.Instance.CamType;
                    Main.Instance.LastSelectedCam = _instance.CamType;
                }
                return _instance;
            }
        }
    }
	/// <summary>
    /// Makes the CamSelector find the two camera scripts and the car. Then selects the settings camtype
    /// </summary>
    public void Init()
    {
        name = System.DateTime.Now.Millisecond.ToString();
        FollowCam = GameObject.Find("FollowCam");
        FollowCamCamera = FollowCam.GetComponentInChildren<Camera>();
        FollowFlyScript = FollowCam.GetComponent<CamController_Follow_Fly>();
        FollowSwingScript = FollowCam.GetComponent<CamController_Follow_Swing>();
        FollowSimpleScript = FollowCam.GetComponent<CamController_Follow_Simple>();
        FPCam = DrivingPlayManager.Current.PlayerCarManager.goCar.transform.Find("FPCamera").gameObject;
        SelectCam("StartingLine");
    }



	public void SwapCam(){
        if (CamType == "FollowCamFly")
            {SelectCam("FollowCamSwing"); return; }

        if (CamType == "FollowCamSwing")
            { SelectCam("Simple"); return;}

        if (CamType == "Simple")
            { SelectCam("FPCam"); return; }

        if (CamType == "FPCam")
            {FollowFlyScript.ResetPosition();
            SelectCam("FollowCamFly");
            return;}

	}

    public void SelectCam(string CamName)
    {
        CamType = CamName;
        GameObject gobtn = GameObject.Find("CamButton");
        if (gobtn == null) { ActiveCam = FollowCam; return; }
        Text txtCam = gobtn.GetComponentInChildren<Text>();
        if (CamType == "StartingLine")
        {
            ActiveCam = FollowCam;
            FPCam.GetComponent<Camera>().enabled = false;
            FPCam.GetComponent<FPCamController>().enabled = false;
            FollowCamCamera.enabled = true;
            FollowSwingScript.enabled = false;
            FollowFlyScript.enabled = false;
            Transform trPlayer = FPCam.transform.parent;
            FollowCam.transform.position = trPlayer.position - trPlayer.forward * 12 + trPlayer.up * 3;
            FollowCam.transform.GetChild(0).localRotation = Quaternion.identity;
            FollowCam.transform.GetChild(0).localPosition = Vector3.zero;
            FollowCam.transform.LookAt(trPlayer);
        }

        if (CamName == "FollowCamFly")
        {
            Main.Instance.LastSelectedCam = CamName;
            ActiveCam = FollowCam;
            FPCam.GetComponent<Camera>().enabled = false;
            FPCam.GetComponent<FPCamController>().enabled = false;
            FollowCamCamera.enabled = true;
            FollowSwingScript.enabled = false;
            FollowFlyScript.enabled = true;
            txtCam.text = "1";
        }
        if (CamName == "Simple")
        {
            Main.Instance.LastSelectedCam = CamName;
            ActiveCam = FollowCam;
            FPCam.GetComponent<Camera>().enabled = false;
            FPCam.GetComponent<FPCamController>().enabled = false;
            FollowCamCamera.enabled = true;
            FollowSimpleScript.enabled = true;
            FollowFlyScript.enabled = false;
            txtCam.text = "3";
        }
        if (CamName == "FPCam")
        {
            Main.Instance.LastSelectedCam = CamName;
            ActiveCam = FPCam;
            FollowCamCamera.enabled = false;
            FPCam.GetComponent<FPCamController>().enabled = true;
            FPCam.GetComponent<FPCamController>().Init();
            FollowFlyScript.enabled = false;
            FollowSwingScript.enabled = false;
            FollowSimpleScript.enabled = false;
            FPCam.GetComponent<Camera>().enabled = true;
            txtCam.text = "4";
        }
        if (CamName == "FollowCamSwing")
        {
            Main.Instance.LastSelectedCam = CamName;
            ActiveCam = FollowCam;
            FollowCamCamera.enabled = true;
            if (FPCam != null)
            { FPCam.GetComponent<Camera>().enabled = false; FPCam.GetComponent<FPCamController>().enabled = false;}
            FollowFlyScript.enabled = false;
            FollowSwingScript.enabled = true;
            txtCam.text = "2";
        }
    }

}
