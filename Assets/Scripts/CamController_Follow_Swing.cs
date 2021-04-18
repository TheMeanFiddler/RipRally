using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CamController_Follow_Swing : MonoBehaviour
{
    public GameObject target;
    Transform trFollowCamInner;
    public GPS Gps { get; set; }
    private float height = 4;
    private float heightDamping = 1;
    private float LerpSpeed = 1;    //This value increases as the velocity increases
    private float distance = 2;
    Vector3 LookAheadPosOffset;
    private Vector3 OldAccel;
    private float OldTiltAngle = 0;
    private bool _horizonTilt = false;
    private Road Rd;
    private float _statCamPosX = 0;
    private float _targetVelocity;
    MovingAvgFloat TiltAngles = new MovingAvgFloat(5);
    // Use this for initialization
    void OnEnable()
    {
        Rd = Road.Instance;
        trFollowCamInner = transform.GetChild(0);
        trFollowCamInner.localPosition = Vector3.zero;
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        OldAccel = Input.acceleration;
#endif

        Gps = DrivingPlayManager.Current.PlayerCarManager.Gps;
        if (Settings.Instance.SteerControl == "Tilt") { _horizonTilt = true; }
    }

    void OnDestroy()
    {
        Gps = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
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

        _targetVelocity = target.GetComponent<Rigidbody>().velocity.magnitude;
        //FollowCam Version 2
        distance = 2f;
        if (_targetVelocity > 15f)
        //THe Cam tries to stay 2m behind the car but the car pulls away becuase of the Lerp
        { LerpSpeed = _targetVelocity / 15f; }
        else
        { LerpSpeed = 1; }

        if (_targetVelocity > 0.5f)
        {
            _statCamPosX = 0;
            //This works OK. Looks ahead a few segments and extrapolates back
            //so the camera swings out as you go round a bend
            Vector3 AP = Gps.LookAheadOffset(_targetVelocity*4); // Rd.XSecs[LookaheadIdx].MidPt;
            Vector3 PP = Gps.LookAheadOffset(_targetVelocity*3); //Rd.XSecs[ProjectedIdx].MidPt;
            Vector3 CurrentDirection = (transform.position - target.transform.position).normalized;
            Vector3 WantedDirection = (PP - AP).normalized + Vector3.up * 0.6f;
            Vector3 NewDirection = Vector3.Lerp(CurrentDirection, WantedDirection, Time.deltaTime);
            //Vector3 wantedPosition = target.transform.position + (direction * distance) + (Vector3.up * height);
            //Vector3 newPosition = Vector3.Slerp(transform.position, wantedPosition, LerpSpeed * Time.deltaTime);
            transform.position = target.transform.position + NewDirection * 10;
            trFollowCamInner.LookAt(target.transform.position);    //I tried looking in front of the car but it needs a lerp
        }
        else
        {
            //Swing the camera round to look at the side of the car
            //Which side of the car to look at:
            if (_statCamPosX == 0) { float Side = Random.value; if (Side > 0.5) _statCamPosX = -5f; else _statCamPosX = 5f; }
            LerpSpeed = 1;
            Vector3 p = target.transform.position + target.transform.right * _statCamPosX + transform.up*2;
            Vector3 newPosition = Vector3.Lerp(transform.position, p, LerpSpeed * Time.deltaTime);
            transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
            //transform.position = target.transform.position + target.transform.up*15f; // * -5f + transform.up;       //remove this line
            trFollowCamInner.LookAt(target.transform.position);
        }

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

}
