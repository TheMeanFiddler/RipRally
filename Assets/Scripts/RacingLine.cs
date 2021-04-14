using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

//https://www.youtube.com/watch?v=mTPyMMigc5Y


public class RacingLine
{
    //public List<Vector3> ControlPoints = new List<Vector3>();   //2BRemoved
    public List<RLPt> CtrlPts = new List<RLPt>();
    public List<Vector3> Path = new List<Vector3>();
    private GameObject goLine;
    private Road Rd;

    //constructor
    public RacingLine()
    {

   }

    /// <summary>
    /// This gets called on a new game. It doesn't get called when you load a saved game
    /// </summary>
    public void Init(){
        Rd = Road.Instance;
        Path.Clear();
        CtrlPts.Clear();
    //Add 2 control points - this is the minimum to get it working
        for (int i = 0; i < 2; i++) {
        RLPt BCP = new RLPt(this, Vector3.zero);
        CtrlPts.Add(BCP);
        }
        RLPt.Current = CtrlPts[0];
        SetCtrlPtIds();
    }
    

    public void CreateGameObject()
    {
        goLine = new GameObject("RacingLine");
    }



    public void CreateRoadMarkers()
    {
        for(int Idx = 1;Idx < CtrlPts.Count-1;Idx++)
        {
            CtrlPts[Idx].CreateRoadMarker();
        }
    }


    public void AddControlPoint(Vector3 Pt, int segCount, BendType bendType, Bend bend)
    {
        if(segCount !=0) segCount = bend.EndSegIdx - bend.StartSegIdx;
        RLPt BCP = new RLPt(this, Pt);
        BCP.BendType = bendType;
        BCP.Bend = bend;
        CtrlPts[CtrlPts.Count - 2].SegCount = segCount;     //set the SegCount of the previous CtrlPt
        CtrlPts.Insert(CtrlPts.Count - 1, BCP);

        SetCtrlPtIds();
        SetSegStartIds();
        CtrlPts[CtrlPts.Count - 1].Pos = Pt;        //cos the last control point is a dummy and is in the same place
        if (BCP.CtrlPtId == 1) {
            if (Rd.IsCircular) CtrlPts[0].Pos = CtrlPts[CtrlPts.Count - 2].Pos;
            }
                else CtrlPts[0].Pos = Pt;           //cos the first one is a dummy too
        if (BCP.CtrlPtId > 1) { AddPathPoints(segCount);}
    }


    /// <summary>
    /// Inserts a control point just after the selected one
    /// </summary>
    public void InsertControlPoint(int idx)
    {
        Vector3 InsertPos = Path[(idx-1) * 20 + 10];
        RLPt InsertedCP = new RLPt(this, InsertPos);
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
        RLPt CPToRemove = CtrlPts[IdxToRemove];
        RLPt PrevCP = CtrlPts[IdxToRemove-1];

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
                try
                {
                    Path[CtrlPts[CtlPtIdx].SegStartIdx + p] = (temp);
                }
                catch (Exception e)
                {
                    Debug.Log("Err " + e.Message);
                }
            }
        }
    }


    public void DeleteAllRoadMarkers()
    {
        foreach (RLPt cp in CtrlPts)
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

    public RLPt CtrlPtPlusOne(int CtrlPtId)
    {
        int rtnId;
        rtnId = CtrlPtId + 1;
        if (Rd.IsCircular && rtnId > CtrlPts.Count - 1) rtnId = rtnId - CtrlPts.Count;
        return CtrlPts[rtnId];
    }

    public RLPt CtrlPtMinusOne(int CtrlPtId)
    {
        int rtnId;
        rtnId = CtrlPtId - 1;
        if (rtnId < 1) rtnId = CtrlPts.Count-1;
        return CtrlPts[rtnId];
    }

    public RLPt CtrlPtPlus(int CtrlPtId, int Increment)
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
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        marker.transform.position = Pos;
    }

    private void PlaceMarker(Vector2 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.transform.localScale=new Vector3(0.3f,20f,0.3f);
        marker.transform.position = new Vector3(Pos.x, 10f, Pos.y);
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


public class RLPt
{
    public int CtrlPtId;
    public string Name;
    public RacingLine Line;
    public BendType BendType { get; set; }
    public Vector3 Pos { get; set; }
    public int SegCount { get; set; }
    public int SegStartIdx { get; set; }
    public GameObject goRdMkr;
    public static RLPt Current;
    public Bend Bend;


    public RLPt(RacingLine line, Vector3 pos)
    {
        Line = line;
        Pos = pos;
    }

    /// <summary>
    /// Adds the RoadMarker gameobject which contains the RoadMarker script
    /// </summary>
    public void CreateRoadMarker()
    {
        Material Mat;
        if(BendType== BendType.Right)
            Mat = (Material)Resources.Load("Prefabs/Materials/BrightGreen");
        else if (BendType==BendType.Left)
            Mat = (Material)Resources.Load("Prefabs/Materials/Car_Red");
        else 
            Mat = (Material)Resources.Load("Prefabs/Materials/Chrome");

        goRdMkr = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Capsule);
        goRdMkr.transform.localScale = new Vector3(2, 1, 2);
        goRdMkr.name = Name;
        goRdMkr.transform.position = Pos;
        goRdMkr.GetComponent<MeshRenderer>().sharedMaterial = Mat;
        RacingLineMarker rlm = goRdMkr.AddComponent<RacingLineMarker>();
        goRdMkr.GetComponent<Collider>().enabled = false;
        rlm.Angle = Bend.Angle;
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



