using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
//using UnityEditor;

public interface iRoadSeg
{
    int Idx { get; set; }
    int SectnIdx { get; set; }
    SegVerts Verts { get; set; }
    Mesh TopSideMesh { get; set; }
    Mesh ColliderMesh { get; set; }
    Mesh terrainMesh { get; set; }
    void CreateFenceColliderVerts();
    TerrainMinimap _TerrainMinimap { get; set; }
}

public class RoadSegment : iRoadSeg
{
    public int Idx { get; set; }
    public int SectnIdx { get; set; }
    public SegVerts Verts { get; set; }
    public Mesh TopSideMesh { get; set; }
    public Mesh ColliderMesh { get; set; }
    public Mesh terrainMesh { get; set; }
    private Mesh UndersideMesh { get; set; }
    public TerrainMinimap _TerrainMinimap { get; set; }
    private Vector2[] mesh2d = new Vector2[4];
    public bool HasMesh = false;
    public GameObject goSeg;
    public GameObject goUnderside;
    public MeshRenderer meshRenderer;
    public string roadMaterial = "Tarmac";
    public string LFenceType = "Fence2";
    public string RFenceType = "Fence2";
    public bool IsBridge = false;
    public Mesh RMesh = new Mesh();
    public Mesh LMesh = new Mesh();
    private MeshCollider _meshCollider;
    private Road Rd;

    //Constructor
    public RoadSegment()
    {
        TopSideMesh = new Mesh();
        ColliderMesh = new Mesh();
        terrainMesh = new Mesh();
        UndersideMesh = new Mesh();
        _TerrainMinimap = new TerrainMinimap();
        Rd = Road.Instance;
    }

    public void CreateGameObjects()
    {
        goSeg = new GameObject();
        goSeg.AddComponent<MeshFilter>();
        meshRenderer = goSeg.AddComponent<MeshRenderer>();
        _meshCollider = goSeg.AddComponent<MeshCollider>();
        _meshCollider.sharedMaterial = (PhysicMaterial)Resources.Load("PhysicMaterials/RoadPhysicsMaterial");
        /*
         if (PlayerManager.Type == "BuilderPlayer")
        {
            goSeg.AddComponent<RoadSegmentUI>();
            goSeg.GetComponent<RoadSegmentUI>().seg = this;
        }
        */
        goSeg.name = "RoadSeg - No Meshes";
        if (GameObject.Find("RoadSection" + SectnIdx) == null) Debug.Log("Cant find Section " + SectnIdx.ToString());
        goSeg.transform.SetParent(GameObject.Find("RoadSection" + SectnIdx).transform);
        goSeg.isStatic = true;
        goUnderside = new GameObject();
        goUnderside.name = "Underside";
        goUnderside.AddComponent<MeshFilter>();
        goUnderside.AddComponent<MeshRenderer>();
        goUnderside.transform.SetParent(goSeg.transform);
        goUnderside.isStatic = true;
    }

    public void BuildMeshes(SegVerts V)
    {
        TopSideMesh = BuildMesh2(V);
        if (roadMaterial == "Dirt" || roadMaterial == "Air")
            ColliderMesh = BuildMesh3(V);
        else
            ColliderMesh = TopSideMesh;
    }

    /// <summary>
    /// For dirt and air roads - Creates a double cube mesh instead of a plane which cant be convex
    /// </summary>
    /// <param name="V"></param>
    Mesh BuildMesh3(SegVerts V)
    {
        Mesh mesh = new Mesh();
        Verts = V;
        Vector3[] vertsArray = new Vector3[12]  { Verts.KerbFL,              Verts.MidPtF,                Verts.KerbFR,                Verts.KerbBL,                Verts.MidPtB,                Verts.KerbBR,
                                                  Verts.KerbFR+Vector3.down, Verts.MidPtF + Vector3.down, Verts.KerbFL + Vector3.down, Verts.KerbBR + Vector3.down, Verts.MidPtB + Vector3.down, Verts.KerbBL + Vector3.down};
        int[] tris = new int[60] { 0, 1, 3, 3, 1, 4, 1, 2, 4, 4, 2, 5, 5, 9, 4, 4, 9, 10, 5, 2, 9, 9, 2, 6, 2, 1, 7, 7, 6, 2, 6, 7, 9, 9, 7, 10, 10, 7, 8, 8, 11, 10, 8, 0, 3, 3, 11, 8, 8, 7, 1, 1, 0, 8, 10, 11, 3, 3, 4, 10 };

        Vector3[] terrainVertsArray = new Vector3[6] { Verts.TerrainFL, Verts.MidPtF, Verts.TerrainFR, Verts.TerrainBL, Verts.MidPtB, Verts.TerrainBR };
        int[] _terrainTris = new int[12] { 0, 1, 3, 3, 1, 4, 1, 2, 4, 4, 2, 5 };



        /*
        Vertex layout

        front       1-7           7-1
                    |/|           |/|
        0-----1-----2-6-----7-----8-0
        |    /|    /| |    /|    /| |
        |  /T O P/  |\|  /B T M/  |\|
        |/    |/    | |/    |/    | |
        3-----4-----5-9----10----11-3
                    |/|           |/|
        back        4-10         10-4

        */

        mesh.Clear();
        mesh.vertices = vertsArray;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        /*terrainMesh.Clear();
        terrainMesh.vertices = terrainVertsArray;
        terrainMesh.triangles = _terrainTris;
        //terrainMesh.uv = uvs;
        terrainMesh.RecalculateBounds();
        terrainMesh.RecalculateNormals();*/

        //HasMesh = true;
        return mesh;
    }

    /// <summary>
    /// Builds meshes for visible roads which consist of a Top plane and underside plane
    /// </summary>
    /// <param name="V"></param>
    Mesh BuildMesh2(SegVerts V)
    {
        Mesh mesh = new Mesh();
        Verts = V;
        Vector3[] vertsArray = new Vector3[6] { Verts.KerbFL, Verts.MidPtF, Verts.KerbFR, Verts.KerbBL, Verts.MidPtB, Verts.KerbBR };
        Vector3[] terrainVertsArray = new Vector3[6] { Verts.TerrainFL, Verts.MidPtF, Verts.TerrainFR, Verts.TerrainBL, Verts.MidPtB, Verts.TerrainBR };
        Vector2[] uvs = new Vector2[6];
        int[] tris = new int[12] { 0, 1, 3, 3, 1, 4, 1, 2, 4, 4, 2, 5 };

        /*
        Vertex layout
        0---1---2
        |  /|  /|
        | / | / |
        |/  |/  |
        3---4---5
        */
        float _offset = 0;
        if (roadMaterial == "Dirt")
        {

        }

        uvs[0] = new Vector2(0.0f + _offset, 1.0f);
        uvs[1] = new Vector2(0.5f + _offset, 1.0f);
        uvs[2] = new Vector2(1.0f + _offset, 1.0f);
        uvs[3] = new Vector2(0.0f + _offset, 0.0f);
        uvs[4] = new Vector2(0.5f + _offset, 0.0f);
        uvs[5] = new Vector2(1.0f + _offset, 0.0f);



        mesh.Clear();
        mesh.vertices = vertsArray;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        ;

        terrainMesh.Clear();
        terrainMesh.vertices = terrainVertsArray;
        terrainMesh.triangles = tris;
        terrainMesh.uv = uvs;
        terrainMesh.RecalculateBounds();
        terrainMesh.RecalculateNormals();
        ;

        UndersideMesh.Clear();
        Vector3[] UndersideVerts = new Vector3[vertsArray.Length];
        for (int n = 0; n < vertsArray.Length; n++) { UndersideVerts[n] = vertsArray[n] + Vector3.down; }
        UndersideMesh.vertices = UndersideVerts;
        UndersideMesh.triangles = new int[12] { 0, 3, 1, 3, 4, 1, 1, 4, 2, 4, 5, 2 };
        UndersideMesh.uv = uvs;
        UndersideMesh.RecalculateBounds();
        UndersideMesh.RecalculateNormals();
        HasMesh = true;
        return mesh;
    }



    public void AddMeshes()
    {
        /*
         A NOTE ON COLLIDERS
         THE dirt and air have colliders set to trigger. This is because we want to raycast onto them
         They are also convex because you cant create a trigger collider without it being convex
         */
        goSeg.GetComponent<MeshFilter>().sharedMesh = TopSideMesh;
        goSeg.transform.position = Vector3.zero;
        goSeg.name = "RoadSeg" + Idx.ToString();
        try
        {
            _meshCollider.sharedMesh = null;        //We had to put this in because the solid road is not convex
            if (roadMaterial != "Dirt" && roadMaterial != "Air") { _meshCollider.isTrigger = false; _meshCollider.convex = false; }
            _meshCollider.sharedMesh = ColliderMesh;        //and then put it back in again
        }
        catch
        {
            Debug.Log("stop");
        }
        goUnderside.GetComponent<MeshFilter>().sharedMesh = UndersideMesh;
    }

    public void DeleteRoad()
    {
        GameObject.Destroy(goSeg);
    }

    public void SetMaterial(string Material)
    {
        roadMaterial = Material;
        if (HasMesh)
        {
            meshRenderer.enabled = true;
            string _actMat = Material;
            if (_actMat == "Washboard")
            {
                int rnd = UnityEngine.Random.Range(0, 2);
                _actMat = _actMat + rnd.ToString();
                //BugBugBugBug? Should be Layer 11?
            }
            if (_actMat == "Dirt" || _actMat == "Air")
            {
                meshRenderer.enabled = false;
                //if (roadMaterial == "Air") goSeg.GetComponent<MeshCollider>().isTrigger = true;
                //goSeg.GetComponent<MeshCollider>().convex = true;
                //goSeg.GetComponent<MeshCollider>().isTrigger = true;
                goSeg.layer = 11;
                goUnderside.GetComponent<MeshRenderer>().enabled = false;
                return;
            }
            else
            {
                //everything except Dirt and air
                Material Mat = Rd.RoadMaterials[_actMat]; //(Material)Resources.Load("Prefabs/Materials/" + M, typeof(Material));
                meshRenderer.material = Mat;
                if (_actMat == "DirtyRoad") _actMat = "Tarmac";
                Material UndersideMat = Rd.RoadMaterials[roadMaterial + "Underside"]; // (Material)Resources.Load("Prefabs/Materials/" + M + "Underside", typeof(Material));
                goSeg.layer = 11;
                goUnderside.GetComponent<MeshRenderer>().material = UndersideMat;
                goSeg.transform.position = Vector3.zero;
            }
        }
    }

    public void SetMaterial()
    {
        if (HasMesh)
        {
            if (roadMaterial.StartsWith("Washb")) roadMaterial = "Washboard";
            string _actMat = roadMaterial;
            if (roadMaterial == "Washboard")
            {
                int rnd = UnityEngine.Random.Range(0, 2);
                _actMat = _actMat + rnd.ToString();
            }
            if (roadMaterial == "Tarmac")
            {
                int _textureOffset = Idx % 4;
                //Debug.Log(Idx);
                _actMat = _actMat + _textureOffset.ToString();
            }
            if (roadMaterial == "Dirt" || roadMaterial == "Air")
            {
                //Debug.Log(Idx);
                meshRenderer.enabled = false;
                //if (roadMaterial == "Air") goSeg.GetComponent<MeshCollider>().isTrigger = true;
                _meshCollider.convex = true;
                _meshCollider.isTrigger = true;
                if (PlayerManager.Type == "BuilderPlayer") { goSeg.layer = 11; } else { goSeg.layer = 11; } //this is called the Trigger layer. It is needed because the WheelController has to ignore it.
                goUnderside.GetComponent<MeshRenderer>().enabled = false;
                return;
            }
            else
            {
                //Debug.Log("Actmat="  + _actMat);
                Material Mat = Rd.RoadMaterials[_actMat]; //(Material)Resources.Load("Prefabs/Materials/" + roadMaterial, typeof(Material));
                meshRenderer.material = Mat;
                    try
                    {
                        Material UndersideMat = Rd.RoadMaterials[roadMaterial + "Underside"]; // (Material)Resources.Load("Prefabs/Materials/" + M + "Underside", typeof(Material));
                        goUnderside.GetComponent<MeshRenderer>().material = UndersideMat;
                    }
                    catch
                    {
                        Debug.Log("err" + roadMaterial);
                    }

            }
        }
    }

    /// <summary>
    /// We build a MeshCollider for the whole section's fence by iterating through all the Kerbs
    /// </summary>
    public void CreateFenceColliderVerts()
    {
        float FenceLeanCoeff = 0f;  //Positive 0.11 means the top of the fence leans inward and the bottom leans outward
        float FenceOffset = 0.1f;   //Positive 0.1 means the middle of the fence extends over the kerb
        iRoadSectn Sec = Road.Instance.Sectns[SectnIdx];
        int SectnStartIdx = Sec.Segments[0].Idx;

        int ReltvIdx = Idx - SectnStartIdx;     //The first Segment of the current section has RelativeIdx = 0; 
        try
        {
            if (ReltvIdx != -1) //Skip if you are on the last segment
            {
                //Contact face of the fence is not vertical. It leans slightly towards the car so the car cannot ride over it.
                //Too much lean and it gets wedged underneath. Too litle and the car bounces off it throwing the rear end off the road


                //19/3/20 Added the Gnd function. This throws the fence down to the ground for Air track.
                //Didn't add it to the last seg of previous sectn. Didn't add it to the last seg of loop
                Sec.FenceColldrVertsR[ReltvIdx * 4] = Vector3.Lerp(Gnd(Verts.KerbBR), Verts.TerrainBR, FenceLeanCoeff - FenceOffset);                           //first inner bottom leans out by 0.11f
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 1] = Vector3.Lerp(Gnd(Verts.KerbBR), Gnd(Verts.TerrainBR), -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;    //first inner top - leans in by 0.11f
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 2] = Vector3.Lerp(Gnd(Verts.KerbBR), Gnd(Verts.TerrainBR), 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 3] = Vector3.Lerp(Gnd(Verts.KerbBR), Gnd(Verts.TerrainBR), 0.2f);
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 4] = Vector3.Lerp(Gnd(Verts.KerbFR), Gnd(Verts.TerrainFR), FenceLeanCoeff - FenceOffset);                       //second inner botttom leans out by 0.11f
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 5] = Vector3.Lerp(Gnd(Verts.KerbFR), Gnd(Verts.TerrainFR), -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;    //second inner top - leans in by 0.11f
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 6] = Vector3.Lerp(Gnd(Verts.KerbFR), Gnd(Verts.TerrainFR), 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 7] = Vector3.Lerp(Gnd(Verts.KerbFR), Gnd(Verts.TerrainFR), 0.2f);

                Sec.FenceColldrVertsL[ReltvIdx * 4] = Vector3.Lerp(Gnd(Verts.KerbBL), Gnd(Verts.TerrainBL), 0.2f);
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 1] = Vector3.Lerp(Gnd(Verts.KerbBL), Gnd(Verts.TerrainBL), 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 2] = Vector3.Lerp(Gnd(Verts.KerbBL), Gnd(Verts.TerrainBL), -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;    //first inner top - leans in by 0.11f
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 3] = Vector3.Lerp(Gnd(Verts.KerbBL), Gnd(Verts.TerrainBL), FenceLeanCoeff - FenceOffset);                           //first inner bottom leans out by 0.11f
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 4] = Vector3.Lerp(Gnd(Verts.KerbFL), Gnd(Verts.TerrainFL), 0.2f);
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 5] = Vector3.Lerp(Gnd(Verts.KerbFL), Gnd(Verts.TerrainFL), 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 6] = Vector3.Lerp(Gnd(Verts.KerbFL), Gnd(Verts.TerrainFL), -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;    //second inner top - leans in by 0.11f
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 7] = Vector3.Lerp(Gnd(Verts.KerbFL), Gnd(Verts.TerrainFL), FenceLeanCoeff - FenceOffset);                       //second inner botttom leans out by 0.11f
            }
            if (ReltvIdx == 0 && SectnIdx > 1)   //Do the final vertices of the previous section
            {
                ReltvIdx = Road.Instance.Sectns[SectnIdx - 1].Segments.Count;
                Sec = Road.Instance.Sectns[SectnIdx - 1];
                Sec.FenceColldrVertsR[ReltvIdx * 4] = Vector3.Lerp(Verts.KerbBR, Verts.TerrainBR, FenceLeanCoeff - FenceOffset);                       //Inner Bottom
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 1] = Vector3.Lerp(Verts.KerbBR, Verts.TerrainBR, -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;        //Inner Top
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 2] = Vector3.Lerp(Verts.KerbBR, Verts.TerrainBR, 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsR[ReltvIdx * 4 + 3] = Vector3.Lerp(Verts.KerbBR, Verts.TerrainBR, 0.2f);

                Sec.FenceColldrVertsL[ReltvIdx * 4] = Vector3.Lerp(Verts.KerbBL, Verts.TerrainBL, 0.2f);
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 1] = Vector3.Lerp(Verts.KerbBL, Verts.TerrainBL, 0.2f) + Vector3.up * 1.3f;
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 2] = Vector3.Lerp(Verts.KerbBL, Verts.TerrainBL, -FenceLeanCoeff - FenceOffset) + Vector3.up * 1.3f;         //First Inner Top
                Sec.FenceColldrVertsL[ReltvIdx * 4 + 3] = Vector3.Lerp(Verts.KerbBL, Verts.TerrainBL, FenceLeanCoeff - FenceOffset);                    //Inner Bottom
            }
            if (Road.Instance.IsCircular && ReltvIdx == 18) //The very last segment in the looped track
            {
                if (SectnIdx == Road.Instance.Sectns.Count - 2)
                {
                    ReltvIdx = 20;          ///bug - there arent always 20 segs in a sectn
                    Sec = Road.Instance.Sectns[SectnIdx];
                    Sec.FenceColldrVertsR[ReltvIdx * 4] = Vector3.Lerp(Road.Instance.Segments[1].Verts.KerbBR, Road.Instance.Segments[1].Verts.TerrainBR, FenceLeanCoeff);
                    Sec.FenceColldrVertsR[ReltvIdx * 4 + 1] = Road.Instance.Segments[1].Verts.KerbBR + Vector3.up * 1.3f;
                    Sec.FenceColldrVertsR[ReltvIdx * 4 + 2] = Road.Instance.Segments[1].Verts.TerrainBR + Vector3.up * 1.3f;
                    Sec.FenceColldrVertsR[ReltvIdx * 4 + 3] = Road.Instance.Segments[1].Verts.TerrainBR;

                    Sec.FenceColldrVertsL[ReltvIdx * 4] = Road.Instance.Segments[1].Verts.TerrainBL;
                    Sec.FenceColldrVertsL[ReltvIdx * 4 + 1] = Road.Instance.Segments[1].Verts.TerrainBL + Vector3.up * 1.3f;
                    Sec.FenceColldrVertsL[ReltvIdx * 4 + 2] = Road.Instance.Segments[1].Verts.KerbBL + Vector3.up * 1.3f;
                    Sec.FenceColldrVertsL[ReltvIdx * 4 + 3] = Road.Instance.Segments[1].Verts.KerbBL;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    Vector3 Gnd(Vector3 Kerb)
    {
        RaycastHit _hit;
        if (roadMaterial == "Air" && Physics.Raycast(Kerb, Vector3.down, out _hit, 10, (1 << 10)))
        {
            return _hit.point;
        }
        else return Kerb;
    }

    public void DeleteFence()
    {

    }



    public void GetTerrainHeights()
    {
        CreateTerrainMinimap();

        for (int j = 0; j < _TerrainMinimap.Heights.GetLength(0); j++)
        {
            for (int k = 0; k < _TerrainMinimap.Heights.GetLength(1); k++)
            {
                Vector2 P2d = TerrainController.Instance.ConvertToWorldCoords(j + _TerrainMinimap.TerrainXBase, k + _TerrainMinimap.TerrainYBase);
                if (VectGeom.ContainsPoint(mesh2d, P2d))
                {
                    _TerrainMinimap.Heights[j, k] = CalcTerrainHeightUsingPlaneRaycasts(P2d);
                }
            }
        }
        /*
                    TerrainHeight TH = new TerrainHeight();
                    TH.j = P.j;
                    TH.k = P.k;
                    Vector2 P2d = TerrainController.Instance.ConvertToWorldCoords(P.j, P.k);
                    TH.y = InterpolateY(P2d);
                    Heights.Add(TH);
                    //GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    //marker.transform.position = new Vector3(P2d.x, WorldHeight, P2d.y);
         */
    }

    public void CreateTerrainMinimap()
    {
        //Given a mesh of 4 vertices we have to find out which terrain points are contained within it
        //expand the search to the bounding box of the mesh

        if (terrainMesh != null)
        {
            Mesh TM = terrainMesh;
            Bounds b = terrainMesh.bounds;
            //GameObject boundsminmarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //boundsminmarker.transform.position = b.center;
            float[] fTerrMin = TerrainController.Instance.ConvertToTerrainCoords(b.min);
            float[] fTerrMax = TerrainController.Instance.ConvertToTerrainCoords(b.max);
            int[] TerrMin = new int[] { (int)fTerrMin[0], (int)fTerrMin[1] };
            int[] TerrMax = new int[] { Mathf.CeilToInt(fTerrMax[0]), Mathf.CeilToInt(fTerrMax[1]) };
            _TerrainMinimap.TerrainXBase = (int)fTerrMin[0];
            _TerrainMinimap.TerrainYBase = (int)fTerrMin[1];
            _TerrainMinimap.Heights = new float[TerrMax[0] - TerrMin[0], TerrMax[1] - TerrMin[1]];
            _TerrainMinimap.RaiseAllowed = (roadMaterial == "Dirt");
            //We will also need the 4 vertices of the mesh itself as Vector2s in plan view
            //So we can work out whether each point is inside the terrain mesh
            /*
            0---1---2
            |  /|  /|
            | / | / |
            |/  |/  |
            3---4---5  We have to swap 3 and 5 round              */
            mesh2d[0] = new Vector2(terrainMesh.vertices[0].x, terrainMesh.vertices[0].z);
            mesh2d[1] = new Vector2(terrainMesh.vertices[2].x, terrainMesh.vertices[2].z);
            mesh2d[2] = new Vector2(terrainMesh.vertices[5].x, terrainMesh.vertices[5].z);
            mesh2d[3] = new Vector2(terrainMesh.vertices[3].x, terrainMesh.vertices[3].z);
        }
    }

    public void AdjustTerrain()
    {
        List<TerrainMod> TerrainMods = _TerrainMinimap.GetTerrainMods();
        TerrainController.Instance.ApplyMods(TerrainMods);
    }

    private float CalcTerrainHeightUsingPlaneRaycasts(Vector2 P2d)
    {
        // Create a collider from the terrain mesh and then raycast onto it tp find the height
        float rtn = 0;
        Plane MidPlane = new Plane(Verts.MidPtF, Verts.MidPtB, Verts.MidPtB + Vector3.up);
        Plane LeftPlane = new Plane(Verts.TerrainBL, Verts.TerrainFL, Verts.MidPtF);
        Plane RightPlane = new Plane(Verts.TerrainFR, Verts.TerrainBR, Verts.MidPtB);
        Vector3 RaycastOrigin = new Vector3(P2d.x, Verts.MidPtB.y - 1, P2d.y);
        Ray r = new Ray(RaycastOrigin, Vector3.up);
        float hitDist;
        if (MidPlane.GetSide(RaycastOrigin))
        {
            if (RightPlane.Raycast(r, out hitDist))
            { rtn = RaycastOrigin.y + hitDist; }
        }
        else
        {
            if (LeftPlane.Raycast(r, out hitDist))
            { rtn = RaycastOrigin.y + hitDist; }
        }
        if (roadMaterial != "Dirt") rtn = rtn - 0.12f;      //Here's the bit where the terrain is slightly below the road. I'm always looking for this bit
        //rtn = rtn + UnityEngine.Random.Range(-0.12f, 0.12f);
        return rtn;
    }


    private float InterpolateY_NotUsed(Vector2 P2d)
    {
        // The two lines that cross the road (Verts.TerrainFL to Verts.TerrainFR   and  Verts.TerrainBL to Verts.TerrainBR are at different heights
        // GIven a point P2d we want to know what height it should be to fall on the plane
        float rtn = 0;
        //First make the two lines 2d
        Vector2 FL = new Vector2(Verts.TerrainFL.x, Verts.TerrainFL.z);
        Vector2 FR = new Vector2(Verts.TerrainFR.x, Verts.TerrainFR.z);
        Vector2 BL = new Vector2(Verts.TerrainBL.x, Verts.TerrainBL.z);
        Vector2 BR = new Vector2(Verts.TerrainBR.x, Verts.TerrainBR.z);
        Vector2 FM = new Vector2(Verts.MidPtF.x, Verts.MidPtF.z);
        Vector2 BM = new Vector2(Verts.MidPtB.x, Verts.MidPtB.z);
        //Then Measure the distance from the point onto the two lines at rightangle to the lines
        float Dist0 = VectGeom.DistPointToLine2D(P2d, FL, FR);
        float Dist1 = VectGeom.DistPointToLine2D(P2d, BL, BR);
        float Dist2 = VectGeom.DistPointToLine2D(P2d, BL, FL);
        float Dist3 = VectGeom.DistPointToLine2D(P2d, BR, FR);
        float yL = (Verts.TerrainBL.y - Verts.TerrainFL.y) * Dist0 / (Dist0 + Dist1) + Verts.TerrainFL.y;
        float yR = (Verts.TerrainBR.y - Verts.TerrainFR.y) * Dist0 / (Dist0 + Dist1) + Verts.TerrainFR.y;
        rtn = (yL - yR) * Dist3 / (Dist2 + Dist3) + yR;
        if (roadMaterial != "Dirt") rtn = rtn - 0.12f;      //Here's the bit where the terrain is slightly below the road. I'm always looking for this bit
        return rtn;
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
        marker.transform.localScale = new Vector3(0.3f, 20f, 0.3f);
        marker.transform.position = new Vector3(Pos.x, 10f, Pos.y);
    }
}


