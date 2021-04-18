using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CamController_Follow_Simple : MonoBehaviour {
	public GameObject target;
    Transform trFollowCamInner;
    Camera _cam;
    public GPS Gps { get; set; }
	private float height = 4;
	private float heightDamping = 1;
	private float LerpSpeed = 1;	//This value increases as the velocity increases
	private float distance = 2;
	private Vector2 mouseOffset;
    private Vector3 OldAccel;
    private float OldTiltAngle = 0;
    private bool _horizonTilt = false;
    private Road Rd;
    Vector3 LookAheadSegPos;
    Vector3 PrevLookPos;
    Vector3 LookAheadPosOffset;
    Vector3[] LookPoses;
    int LookPosesIdx;
    Vector3 LookAheadPosAvg;
    Vector3 LookOffset;
    private float _statCamPosX = 0;
    MovingAvgFloat TiltAngles = new MovingAvgFloat(5);
    // Use this for initialization
    void OnEnable ()
    {
        Rd = Road.Instance;
        trFollowCamInner = transform.GetChild(0);
        _cam = trFollowCamInner.GetComponent<Camera>();
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        OldAccel = Input.acceleration;
        LookPoses = new Vector3[5];
#endif
        if (Settings.Instance.SteerControl== "Tilt") { _horizonTilt = true; }
    }



	// Update is called once per frame
	void LateUpdate () {
            MoveCamera();
	}

    public void MoveCamera()
    {
        
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
            if (target == null)
            {
                return;
            }
        }

        //FollowCam Version 1. This one is fine
        if (target.GetComponent<Rigidbody>().velocity.magnitude > 15f)
        //Need to catch up with the car or it disappears in the distance
        { LerpSpeed = target.GetComponent<Rigidbody>().velocity.magnitude / 12; }
        else
        { LerpSpeed = 1; }
        //Removed a bit here
        height = 3; distance = 1.5f;

        if (target.GetComponent<Rigidbody>().velocity.magnitude > 0.5f)
        {
            _statCamPosX = 0;
            //This works OK. It simply stays behind the cars velocity vector so you can see skids
            //THe Cam tries to stay 2m behind the car but the car pulls away becuase of the Lerp
            //The outerCam deals with the horizontal movement. The InnerCam deals with the height and the rotation
            Vector3 direction = -(target.GetComponent<Rigidbody>().velocity.normalized);
            Vector3 wantedPosition = target.transform.position + (direction * distance);
            if ((wantedPosition - target.transform.position).sqrMagnitude < 4) { wantedPosition += Vector3.up; }
            transform.position = Vector3.Slerp(transform.position, wantedPosition, LerpSpeed * Time.deltaTime);
//look ahead - We look ahead 15 segments
            //then we have to figure out how far across the current segment the car has driven
            //and add that on as a proportion of the 15th segments length
            float LookAheadDist = trFollowCamInner.localPosition.y-8;
            LookOffset = Vector3.zero;

        }
        else
        {
            LerpSpeed = 1; height = 1;
            //Swing the camera round to look at the side of the car
            //Which side of the car to look at:
            if (_statCamPosX == 0) { float Side = Random.value; if (Side > 0.5) _statCamPosX = -5f; else _statCamPosX = 5f; }
            Vector3 p = target.transform.position + Vector3.right * _statCamPosX;
            Vector3 newPosition = Vector3.Lerp(transform.position, p, LerpSpeed * Time.deltaTime);
            transform.position = newPosition;

            //Now lerp the height a bit slower
            Vector3 wantedPosition = Vector3.up * height;
            trFollowCamInner.localPosition = Vector3.Slerp(trFollowCamInner.localPosition, wantedPosition, LerpSpeed/2 * Time.deltaTime);
            LookOffset = Vector3.zero;
            //for (int x = 0; x < LookPoses.Length; x++) { LookPoses[x] = Vector3.zero; }
        }
        Vector3 FinalLookPosOffset;
        FinalLookPosOffset = Vector3.Lerp(PrevLookPos, LookOffset, Time.deltaTime);
        trFollowCamInner.LookAt(FinalLookPosOffset + target.transform.position);
        PrevLookPos = FinalLookPosOffset;


#if UNITY_EDITOR
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        
        if (!_horizonTilt) return;

        //this works but there's a delay
        float TiltAngle = Mathf.Atan2(-Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg;
        //float DesiredTiltAngle = -Mathf.Asin(accelx) * Mathf.Rad2Deg;  //this was really jittery at big angles
        //if (accel.y > 0) { if (accel.x < 0) DesiredTiltAngle = -180 - DesiredTiltAngle; else DesiredTiltAngle = -DesiredTiltAngle + 180; }
        TiltAngles.Push(TiltAngle);
        float SmoothTiltAngle = TiltAngles.Avg; // OldTiltAngle * 0.7f + DesiredTiltAngle * 0.3f;
        trFollowCamInner.Rotate(Vector3.forward, SmoothTiltAngle);

        //This version tracks the Angular velocity of the phone
#endif
    }

    internal void ResetPosition()
    {
        target = GameObject.FindWithTag("Player");
        transform.position = target.transform.position - target.transform.forward * 12 + target.transform.up * 3;
    }

    void OnDestroy()
    {
        Gps = null;
    }
}
