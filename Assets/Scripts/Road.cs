using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public interface iRoad
{
    List<iRoadSectn> Sectns { get; }
    List<RoadSegment> Segments { get; }
    CircleList<XSec> XSecs { get; }
    Vector3 StartingLinePos { get; set; }
    Quaternion StartingLineRot { get; set; }
    XSec XSecCircular(int Idx);
    RacingLine RacingLine { get; }
}

public sealed class Road : iRoad
//Singleton class
{

    private List<iRoadSectn> _Sectns = new List<iRoadSectn>();
    private List<RoadSegment> _Segments = new List<RoadSegment>();
    private CircleList<XSec> _XSecs = new CircleList<XSec>();
    private List<XSec> _geomXSecs = new List<XSec>();
    private RacingLine _racingLine = new RacingLine();
    private BezierLine _bez;
    public GameObject goRoad;
    public GameObject StartingLine;
    public Vector3 BuilderPos;
    public Quaternion BuilderRot;
    public int StartingLineSegIdx = 10;
    public bool IsCircular = false;
    static Road _instance;
    static readonly object padlock = new object();
    public List<iRoadSectn> Sectns { get { return _Sectns; } }
    public List<RoadSegment> Segments { get { return _Segments; } }
    public CircleList<XSec> XSecs { get { return _XSecs; } }
    public List<XSec> GeomXSecs { get { return _XSecs; } }
    public CircleList<Bend> Bends { get; set; }
    public RacingLine RacingLine { get { return _racingLine; } }
    public Vector3 StartingLinePos { get; set; }
    public Quaternion StartingLineRot { get; set; }
    public int RutCount { get; set; }
    public int SkidMkId { get; set; }
    public Queue<FlatLineRenderer> SkidMks;
    public Dictionary<String, Material> RoadMaterials;

    //THis is where it instantiates itself
    public static Road Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new Road(); }
                return _instance;
            }
        }
    }
    // Constructor - Use this for initialization
    public Road()
    {
        Init();
    }

    public void Init()
    {
        //This is just because the sections and segments start at Idx=1
        //Segment[0] doesn't do anything
        Segments.Clear();
        XSecs.Clear();
        Sectns.Clear();
        //RacingLine = new RacingLine();
        IsCircular = false;
        RoadSectn sectn = new RoadSectn();
        Sectns.Add(sectn);
        sectn.Idx = 0;
        //Put all the materials in a dictionary for faster retrieval
        Material M;
        RoadMaterials = new Dictionary<string, Material>();
        M = (Material)Resources.Load("Prefabs/Materials/Tarmac", typeof(Material));
        RoadMaterials.Add("Tarmac", M);
        M = (Material)Resources.Load("Prefabs/Materials/Tarmac0", typeof(Material));
        RoadMaterials.Add("Tarmac0", M);
        M = (Material)Resources.Load("Prefabs/Materials/Tarmac1", typeof(Material));
        RoadMaterials.Add("Tarmac1", M);
        M = (Material)Resources.Load("Prefabs/Materials/Tarmac2", typeof(Material));
        RoadMaterials.Add("Tarmac2", M);
        M = (Material)Resources.Load("Prefabs/Materials/Tarmac3", typeof(Material));
        RoadMaterials.Add("Tarmac3", M);
        M = (Material)Resources.Load("Prefabs/Materials/Washboard0", typeof(Material));
        RoadMaterials.Add("Washboard0", M);
        M = (Material)Resources.Load("Prefabs/Materials/Washboard1", typeof(Material));
        RoadMaterials.Add("Washboard1", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtyRoad", typeof(Material));
        RoadMaterials.Add("DirtyRoad", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtRoad0", typeof(Material));
        RoadMaterials.Add("DirtRoad0", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtRoad1", typeof(Material));
        RoadMaterials.Add("DirtRoad1", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtRoad2", typeof(Material));
        RoadMaterials.Add("DirtRoad2", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtRoad3", typeof(Material));
        RoadMaterials.Add("DirtRoad3", M);
        M = (Material)Resources.Load("Prefabs/Materials/TarmacUnderside", typeof(Material));
        RoadMaterials.Add("TarmacUnderside", M);
        M = (Material)Resources.Load("Prefabs/Materials/WashboardUnderside", typeof(Material));
        RoadMaterials.Add("WashboardUnderside", M);
        M = (Material)Resources.Load("Prefabs/Materials/DirtyRoadUnderside", typeof(Material));
        RoadMaterials.Add("DirtyRoadUnderside", M);
        SkidMks = new Queue<FlatLineRenderer>();
    }

    public FlatLineRenderer NextSkidMk(FlatLineRenderer Prev = null)
    {
        if (SkidMks.Count == 0) return null;
        if (Prev != null)
        {
            Prev.enabled = false;
            SkidMks.Enqueue(Prev);
        }
        if (SkidMks.Count == 0) return null;
        FlatLineRenderer nxt = SkidMks.Dequeue();
        nxt.enabled = true;
        return nxt;
    }

    public void CalculateBends()
    {
        if (!IsCircular) return;
        Bends = new CircleList<Bend>();
        Bend _bend = new Bend();
        int BendId = 0;
        MovingAvgFloat AngleQueue = new MovingAvgFloat(5);
        _bez = BezierLine.Instance;
        int XSecCount = XSecs.Count();
        Vector3 PrevMid;
        Vector3 ThisMid;
        Vector3 NextMid;
        BendType _bt = BendType.Unknown;
        BendType _prevbt = BendType.Unknown;
        int Incr = 1;
        float AngleThreshold = 1.34f;
        foreach (XSec X in XSecs)
        {
            PrevMid = XSecs.Prev(X).MidPt;
            ThisMid = X.MidPt;
            NextMid = XSecs.Next(X).MidPt;
            float Angle = VectGeom.SignedAngle(ThisMid + ThisMid - PrevMid, ThisMid, NextMid, Vector3.up);
            float _segDist = Vector3.Distance(ThisMid, NextMid);
            AngleQueue.Push(Angle);
            if (Mathf.Abs(AngleQueue.Avg) < AngleThreshold) _bt = BendType.Straight;
            else _bt = AngleQueue.Avg > 0 ? BendType.Right : BendType.Left;

            if (_prevbt == _bt)
            {
                _bend.Angle += Angle;
            }
            else
            {
                if (_prevbt == BendType.Left || _prevbt == BendType.Right)
                {  //Finish off the previous bend
                    if (Mathf.Abs(_bend.Angle) < 15)
                    {
                        //ignore small bends
                        Bends.Remove(_bend);
                        BendId--;
                    }
                    else
                    {
                        //take 2 off cos of moving avg
                        _bend.EndXSec = XSecs[X.Idx - 2];
                        _bend.EndSegIdx = _bend.EndXSec.Idx;
                        _bend.ApexXSec = XSecs[_bend.StartSegIdx + Mathf.RoundToInt(XSecs.Diff(_bend.StartXSec, _bend.EndXSec) / 2)];
                        _bend.ApexSegIdx = _bend.ApexXSec.Idx;
                        if (_bend.Type == BendType.Right) { _bend.ApexPos = _bend.ApexXSec.KerbR + (_bend.ApexXSec.KerbL - _bend.ApexXSec.KerbR).normalized; _bend.MinTurninPos = _bend.StartXSec.KerbR + (_bend.StartXSec.KerbL - _bend.StartXSec.KerbR).normalized * (_bend.Concatenated ? 4 : 2); }
                        if (_bend.Type == BendType.Left) { _bend.ApexPos = _bend.ApexXSec.KerbL + (_bend.ApexXSec.KerbR - _bend.ApexXSec.KerbL).normalized; _bend.MinTurninPos = _bend.StartXSec.KerbL + (_bend.StartXSec.KerbR - _bend.StartXSec.KerbL).normalized * (_bend.Concatenated ? 4 : 2); }
                        //Fmax = mv^2/r
                        float c = (_bend.EndSegIdx - _bend.StartSegIdx) * 360 / _bend.Angle;
                        float r = Mathf.Abs(c) / Mathf.PI / 2f;
                        //Debug.Log("Bend" + _bend.BendId + " radius = " + r);
                        _bend.SqrtRad = Mathf.Sqrt(r);

                    }
                }
                else
                {   //We might be starting a new bend or carrying n the previous one
                    if (_bt == BendType.Left || _bt == BendType.Right)
                    {
                        bool StartNewBend = true;

                        if (BendId > 0 && X.Idx - Bends[BendId - 1].EndSegIdx < 15 && Bends[BendId - 1].Type == _bt) ///bugbugbug circle bug
                        { //if the bend we've just finished is close to and the same sign as one before
                          //We just carry on with this bend and dont create a new one
                            StartNewBend = false;
                            _bend.Concatenated = true;
                            GameObject.Destroy(GameObject.Find("Turnin" + _bend.BendId));
                            GameObject.Destroy(GameObject.Find("Apex" + _bend.BendId));
                            GameObject.Destroy(GameObject.Find("BendStart" + _bend.BendId));
                        }

                        if (StartNewBend)
                        {
                            //Create a new Bend
                            _bend = new Bend();
                            _bend.Type = _bt;
                            _bend.Sign = _bt == BendType.Right ? (Int16)1 : (Int16)(-1);
                            _bend.StartXSec = XSecs[X.Idx - 2];//-2 cos of moving avg
                            _bend.StartSegIdx = _bend.StartXSec.Idx;
                            _bend.Angle = Angle;
                            _bend.BendId = BendId;
                            Bends.Add(_bend);
                            BendId++;
                        }
                    }
                }
                //******************************************************************************************

            }
            _prevbt = _bt;
        }
        //This is the last bend. NOt a brilliant bit but it doesn't crash
        if (_bt == BendType.Straight) goto Finalise;
        _bend.EndSegIdx = XSecs.Count - 1;
        _bend.EndXSec = XSecs[_bend.EndSegIdx];
        if (_bend.EndSegIdx <= _bend.StartSegIdx && Bends.Count() > 0)
        { Bends.Remove(_bend); Bends.Last().EndSegIdx = XSecs.Count; }
        else
        {
            _bend.ApexSegIdx = Mathf.RoundToInt((_bend.EndSegIdx + _bend.StartSegIdx) / 2);
            _bend.ApexXSec = XSecs[_bend.ApexSegIdx];
            if (_bend.Type == BendType.Right) _bend.ApexPos = _bend.ApexXSec.KerbR + (_bend.ApexXSec.KerbL - _bend.ApexXSec.KerbR).normalized;
            if (_bend.Type == BendType.Left) _bend.ApexPos = _bend.ApexXSec.KerbL + (_bend.ApexXSec.KerbR - _bend.ApexXSec.KerbL).normalized;
        }
    Finalise:
        CalculateTurninAndExitPoints();
        PopulateXSecCurrBends();
    }

    private void CalculateTurninAndExitPoints()
    {
        foreach (Bend _bend in Bends)
        {
            _bend.CalcTurnin();
            //**********************DEBUGGING***************************************
            //if (_bend.Type != BendType.Straight) PlaceApexMarker(_bend.ApexPos, 15, "Green", "Apex" + _bend.BendId);
            //if (_bend.Type == BendType.Right) PlaceTurninMarker(_bend.TurninPos, _bend.StartXSec.KerbR, "Turnin" + _bend.BendId);
            //if (_bend.Type == BendType.Left) PlaceTurninMarker(_bend.TurninPos, _bend.StartXSec.KerbL, "Turnin" + _bend.BendId);
            //if (_bend.Type == BendType.Right) PlaceMarker(PrimitiveType.Cylinder, _bend.StartXSec.KerbR, 10, "BendStart" + _bend.BendId);
            //if (_bend.Type == BendType.Left) PlaceMarker(PrimitiveType.Cylinder, _bend.StartXSec.KerbL, 10, "BendStart" + _bend.BendId);
            //for (int s = _bend.StartSegIdx; s < _bend.EndSegIdx; s++) { PlaceMarker(PrimitiveType.Sphere, XSecs[s].MidPt, 10, "BendMrkr"); }
            //Debug.Log("Bend" + _bend.BendId + ": Radius=" + _bend.Radius + "    Speed=" + _bend.Speed + "  Angle=" + _bend.Angle + " AngPerSeg=" + _bend.AnglePerSeg);
            //**********************************************************************
        }
    }
    private void PopulateXSecCurrBends()
    {
        foreach (Bend _bend in Bends)
        {
            //These two saved a massive performance hit
            Bend _nb = Bends.Next(_bend);
            for (XSec x = _bend.TurninXSec; x != _bend.ExitXSec; x = XSecs.Next(x))
                x.CurrBend = _bend;
            if (_nb.TurninXSec == null)
                Debug.Log("BendId" + _bend.BendId + " no turnin");
            else
            for (XSec x = _bend.TurninXSec; x != _nb.TurninXSec; x = XSecs.Next(x))
                x.NextBend = _nb;
        }
    }




    public void CreateGameObjects()
    {
        UnityEngine.Profiling.Profiler.BeginSample("RdCreateGOs");
        goRoad = GameObject.Find("Road");
        for (int Idx = 1; Idx < Sectns.Count; Idx++)
        {
            Sectns[Idx].CreateGameObjects();
        }
        for (int Idx = 0; Idx < Segments.Count; Idx++)
        {
            Segments[Idx].CreateGameObjects();
        }

        UnityEngine.Profiling.Profiler.EndSample();
    }
    public bool SegmentExists(int Idx)
    {
        bool rtn = false;
        foreach (RoadSegment S in Segments)
        {
            if (S.Idx == Idx) { rtn = true; break; }
        }
        return rtn;
    }

    public void RenderRoad()
    {
        UnityEngine.Profiling.Profiler.BeginSample("RenderRoad");
        foreach (RoadSegment seg in Segments)
        {
            if (seg.HasMesh)
            {
                if (seg.goSeg == null) seg.CreateGameObjects();
                seg.AddMeshes();
                seg.SetMaterial();
                seg.CreateFenceColliderVerts();
            }
        }
        foreach (RoadSectn RS in Sectns)
        {
            RS.CreateFence();
            if (PlayerManager.Type != "BuilderPlayer")
                RS.CreateFenceColliders();
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    public void RemoveRuts()
    {
        try
        {
            List<GameObject> children = new List<GameObject>();
            foreach (GameObject r in GameObject.FindGameObjectsWithTag("Rut"))
            {
                children.Add(r);
            }
            children.ForEach(child => GameObject.Destroy(child));
            RutCount = 0;
            children.Clear();

            Transform Skidmarks = GameObject.Find("Skidmarks").transform;
            foreach (Transform tran in Skidmarks)
            {
                children.Add(tran.gameObject);
            }
            children.ForEach(child => GameObject.Destroy(child));
        }
        catch (System.Exception e) { Debug.Log(e.ToString()); }

    }



    public void DeleteLastSectn()
    {
        //Actually we delete the penultimate section because the last section is empty
        if (Sectns.Count < 2) return;
        int LastSectnId = Sectns.Count - 1;
        int PenultSectnId = Sectns.Count - 2;
        iRoadSectn LastSection = Sectns.Last();
        iRoadSectn PenultSection = Sectns[Sectns.Count - 2];
        //iRoadSectn PrevSection = Sectns[Sectns.Count - 3];
        //int PrevSegCount = PrevSection.Segments.Count;
        int PenultSegCount = PenultSection.Segments.Count;
        int PenultSegStartId = PenultSegCount > 0 ? PenultSection.Segments[0].Idx : 0;

        //I tested this function using Watches on all the XSecCounts, SegmentCounts and SegStart and SegCount for each section
        //comparing them before adding and deleting a section and then after

        //Remove Section's Segment GameObjects
        foreach (RoadSegment seg in PenultSection.Segments)
            GameObject.Destroy(seg.goSeg);

        if (!IsCircular)
        {
            Segments.RemoveRange(PenultSegStartId, PenultSegCount);
            PenultSection.Segments.Clear();
            PenultSection.DeleteFence();
            XSecs.RemoveRange(PenultSegStartId, PenultSegCount);
            //PenultSection.XSecs.Clear();
        }

        if (IsCircular)
        {

            PenultSection.DeleteFence();
            Segments.RemoveRange(PenultSegStartId, PenultSegCount);
            PenultSection.Segments.Clear();
            XSecs.RemoveRange(PenultSegStartId, PenultSegCount);
            //PenultSection.XSecs.Clear();
            //Road.Instance.XSecs.RemoveRange(PrevSegStartId, PrevSegCount);
        }

        //Delete Section
        GameObject.Destroy(LastSection.goSectn);
        Sectns.Remove(LastSection);

        BezierLine.Instance.RemoveLastControlPoint();  //also removes the path points and selects the previous CtrlPt
        IsCircular = false;
        Game.current.Dirty = true;
    }

    public void OrganiseObjectsUsingBezierCtrlPtsAndPath()
    {
        SetSectionIds();
        foreach (BezCtrlPt CtrlPt in BezierLine.Instance.CtrlPts)
        {
            if (CtrlPt.CtrlPtId == BezierLine.Instance.CtrlPts.Count - 1) break;
            Sectns[CtrlPt.CtrlPtId].name = "RoadSection" + CtrlPt.CtrlPtId;
            if (Sectns[CtrlPt.CtrlPtId].goSectn != null)
            {
                Sectns[CtrlPt.CtrlPtId].goSectn.name = "RoadSection" + CtrlPt.CtrlPtId;
            }
            if (CtrlPt.goRdMkr != null)
            {
                CtrlPt.goRdMkr.name = "RoadMarker" + CtrlPt.CtrlPtId;
                CtrlPt.goRdMkr.transform.SetParent(Sectns[CtrlPt.CtrlPtId].goSectn.transform);
                CtrlPt.goRdMkr.GetComponent<RoadMarker>().Index = CtrlPt.CtrlPtId;
            }
            Sectns[CtrlPt.CtrlPtId].Segments.Clear();
            for (int pth = CtrlPt.SegStartIdx; pth < CtrlPt.SegStartIdx + CtrlPt.SegCount; pth++)
            {
                Sectns[CtrlPt.CtrlPtId].Segments.Add(Segments[pth]);
                Segments[pth].SectnIdx = CtrlPt.CtrlPtId;
                Segments[pth].goSeg.transform.SetParent(Sectns[CtrlPt.CtrlPtId].goSectn.transform);
            }


        }
    }

    public void SetXSecIds()
    {
        for (int i = 0; i < XSecs.Count; i++)
        {
            XSecs[i].Idx = i;
        }
    }

    public void ListXSecs()
    {
        foreach (XSec x in XSecs)
        {
            Debug.Log("Index=" + XSecs.IndexOf(x) + " - " + x.Idx + x.MidPt);
        }
    }


    internal void SetSectionIds()
    {
        for (int s = 0; s < Sectns.Count; s++)
        {
            Sectns[s].Idx = s;
            if (s != 0) Sectns[s].goSectn.name = "RoadSection" + s;
        }
    }


    /// <summary>
    /// Dont need this any more - convert to XSecs[i] overload
    /// </summary>
    /// <param name="Idx"></param>
    /// <returns></returns>
    public XSec XSecCircular(int Idx)
    {
        if (IsCircular)
        {
            if (Idx > XSecs.Count - 1) return XSecs[Idx - XSecs.Count];
            if (Idx < 0) return XSecs[XSecs.Count + Idx];
        }
        return XSecs[Idx];
    }

    public XSec XSecPlusOne(XSec XS)
    {
        int Idx = XS.Idx + 1;
        if (Idx == _XSecs.Count)
        {
            return _XSecs[0];
        }
        else
        {
            return _XSecs[Idx];
        }
    }

    public iRoadSeg SegPlusOne(iRoadSeg seg)
    {
        int Idx = seg.Idx + 1;
        if (Idx == _Segments.Count)
        {
            return _Segments[0];
        }
        else
        {
            return _Segments[Idx];
        }
    }

    public iRoadSeg SegMinusOne(iRoadSeg seg)
    {
        int Idx = seg.Idx - 1;
        if (Idx == -1)
        {
            return _Segments.Last();
        }
        else
        {
            return _Segments[Idx];
        }
    }

    private void PlaceMarker(PrimitiveType Type, Vector3 Pos, float height, string name)
    {
        GameObject marker = GameObject.CreatePrimitive(Type);
        marker.transform.localScale = new Vector3(1f, height, 1f);
        marker.name = name;
        marker.GetComponent<Collider>().enabled = false;
        marker.transform.position = Pos;
    }

    private void PlaceMarkerArrow(Vector3 Pos, Vector3 LookAt, string name)
    {
        UnityEngine.Object objArrow = Resources.Load("Prefabs/Gizmos/GizmoArrow");
        GameObject marker = (GameObject)GameObject.Instantiate(objArrow);
        marker.transform.localScale = new Vector3(6f, 0.5f, 0.5f);
        marker.name = name;
        marker.transform.position = Pos;
        marker.transform.LookAt(LookAt);
        marker.transform.Rotate(Vector3.up, -90, Space.Self);
    }

    public void PlaceMarker(Vector3 Pos, float height, string Color, string name)
    {
        Material Mat;
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.transform.localScale = new Vector3(0.5f, height / 5, 0.5f);
        if (Color == "Red")
            Mat = (Material)Resources.Load("Prefabs/Materials/Car_Red");
        else if (Color == "Green")
            Mat = (Material)Resources.Load("Prefabs/Materials/BrightGreen");
        else if (Color == "White")
            Mat = (Material)Resources.Load("Prefabs/Materials/Chrome");
        else
            Mat = (Material)Resources.Load("Prefabs/Materials/Chrome");
        marker.GetComponent<MeshRenderer>().sharedMaterial = Mat;
        marker.name = name;
        marker.transform.position = Pos;
        marker.GetComponent<Collider>().enabled = false;
    }

    private void PlaceTurninMarker(Vector3 TurninPos, Vector3 BendStartPos, string name)
    {
        GameObject marker = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/TurninMarker"));
        marker.name = name;
        marker.GetComponentInChildren<Text>().text = name;
        marker.transform.position = TurninPos;
        marker.transform.LookAt(BendStartPos);
        marker.transform.Find("TurninArrow").localScale = new Vector3(1, 1, Vector3.Distance(TurninPos, BendStartPos));
    }

    private void PlaceApexMarker(Vector3 Pos, float height, string Color, string name)
    {
        GameObject marker = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/ApexMarker"));
        marker.name = name;
        marker.GetComponentInChildren<Text>().text = name;
        marker.transform.position = Pos;
    }

    public void PlaceCircleMarker(Vector3 Pos, float Rad, string Name)
    {
        GameObject goCirc;

        CapsuleCollider Circ;
        goCirc = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goCirc.GetComponent<BoxCollider>().enabled = false;
        Circ = goCirc.AddComponent<CapsuleCollider>();
        goCirc.name = Name;
        Circ.height = 100;
        goCirc.transform.position = Pos;
        Circ.radius = Rad;
    }
    public void CreateRoofCollider()
    {
        if (XSecs.Count != 0)

        //  0 - 2 - 4 - 6 -...This section has 3 XSecs. 6 and 7 belong to the  next Sectn
        //  | \ | \ | \ |      so we'll only put a roof over the first 2
        //  1 - 3 - 5 - 7

        {
            Vector3[] RoofVerts = new Vector3[XSecs.Count * 2];
            int[] RoofTris = new int[XSecs.Count * 6 - 6];
            for (int i = 0; i < XSecs.Count() - 1; i++)
            {
                RoofVerts[i * 2] = XSecs[i].TerrainL + Vector3.up * 2;
                RoofVerts[i * 2 + 1] = XSecs[i].TerrainR + Vector3.up * 2;
                RoofTris[i * 6] = i * 2;
                RoofTris[i * 6 + 1] = i * 2 + 1;
                RoofTris[i * 6 + 2] = i * 2 + 3;
                RoofTris[i * 6 + 3] = i * 2;
                RoofTris[i * 6 + 4] = i * 2 + 3;
                RoofTris[i * 6 + 5] = i * 2 + 2;
            }

            Mesh MR = new Mesh();
            MR.vertices = RoofVerts;
            MR.triangles = RoofTris;
            MR.RecalculateBounds();
            MR.RecalculateNormals();
            ;
            GameObject goTempRoof = new GameObject();
            goTempRoof.name = "goTempRoof";
            goTempRoof.AddComponent<MeshCollider>().sharedMesh = MR;
        }
    }

    public void RemoveRoofCollider()
    {
        if (XSecs.Count != 0)
        {
            GameObject.Destroy(GameObject.Find("goTempRoof"));
        }
    }
}

public class XSec : IEquatable<XSec>
{
    public int Idx { get; set; }
    public Vector3 MidPt { get; set; }
    public Vector3 KerbL { get; set; }
    public Vector3 KerbR { get; set; }
    public Vector3 TerrainL { get; set; }
    public Vector3 TerrainR { get; set; }
    public Bend CurrBend { get; set; }
    public Bend NextBend { get; set; }
    private Road Rd = Road.Instance;

    //constructor
    public XSec(int I)
    {
        Idx = I;
    }

    public void Adjust(XSec XSecAdj)
    {
        KerbL = KerbL + XSecAdj.KerbL;
        KerbR = KerbR + XSecAdj.KerbR;
        MidPt = (KerbL + KerbR) / 2;
        TerrainL = (KerbL - MidPt) * 1.3f + MidPt;
        TerrainR = (KerbR - MidPt) * 1.3f + MidPt;
    }

    public bool Equals(XSec other)
    {
        return (this.Idx == other.Idx);
    }

    public bool IsBefore(XSec other)
    {
        return Rd.XSecs.IsOrderedAs(this, other);
    }

    public bool IsOnOrBefore(XSec other)
    {
        return Rd.XSecs.IsOrderedAs(this, other) || this == other;
    }

    public bool IsAfter(XSec other)
    {
        return Rd.XSecs.IsOrderedAs(other, this);
    }


    public Vector3 MidLeft { get { return (MidPt + KerbL) / 2; } }
    public Vector3 MidRight { get { return (MidPt + KerbR) / 2; } }

    public Vector3 Forward
    {
        get
        {
            return (Road.Instance.XSecs[Idx + 1].MidPt) - MidPt;
        }
    }

    public Vector3 Right
    {
        get
        {
            return (Vector3.Cross(Forward, Vector3.up));
        }
    }
}

public class Bend : IEquatable<Bend>
{
    public int BendId;
    public int CtrlPtId;
    public int StartSegIdx;
    public XSec StartXSec;
    public int EndSegIdx;
    public XSec EndXSec;
    public BendType Type;
    /// <summary>
    /// +1 for right and -1 for left;
    /// </summary>
    public Int16 Sign;
    public bool Concatenated;
    public float Angle;
    public int FlickSegs;
    public int TurninSegIdx;
    public XSec TurninXSec;
    public XSec ApexXSec;
    public int ApexSegIdx;
    public XSec ExitXSec;
    public Vector3 ExitPos;
    public float AnglePerSeg { get { return Angle / (EndSegIdx - StartSegIdx); } }
    public float RacelineAnglePerSeg;
    public float RacelineSegLength;
    public float SqrtRad;
    public float Radius;
    public float Speed;
    public float Grad;
    public float FlickSegsMultiplier = 1;
    public float DriftAngle;
    public Vector3 ApexPos;
    /// <summary>
    /// Turnin Dist from outer kerb
    /// Default = 4m
    /// </summary>
    public float TurninGap = 4;
    public Vector3 MinTurninPos;
    public Vector3 TurninPos;
    public Bend PrevBend { get { return Rd.Bends[BendId - 1]; } }
    public Bend NextBend { get { return Rd.Bends[BendId + 1]; } }
    private Road Rd = Road.Instance;

    internal void CalcTurnin()
    {
        //GameObject goCirc;

        //CapsuleCollider Circ;
        //goCirc = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //goCirc.GetComponent<BoxCollider>().enabled = false;
        //Circ = goCirc.AddComponent<CapsuleCollider>();
        //goCirc.name = "CircColl" + BendId;
        //Circ.height = 100;
        Radius = SqrtRad * SqrtRad;
        Vector3 Centre = new Vector3(0, 0, 0);
        Vector3 C = new Vector3(0, 0, 0);
        Vector3 AP = VectGeom.Convert2d(ApexPos);
        Vector3 TP = new Vector3(0, 0, 0);
        Vector3 EP = new Vector3(0, 0, 0);
        int e = 0;
        float ExitFrac = 1;//1 is a wide turn, 0 is tight
        float SmallestRadErr = 1000;
        PossBendCircle bbc = new PossBendCircle();
        Vector3 ApexPos2d = VectGeom.Convert2d(ApexPos);
        float NxtBndDist = Rd.XSecs.Diff(ApexXSec, NextBend.ApexXSec);
        //Method:
        //Try a range of turnin segs
        //For each Turnin seg, draw a line perpendicular to the fwd direction
        //and find the turninPt (TP)
        //Find the midpoint (MP) of the line from apex to turninPoint and draw a line perpendicular to this (MPerp)
        //Where MPerp crosses TPerp is the centre of the circle, C
        /// <image url="$(SolutionDir)\CommonImages\CalcTurninAlgorithm.png" scale="1.2"/>
        if (Type == BendType.Right)
        {
            if (NextBend.Type == BendType.Left && NxtBndDist < 200) ExitFrac = NxtBndDist / 200;
            for (XSec tx = Rd.XSecs[StartSegIdx-70]; tx.IsBefore(Rd.XSecs[StartSegIdx]); tx = Rd.XSecs.Next(tx))    //removed circlebug
            {
                Vector2 TPerp = VectGeom.Convert2d(Vector3.Cross(Vector3.up, tx.Forward));
                TP = VectGeom.Convert2d(tx.KerbL + (tx.KerbR - tx.KerbL).normalized * TurninGap);
                Vector2 MP = (ApexPos2d + TP) / 2;
                Vector2 MPerp = new Vector2((ApexPos2d - TP).y, -(ApexPos2d - TP).x);
                if (VectGeom.LineLineIntersection(out C, TP, TPerp, MP, MPerp)) //these vars are all Vector3(x,0,y)
                {
                    float biggestCos = 0;
                    float R = Vector2.Distance(TP, C);
                    PossBendCircle pbc = new PossBendCircle();
                    float RadErr = 1000;
                    for (e = EndXSec.Idx + 70; e > EndXSec.Idx; e--) //bugbugbug circlebug
                    {
                        Vector2 WideExitPt = VectGeom.Convert2d(Rd.XSecs[e].KerbL + (Rd.XSecs[e].KerbR - Rd.XSecs[e].KerbL).normalized * 2);
                        Vector2 TightExitPoint = VectGeom.Convert2d(Rd.XSecs[e].KerbR + (Rd.XSecs[e].KerbL - Rd.XSecs[e].KerbR).normalized * 2);
                        EP = Vector2.Lerp(TightExitPoint, WideExitPt, ExitFrac);
                        Vector3 EPerp = VectGeom.Convert2d(-Rd.XSecs[e].Right).normalized;
                        float cos = Mathf.Abs(Vector2.Dot((EP - C).normalized, EPerp));
                        if (cos > biggestCos)
                        {
                            RadErr = Mathf.Abs(Vector3.Distance(EP, C) - R);
                            pbc = new PossBendCircle { C = C, RSq = 0, RadErr = RadErr, EP = EP, EX = Rd.XSecs[e], TP = TP, TX = tx };
                            biggestCos = cos;
                        }
                    }
                    if (RadErr < SmallestRadErr)
                    {
                        bbc = new PossBendCircle { C = pbc.C, RSq = 0, RadErr = pbc.RadErr, EP = pbc.EP, EX = pbc.EX, TP = pbc.TP, TX = pbc.TX };
                        SmallestRadErr = pbc.RadErr;
                    }
                }
            }
            Centre = new Vector3(bbc.C.x, ApexPos.y, bbc.C.y);
            Radius = Vector2.Distance(bbc.TP, bbc.C);
            TurninXSec = bbc.TX;
            TurninSegIdx = TurninXSec.Idx;
            TurninPos = new Vector3(bbc.TP.x, ApexPos.y, bbc.TP.y);
            ExitXSec = bbc.EX;
            ExitPos = new Vector3(bbc.EP.x, ApexPos.y, bbc.EP.y);
            goto FoundCentre;
            //Not found Centre
            Debug.Log("No Centre for Bend" + BendId);
            ExitXSec = Rd.XSecs[e];
            Debug.Break();
        }
        if (Type == BendType.Left)
        {
            if (NextBend.Type == BendType.Right && NxtBndDist < 200) ExitFrac = NxtBndDist / 200;
            for (int t = StartSegIdx - 70; t < StartSegIdx; t++)    //bugbugbug circlebug
            {
                Vector2 TPerp = VectGeom.Convert2d(Vector3.Cross(Vector3.up, Rd.XSecs[t].Forward));
                TP = VectGeom.Convert2d(Rd.XSecs[t].KerbR + (Rd.XSecs[t].KerbL - Rd.XSecs[t].KerbR).normalized * TurninGap);
                Vector2 MP = (ApexPos2d + TP) / 2;
                Vector2 MPerp = new Vector2((ApexPos2d - TP).y, -(ApexPos2d - TP).x);
                if (VectGeom.LineLineIntersection(out C, TP, TPerp, MP, MPerp)) //these vars are all Vector3(x,0,y)
                {
                    float biggestCos = 0;
                    float R = Vector2.Distance(TP, C);
                    PossBendCircle pbc = new PossBendCircle();
                    float RadErr = 1000;
                    for (e = EndXSec.Idx + 70; e > EndXSec.Idx; e--) //bugbugbug circlebug
                    {
                        Vector2 WideExitPt = VectGeom.Convert2d(Rd.XSecs[e].KerbR + (Rd.XSecs[e].KerbL - Rd.XSecs[e].KerbR).normalized * 2);
                        Vector2 TightExitPoint = VectGeom.Convert2d(Rd.XSecs[e].KerbL + (Rd.XSecs[e].KerbR - Rd.XSecs[e].KerbL).normalized * 2);
                        EP = Vector2.Lerp(TightExitPoint, WideExitPt, ExitFrac);
                        Vector3 EPerp = VectGeom.Convert2d(Rd.XSecs[e].Right).normalized;
                        float cos = Mathf.Abs(Vector2.Dot((EP - C).normalized, EPerp));
                        if (cos > biggestCos)
                        {
                            RadErr = Mathf.Abs(Vector3.Distance(EP, C) - R);
                            pbc = new PossBendCircle { C = C, RSq = 0, RadErr = RadErr, EP = EP, EX = Rd.XSecs[e], TP = TP, TX = Rd.XSecs[t] };
                            biggestCos = cos;
                        }
                    }
                    if (RadErr < SmallestRadErr)
                    {
                        bbc = new PossBendCircle { C = pbc.C, RSq = 0, RadErr = pbc.RadErr, EP = pbc.EP, EX = pbc.EX, TP = pbc.TP, TX = pbc.TX };
                        SmallestRadErr = pbc.RadErr;
                    }
                }
            }
            Centre = new Vector3(bbc.C.x, ApexPos.y, bbc.C.y);
            Radius = Vector2.Distance(bbc.TP, bbc.C);
            TurninXSec = bbc.TX;
            TurninSegIdx = TurninXSec.Idx;
            TurninPos = new Vector3(bbc.TP.x, ApexPos.y, bbc.TP.y);
            ExitXSec = bbc.EX;
            ExitPos = new Vector3(bbc.EP.x, ApexPos.y, bbc.EP.y);
            goto FoundCentre;
            //Not found Centre
            Debug.Log("No Centre for Bend" + BendId);
            ExitXSec = Rd.XSecs[e];
            Debug.Break();
        }
    FoundCentre:
        //goCirc.transform.position = Centre;
        //Circ.radius = Radius;
        Angle = VectGeom.SignedAngle(TurninXSec.Forward, ExitXSec.Forward);
        float RacelineSegCount = Rd.XSecs.Diff(TurninXSec, ExitXSec); ;
        RacelineAnglePerSeg = Angle / RacelineSegCount;
        RacelineSegLength = Angle * Mathf.Deg2Rad * Radius / RacelineSegCount;
        CalculateSpeed();
        //Circ.enabled = false; //cos the gameobject doesnt get destroyed till end of frame
                              //GameObject.Destroy(goCirc);
        return;

    NoTurnin:
        if (TurninXSec == null)
        {
            TurninXSec = Rd.XSecs[ApexSegIdx - 50]; TurninSegIdx = TurninXSec.Idx;
            ExitXSec = Rd.XSecs[ApexSegIdx + (ApexSegIdx - TurninSegIdx)];
            if (Type == BendType.Right) TurninPos = TurninXSec.KerbL + (TurninXSec.KerbR - TurninXSec.KerbL).normalized * 4f;
            else TurninPos = TurninXSec.KerbR + (TurninXSec.KerbL - TurninXSec.KerbR).normalized * 4f;
            Radius = 100;
            AnalyseTurnin();
            CalculateSpeed();
        }
        //Circ.enabled = false; //cos the gameobject doesnt get destroyed till end of frame
                              //GameObject.Destroy(goCirc);
    }

    struct PossBendCircle { public Vector3 C; public float RSq; public float RadErr; public XSec TX; public Vector3 TP; public XSec EX; public Vector3 EP; }

    private void AnalyseTurnin()
    {
        return;
        int Adj = Mathf.RoundToInt(Mathf.Abs(RacelineAnglePerSeg) * 5 - 5); // for hairpins, shift the turnin earlier so it doesnt hit the fence
        switch (Rd.Segments[TurninSegIdx].roadMaterial)
        {
            case "Tarmac": Adj = Mathf.RoundToInt(Mathf.Abs(RacelineAnglePerSeg) * 5 - 5); break;
            case "Washboard": Adj = Mathf.RoundToInt(Mathf.Abs(RacelineAnglePerSeg) * 5 - 5); break;
            case "DirtyRoad": Adj = Mathf.RoundToInt(Mathf.Abs(RacelineAnglePerSeg) * 5 - 5); break;
            case "Dirt": Adj = Mathf.RoundToInt(Mathf.Abs(RacelineAnglePerSeg) * 5 - 7); break;
        }
        TurninXSec = Rd.XSecs[TurninSegIdx - Adj];
        TurninSegIdx = TurninXSec.Idx;
    }

    private void CalculateSpeed()
    {
        Speed = Mathf.Sqrt(Radius) * 2.8f;// + (2*Radius-70)/30;
        if (Speed > 25) Speed = Speed + (Speed - 25) / 2;
        switch (Rd.Segments[StartSegIdx].roadMaterial)
        {
            case "Tarmac": Speed = Speed * 1.6f; DriftAngle = 10f * Mathf.Rad2Deg / Radius; break;
            case "Washboard": Speed = Speed * 1.4f; DriftAngle = 12f * Mathf.Rad2Deg / Radius; break;
            case "DirtyRoad": Speed = Speed * 1.2f; DriftAngle = 16f * Mathf.Rad2Deg / Radius; break;
            case "Dirt": { Speed = Speed * 1f; DriftAngle = 20f * Mathf.Rad2Deg / Radius; break; }
        }
        if (Radius > 50) DriftAngle = 0;
        Grad = (ApexPos.y - TurninPos.y) / Vector3.Distance(ApexPos, TurninPos);
        switch (Rd.Segments[TurninSegIdx].roadMaterial)
        {
            case "Tarmac": FlickSegsMultiplier = 0.8f; break;
            case "Washboard": FlickSegsMultiplier = 0.9f; break;
            case "DirtyRoad": FlickSegsMultiplier = 1.4f; break;
            case "Dirt": FlickSegsMultiplier = 1f; break;
        }
        FlickSegs = Mathf.RoundToInt(150 / Radius * FlickSegsMultiplier);
    }

    public bool IsBefore(Bend other)
    {
        return Rd.Bends.IsOrderedAs(this, other);
    }

    public bool Equals(Bend other)
    {
        return this.BendId == other.BendId;
    }
}

public enum BendType { Straight, Right, Left, Unknown };

public enum RoadMat : byte { Tarmac, Washboard, DirtyRoad, Dirt, Air }