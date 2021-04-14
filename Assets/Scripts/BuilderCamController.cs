using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuilderCamController : MonoBehaviour {
    private GameObject FollowCam;
    private GameObject BuilderCam;
    private GameObject FPCamPlaceholder;
    private GameObject MapCam;
    private string ActiveCam;
    private RoadBuilder RoadBuilder;
    float tilt;
    float smoothedTilt;
    Quaternion tiltQuat;
    private float tiltOffset;
    private float coeffSmooth = 0.3f;
    private float tiltVel = 0f;
    public bool FreezeTilt = false;
	// Use this for initialization
	void Start () {
        Init();
	}

    void Init()
    {
        Debug.Log("BuilderCamStsrt");
        FollowCam = GameObject.Find("FollowCam");
        BuilderCam = GameObject.Find("BuilderCamera");
        //MapCam = GameObject.Find ("MapCamera");
        BuilderCam.GetComponent<Camera>().enabled = true;
        //FPCam.camera.farClipPlane = 500;
        FollowCam.GetComponentInChildren<Camera>().enabled = false;
        //FollowCam.GetComponent<CamController_Follow_Swing>().enabled = false;
        FollowCam.GetComponent<CamController_Follow_Fly>().enabled = false;
        FollowCam.GetComponent<CamController_Follow_Swing>().enabled = false;
        //MapCam.camera.enabled = false;
        ActiveCam = "FPCam";
        #if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
                tiltOffset = Input.acceleration.y-0.5f;
        #endif
        RoadBuilder = GetComponent<RoadBuilder>();
    }
	// Update is called once per frame
	void Update ()
    {
        if (!FreezeTilt) {
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        tilt = (Input.acceleration.y - tiltOffset) * 90;
        smoothedTilt = Mathf.SmoothDampAngle(BuilderCam.transform.rotation.eulerAngles.x, tilt, ref tiltVel, coeffSmooth);
        //camera glances right and then left

        tiltQuat = Quaternion.Euler(smoothedTilt, 0, 0);
        BuilderCam.transform.localRotation = tiltQuat;
#endif

#if UNITY_EDITOR
        
        tilt = -Input.mousePosition.y / Screen.height * 90 + 45;
        smoothedTilt = Mathf.SmoothDampAngle(BuilderCam.transform.rotation.eulerAngles.x, tilt, ref tiltVel, coeffSmooth);
        tiltQuat = Quaternion.Euler(tilt, 0, 0);
        //BuilderCam.transform.localRotation = tiltQuat;!!!!!!!!!!!!!

#endif
        }
    }
}
