using UnityEngine;
using System.Collections.Generic;

public class FlatLineRenderer:MonoBehaviour
{
    protected List<int> _tris;
    protected List<Vector3> _verts;
    protected List<Vector2> _uvs;
    protected Material _mat;
    protected MeshRenderer MR;
    protected MeshFilter MF;
    protected Mesh M;
    public List<Vector3> Nodes{get;set;}
    public Material Material { set { _mat = value; } }
    public float Width { get; set; }
    private float LastDrawTime = 0;

    void Start()
    {
        _tris = new List<int>();
        Nodes = new List<Vector3>();
        _verts = new List<Vector3>();
        _uvs = new List<Vector2>();
        MR = gameObject.GetComponent<MeshRenderer>();
        MF = gameObject.GetComponent<MeshFilter>();
        M = new Mesh();
        enabled = false;
    }

    public void Init()  //not used any more
    {
        _tris = new List<int>();
        Nodes = new List<Vector3>();
        _verts = new List<Vector3>();
        _uvs = new List<Vector2>();
        gameObject.AddComponent<MeshRenderer>();
        MR = gameObject.GetComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        MF = gameObject.GetComponent<MeshFilter>();
        M = new Mesh();
    }

    public void SetMaterial(Material Mat)
    {
        MR.sharedMaterial = Mat;
    }

    public void AddNode(Vector3 pos, bool Visible=true)
    {
        //  0 - 2 - 4 - 6 - 8 - 10 - 12 
        //  | / | / | / | / | 
        //  1 - 3 - 5 - 7 - 9
        int NodeCount = Nodes.Count;
        if (NodeCount > 0)
        {
            if (Vector3.SqrMagnitude(Nodes[NodeCount - 1] - pos) < 0.1f) return; //If its only a little increment then dont add a node 0.05f is too slow
        }
        Nodes.Add(pos);
        //Debug.Log(name + " " + pos.ToString());
        NodeCount++;
        if (NodeCount < 2) return;
        //
        Vector3 Perp = Vector3.Cross(pos - Nodes[NodeCount - 2], Vector3.up).normalized;
        if (NodeCount == 2)
        {
            _verts.Add(Nodes[0] + Perp * Width);
            _verts.Add(Nodes[0] - Perp * Width);

        }
        _verts.Add(pos + Perp * Width);
        _verts.Add(pos - Perp * Width);
        if (Visible)
        {
            _tris.Add(NodeCount * 2 - 3);   //1
            _tris.Add(NodeCount * 2 - 2);   //2
            _tris.Add(NodeCount * 2 - 4);   //0

            _tris.Add(NodeCount * 2 - 3);   //1
            _tris.Add(NodeCount * 2 - 1);   //3
            _tris.Add(NodeCount * 2 - 2);   //2
        }
        else
        {
            _tris.Add(NodeCount * 2 - 3);   //1
            _tris.Add(NodeCount * 2 - 4);   //0
            _tris.Add(NodeCount * 2 - 2);   //2

            _tris.Add(NodeCount * 2 - 2);   //2
            _tris.Add(NodeCount * 2 - 1);   //3
            _tris.Add(NodeCount * 2 - 3);   //1

        }

        _uvs.Clear();
        for (int n = 0; n < NodeCount; n++)
        {
            float frac = (float)n / NodeCount-1;
            _uvs.Add(new Vector2(frac,0));
            _uvs.Add(new Vector2(frac,1));
        }
        if (Time.time - LastDrawTime > 0.1 && MF!=null) //Only draw the line every 5 segments
        {
                M.vertices = _verts.ToArray();
                M.triangles = _tris.ToArray();
                M.uv = _uvs.ToArray();
                //M.RecalculateBounds();
                //M.RecalculateNormals();
                //M.Optimize();
                MF.sharedMesh = M;
                LastDrawTime = Time.time;
        }
        //Debug.Log("NodeCount" + NodeCount.ToString());
    }

    public float Length { get { return Vector3.Distance(Nodes[0], Nodes[Nodes.Count - 1]); } }

    void OnDisable()
    {
        _verts.Clear();
        _tris.Clear();
        _uvs.Clear();
        Nodes.Clear();
        M = new Mesh();
    }

}

