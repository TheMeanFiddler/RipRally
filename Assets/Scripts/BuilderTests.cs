using UnityEngine;
using System.Threading;

class BuilderTests
{
    private static BuilderTests _instance;
    static readonly object padlock = new object();

    GameObject goPlayer;
    RoadBuilder RB;
    BezierLine Bez;
    Road Rd;

    public static BuilderTests Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new BuilderTests(); }
                return _instance;
            }
        }
    }

    public BuilderTests()
    {
        goPlayer = GameObject.Find("BuilderPlayer(Clone)");
        RB = goPlayer.GetComponent<RoadBuilder>();
        Rd = Road.Instance;
        Bez = BezierLine.Instance;
    }

    public void TestAll()
    {
    }

    private void InsertSection()
    {
        Reset();
        FivePointLine();
        Bez.CtrlPts[2].goRdMkr.GetComponent<RoadMarker>().Select();
        RB.InsertSection();
    }

    private void CreateSkidmark()
    {
        GameObject goSkid = new GameObject("Skid");
        FlatLineRenderer FLR = goSkid.AddComponent<FlatLineRenderer>();
        Material Mat = (Material)Resources.Load("Prefabs/Materials/SkidMark");
        FLR.Init();
        FLR.SetMaterial(Mat);
        FLR.Width = 1;
        FLR.AddNode(new Vector3(-20, 10, 0));
        FLR.AddNode(new Vector3(-30, 10, 0));
        FLR.AddNode(new Vector3(-40, 10, 0));
        FLR.AddNode(new Vector3(-50, 10, 0));
        FLR.AddNode(new Vector3(-60, 10, 0));

    }

    void Reset()
    {
        Rd.Sectns.Clear();
        Rd.Segments.Clear();
        Rd.XSecs.Clear();
        Bez.Path.Clear();
        Bez.CtrlPts.Clear();
        Bez.Init();
        Rd.Init();
    }

    void FivePointLine()
    {
        RB.AddSection(new Vector3(0, 10, 0));
        RB.AddSection(new Vector3(-20, 10, 0));
        RB.AddSection(new Vector3(-40, 10, 0));
        RB.AddSection(new Vector3(-60, 10, 0));
        RB.AddSection(new Vector3(-80, 10, 0));
    }

    void FivePointSquare()
    {
        RB.AddSection(new Vector3(200, 10, -300));
        RB.AddSection(new Vector3(200, 10, -240));
        RB.AddSection(new Vector3(240, 10, -240));
        RB.AddSection(new Vector3(240, 10, -300));
        RB.JoinSection(new Vector3(200, 10, -300));
    }
}

