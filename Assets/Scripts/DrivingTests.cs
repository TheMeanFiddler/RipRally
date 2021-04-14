using UnityEngine;
using System.Threading;

class DrivingTests
{
    private static DrivingTests _instance;
    static readonly object padlock = new object();

    GameObject goPlayer;
    RoadBuilder RB;
    BezierLine Bez;
    Road Rd;

    public static DrivingTests Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new DrivingTests(); }
                return _instance;
            }
        }
    }

    public DrivingTests()
    {

    }
    public string TestCondition = "";
    public void MaterialTest()
    {
        if (TestCondition == "" || TestCondition == "B")
        {
            FreezeCarsAndCamera();
            
            TestCondition = "A";
        }
        else
        {
            FreezeCarsAndCamera();
            ToggleCarMats();
            TestCondition = "B";
        }


    }

    private void FreezeCarsAndCamera()
    {
        DrivingPlayManager.Current.RespawnCars();
        Race.Current.Started = false;    //becuase we might have got here by the menu restart
        foreach (Racer r in Race.Current.Racers)
        {
            r.GetOpposition();
            r.Lap = 1;
            r.Progrss = 0;
            r.PrevProgrss = 0;
            r.TotProgrss = 0;
            r.VehicleManager.VehicleController.EndSkidmarks = true;
            r.goRacer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            CamSelector.Instance.Init();
            Transform SL = GameObject.Find("StartingLine").transform;
            CamSelector.Instance.ActiveCam.transform.position = SL.position + SL.forward * -20 + Vector3.up*5;
            CamSelector.Instance.ActiveCam.transform.LookAt(SL);
        }
    }
    public void ToggleCarMats()
    {
        foreach(iVehicleManager VM in DrivingPlayManager.Current.VehicleManagers)
        {
            Transform tr = VM.goCar.transform.Find("car");
            foreach (Transform child in tr)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    Material[] ms;
                    ms = mr.sharedMaterials;
                    for (int idx = 0; idx < ms.Length; idx++)
                    {
                        {
                            ms[idx] = null;
                        }
                    }
                    mr.sharedMaterials = ms;
                    
                }
            }
        }
    }

    public void ToggleFarClip()
    {
        //makes a 2fps difference
        Camera Cam = GameObject.Find("FollowCamInner").GetComponent<Camera>();
        if (Cam.farClipPlane == 120)
            Cam.farClipPlane = 80;
        else
            Cam.farClipPlane = 120;
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

