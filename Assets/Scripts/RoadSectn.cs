using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public interface iRoadSectn
{
    int Idx { get; set; }
    string name { get; set; }
    GameObject goSectn { get; set; }
    List<RoadSegment> Segments { get; }
    string LFenceType { get; set; }
    string RFenceType { get; set; }
    string RoadMaterial { get; set; }
    List<Vector3> LFenceVerts { get; set; }
    List<Vector3> RFenceVerts { get; set; }
    //List<XSec> XSecs { get; set; } //not used
    Vector3[] FenceColldrVertsL { get; set; }
    Vector3[] FenceColldrVertsR { get; set; }
    bool Chargeable { get; set; }
    List<AILesson> AILessons { get; set; }


    void CreateGameObjects();
    void AddXSecs(int segCount);
    void CreateFenceColliders();
    /// <summary>
    /// Calculate the fence vertices after the control point
    /// </summary>
    void CalcVisibleFenceVerts();
    void DeleteFence();
    void CreateFence();
    void SetMaterial(string roadMaterial);
}

public class RoadSectn: iRoadSectn
{
    private List<RoadSegment> _Segs = new List<RoadSegment>();
    public string RoadMaterial { get; set; }
    private string _LFenceType = "Fence1";
    private string _RFenceType = "Fence1";
    private List<Vector3> _LFenceVerts = new List<Vector3>();
    private List<Vector3> _RFenceVerts = new List<Vector3>();

    public int Idx { get; set; }
    public string name { get; set; }
    public GameObject goSectn { get; set; }
    public List<RoadSegment> Segments { get { return _Segs; } }
    public List<XSec> XSecs { get; set; } //not used
    public string LFenceType { get { return _LFenceType; } set { _LFenceType = value; } }
    public string RFenceType { get { return _RFenceType; } set { _RFenceType = value; } }
    public List<Vector3> LFenceVerts { get { return _LFenceVerts; } set { _LFenceVerts = value; } }
    public List<Vector3> RFenceVerts { get { return _RFenceVerts; } set { _RFenceVerts = value; } }
    public Vector3[] FenceColldrVertsL { get; set; }
    public Vector3[] FenceColldrVertsR { get; set; }
    int[] FenceTris;
    public bool Chargeable { get; set; }
    public List<AILesson> AILessons { get; set; }

    /// <summary>
    /// Create the section gameobject with a meshfilter and renderer.
    /// <para>Name the gameobject. Make the marker a parent of it.</para> Build empty fence meshes
    /// </summary>
    public void CreateGameObjects()
    {
        goSectn = new GameObject();
        goSectn.transform.parent = Road.Instance.goRoad.transform;
        goSectn.AddComponent<MeshFilter>();
        goSectn.AddComponent<MeshRenderer>();
        goSectn.name = name;
        goSectn.layer = 13;
        goSectn.isStatic = true;
        //Road.Instance.Sectns.Add(Sectn.GetComponent<RoadSection>()); //Already added
        GameObject goMrkr = GameObject.Find("RoadMarker" + Idx.ToString());
        if (goMrkr != null) goMrkr.transform.SetParent(goSectn.transform);
        BuildFenceColliderTriangles();
    }

    public void AddXSecs(int segCount)
    {
        XSecs = new List<XSec>(); 
        for (int i = 0; i < segCount; i++)
        {
            XSec X = new XSec(Road.Instance.XSecs.Count);
            Road.Instance.XSecs.Add(X);
            //this.XSecs.Add(X);
        }
    }


    public void BuildFenceColliderTriangles()
    {

        //  3 - 0 - 4 - 8 - 12 - 16 - 20 - 24 - 28 - 32 - 36 - 40 - 44 - 48 - 52 - 56 - 60 - 64 - 68 - 72 - 76 - 80
        //  | \ | / | / |                                                                                        |
        //  2 - 1 - 5 - 9         //top of fence lhs                                                             81
        //      | \ | \ |                                                                                        |
        //      2 - 6 - 10        //top of fence rhs                                                             82
        //      | / | / |                                                                                        |
        //      3 - 7 - 11        //road level rhs                                                               83 - 82
        //      | \ | \ |                                                                                        |  / |
        //      0 - 4 - 8         //road level lhs                                                               80 - 81

        List<int> WholeMesh = new List<int>();
        //Here's the end rectangle
        FenceTris = new int[] { 2, 3, 1, 1, 3, 0 };
        //Add it to the whole mesh
        WholeMesh.AddRange(FenceTris);
        int end = Segments.Count * 4 - 3;
        for (int n = 0; n < end; n += 4)
        {
            FenceTris = new int[] {
                n  ,n+4,n+1,
                n+1,n+4,n+5,
                n+5,n+6,n+1,
                n+1,n+6,n+2,
                n+2,n+6,n+3,
                n+3,n+6,n+7,
                n+7,n+4,n+3,
                n+3,n+4, n };
            WholeMesh.AddRange(FenceTris);
        }
        //FenceTris = new int[] { 83, 82, 80, 80, 82, 81 };
        FenceTris = new int[] { end+6, end+5, end+3, end+3, end+5, end+4 };
        WholeMesh.AddRange(FenceTris);
        FenceTris = WholeMesh.ToArray();
        int VertCount = (Segments.Count+1)*4;
        FenceColldrVertsL = new Vector3[VertCount];
        FenceColldrVertsR = new Vector3[VertCount];
    }


    public void CreateFenceColliders()
    {
        if (Idx != 0 && this!=Road.Instance.Sectns.Last())
        {
            if (RFenceType != "Fence0")
            {
                Mesh MR = new Mesh();
                MR.vertices = FenceColldrVertsR;
                MR.triangles = FenceTris;
                MR.RecalculateBounds();
                MR.RecalculateNormals();
                MeshCollider MCR = goSectn.AddComponent<MeshCollider>();
                MCR.material = (PhysicMaterial)Resources.Load("PhysicMaterials/FencePhysicsMaterial");
                MCR.sharedMesh = MR;
                
                
            }
            if (LFenceType != "Fence0")
            {
                Mesh ML = new Mesh();
                ML.vertices = FenceColldrVertsL;
                ML.triangles = FenceTris;
                ML.RecalculateBounds();
                ML.RecalculateNormals();
                MeshCollider MCL = goSectn.AddComponent<MeshCollider>();
                MCL.material = (PhysicMaterial)Resources.Load("PhysicMaterials/FencePhysicsMaterial");
                MCL.sharedMesh = ML;
            }
        }
    }
    /// <summary>
    /// Used for ChaseCamera to stop it flying upwards
    /// </summary>

    /// <summary>
    /// Calculate the fence vertices after the control point
    /// </summary>
    public void CalcVisibleFenceVerts()
    {
        //This uses the XSecs to fill the two lists LFenceVerts and RFenceVerts
        //These are used to create the two gameobjects, LFence and RFence
        //The verts are always 3m apart.
        RaycastHit _hit;

        if (Segments.Count > 1)
        {
            Road Rd = Road.Instance;
            LFenceVerts.Clear();
            RFenceVerts.Clear();


            List<Vector3> LKerbs = (from x in Rd.XSecs
                                    where x.Idx >= Segments[0].Idx && x.Idx <= Segments.Last().Idx+1
                                    select x.KerbL).ToList<Vector3>();
            if(RoadMaterial=="Air") LKerbs = LKerbs.Select(k => Gnd(k)).ToList();
            //See I added the first xsec of the next section
            //if (Rd.Sectns[Idx + 1].Segments.Count==0) return;                              //So we don't try to calculate for the last section
            Vector3 CurrPos = LKerbs[0];
            LFenceVerts.Add(CurrPos);
            int NxtKerbIdx = 1;
            float SegLength;
            float TotDist;
            float Bckdist;
            float DistRem = Vector3.Distance(CurrPos, LKerbs.Last());
            while (DistRem > 3.0f)
            {
                //Find the next kerb point 3 meters along
                SegLength = 0;
                TotDist = Vector3.Distance(CurrPos, LKerbs[NxtKerbIdx]);
                if (TotDist > 3) SegLength = Vector3.Distance(CurrPos, LKerbs[NxtKerbIdx]);
                while (TotDist < 3)
                {
                    CurrPos = LKerbs[NxtKerbIdx];
                    //Yeah but the air road fences should be on the ground

                    NxtKerbIdx++;
                    SegLength = Vector3.Distance(CurrPos, LKerbs[NxtKerbIdx]);
                    TotDist += SegLength;
                } //Now we've got to the KerbPoint after the 3m mark.
                //We have to go backward to the 3m mark
                Bckdist = TotDist - 3;
                CurrPos = Vector3.Lerp(LKerbs[NxtKerbIdx], CurrPos, Bckdist / SegLength);
                LFenceVerts.Add(CurrPos);
                DistRem = Vector3.Distance(CurrPos, LKerbs.Last());
            }
            //Add the first kerb point of the next section
            XSec LastXSec = Rd.XSecs[Segments.Last().Idx];
            LFenceVerts.Add(RoadMaterial=="Air"? Gnd(Rd.XSecPlusOne(LastXSec).KerbL): Rd.XSecPlusOne(LastXSec).KerbL);

//Right Kerbs
            List<Vector3> RKerbs = (from x in Rd.XSecs
                                    where x.Idx >= Segments[0].Idx && x.Idx <= Segments.Last().Idx + 1  //See I added the first xsec of the next section
                                    select x.KerbR).ToList<Vector3>();
            if (RoadMaterial == "Air") RKerbs = RKerbs.Select(k => Gnd(k)).ToList();

            CurrPos = RKerbs[0];
            RFenceVerts.Add(CurrPos);
            NxtKerbIdx = 1;
            DistRem = Vector3.Distance(CurrPos, RKerbs.Last());
            while (DistRem > 3.0f)
            {
                //Find the next kerb point 3 meters along
                SegLength = 0;
                TotDist = Vector3.Distance(CurrPos, RKerbs[NxtKerbIdx]);
                if (TotDist > 3) SegLength = Vector3.Distance(CurrPos, RKerbs[NxtKerbIdx]);
                while (TotDist < 3)
                {
                    CurrPos = RKerbs[NxtKerbIdx];
                    NxtKerbIdx++;
                    SegLength = Vector3.Distance(CurrPos, RKerbs[NxtKerbIdx]);
                    TotDist += SegLength;
                } //Now we've got to the KerbPoint after the 3m mark.
                //We have to go backward to the 3m mark
                Bckdist = TotDist - 3;
                CurrPos = Vector3.Lerp(RKerbs[NxtKerbIdx], CurrPos, Bckdist / SegLength);
                RFenceVerts.Add(CurrPos);
                DistRem = Vector3.Distance(CurrPos, RKerbs.Last());
            }
            RFenceVerts.Add(RoadMaterial == "Air" ? Gnd(Rd.XSecPlusOne(LastXSec).KerbR): Rd.XSecPlusOne(LastXSec).KerbR);
        }
    }

    Vector3 Gnd(Vector3 Kerb)
    {
        RaycastHit _hit;
        if (Physics.Raycast(Kerb, Vector3.down, out _hit, 10, (1 << 10)))
        {
            return _hit.point;
        }
        else return Kerb;
    }

public void DeleteFence()
    {
        try
        {
            Transform trFences = goSectn.transform.Find("Fences");
            if (trFences != null)
            {
                GameObject.DestroyObject(trFences.gameObject);
            }
        }
        catch { }

    }
    public void CreateFence()
    {

        UnityEngine.Object LFencePrefab;
        UnityEngine.Object RFencePrefab;
        UnityEngine.Object KerbPrefab;
        GameObject goFences;
        if (Idx > 0)
        {
            goFences = new GameObject("Fences");
            goFences.transform.SetParent(goSectn.transform);

            LFencePrefab = Resources.Load("Prefabs/Fences/" + LFenceType + "Prefab");
            RFencePrefab = Resources.Load("Prefabs/Fences/" + RFenceType + "Prefab");
            if (Segments.Count != 0)
            {
                String KerbMat = this.RoadMaterial; // Segments[0].roadMaterial;
                if (KerbMat.StartsWith("Washboard")) KerbMat = "Washboard";
                KerbPrefab = Resources.Load("Prefabs/Kerb/Kerb" + KerbMat + "Prefab");
            }
            else KerbPrefab = null;
            float FenceHeight = 1;
            float FenceLean = 0;
            if (LFencePrefab != null)
            {
                for (int VIdx = 0; VIdx < LFenceVerts.Count - 1; VIdx++)
                {
                    Transform trLeftFence = ((GameObject)UnityEngine.Object.Instantiate(LFencePrefab, Vector3.zero, Quaternion.identity)).transform;
                    trLeftFence.position = LFenceVerts[VIdx];
                    trLeftFence.LookAt(LFenceVerts[VIdx + 1]);
                    if (LFenceType == "StoneWall")
                    {
                        FenceHeight = FenceHeight + UnityEngine.Random.Range(-0.2f, 0.2f);
                        FenceHeight = Mathf.Clamp(FenceHeight, 0.2f, 1.5f);
                        trLeftFence.localScale = new Vector3(1, FenceHeight, 1);
                    }
                    if (LFenceType == "Fence2")
                    {
                        FenceLean = FenceLean + UnityEngine.Random.Range(-6f, 6f);
                        FenceLean = Mathf.Clamp(FenceLean, -30f, 30f);
                        trLeftFence.Rotate(Vector3.forward, FenceLean, Space.Self);
                        if(UnityEngine.Random.Range(0,20)==1) trLeftFence.Rotate(Vector3.right, 25, Space.Self);
                    }
                    if (VIdx == LFenceVerts.Count - 2) trLeftFence.localScale = new Vector3(1, 1, Vector3.Distance(LFenceVerts[VIdx + 1], LFenceVerts[VIdx]) / 3);
                    trLeftFence.name = "FenceL" + VIdx.ToString();
                    trLeftFence.SetParent(goFences.transform);
                }
            }
            if (RFencePrefab != null)
            {
                for (int VIdx = 0; VIdx < RFenceVerts.Count - 1; VIdx++)
                {

                    Transform trRightFence = ((GameObject)UnityEngine.Object.Instantiate(RFencePrefab, Vector3.zero, Quaternion.identity)).transform;
                    trRightFence.position = RFenceVerts[VIdx + 1];
                    trRightFence.LookAt(RFenceVerts[VIdx]);
                    if (RFenceType == "StoneWall")
                    {
                        FenceHeight = FenceHeight + UnityEngine.Random.Range(-0.2f, 0.2f);
                        FenceHeight = Mathf.Clamp(FenceHeight, 0.2f, 1.5f);
                        trRightFence.localScale = new Vector3(1, FenceHeight, 1);
                    }
                    if (RFenceType == "Fence2")
                    {
                        FenceLean = FenceLean + UnityEngine.Random.Range(-6f, 6f);
                        FenceLean = Mathf.Clamp(FenceLean, -30f, 30f);
                        trRightFence.Rotate(Vector3.forward, FenceLean, Space.Self);
                    }
                    if (VIdx == RFenceVerts.Count - 2) trRightFence.transform.localScale = new Vector3(1, 1, Vector3.Distance(RFenceVerts[VIdx + 1], RFenceVerts[VIdx]) / 3);
                    trRightFence.name = "FenceR" + VIdx.ToString();
                    trRightFence.SetParent(goFences.transform);

                }
            }

            if (KerbPrefab != null)
            {
                for (int VIdx = 0; VIdx < LFenceVerts.Count - 1; VIdx++)
                {
                    {
                        GameObject goKerb = (GameObject)UnityEngine.Object.Instantiate(KerbPrefab, Vector3.zero, Quaternion.identity);
                        goKerb.transform.position = LFenceVerts[VIdx];
                        goKerb.transform.LookAt(LFenceVerts[VIdx + 1]);
                        if (VIdx == LFenceVerts.Count - 2) goKerb.transform.localScale = new Vector3(1, 1, Vector3.Distance(LFenceVerts[VIdx + 1], LFenceVerts[VIdx])/3);
                        goKerb.name = "KerbL" + VIdx.ToString();
                        goKerb.transform.SetParent(goFences.transform);
                    }
                }
                for (int VIdx = 0; VIdx < RFenceVerts.Count - 1; VIdx++)
                {
                    {
                        GameObject goKerb = (GameObject)UnityEngine.Object.Instantiate(KerbPrefab, Vector3.zero, Quaternion.identity);
                        goKerb.transform.position = RFenceVerts[VIdx + 1];
                        goKerb.transform.LookAt(RFenceVerts[VIdx]);
                        if (VIdx == RFenceVerts.Count - 2) goKerb.transform.localScale = new Vector3(1, 1, Vector3.Distance(RFenceVerts[VIdx + 1], RFenceVerts[VIdx])/3);
                        goKerb.name = "KerbR" + VIdx.ToString();
                        goKerb.transform.SetParent(goFences.transform);
                    }
                }
            }
        }
    }

    public void SetMaterial(string roadMaterial)
    {
        RoadMaterial = roadMaterial;
        foreach (RoadSegment seg in Segments)
        {
            seg.SetMaterial(roadMaterial);
        }
        Game.current.Dirty = true;
    }


    private void PlaceMarker(Vector3 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.localScale = new Vector3(0.2f, 10f, 0.2f);
        marker.GetComponent<Collider>().enabled = false;
        marker.transform.position = Pos;
    }

}



