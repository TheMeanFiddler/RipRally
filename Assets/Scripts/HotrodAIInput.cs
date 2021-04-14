using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HotrodAIInput : MonoBehaviour, iInputManager
{
    int SegIdx = 1;
    float HeadingAngle;
    Bend _currBend;
    XSec _currXSec;
    Bend _nextBend;
    float Accel = 0.9f;
    float Steer = 0;
    bool CR_Running = false;
    Road Rd = Road.Instance;
    private GPS _gps;
    private GPS _playerGPS;
    public GPS Gps { set { _gps = value; } }
    private Ray _collisionRay;
    private RaycastHit CollisionHit;
    int Timer = 0;
    CollisionDirection _opponentCollisionDirec = CollisionDirection.None;

    void Start()
    {
        _playerGPS = DrivingPlayManager.Current.PlayerCarManager.Gps;
    }


    void Update()
    {
        if (CR_Running) { return; }
        if (Timer == 0)
        {
            Timer = 10;
        }
        Timer--;
        if (Timer != 0) { return; }
        Accel = 0.9f;
        BrakeForce = 0;
        Steer = 0;
        SegIdx = _gps.CurrSegIdx;
        _currXSec = _gps.CurrXSec;
        _nextBend = _gps.NextBend;
        _currBend = _gps.CurrBend;
        bool FirstHalfOfBend = _currBend != null && _currXSec.IsBefore(_currBend.ApexXSec);
        //Vector3 B = Road.Instance.Segments[SegIdx].verts[4];
        Vector3 SegFwdDir = Road.Instance.XSecs[SegIdx].Forward;
        Vector3 Heading = transform.forward;
        HeadingAngle = Vector3.Angle(Heading, SegFwdDir);
        Vector3 cross = Vector3.Cross(Heading, SegFwdDir);
        if (cross.y > 0)
            HeadingAngle = -HeadingAngle;
        //Postition across the cross sec
        Vector3 L = Road.Instance.XSecs[SegIdx].KerbL;
        Vector3 R = Road.Instance.XSecs[SegIdx].KerbR;
        float DistFromLKerb = Vector3.Distance(L, transform.position);
        float DistFromRKerb = Vector3.Distance(R, transform.position);
        //Calculate entry speed for next bend
        float _entrySpeed = Mathf.Abs(50 / _nextBend.AnglePerSeg);
        //Coming into a sharp bend too fast slow down
        if (_gps.SegsPerSec - _entrySpeed > (_nextBend.StartSegIdx>_gps.CurrSegIdx?_nextBend.StartSegIdx - _gps.CurrSegIdx: _nextBend.StartSegIdx + Rd.Segments.Count - _gps.CurrSegIdx))
        {
            Accel = 0; BrakeForce = 1f;
        }
        else { Accel = 1; BrakeForce = 0; }


        //Aim for the middle of the 10th segment in front
        XSec AimSec = Road.Instance.XSecCircular(SegIdx + 20);
        Vector3 AimPoint = AimSec.MidPt;
        if (FirstHalfOfBend)
        {
            if (_currBend.AnglePerSeg > 1)
            {
                if (_currBend.Type == BendType.Left) AimPoint = AimSec.MidLeft;
                if (_currBend.Type == BendType.Right) AimPoint = AimSec.MidRight;
            }
        }
        Vector3 LongAimPoint = Road.Instance.XSecCircular(SegIdx + 40).MidPt;

        //steer round the car in front
        Vector3 FrontOfCar = transform.position + transform.up * 0.5f + transform.forward * 2.5f;
        _collisionRay = new Ray(FrontOfCar, AimPoint + Vector3.up - FrontOfCar);
        int layerMask = (1 << 8);

        if (Physics.Raycast(_collisionRay, out CollisionHit, 50, layerMask))
        {
            if (CollisionHit.collider.name != this.name)
                if (CollisionHit.rigidbody.velocity.sqrMagnitude < 1)
                {
                    StartCoroutine(ThreePointTurn("R"));
                }
                else
                    AimPoint = (Road.Instance.XSecCircular(SegIdx + 10).MidPt + Road.Instance.XSecCircular(SegIdx + 10).KerbR) / 2;
        }
        //Debug.DrawRay(FrontOfCar, AimPoint + Vector3.up - FrontOfCar, Color.red);

        //***********************************************************************************
        //Steer towards the aim point
        float DesiredHeadingAngle = Vector3.Angle(AimPoint - transform.position, SegFwdDir);
        Vector3 crossDes = Vector3.Cross(AimPoint - transform.position, SegFwdDir);
        if (crossDes.y > 0) DesiredHeadingAngle = -DesiredHeadingAngle;

        // Pointing towards kerb - do a 3 point turn
        if (HeadingAngle > -100 && HeadingAngle < -80 && DistFromLKerb < 3)
        {
            StartCoroutine(ThreePointTurn("L"));
            return;
        }
        if (HeadingAngle < 100 && HeadingAngle > 80 && DistFromRKerb < 3)
        {
            StartCoroutine(ThreePointTurn("R"));
            return;
        }

        //Coming up to a hill, put your foot down
        if (LongAimPoint.y - SegFwdDir.y > 0)
        {
            Accel = ((LongAimPoint.y - SegFwdDir.y) * 0.005f) + 0.9f;
        }


        //coming up to a hill bend slow down
        if (Mathf.Abs(_nextBend.Angle) > 10 && _gps.NextHill > 2 && _gps.Speed > 10)
        {
            Accel = 0; BrakeForce = 0.5f;
        }

        //Positive steer = right
        Steer = (DesiredHeadingAngle - HeadingAngle);

        //Start of bend - flick the wheel
        //copied from CarAIInput
        /*
        //Slide the Adjusted turnin point along the gate
        SlideFrac = Mathf.Sqrt(Mathf.Clamp01(TurnInSpeed / _nextBend.Speed)); // - Mathf.Abs(Mathf.Sin(DriftAngle)) * 1.5f); //SlideFrac=1 for a big swingout
        //Debug.Log("SlideFrac=" + TurnInSpeed  + "/" + _nextBend.Speed + "=" + SlideFrac);
        AdjustedTurninPt = Vector3.Lerp(_nextBend.MinTurninPos, _nextBend.TurninPos, SlideFrac);
        AdjustedTurninXSec = Rd.XSecs[Mathf.RoundToInt(Mathf.Lerp(_nextBend.StartSegIdx, _nextBend.TurninSegIdx, SlideFrac))];
        */

        if (_opponentCollisionDirec == CollisionDirection.Left && Steer < 10) Steer = 10;
        if (_opponentCollisionDirec == CollisionDirection.Right && Steer > -10) Steer = -10;
    }

    void OnCollisionStay(Collision coll)
    {
        if (coll.collider.name == "ColldrFL") _opponentCollisionDirec = CollisionDirection.Right;
        if (coll.collider.name == "ColldrFR") _opponentCollisionDirec = CollisionDirection.Left;
    }

    void OnCollisionExit(Collision coll)
    {
        _opponentCollisionDirec = CollisionDirection.None;
    }


    IEnumerator ThreePointTurn(string Dir)
    {
        CR_Running = true;
        float TempSteer;
        float TempAccel;
        if (Dir == "R") { TempSteer = 90; } else { TempSteer = -90; }
        TempAccel = -1;
        for (int n = 0; n < 60; n++)
        {
            Steer = TempSteer;
            Accel = TempAccel;
            //Debug.Log("3ptRev " + n.ToString());
            yield return 0;
        }

        CR_Running = false;
        Accel = 1f;
        yield return 0;
    }
    public float WMovement()
    {
        return 0;
    }
    public float ZMovement()
    {
        return Accel;
    }

    public float XMovement()
    {
        return Steer;
    }

    public float YMovement()
    {
        return 0;
    }

    public bool Brake()
    {
        return false;
    }

    public float BrakeForce { get; set; }

    public void Dispose()
    {
        //Not used - uses this instead...
    }
    void OnDestroy()
    {
        Gps = null;
        _gps = null;
        _playerGPS = null;
    }
}

