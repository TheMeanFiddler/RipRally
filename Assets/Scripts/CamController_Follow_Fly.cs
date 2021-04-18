using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class CamController_Follow_Fly : MonoBehaviour {
	public GameObject target;
    public Rigidbody _rbTarget;
    Transform trFollowCamInner;
    Camera _cam;
    public GPS Gps { get; set; }
	private float height = 4;
	private float LerpSpeed = 1;	//This value increases as the velocity increases
	private Vector3 _outerCamWantedPos;
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
    Vector3 LookPosesSum;
    Vector3 LookAheadPosAvg;
    Vector3 LookOffset;
    float _frustrumAngle;
    private float _statCamPosX = 0;
    private float _targetVelocity;
    MovingAvgFloat TiltAngles = new MovingAvgFloat(5);

    // Use this for initialization

    void Start()
    {
        Rd = Road.Instance;
        _cam = trFollowCamInner.GetComponent<Camera>();
        _frustrumAngle = _cam.fieldOfView / _cam.aspect;
    }

    void OnEnable ()
    {
        trFollowCamInner = transform.GetChild(0);
        
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        OldAccel = Input.acceleration;
        LookPoses = new Vector3[5];
        LookPosesSum = new Vector3(0, 0, 0);
#endif
        Gps = DrivingPlayManager.Current.PlayerCarManager.Gps;
        if (Settings.Instance.SteerControl== "Tilt") { _horizonTilt = true; }
    }



	// Update is called once per frame
	void LateUpdate () {
            MoveCamera();
	}

    public void MoveCamera()
    {
        
        if (_rbTarget == null)
        {
            {
                target = GameObject.FindWithTag("Player");
            }
            if (target == null)
            {
                return;
            }
            _rbTarget = target.GetComponent<Rigidbody>();
        }
        if (Gps == null)
        {
            try { Gps = target.GetComponent<CarPlayerController>().Gps; }
            catch
            {
                try { Gps = target.GetComponent<HotrodPlayerController>().Gps; }
                catch
                {
                    try { Gps = target.GetComponent<ModelTPlayerController>().Gps; } catch { }
                }
            }
            if (Gps == null)
            {
                Debug.Log("GPS is null");
            }
        }
        _targetVelocity = _rbTarget.velocity.magnitude;
        //FollowCam Version 1. This one is fine
        if (_targetVelocity > 15f)
        //Need to catch up with the car or it disappears in the distance
        { LerpSpeed = _targetVelocity / 12; }
        else
        { LerpSpeed = 1; }
        //This helps you see round bends because the cam moves up ans over the car so you can see the curve better
        Bend _nextBend = Gps.NextBend;
        if (Mathf.Abs(_nextBend.Angle) > 90 && Gps.CurrSegIdx > _nextBend.TurninXSec.Idx)
            height = 40;
        else
            height = 12;

        if (_targetVelocity > 0.5f)
        {
            _statCamPosX = 0;
            //This works OK. It simply stays behind the cars velocity vector so you can see skids
            //THe Cam tries to stay 2m behind the car but the car pulls away becuase of the Lerp
            //The outerCam deals with the horizontal movement. The InnerCam deals with the height and the rotation
            Vector3 wantedPosition = target.transform.position - new Vector3(_rbTarget.velocity.x, 0, _rbTarget.velocity.z).normalized*4;
            transform.position = Vector3.Slerp(transform.position, wantedPosition, LerpSpeed * Time.deltaTime);
            
            //Now lerp the height a bit slower than the distance
            wantedPosition = Vector3.up * height;
            trFollowCamInner.localPosition = Vector3.Slerp(trFollowCamInner.localPosition, wantedPosition, LerpSpeed/4 * Time.deltaTime);
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
        }

        trFollowCamInner.LookAt(target.transform.position);
        trFollowCamInner.Rotate(new Vector3(-_frustrumAngle / 2,0,0),Space.Self);


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
        LookPoses = null;
    }
}
