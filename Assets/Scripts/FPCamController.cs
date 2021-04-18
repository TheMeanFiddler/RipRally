using UnityEngine;

public class FPCamController:MonoBehaviour
{
    Transform _parent;
    public GPS Gps;
    Vector3 _defaultPos;
    private Vector3 OldAccel;
    private float OldTiltAngle = 0;
    private bool _horizonTilt = false;

    void Start()
    {
        _parent = transform.parent;
    }

    public void Init()
    {
        Gps = DrivingPlayManager.Current.PlayerCarManager.Gps;
        if (Settings.Instance.SteerControl == "Tilt") { _horizonTilt = true; }
#if UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        OldAccel = Input.acceleration;
#endif
    }

    void LateUpdate()
    {
        float _localLookDesty;
        float _localLooky = transform.localEulerAngles.y;
        if (_localLooky > 180) _localLooky = _localLooky - 360;
        transform.LookAt(_parent.position + Gps.LookAheadOffset(30));
        float _localLookx = transform.localEulerAngles.x;
        if (_localLookx > 30 && _localLookx <= 180) _localLookx = 30;
        if (_localLookx > 180 && _localLookx < 330) _localLookx = 330;
        _localLookDesty = transform.localEulerAngles.y;
        if (_localLookDesty > 180) _localLookDesty = _localLookDesty - 360;

        if (_localLookDesty > 80) _localLookDesty = 80;
        if (_localLookDesty < -80) _localLookDesty = -80;
        if(Mathf.Abs(_localLookDesty - _localLooky)>50)
            _localLookDesty = Mathf.Lerp(_localLooky, _localLookDesty, Time.deltaTime);
        if (_localLookDesty < 0) _localLookDesty = 360 + _localLookDesty;

        transform.localEulerAngles = new Vector3(_localLookx, _localLookDesty, transform.localEulerAngles.z);
#if UNITY_EDITOR
#elif UNITY_ANDROID || UNITY_IOS || UNITY_IPHONE
        if (!_horizonTilt) return;
        Vector3 accel = Vector3.Lerp(OldAccel, Input.acceleration, 0.5f);
        float accelx = Mathf.Clamp(accel.x, -1, 1);
        float DesiredTiltAngle = -Mathf.Asin(accelx) * Mathf.Rad2Deg;
        //if (accel.y > 0) { if (accel.x < 0) DesiredTiltAngle = -180 - DesiredTiltAngle; else DesiredTiltAngle = -DesiredTiltAngle + 180; }
        float TiltAngle = OldTiltAngle * 0.7f + DesiredTiltAngle * 0.3f;
        transform.Rotate(Vector3.forward, TiltAngle);
        OldAccel = Input.acceleration;
        OldTiltAngle = TiltAngle;
#endif
    }
}



