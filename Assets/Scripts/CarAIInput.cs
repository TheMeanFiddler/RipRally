using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 uncomment _playerGPS = 
 Uncomment the yellow chunk - PlayerCollision
 Remove the trAim stuff
 Remove the txtTraceStuff
 Remove the Cam Stuff
 Remove NextTest()
*/
public class CarAIInput : MonoBehaviour, iInputManager
{
    Road Rd;
    int SegIdx = 1;
    XSec _currXSec;
    Bend _currBend;
    Vector3 SegFwdDir;
    float HeadingAngle;
    float _gpsNextBendAngle;
    float _speed;
    Rigidbody _rb;
    Bend _nextBend;
    Bend _approachBend; //Not quite the same
    float TurnInSpeed;
    /// <summary>
    /// Multiply bend speed on the fly.
    /// You can use this on hills and hazards to speed up or slow down
    /// </summary>
    float SpeedAdj = 1; //multiply bend speed on the fly - say you decide to take a bend a bit slower
    float SlideFrac;
    XSec FlickXSec;
    Vector3 AdjustedTurninPt = Vector3.zero;
    XSec AdjustedTurninXSec;
    float PrevFrac;     //Used for turnin and Exit
    int PrevSegIdx;
    float FracPerSeg;
    float ProjectedFrac;
    float DriftAngle;
    bool ReachedFlickEnd;
    float _currGrad;
    float Accel = 1f;
    Vector3 AimPoint;
    float Steer = 0;
    float? SteerOverride = null;
    bool CR_Running = false;
    private GPS _gps;
    private GPS _playerGPS;
    public GPS Gps { set { _gps = value; } }
    private Ray CollisionRay;
    private RaycastHit CollisionHit;
    int Timer = 0;
    float DistFromLKerb;
    float DistFromRKerb;
    bool _playerCollision = false;
    float _playerCollisionTime = -10000;
    CollisionDirection _opponentCollisionDirec = CollisionDirection.None;
    AIInputBuffer _inputBuffer = new AIInputBuffer();
    List<AILesson> AILessons = new List<AILesson>();
    BendPhase _bendPhase;
    //These are calculated in CalcBrakePoint and used in the Approach phase
    float DistToBrakePt;
    float LiftOffSpeed;
    XSec BrakeXSec;
    Vector3 BrakePt;

    float FlickSegs = 0;
    Transform trSTurnMarkerEntry;
    Transform trSTurnMarkerExit;
    Transform trSTurnMarkerInflection;


    void Start()
    {
        Rd = Road.Instance;
        AdjustedTurninXSec = Road.Instance.XSecs[0];
        _playerGPS = DrivingPlayManager.Current.PlayerCarManager.Gps;
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _speed = _gps.Speed;
        _currXSec = _gps.CurrXSec;
        PrevSegIdx = SegIdx;
        SegIdx = _currXSec.Idx;
        if (CR_Running) { return; }
        if (Timer == 0)
        {
            Timer = 5;
        }
        Timer--;
        if (Timer != 4) { goto DoThisEveryFrame; }
        //This bit only runs every 5 frames
        _currBend = _gps.CurrBend;
        _nextBend = _gps.NextBend;
        _gpsNextBendAngle = _nextBend.Angle;
        SegFwdDir = _currXSec.Forward;
        Vector3 Heading = transform.forward;
        HeadingAngle = Vector3.Angle(Heading, SegFwdDir);
        Vector3 cross = Vector3.Cross(Heading, SegFwdDir);
        if (cross.y > 0)
            HeadingAngle = -HeadingAngle;
        //Postition across the cross sec
        Vector3 L = Road.Instance.XSecs[SegIdx].KerbL;
        Vector3 R = Road.Instance.XSecs[SegIdx].KerbR;
        DistFromLKerb = Vector3.Distance(_currXSec.KerbL, transform.position);
        DistFromRKerb = Vector3.Distance(_currXSec.KerbR, transform.position);

        _bendPhase = BendPhase.Straight;

        int ApproachSegs = Mathf.RoundToInt(0.5f * _speed * _speed + _speed / 2);
        XSec _approachXSec = Rd.XSecs[SegIdx + ApproachSegs];
        XSec _decisionXSec = Rd.XSecs[SegIdx + Mathf.RoundToInt(_speed * 2)];
        bool FirstHalfOfBend = _currBend != null && _currXSec.IsBefore(_currBend.ApexXSec);
        if (!FirstHalfOfBend)
        {
            _approachBend = _nextBend;

            if (_approachBend.TurninXSec.IsBefore(_approachXSec))
            {
                _bendPhase = BendPhase.Approach;
                SpeedAdj = 1f;
                DriftAngle = _approachBend.DriftAngle * Mathf.Pow(TurnInSpeed / _approachBend.Speed, 2.8f) * _approachBend.Sign; //this is cool

                //Slide the Adjusted turnin point along the gate
                SlideFrac = Mathf.Sqrt(Mathf.Clamp01(TurnInSpeed / _nextBend.Speed)); // - Mathf.Abs(Mathf.Sin(DriftAngle)) * 1.5f); //SlideFrac=1 for a big swingout
                //Debug.Log("SlideFrac=" + TurnInSpeed  + "/" + _nextBend.Speed + "=" + SlideFrac);
                AdjustedTurninPt = Vector3.Lerp(_nextBend.MinTurninPos, _nextBend.TurninPos, SlideFrac);
                AdjustedTurninXSec = Rd.XSecs[Mathf.RoundToInt(Mathf.Lerp(_nextBend.StartSegIdx, _nextBend.TurninSegIdx, SlideFrac))];

                CalcBrakePoint();

                //Have we reached the decision point? We only decide the flickSegs once we reach the decision point
                if ((_approachBend.TurninXSec.IsBefore(_decisionXSec) && FlickXSec == null) || (FlickXSec != null && _currXSec.IsOnOrBefore(FlickXSec)))
                {
                    FlickSegs = 0;
                    ReachedFlickEnd = false;
                    //This is the basic flickseg pre-calculation = 150/Radius*FlickSegMultiplier
                    FlickSegs = _approachBend.FlickSegs;
                    //Are we approaching at an angle or spinning?
                    float FlickSegAdj = Rd.XSecs.Diff(AdjustedTurninXSec, _approachBend.ApexXSec) * (TurnInSpeed - _approachBend.Speed) / _approachBend.Speed; // (HeadingAngle + _rb.angularVelocity.y * 20) * _approachBend.FlickSegsMultiplier * -_approachBend.Sign;
                    //if (_approachBend.Type == BendType.Right) FlickSegAdj = -FlickSegAdj;
                    Debug.Log("FlickSegs = " + FlickSegs + "   FlickSegAdj=(" + Rd.XSecs.Diff(AdjustedTurninXSec, _approachBend.ApexXSec) + " * " + (TurnInSpeed - _approachBend.Speed) / _approachBend.Speed + " = " + FlickSegAdj + "\nTotal=" + (FlickSegs + FlickSegAdj));
                    FlickSegs += FlickSegAdj;
                    //Too slow, the flick will be too harsh
                    Debug.Log("FlickSegs*=" + TurnInSpeed + "^2/225=" + FlickSegs);
                    //Wrong siede of the road
                    if (_approachBend.Type == BendType.Right ? (DistFromLKerb > DistFromRKerb) : (DistFromRKerb > DistFromLKerb)) FlickSegs = 0;
                    //Is there a long curve on the turnin?
                    //Debug.Log("TurninCurve=" + Vector3.Angle(_approachBend.StartXSec.Forward, _approachBend.TurninXSec.Forward) + "\nFlickSegs=" + FlickSegs);
                    //RequiredRotation worked ok but didnt take into account the initial rotation
                    //float RequiredRotation = Vector3.Angle(transform.forward, _approachBend.ApexXSec.Forward);
                    //FlickXSec = Rd.XSecs[Mathf.RoundToInt(_approachBend.TurninSegIdx - Mathf.Abs(RequiredRotation) / 2.8f)];
                    FlickXSec = Rd.XSecs[AdjustedTurninXSec.Idx - Mathf.RoundToInt(FlickSegs)];
                } //End of Decision

                if (_currXSec.IsAfter(BrakeXSec) && _currXSec.IsBefore(FlickXSec)) _bendPhase = BendPhase.Brake;
                if (FlickXSec != null && _currXSec.IsAfter(FlickXSec))
                {
                    _bendPhase = BendPhase.Turnin;
                }

                PrevFrac = SlideFrac; //used in the turnin
            }//end if Approach
        }   //end if !FirstHalfOfBend
        else
        {   //if FIrstHalfOfBend
            _approachBend = _currBend;
            _bendPhase = BendPhase.Turnin;
        }
        if (_currBend != null && !FirstHalfOfBend)
            if (_bendPhase != BendPhase.Approach)
            { _bendPhase = BendPhase.Exit; FlickXSec = null; }

        if (_bendPhase == BendPhase.Straight)
        {
            Vector3 _apprXSecFwd = _approachXSec.Forward;
            float _curveAngle = Vector3.Angle(SegFwdDir, _apprXSecFwd);
            if (Vector3.Cross(SegFwdDir, _apprXSecFwd).y > 0) _curveAngle = -_curveAngle;
            if (_curveAngle > 15) _bendPhase = BendPhase.LCurve;
            if (_curveAngle < -15) _bendPhase = BendPhase.RCurve;
        }
    //************************************************************************************************
    DoThisEveryFrame:

        AimPoint = Rd.XSecs[SegIdx + 20].MidPt;
        SteerOverride = null;

        if (_bendPhase == BendPhase.Approach)
        {
            Accel = 1; BrakeForce = 0;
            float AvgApprSpeed = (_speed + LiftOffSpeed) / 2;
            float TimeToBrakePt = DistToBrakePt / AvgApprSpeed;

            //Aim for half the dist to the turnin
            XSec AimXSec = Rd.XSecs[_currXSec.Idx + Rd.XSecs.Diff(_currXSec, AdjustedTurninXSec) / 2];
            Vector3 T = (AimXSec.KerbR - AimXSec.KerbL).normalized * _approachBend.TurninGap;

            if (_approachBend.Type == BendType.Right)
            {
                if (TimeToBrakePt < 1) SlideFrac = DistFromRKerb / (DistFromLKerb + DistFromRKerb - _approachBend.TurninGap);
                AimPoint = Vector3.Lerp(AimXSec.KerbR, AimXSec.KerbL + T, SlideFrac);
            }
            else
            {
                if (TimeToBrakePt < 1) SlideFrac = DistFromLKerb / (DistFromLKerb + DistFromRKerb - _approachBend.TurninGap);
                AimPoint = Vector3.Lerp(AimXSec.KerbL, AimXSec.KerbR - T, SlideFrac);
            }

            //I abandoned this because it never had time to straighten up
            /// <image url="$(SolutionDir)\CommonImages\SCurve.png" scale="0.7"/>

        }

        if (_bendPhase == BendPhase.Brake)
        {
            Accel = 0; BrakeForce = 1;
            AimPoint = (AdjustedTurninPt + transform.position + transform.forward) / 2;
        }

        int SegsToApex = 0;
        float diff = 0;


        if (_bendPhase == BendPhase.Turnin)
        {
            float TotSegsToApex = Rd.XSecs.Diff(AdjustedTurninXSec, _approachBend.ApexXSec);
            float SegFrac = 1 - SegsToApex / TotSegsToApex;
            //SegFrac starts at 0 at the turninPt and ends at 1 at the apex
            float TotalRequiredRotation = VectGeom.SignedAngle(AdjustedTurninXSec.Forward, _approachBend.ApexXSec.Forward) + DriftAngle;
            float CurrRotation = VectGeom.SignedAngle(AdjustedTurninXSec.Forward, transform.forward);
            float da = (Mathf.Abs(DriftAngle) < 20 && SegFrac > 0.5f) ? DriftAngle / 2 : DriftAngle;
            float RequiredRotation = VectGeom.SignedAngle(transform.forward, _approachBend.ApexXSec.Forward) + DriftAngle;
            SegsToApex = Rd.XSecs.Diff(_currXSec, _approachBend.ApexXSec);
            float TimeToApex = SegsToApex / _gps.SegsPerSec;
            float ProjectedRotation = _rb.angularVelocity.y * 57.3f * TimeToApex;
            float ProjectedRotation2 = CurrRotation / SegFrac;
            diff = ProjectedRotation - RequiredRotation;

            Accel = 1; BrakeForce = 0;
            if (_speed < 8)
            { AimPoint = _approachBend.Type == BendType.Right ? Rd.XSecs[SegIdx + 10].MidRight : Rd.XSecs[SegIdx + 10].MidLeft; BrakeForce = 0; }
            else
            {
                AimPoint = _approachBend.ApexPos;

                //Debug.Log("rotdiff=" + ProjectedRotation + " - " + RequiredRotation + " = " + diff);
                if (_approachBend.Type == BendType.Right)
                {

                    /*
                    //Manage the Fractional Distance
                    if (PrevSegIdx != SegIdx)
                    {
                        FracPerSeg = (Frac - PrevFrac) / (SegIdx - PrevSegIdx);
                        ProjectedFrac = Frac + FracPerSeg * SegsToApex;
                        //Debug.Log("ProjectedFrac=" + ProjectedFrac);
                        PrevFrac = Frac;
                    }
                    */

                    //Stop understeer
                    if (diff < 0) { BrakeForce = Mathf.Clamp01(-diff / 80); Accel = 1 - BrakeForce; SteerOverride = 40; }
                    if (diff < -40) { BrakeForce = Mathf.Clamp01(-diff / 40); Accel = 1; }
                    if (_rb.angularVelocity.y < 0) { BrakeForce = 0; AimPoint = _approachBend.ApexPos; } //stop the backskid
                }
                if (_approachBend.Type == BendType.Left)
                {
                    if (diff > 0) { BrakeForce = Mathf.Clamp01(diff / 80); Accel = 1 - BrakeForce; SteerOverride = -40; }
                    if (diff > 40) { BrakeForce = Mathf.Clamp01(diff / 40); Accel = 1; }
                    if (_rb.angularVelocity.y > 0) { BrakeForce = 0; AimPoint = _approachBend.ApexPos; } //stop the backskid
                    //if (SegsToApex > 40) { BrakeForce = 0; Accel = 1; AimPoint = Rd.XSecs[SegIdx + 10].MidPt; }  //Stop the serious oversteer
                }

            }
        }
        if (_bendPhase == BendPhase.Exit)
        {
            Accel = 1;
            Vector3 _transverse = (_currXSec.KerbR - _currXSec.KerbL).normalized;
            float SpeedTwdRKerb = Vector3.Dot(_rb.velocity, _transverse);
            //Debug.Log("SpeedTwdKerb=" + SpeedTwdRKerb);
            AimPoint = _currBend.ExitPos; BrakeForce = 0;
            if (_currBend.Type == BendType.Right)
            {
                if (Rd.XSecs.Diff(_currXSec, _currBend.ExitXSec) > 5)
                    AimPoint = _currBend.ExitXSec.MidRight;
            }
            if (_currBend.Type == BendType.Left)
            {
                if (Rd.XSecs.Diff(_currXSec, _currBend.ExitXSec) > 5)
                    AimPoint = _currBend.ExitXSec.MidLeft;
            }
        }

        XSec LongAimXSec = Rd.XSecs[SegIdx + 40];
        Vector3 LongAimPoint = LongAimXSec.MidPt;

        if (_bendPhase == BendPhase.Straight)
        {
            Accel = 1; BrakeForce = 0;
            if (_nextBend == null)
                Debug.Log("nbnull");
            if (_nextBend.Type == BendType.Right)
            {
                AimPoint = Rd.XSecs[SegIdx + 20].MidLeft;
            }
            else
            {
                AimPoint = Rd.XSecs[SegIdx + 20].MidRight;
            }
        }
        if (_bendPhase == BendPhase.RCurve) { Accel = 1; BrakeForce = 0; AimPoint = Rd.XSecs[SegIdx + 20].MidRight; }
        if (_bendPhase == BendPhase.LCurve) { Accel = 1; BrakeForce = 0; AimPoint = Rd.XSecs[SegIdx + 20].MidLeft; }

        //steer round the car in front
        Vector3 FrontOfCar = transform.position + transform.up * 0.5f + transform.forward * 2.5f;
        CollisionRay = new Ray(FrontOfCar, AimPoint + Vector3.up - FrontOfCar);
        int layerMask = (1 << 8);

        if (Physics.Raycast(CollisionRay, out CollisionHit, 50, layerMask))
        {
            if (CollisionHit.collider.name != this.name && CollisionHit.collider.name != "ColldrF")
                if (CollisionHit.rigidbody.velocity.sqrMagnitude < 1)
                {
                    StartCoroutine(ThreePointTurn("R"));
                }
                else
                {
                    AimPoint = Rd.XSecs[SegIdx + 10].MidRight;
                }
        }
        //Debug.DrawRay(FrontOfCar, AimPoint + Vector3.up - FrontOfCar, Color.red);

        //**************************************************************************************
        //Coming up to a hill, put your foot down
        if (LongAimPoint.y - SegFwdDir.y > 0)
        {
            //Accel = ((LongAimPoint.y - SegFwdDir.y) * 0.005f) + 0.9f;
        }

        //coming up to a hill bend slow down
        if (Mathf.Abs(_gpsNextBendAngle) > 10 && _gps.NextHill > 2 && _gps.Speed > 10)
        {
            // Accel = 0; BrakeForce = 0.5f;
        }

        //***********************************************************************************


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

        if (SteerOverride == null)
        {
            //Steer towards the aim point
            float DesiredHeadingAngle = Vector3.Angle(AimPoint - transform.position, SegFwdDir);
            Vector3 crossDes = Vector3.Cross(AimPoint - transform.position, SegFwdDir);
            if (crossDes.y > 0) DesiredHeadingAngle = -DesiredHeadingAngle;

            //Positive steer = right
            Steer = (DesiredHeadingAngle - HeadingAngle);
        }
        else
            Steer = (float)SteerOverride;

        //if (_bendPhase == BendPhase.Flick) Steer = _gpsNextBendAngle > 0 ? 40 : -40;
        if (_opponentCollisionDirec == CollisionDirection.Left && Steer < 0) Steer = 10;
        if (_opponentCollisionDirec == CollisionDirection.Right && Steer > 0) Steer = -10;

        if (_playerCollision)
        {
            if (Time.time - _playerCollisionTime > 3)
            {
                if (_playerGPS.IsOnRoad)
                {
                    if (!_gps.IsOnRoad)
                    {
                        Main.Instance.PopupMsg("Road Rage Bonus\n$20", Color.red);
                        Race.Current.HogBonus += 20;
                        UserDataManager.Instance.Data.Coins += 10;
                    }
                    else if (_playerGPS.CurrSegIdx - _gps.CurrSegIdx > 12 &&_playerGPS.CurrSegIdx - _gps.CurrSegIdx <100)
                    {
                        int pts = (_playerGPS.CurrSegIdx - _gps.CurrSegIdx) / 6;
                        Race.Current.HogBonus += pts;
                        Main.Instance.PopupMsg("Road Hog Bonus\n$" + pts.ToString(), Color.red);
                        UserDataManager.Instance.Data.Coins += pts;
                    }
                }
                _playerCollision = false;
            }
        }

        _inputBuffer.RecordInput(Accel, BrakeForce, Steer, Time.time);
    }


    void OnCollisionStay(Collision coll)
    {
        if (coll.collider.name == "ColldrFL") _opponentCollisionDirec = CollisionDirection.Right;
        if (coll.collider.name == "ColldrFR") _opponentCollisionDirec = CollisionDirection.Left;
        if (coll.contacts[0].otherCollider.name.StartsWith("RoadSec"))
        {
            if (DistFromLKerb < DistFromRKerb)
            {
                AILessons.Add(new AILesson(SegIdx, _inputBuffer, AIResult.HitLFence));
            }
            else
            {
                AILessons.Add(new AILesson(SegIdx, _inputBuffer, AIResult.HitRFence));
            }
        }
    }

    void OnCollisionExit(Collision coll)
    {
        _opponentCollisionDirec = CollisionDirection.None;
    }

    void CalcBrakePoint()
    {
        //Accel coeffs a,b,c Brake Coeffs f,g,h
        float a, b, c, f, g, h;
        float St = _nextBend.Speed;
        float DistToTurnin = Vector3.Distance(transform.position, AdjustedTurninPt);
        switch (_gps.RoadMat)
        {
            case "Dirt": a = 0.125f; b = 0f; break;
            case "DirtyRoad": a = 0.2764f; b = -2.4993f; break;
            case "Washboard": a = 0.1606f; b = -1.0945f; break;
            case "Tarmac": a = 0.1070f; b = -0.2337f; break;
            default: a = 0.3236f; b = -2.1428f; break;
        }
        switch (Rd.Segments[_nextBend.TurninSegIdx].roadMaterial)
        {
            case "Dirt": f = -0.0832f; g = 0f; break;
            case "DirtyRoad": f = -0.05432f; g = -0.1585f; break;
            case "Washboard": f = -0.04990f; g = -0.2070f; break;
            case "Tarmac": f = -0.04776f; g = -0.2191f; break;
            default: f = -0.08367f; g = 0.02521f; break;
        }
        float _adjSpeed = _nextBend.Speed * SpeedAdj;
        c = -a * _speed * _speed - b * _speed;
        h = DistToTurnin - f * _adjSpeed * _adjSpeed - g * _adjSpeed;
        //Sl is the liftoff speed
        LiftOffSpeed = (-(b - g) + Mathf.Sqrt((b - g) * (b - g) - 4 * (a - f) * (c - h))) / (2 * (a - f));
        //Dl is the liftoff dist
        DistToBrakePt = a * LiftOffSpeed * LiftOffSpeed + b * LiftOffSpeed + c;
        if (DistToBrakePt < DistToTurnin)
        {   //Enough time to accelerate before braking
            int SegsToBrakePt = Mathf.RoundToInt(DistToBrakePt * Rd.XSecs.Diff(_currXSec, AdjustedTurninXSec) / DistToTurnin);
            BrakeXSec = Rd.XSecs[_currXSec.Idx + SegsToBrakePt];
            TurnInSpeed = _adjSpeed;
        }
        else
        //going too slow. No need to brake
        {
            DistToBrakePt = DistToTurnin;
            TurnInSpeed = Mathf.Sqrt(_speed * _speed + 2 * a * DistToTurnin);
            BrakeXSec = AdjustedTurninXSec;
        }
    }


    IEnumerator CurveSteer(float Rad, Vector3 EndPt, Vector3 EndDir, bool R)
    {
        float PrevDistSqToEndPt = 1000;
        CR_Running = true;
        while (true)
        {
            float DistSqToEndPt = (transform.position - EndPt).sqrMagnitude;
            Steer = R == true ? 10 : -10;
            if (DistSqToEndPt > PrevDistSqToEndPt) break;
            PrevDistSqToEndPt = DistSqToEndPt;
            yield return new WaitForEndOfFrame();
        }
        CR_Running = false;
        yield return 0;
    }

    IEnumerator ThreePointTurn(string Dir)
    {
        CR_Running = true;
        float TempSteer;
        float TempAccel;
        if (Dir == "R") { TempSteer = 90; } else { TempSteer = -90; }
        TempAccel = -1;
        for (int n = 0; n < 10; n++)
        {
            Steer = TempSteer;
            Accel = TempAccel;
            //Debug.Log("3ptRev " + n.ToString());
            yield return new WaitForEndOfFrame();
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

    public void Dispose() { }
}

public class SqueakInput : MonoBehaviour, iInputManager
{
    public float Accel;
    public float Steer = -0.15f;
    public GPS Gps;

    void Start()
    {

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

    public void Dispose() { }
}

enum CollisionDirection { None, Front, Left, Right }
enum BendPhase { Straight, RCurve, LCurve, Brake, Approach, Turnin, Exit }
