using UnityEngine;
using System.Linq;


public delegate void GPSEventHandler(GPSEventArgs args);

/// <summary>
/// Position sensor who's primary job is to say what segment a vehicle is on
/// </summary>
/// 
public class GPS
{
    public int CurrSegIdx { get; set; }
    public XSec CurrXSec;
    public Bend CurrBend;
    public Vector3 RaycastHitPos { get; set; }
    int layerMask = ~((1 << 8) + (1 << 10) + (1 << 13) + (1 << 2));
    //we want to collide against everything except these layers. The ~ operator does this, it inverts a bitmask.
    //8 = Car, 10 = terrain, 2 = IgnoreRaycast, 13 = fence
    int airLayerMask = ~((1 << 8) + (1 << 2));
    public bool IsOnRoad { get; set; }
    public bool IsInAir { get; set; }
    public event GPSEventHandler OnTakeOff;
    public event GPSEventHandler OnLand;
    public event GPSEventHandler OnRecovery;
    public event GPSEventHandler OnDrift;
    public event GPSEventHandler OnDriftEnd;
    public event GPSEventHandler OnDriftFail;
    bool JustLanded;
    float PrevAngle;
    bool _drifting;
    int AirRecoveryTimer;
    int _driftStartSegIdx;
    private GameObject _goVehicle;
    private bool Player = false;
    public bool RecoveryAllowed { get; set; }
    private Rigidbody _rb;
    private Road Rd;
    public float NextHill { get; set; }
    public string RoadMat { get; set; }
    RaycastHit hit;
    int PrevSegIdx;
    public float SegTime;
    float _segsPerSec;
    //Bend _firstStraightBend;


    public GPS(GameObject GO)
    {
        _goVehicle = GO;
        if (_goVehicle.name == "Vehicle0") Player = true;
        _rb = _goVehicle.GetComponent<Rigidbody>();
        Rd = Road.Instance;
        //_firstStraightBend = Rd.Bends.FirstOrDefault(b => b.Type == BendType.Straight);
    }

    public float SegsPerSec
    {
        get { return _segsPerSec; }
    }


    public Bend NextBend
    {
        get
        {
            return CurrXSec.NextBend;
        }
    }

    public iRoadSectn CurrectSectn
    {
        get
        {
            return Rd.Sectns[Rd.Segments[CurrSegIdx].SectnIdx];
        }
    }

    public float NextBendAnglePerSeg
    {
        get
        {
            float rtn;
            Bend b = NextBend;
            if (b == null)
            {
                rtn = 0;
            }
            else
            {
                rtn = b.AnglePerSeg;
            }
            return rtn;
        }
    }

    public float Speed
    {
        get { return _rb.velocity.magnitude; }
    }
    public float SqrSpeed
    {
        get { return _rb.velocity.sqrMagnitude; }
    }

    public void UpdateSegIdx()
    {
        //raycast to see which segment is underneath us
        if (Physics.Raycast(_goVehicle.transform.position + Vector3.up * 3 + _goVehicle.transform.forward, Vector3.down, out hit, 8, layerMask))
        {
            RaycastHitPos = hit.point;
            if (hit.collider.name.Contains("Seg"))
            {
                CurrSegIdx = System.Convert.ToInt16(hit.collider.name.Substring(7));
                CurrXSec = Rd.XSecs[CurrSegIdx];
                CurrBend = CurrXSec.CurrBend;
                RoadMat = Rd.Segments[CurrSegIdx].roadMaterial;
                IsOnRoad = true;
            }
            else
            {
                IsOnRoad = false;
            }
        }
        else
        { IsOnRoad = false; }

        //Calculate How long to change segments
        if (PrevSegIdx != CurrSegIdx) { _segsPerSec = (CurrSegIdx - PrevSegIdx) / (Time.time - SegTime); SegTime = Time.time; }
        PrevSegIdx = CurrSegIdx;
        if (Time.time - SegTime > 5 && Player == true && RecoveryAllowed)
        {
            //SHOW THE RECOVERY PANEL
            RecoveryAllowed = false;
            MusicPlayer.Instance.StepDown();
            Transform canv = GameObject.FindObjectOfType<Canvas>().transform;
            GameObject pnlRecover = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlRecover"), canv);
            //pnlRecover.transform.localScale = Vector3.one;
            //pnlRecover.GetComponent<RectTransform>().anchoredPosition = Vector2.up;
            //pnlRecover.GetComponent<RectTransform>().offsetMin = new Vector2(0, 350);
            //pnlRecover.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            pnlRecover.GetComponent<RecoverPanel>().Init(this, _goVehicle, CurrSegIdx);
        }

        //See if we are in the air - raycast to see if there is anything at all below us by one metre
        if (Physics.Raycast(_goVehicle.transform.position + Vector3.up + _goVehicle.transform.forward, Vector3.down, out hit, 2, airLayerMask))
        {
            if (IsInAir) { Land(); }
            IsInAir = false;
            if (hit.collider.name.Contains("Terrain"))
            { RoadMat = "Dirt"; }
        }
        else
        {
            if (!IsInAir) { TakeOff(); }
            IsInAir = true;
        }

        if (JustLanded)
        {
            AirRecoveryTimer++;
            Vector3 RoadVector = Road.Instance.XSecs[CurrSegIdx].Forward;
            Vector3 VehVector = _goVehicle.transform.forward;
            float Angl = Vector3.Angle(RoadVector, VehVector);
            float DiffAngl = Angl - PrevAngle;

            if (AirRecoveryTimer > 30 && Angl < 20 && DiffAngl > -30 && DiffAngl < 30)
            {
                Recovery();
                JustLanded = false;
            }
            PrevAngle = Angl;
        }

        //Drift measurement
        if (IsOnRoad && _rb.velocity.sqrMagnitude > 0.2f)
        {
            float _driftAngle = Vector3.Angle(_goVehicle.transform.forward, _rb.velocity);
            if (_drifting && _driftAngle > 90) { DriftFail(); }

            if (_driftAngle > 20 && !IsInAir)
            {
                Vector3 SegFwd = Rd.XSecs[CurrSegIdx].Forward;
                int _segIdxPlusOne = CurrSegIdx + 1;
                if (_segIdxPlusOne > Rd.Segments.Count - 1) _segIdxPlusOne = _segIdxPlusOne - Rd.Segments.Count;
                Vector3 SegPlusOneFwd = Rd.XSecs[_segIdxPlusOne].Forward;
                Vector3 cross = Vector3.Cross(SegPlusOneFwd, SegFwd);
                Vector3 vehcross = Vector3.Cross(_goVehicle.transform.forward, _rb.velocity);
                if (Mathf.Sign(vehcross.y) == Mathf.Sign(cross.y)) // { Debug.Log("OK"); } else { Debug.Log("Fail"); }
                { if (!_drifting) { Drift(); } }
                else { if (_drifting) EndDrift(); }
            }
            else if (_drifting) { EndDrift(); }
        }
        else if (_drifting) { DriftFail(); }


        /*
        if (_drifting)
        {
            _driftRecoveryTimer++;
            Vector3 RoadVector = Road.Instance.XSecs[SegIdx].Forward;
            Vector3 VehVector = _goVehicle.transform.forward;
            float Angl = Vector3.Angle(RoadVector, VehVector);
            float DiffAngl = Angl - PrevAngle;

            if (_driftRecoveryTimer > 100 && Angl < 10 && DiffAngl > 0 && DiffAngl < 10)
            {
                DriftRecovery();
                _drifting = false;
            }
            PrevAngle = Angl;
        }
        */
    }

    protected virtual void Drift()
    {
        if (OnDrift != null)
        {
            _drifting = true;
            GPSEventArgs args = new GPSEventArgs { SegIdx = CurrSegIdx };
            OnDrift(args);
        }
    }

    protected virtual void EndDrift()
    {
        _drifting = false;
        GPSEventArgs args = new GPSEventArgs { SegIdx = CurrSegIdx };
        OnDriftEnd(args);
    }

    protected virtual void DriftFail()
    {
        _drifting = false;
        GPSEventArgs args = new GPSEventArgs { SegIdx = CurrSegIdx };
        OnDriftFail(args);
    }

    protected virtual void TakeOff()
    {
        if (OnTakeOff != null)
        {
            GPSEventArgs args = new GPSEventArgs { SegIdx = CurrSegIdx };
            OnTakeOff(args);
        }
    }

    protected virtual void Land()
    {
        if (OnLand != null)
        {
            JustLanded = true;
            AirRecoveryTimer = 0;
            GPSEventArgs args = new GPSEventArgs { SegIdx = CurrSegIdx };
            OnLand(args);
        }
    }

    protected virtual void Recovery()
    {
        if (OnRecovery != null)
        {
            GPSEventArgs args = new GPSEventArgs { SegIdx = this.CurrSegIdx };
            OnRecovery(args);
        }
    }

    protected virtual void DriftRecovery()
    {
        if (OnDriftEnd != null)
        {
            GPSEventArgs args = new GPSEventArgs { SegIdx = this.CurrSegIdx };
            OnDriftEnd(args);
        }
    }

    public void CollideRoadSection()
    {
        if (_drifting)
        {
            DriftFail();
        }
    }

    public float SegmentFracProgress
    {
        get
        {
            int PrevCarIdx = CurrSegIdx;
            int NextCarIdx = CurrSegIdx + 1;
            if (NextCarIdx > Rd.Segments.Count - 1) NextCarIdx = NextCarIdx - Rd.Segments.Count;
            float DistTrav = VectGeom.DistancePointToLine(Rd.XSecs[PrevCarIdx].KerbL, Rd.XSecs[PrevCarIdx].KerbR, RaycastHitPos);
            float DistRem = VectGeom.DistancePointToLine(Rd.XSecs[NextCarIdx].KerbL, Rd.XSecs[NextCarIdx].KerbR, RaycastHitPos);
            return CurrSegIdx + DistTrav / (DistTrav + DistRem);
        }
    }

    public float NextBendAngle()
    {
        if (!IsOnRoad) return 0;
        try
        {
            return NextBend.Angle;
        }
        catch { return 0; }
    }

    public Vector3 NextInnerCurve
    {
        get
        {
            //find the right kerb
            float ViewAnglR = -170;
            float ViewAnglL = 170;
            Vector3 rtn = Vector3.zero;
            Vector2 Pt1 = VectGeom.Convert2d(Rd.XSecs[CurrSegIdx].Forward);
            Vector2 Pt2R, Pt2L;
            int s;

            for (s = CurrSegIdx; s < CurrSegIdx + 200; s++)
            {
                if (s > Rd.XSecs.Count - 1) s -= Rd.XSecs.Count - 1;
                Pt2L = VectGeom.Convert2d(Rd.XSecs[s].KerbL - _goVehicle.transform.position);
                float AnglL = Vector2.Angle(Pt1, Pt2L);
                Vector3 cross = Vector3.Cross(new Vector3(Pt1.x, 0, Pt1.y), new Vector3(Pt2L.x, 0, Pt2L.y));
                if (cross.y > 0) AnglL = -AnglL;
                if (AnglL < ViewAnglL) { ViewAnglL = AnglL; } else { rtn = Rd.XSecs[s - 1].KerbL * 0.7f + Rd.XSecs[s - 1].MidPt * 0.3f; break; }

                Pt2R = VectGeom.Convert2d(Rd.XSecs[s].KerbR - _goVehicle.transform.position);
                float AnglR = Vector2.Angle(Pt1, Pt2R);
                cross = Vector3.Cross(new Vector3(Pt1.x, 0, Pt1.y), new Vector3(Pt2R.x, 0, Pt2R.y));
                if (cross.y > 0) AnglR = -AnglR;
                if (AnglR > ViewAnglR) { ViewAnglR = AnglR; } else { rtn = Rd.XSecs[s - 1].KerbR * 0.7f + Rd.XSecs[s - 1].MidPt * 0.3f; break; }
            }

            return rtn;

        }
    }

    public Vector3 LookAheadOffset(float LookAheadDist)
    {
        float LookaheadSegmentProgress = SegmentFracProgress + LookAheadDist;
        int LookaheadIdx = (int)LookaheadSegmentProgress;
        float frac = LookaheadSegmentProgress - LookaheadIdx;
        if (LookaheadIdx > Rd.Segments.Count - 1) LookaheadIdx = LookaheadIdx - Rd.Segments.Count;
        int LookaheadIdxPlusOne = LookaheadIdx + 1;
        if (LookaheadIdxPlusOne > Rd.Segments.Count - 1) LookaheadIdxPlusOne = LookaheadIdxPlusOne - Rd.Segments.Count;
        return Rd.XSecs[LookaheadIdx].MidPt + (Rd.XSecs[LookaheadIdxPlusOne].MidPt - Rd.XSecs[LookaheadIdx].MidPt) * frac - _goVehicle.transform.position;
    }

    void PlaceMarker(Vector3 Pt)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        marker.transform.position = Pt;
    }

    //Destructor
    ~GPS()
    {
        _goVehicle = null;
        Rd = null;
    }
}

public class ReplayerGPS
{
    private GameObject _goVehicle;
    private Road Rd;

    public ReplayerGPS(GameObject go)
    {
        _goVehicle = go;
        Rd = Road.Instance;
    }

    //Destructor
    ~ReplayerGPS()
    {
        _goVehicle = null;
        Rd = null;
    }
}

