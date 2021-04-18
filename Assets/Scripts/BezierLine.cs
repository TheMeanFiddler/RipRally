using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

//https://www.youtube.com/watch?v=mTPyMMigc5Y


public class BezierLine
{
    //public List<Vector3> ControlPoints = new List<Vector3>();   //2BRemoved
    public int LineId;
    public List<BezCtrlPt> CtrlPts = new List<BezCtrlPt>();
    public List<Vector3> Path = new List<Vector3>();
    private GameObject goLine;
    private LineRenderer line;
    private Road Rd;
    static BezierLine _instance;
    static readonly object padlock = new object();

    public static BezierLine Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new BezierLine(); }
                return _instance;
            }
        }
    }

    //constructor
    public BezierLine()
    {
        Rd = Road.Instance;
   }

    /// <summary>
    /// This gets called on a new game. It doesn't get called when you load a saved game
    /// </summary>
    public void Init(){
        Path.Clear();
        CtrlPts.Clear();
    //Add 2 control points - this is the minimum to get it working
        for (int i = 0; i < 2; i++) {
        BezCtrlPt BCP = new BezCtrlPt(this, Vector3.zero);
        CtrlPts.Add(BCP);
        }
        BezCtrlPt.Current = CtrlPts[0];
        SetCtrlPtIds();
    }
    

    public void CreateGameObject()
    {
        goLine = new GameObject("BezierLine");
        line = goLine.AddComponent<LineRenderer>();
        Material Matrl = (Material)Resources.Load("Prefabs/Materials/BezierLine");
        line.material = Matrl; //new Material((Shader.Find("Mobile/Particles/Additive"));
        line.startWidth=0.3F;
        //line.SetColors(Color.green, Color.green);
    }

    public void SetWidth(float w)
    {
        line.startWidth = w;
    }

    public void CreateRoadMarkers()
    {
        for(int Idx = 1;Idx < CtrlPts.Count-1;Idx++)
        {
            CtrlPts[Idx].CreateRoadMarker();
        }
        AlignAllRoadMarkers();
    }


    public void AddControlPoint(Vector3 Pt, float BankAngle, int segCount)
    {
        BezCtrlPt BCP = new BezCtrlPt(this, Pt);
        BCP.BankAngle = BankAngle;
        CtrlPts[CtrlPts.Count - 2].SegCount = segCount;     //set the SegCount of the previous CtrlPt
        CtrlPts.Insert(CtrlPts.Count - 1, BCP);

        SetCtrlPtIds();
        SetSegStartIds();
        CtrlPts[CtrlPts.Count - 1].Pos = Pt;        //cos the last control point is a dummy and is in the same place
        if (BCP.CtrlPtId == 1) CtrlPts[0].Pos = Pt;           //cos the first one is a dummy too
        if (BCP.CtrlPtId > 1) { AddPathPoints(segCount);}
    }


    /// <summary>
    /// Inserts a control point just after the selected one
    /// </summary>
    public void InsertControlPoint(int idx)
    {
        Vector3 InsertPos = Path[(idx-1) * 20 + 10];
        BezCtrlPt InsertedCP = new BezCtrlPt(this, InsertPos);
        InsertPathPoints(idx);        //CtrlPt 3 is Path 40
        CtrlPts.Insert(idx + 1, InsertedCP);  //this adds an element just before idx+1
        SetCtrlPtIds();
        InsertedCP.CreateRoadMarker();
        //Rename all the Roadmarkers after the inserted one
        for (int idxAdj = idx + 2; idxAdj < CtrlPts.Count; idxAdj++)
        {
            GameObject goRM = CtrlPts[idxAdj].goRdMkr;
            if (goRM != null)
            {
                RoadMarker RM = goRM.GetComponent<RoadMarker>();
                RM.name = "RoadMarker" + idxAdj;
                RM.Index = idxAdj;
            }
        }
        //dunno why it ended up with the wrong parent
        CtrlPts[idx+2].goRdMkr.transform.SetParent(Rd.Sectns[idx+2].goSectn.transform);
    }

    public void RemoveLastControlPoint()
    {
        int IdxToRemove = CtrlPts.Count - 2;
        BezCtrlPt CPToRemove = CtrlPts[IdxToRemove];
        BezCtrlPt PrevCP = CtrlPts[IdxToRemove-1];

        //This isnt quite right but it'll do
        //I hope it doesn't cause a memory leak
        //DO we have to remove the Section and Segments and the gameobjects?
        int PrevCtrlPtId=-1;
        int NxtCtrlPtId=-1;
        try
        {
            PrevCtrlPtId = CPToRemove.CtrlPtId-1;
            NxtCtrlPtId = CPToRemove.CtrlPtId+1;
        }
        catch (Exception e) {Debug.Log(e.Message); }
        //not needed ? //if(Road.Instance.IsCircular) Path.RemoveRange(Path.Count - 20, 20);
        Path.RemoveRange(PrevCP.SegStartIdx, PrevCP.SegCount);
        CtrlPts.Remove(CPToRemove);
        PrevCP.SegCount = 0;
        PrevCP.Select();
        SetCtrlPtIds();
        SetSegStartIds();
        DrawLine();
    }

    public void SetCtrlPtIds()
    {
        for (int i = 0; i < CtrlPts.Count;i++ ) {
            CtrlPts[i].CtrlPtId = i;
        }
    }

    public void SetSegStartIds()
    {
        for (int i = 1; i < CtrlPts.Count; i++) { CtrlPts[i].SegStartIdx = CtrlPts[i-1].SegStartIdx + CtrlPts[i-1].SegCount; }
    }

    public Vector3 LimitSlope(Vector3 Pt, float MaxSlope)
    {
        if (CtrlPts.Count == 2) return Pt;                  //Because the first click can be anywhere
        BezCtrlPt PrevCtrlPt = CtrlPts[CtrlPts.Count - 2];
        Vector3 Pt1 = PrevCtrlPt.Pos;
        Vector3 Pt2 = Pt;
        float HorizDist = Vector2.Distance(new Vector2(Pt1.x, Pt1.z), new Vector2(Pt2.x, Pt2.z));
        //If point is too close move it further away
        if (HorizDist < 20)
        {
            Vector3 NewPt_3d = Pt1 + Vector3.Normalize(Pt2 - Pt1) * 20f;
            Pt2 = NewPt_3d;
            HorizDist = Vector2.Distance(new Vector2(Pt1.x, Pt1.z), new Vector2(Pt2.x, Pt2.z));
        }
        //if point is too high lower it
        float VertDist = Pt2.y - Pt1.y;
        float Slope = 0;
        if(HorizDist>0) Slope = VertDist / HorizDist;
        if (Slope > MaxSlope)
        {
            //Debug.Log("Lowering cos too steep");
            Slope=MaxSlope;
            float NewVertDist = Slope * HorizDist;
            Pt2.y = Pt1.y + NewVertDist;
        }
        //Dont allow the angle less than 30 degrees
        float angle = 180;
        if (CtrlPts.Count > 3)
        {
            angle = Angle(CtrlPts[CtrlPts.Count - 3].Pos, PrevCtrlPt.Pos, Pt2);
            if (Mathf.Abs(angle) < 45)
            {
                //Debug.Log("Widening angle cos its too acute");
                float RotAngle;
                if (angle > 0) RotAngle = 45; else RotAngle = -45;
                Quaternion Rot = Quaternion.Euler(0, RotAngle - angle, 0);
                Vector3 NewPt_3d = VectGeom.RotateAroundPoint(Pt2, Pt1, Rot);
                Pt2 = NewPt_3d;
                angle = RotAngle;
            }
        }

        //if hairpins are too sharp and too close together, spread them out
        
        if (HorizDist * Mathf.Abs(angle) < 1500)
        {
            //Debug.Log("Moving CtrlPt away from hairpin");
            Vector3 NewPt_3d = Pt1 + Vector3.Normalize(Pt2 - Pt1) *  1500/ Mathf.Abs(angle);
            Pt2 = NewPt_3d;
            HorizDist = Vector2.Distance(new Vector2(Pt1.x, Pt1.z), new Vector2(Pt2.x, Pt2.z));

        }

        if (Slope > 0 && Mathf.Abs(angle) / Slope < 270)
        {
            //Debug.Log("Flattening steep hairpin");
            Slope = Mathf.Abs(angle) / 270;
            float NewVertDist = Slope * HorizDist;
            Pt2.y = Pt1.y + NewVertDist;
        }

        return Pt2;
        //Debug.Log("Dist = " + HorizDist.ToString());
        //Debug.Log("Slope = " + Slope.ToString());
        //Debug.Log("Angle = " + angle.ToString());
        //Typical values
        //      Dist 10 - 30
        //      Slope < 0.3
    }



    private void AddPathPoints(int segCount)
    {
        for (int i = 0; i < segCount; i++) Path.Add(Vector3.zero);
    }

    /// <summary>
    /// Insert 20 empty path points just before the controlpoint 
    /// </summary>
    /// <param name="CtrlPtId"></param>
    private void InsertPathPoints(int CtrlPtId)
    {
        for (int i = 0; i < 20; i++) Path.Insert(CtrlPtId*20, Vector3.zero);
    }

    /// <summary>
    /// Interpolates the section before and after the control point
    /// </summary>
    /// <param name="CtlPtIdx"></param>
    public void Interp(int CtlPtIdx)
    {
        //Interpolate the section before the control point
        if (CtlPtIdx > 1 && CtlPtIdx < CtrlPts.Count - 1)
        {
            Vector3 a = CtrlPts[CtlPtIdx - 2].Pos;
            Vector3 b = CtrlPts[CtlPtIdx - 1].Pos;
            Vector3 c = CtrlPts[CtlPtIdx].Pos;
            Vector3 d = CtrlPts[CtlPtIdx + 1].Pos;
            float t=0.5f;
            int NoOfSegs = CtrlPts[CtlPtIdx - 1].SegCount;
            for (int p = 0; p < NoOfSegs; p++)
            {
                float u = (float)p / NoOfSegs;

                //Coeffs for the catmullrom equation
                Vector3 c0 = b;
                Vector3 c1 = -t*a + t*c;
                Vector3 c2 = 2f*t*a + (t-3f)*b + (3f-2f*t)*c -t*d;
                Vector3 c3 = -t*a + (2f-t)*b + (t-2f)*c + t*d;
                Vector3 temp = c0 + c1*u + c2*u*u + c3*u*u*u;
                //Vector3 temp = .5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
                Path[CtrlPts[CtlPtIdx - 1].SegStartIdx + p] = (temp);
            }
        }
        //Interpolate the section after the control point
        if (CtlPtIdx ==0)return;
        if(CtlPtIdx < CtrlPts.Count - 2) //Dont bother if this is the penultimate control point
        {
            Vector3 a = CtrlPts[CtlPtIdx - 1].Pos;
            Vector3 b = CtrlPts[CtlPtIdx].Pos;
            Vector3 c = CtrlPts[CtlPtIdx + 1].Pos;
            Vector3 d = CtrlPts[CtlPtIdx + 2].Pos;
            float t = 0.5f;
            int NoOfSegs = CtrlPts[CtlPtIdx].SegCount;
            for (int p = 0; p < NoOfSegs; p++)
            {
                float u = (float)p / NoOfSegs;


                Vector3 c0 = b;
                Vector3 c1 = -t * a + t * c;
                Vector3 c2 = 2f * t * a + (t - 3f) * b + (3f - 2f * t) * c - t * d;
                Vector3 c3 = -t * a + (2f - t) * b + (t - 2f) * c + t * d;
                Vector3 temp = c0 + c1 * u + c2 * u * u + c3 * u * u * u;
                Path[CtrlPts[CtlPtIdx].SegStartIdx + p] = (temp);
            }
        }
    }

    public void AlignAllRoadMarkers()
    {
        foreach (BezCtrlPt C in CtrlPts)
        {
            C.AlignRoadMarker();
        }
    }

    public void DeleteAllRoadMarkers()
    {
        foreach (BezCtrlPt cp in CtrlPts)
        {
            cp.DeleteRoadMarker();
        }
    }

    public Vector3 PathPlusOne(int PathId)
    {
        int rtnId;
        rtnId = PathId + 1;
        if (Rd.IsCircular && rtnId > Path.Count - 1) rtnId = rtnId - Path.Count;
        return Path[rtnId];
    }

    public BezCtrlPt CtrlPtPlusOne(int CtrlPtId)
    {
        int rtnId;
        rtnId = CtrlPtId + 1;
        if (Rd.IsCircular && rtnId > CtrlPts.Count - 1) rtnId = rtnId - CtrlPts.Count;
        return CtrlPts[rtnId];
    }

    public BezCtrlPt CtrlPtMinusOne(int CtrlPtId)
    {
        int rtnId;
        rtnId = CtrlPtId - 1;
        if (rtnId < 1) rtnId = CtrlPts.Count + rtnId - 3;
        return CtrlPts[rtnId];
    }

    public BezCtrlPt CtrlPtPlus(int CtrlPtId, int Increment)
    {
        int rtnId;
        rtnId = CtrlPtId + Increment;
        if (rtnId > CtrlPts.Count - 1) rtnId = rtnId - CtrlPts.Count;
        else if (rtnId<1) rtnId = CtrlPts.Count + rtnId-2;
        return CtrlPts[rtnId];
    }

    public Vector2 Convert2d(Vector3 V)
    {
        return new Vector2(V.x,V.z);
    }





    private void PlaceMarker(Vector3 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        marker.transform.position = Pos;
    }

    private void PlaceMarker(Vector2 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.transform.localScale=new Vector3(0.3f,20f,0.3f);
        marker.transform.position = new Vector3(Pos.x, 10f, Pos.y);
    }





    public void DrawLine()
    {
        line.positionCount=Path.Count;

        for (int p = 0; p < Path.Count; p++)
        {
            line.SetPosition(p, Path[p] + Vector3.up * 0.5f);
        }
    }

    public void EraseLine()
    {
        line.positionCount = 0;
    }

    public float Angle(int CtrlPtIdx)
    {
        float rtn;
        if (CtrlPtIdx < 2) return 180;
        Vector2 Pt0_2d = VectGeom.Convert2d(CtrlPts[CtrlPtIdx - 1].Pos);
        Vector2 Pt1_2d = VectGeom.Convert2d(CtrlPts[CtrlPtIdx].Pos);
        Vector2 Pt2_2d = VectGeom.Convert2d(CtrlPts[CtrlPtIdx+1].Pos);
        rtn = Vector2.Angle(Pt0_2d - Pt1_2d, Pt2_2d - Pt1_2d);
        //Work out the sign of the angle b doing a cross product
        Vector3 cross = Vector3.Cross(Pt0_2d - Pt1_2d, Pt2_2d - Pt1_2d);
        if (cross.z > 0)
            rtn = -rtn;
        return rtn;
    }

    public float Angle(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float rtn;
        Vector2 Pt0_2d = VectGeom.Convert2d(p0);
        Vector2 Pt1_2d = VectGeom.Convert2d(p1);
        Vector2 Pt2_2d = VectGeom.Convert2d(p2);
        rtn = Vector2.Angle(Pt0_2d - Pt1_2d, Pt2_2d - Pt1_2d);
        //Work out the sign of the angle b doing a cross product
        Vector3 cross = Vector3.Cross(Pt0_2d - Pt1_2d, Pt2_2d - Pt1_2d);
        if (cross.z > 0)
            rtn = -rtn;
        return rtn;
    }

    //might use this in future
    static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float percentComplete)
    //might use this in future
    /////////http://wiki.unity3d.com/index.php/Interpolate
    {

        // tension is often set to 0.5 but you can use any reasonable value:
        // http://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
        //
        // bias and tension controls:
        // http://local.wasp.uwa.edu.au/~pbourke/miscellaneous/interpolation/

        float percentCompleteSquared = percentComplete * percentComplete;
        float percentCompleteCubed = percentCompleteSquared * percentComplete;

        return previous * (-0.5f * percentCompleteCubed +
                                   percentCompleteSquared -
                            0.5f * percentComplete) +
                start * (1.5f * percentCompleteCubed +
                           -2.5f * percentCompleteSquared + 1.0f) +
                end * (-1.5f * percentCompleteCubed +
                            2.0f * percentCompleteSquared +
                            0.5f * percentComplete) +
                next * (0.5f * percentCompleteCubed -
                            0.5f * percentCompleteSquared);
    }

}

public class SegVerts {
    public int Idx { get; set; }
    public Vector3 MidPtF { get; set; }
    public Vector3 MidPtB { get; set; }
    public Vector3 KerbFL { get; set; }
    public Vector3 KerbFR { get; set; }
    public Vector3 KerbBL { get; set; }
    public Vector3 KerbBR { get; set; }
    public Vector3 TerrainFL { get; set; }
    public Vector3 TerrainFR { get; set; }
    public Vector3 TerrainBL {get;set;}
    public Vector3 TerrainBR {get;set;}
}

public class BezCtrlPt
{
    public int CtrlPtId;
    public BezierLine Line;
    public Vector3 Pos { get; set; }
    public float BankAngle { get; set; }
    public int SegCount { get; set; }
    public int SegStartIdx { get; set; }
    public GameObject goRdMkr;
    public static BezCtrlPt Current;


    public BezCtrlPt(BezierLine line, Vector3 pos)
    {
        Line = line;
        Pos = pos;
        BankAngle = 0;
    }

    /// <summary>
    /// Adds the RoadMarker gameobject which contains the RoadMarker script
    /// </summary>
    public void CreateRoadMarker()
    {
        UnityEngine.Object objRoadMarker = Resources.Load("Prefabs/RoadMarker");
        goRdMkr = (GameObject)GameObject.Instantiate(objRoadMarker, Pos, Quaternion.identity);
        objRoadMarker = null;
        RoadMarker RoadMarker = goRdMkr.gameObject.GetComponent<RoadMarker>();
        RoadMarker.name = "RoadMarker" + CtrlPtId;
        RoadMarker.CtrlPt = this;
        RoadMarker.Index = CtrlPtId;
        goRdMkr.transform.SetParent(Road.Instance.Sectns[CtrlPtId].goSectn.transform);
    }

    public void AlignRoadMarker()
    {
        if (CtrlPtId == 0) return;
        if (goRdMkr == null) return;
        if (BezierLine.Instance.Path.Count == 0) return;
        if (SegStartIdx > BezierLine.Instance.Path.Count - 2) return;
        if (BezierLine.Instance.Path[SegStartIdx + 1] == Vector3.zero) return;

            goRdMkr.transform.LookAt(BezierLine.Instance.Path[SegStartIdx + 1]);
        if (BankAngle < 0)
        {
            goRdMkr.transform.Find("GizmoBankL").localEulerAngles = new Vector3(0, 0, BankAngle);
            goRdMkr.transform.Find("GizmoBankR").localEulerAngles = Vector3.zero;
        }
        if (BankAngle > 0)
        {
            goRdMkr.transform.Find("GizmoBankR").localEulerAngles = new Vector3(0, 0, BankAngle);
            goRdMkr.transform.Find("GizmoBankL").localEulerAngles = Vector3.zero;
        }
    }

    public void DeleteRoadMarker()
    {
        GameObject.Destroy(goRdMkr);
        goRdMkr = null;
    }

    public void Select()
    {
        if (goRdMkr == null) { return; }
        RoadMarker RM = goRdMkr.gameObject.GetComponent<RoadMarker>();
        if (!RM.Selected) RM.Select();
        Current = this;
        RM = null;
    }
}



