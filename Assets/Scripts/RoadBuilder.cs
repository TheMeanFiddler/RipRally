using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Linq;
//using UnityEditor;
/// <summary>
/// RoadBuilder is the class that 'project manages' all the other roadbuilding services
/// <para>including the Bezier, control points, Road and XSecs, Sections and Segments</para>
/// It also runs a Coroutine called BuildSegments
/// </summary>
public class RoadBuilder : MonoBehaviour
{
    private GameObject _canvas;
    private ToolboxController Toolbox;
    private Road Rd;
    private Camera BuilderCam;
    //Road stuff;
    public BezierLine Bez;
    public Transform BezPtPrefab;
    private string RoadMat = "Tarmac";
    private string LFenceType = "Fence2";
    private string RFenceType = "Fence2";
    int RoadWidth = 8;
    public bool Dragging = false;
    private bool _mouseDownOnUI = false;
    Queue<int> BuildQueue = new Queue<int>();
    Queue<XSec> AdjQueue = new Queue<XSec>();
    public LineRenderer line = new LineRenderer();
    Tutorial Tut;
    GameObject goPlanCam;
    bool TakeShot;

    void Awake()
    {
        StartCoroutine(BuildSegments());
        //StartCoroutine(AdjustSegments());     //Not used any more because adjustments are always calculated after the XSecs
        //        DontDestroyOnLoad(this);    //needed because when you shut the app down its going to run OnDestroy and restore the terrain backup
    }
    // Use this for initialization
    void Start()
    {
        _canvas = GameObject.Find("BuilderGUICanvas(Clone)");
        BuilderCam = GameObject.Find("BuilderCamera").GetComponent<Camera>();
        Toolbox = _canvas.GetComponent<ToolboxController>();
        Toolbox.ToolOptionChanged += ToolChange;
        Bez = BezierLine.Instance;
        Rd = Road.Instance;
        if (Road.Instance.Segments.Count==0 || Road.Instance.Segments[0].goSeg == null)    //Cos When they might already be built
        {
            if (Road.Instance.StartingLinePos != null)
            {
                PlaceStartingLine(Road.Instance.StartingLineSegIdx);
            }
        }
        Bez.CreateGameObject();
        Bez.DrawLine();
        if(GameData.current.MacId== SystemInfo.deviceUniqueIdentifier)
        Bez.CreateRoadMarkers();
        //Open the tutorial if building first track
        if (SaveLoadModel.savedGames.Count < 3)     //!!!!!change to 3
        {
            UnityEngine.Object objPnl1 = Resources.Load("Prefabs/pnlTutorialBuild1");
            GameObject goVid = (GameObject)(GameObject.Instantiate(objPnl1, _canvas.transform));
            goVid.transform.localScale = Vector3.one;
            goVid.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        //Open the turorial panel so you can say how many cones left
        UnityEngine.Object objPnl = Resources.Load("Prefabs/pnlTutorialBuild2");
        GameObject goTut = (GameObject)(GameObject.Instantiate(objPnl, _canvas.transform));
        Tut = goTut.GetComponent<Tutorial>();
        Tut.transform.localScale = Vector3.one;
        Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Tut.gameObject.SetActive(false);

    }
    /// <summary>
    /// Event handler for switching between ToolOptions
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ToolChange(GameObject sender, ToolOptionChangedEventArgs e)
    {
        if (e.Opt.Type == ToolboxController.ToolType.Road)
        {
            RoadMat = e.Opt.Name;
            if (BezCtrlPt.Current != null)
            {
                int CtrlPtId = BezCtrlPt.Current.CtrlPtId;
                Rd.Sectns[CtrlPtId].SetMaterial(e.Opt.Name);
                Rd.Sectns[CtrlPtId].Chargeable = true;
                BuildQueue.Enqueue(CtrlPtId);
            }
        }
        if (e.Opt.Type == ToolboxController.ToolType.Fence && e.Side == "L")
        {
            LFenceType = e.Opt.Name;
            if (BezCtrlPt.Current != null)
            {
                int CtrlPtId = BezCtrlPt.Current.CtrlPtId;
                Rd.Sectns[CtrlPtId].LFenceType = e.Opt.Name;
                BuildQueue.Enqueue(CtrlPtId);
                Game.current.Dirty = true;
            }
        }
        if (e.Opt.Type == ToolboxController.ToolType.Fence && e.Side == "R")
        {
            RFenceType = e.Opt.Name;
            if (BezCtrlPt.Current != null)
            {
                int CtrlPtId = BezCtrlPt.Current.CtrlPtId;
                Rd.Sectns[CtrlPtId].RFenceType = e.Opt.Name;
                BuildQueue.Enqueue(CtrlPtId);
                Game.current.Dirty = true;
            }
        }
        if (e.Opt.Type == ToolboxController.ToolType.Gizmo)
        {
            PlaceableObject.Current.ShowGizmo(e.Opt.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check we're not clicking on a UI element
        if (Input.GetMouseButtonDown(0))
        {
            foreach(RectTransform RT in _canvas.GetComponentsInChildren<RectTransform>(false))
            {
                if (RT.name == "RR") break;   //because RR covers the whole screen
                if (RT.name.StartsWith("BuilderGUI")) continue;    //So does the canvas but we just want to skip this
                Vector3[] worldCorners = new Vector3[4];
                RT.GetWorldCorners(worldCorners);
                if (Input.mousePosition.x >= worldCorners[0].x && Input.mousePosition.x < worldCorners[2].x
                                        && Input.mousePosition.y >= worldCorners[0].y && Input.mousePosition.y < worldCorners[2].y)
                {
                    Debug.Log(RT.name);
                    _mouseDownOnUI = true;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Check we didn't drag the mouse onto the terrain
            if (Dragging == false && _mouseDownOnUI == false)
            {
                //If you've clicked on a UI element then you didn't mean to click the terrain

                foreach (RectTransform RT in _canvas.GetComponentsInChildren<RectTransform>(false))
                {
                    if (RT.name == "RR") break;   //because RR covers the whole screen
                    if (RT.name.StartsWith("BuilderGUI")) continue;
                    Vector3[] worldCorners = new Vector3[4];
                    RT.GetWorldCorners(worldCorners);
                    if (Input.mousePosition.x >= worldCorners[0].x && Input.mousePosition.x < worldCorners[2].x
                                            && Input.mousePosition.y >= worldCorners[0].y && Input.mousePosition.y < worldCorners[2].y)
                    {
                        return;
                    }
                }
                RaycastHit Hit;
                Ray R = BuilderCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(R, out Hit))
                {
                    if (Hit.collider.name.Contains("Terrain"))
                    {
                        if (Toolbox.SelectedTool == "RoadSectn")// && BezCurr.CtrlPts.Count - BezCtrlPt.Current.CtrlPtId ==2)
                            AddSection(Hit.point);
                        if (Toolbox.SelectedTool == "Scenery")
                            AddScenery(Hit.point);
                        Game.current.Dirty = true;
                    }
                    if (Hit.collider.name.Contains("RoadSeg"))
                    {
                        if (Toolbox.SelectedTool == "StartingLine")
                        {
                            int SegNo = int.Parse(Hit.collider.name.Substring(7));
                            //InsertMarker(SegNo)
                            PlaceStartingLine(SegNo);
                        }
                        if (Toolbox.SelectedTool == "Scenery")
                            AddScenery(Hit.point);
                        Game.current.Dirty = true;
                    }
                }
            }

            //Draw the line
            Bez.DrawLine();

            //GetComponent<BuilderCamController>().FreezeTilt = false;
            _mouseDownOnUI = false;
            Dragging = false;
        }
    }

    public void AddSection(Vector3 Point)
    //We add the section but we can't say how many segments it has
    //So we add the Xsecs and Segs for the previous section
    {
        if (Road.Instance.IsCircular) return;
        if (Bez.CtrlPts.Count>3 && Vector3.Distance(Point, Bez.CtrlPts[Bez.CtrlPts.Count-2].Pos) > 80)
        {
            Tut.gameObject.SetActive(true); Tut.ShowSpeech("This cone is too far from the last", 5); return;
        }
        int NewIdx = Road.Instance.Sectns.Count;
        int SegCount;
        Point = Bez.LimitSlope(Point, 0.33f);
        if (Bez.CtrlPts.Count == 2) SegCount = 0; else SegCount = Mathf.CeilToInt(Vector3.Distance(Point, Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos) / 0.84f);
        Bez.AddControlPoint(Point, 0, SegCount);
        Bez.Interp(NewIdx - 1);
        Bez.DrawLine();

        RoadSectn Sectn = new RoadSectn();

        Sectn.Idx = NewIdx;
        Sectn.name = "RoadSection" + (NewIdx);
        Rd.Sectns[NewIdx - 1].Chargeable = true;
        Rd.Sectns[NewIdx - 1].AddXSecs(SegCount);   //Add XSecs to the PREVIOUS Section
        Sectn.CreateGameObjects();
        Road.Instance.Sectns.Add(Sectn);    // Section1 comes after RoadMarker1
        Bez.CtrlPts[NewIdx].CreateRoadMarker();
        Bez.AlignAllRoadMarkers();

        XSecCalculator.CalcXSecs(NewIdx - 1, RoadWidth);       //calculates for the section up to the previous marker
        for (int Idx = Bez.CtrlPts[NewIdx - 1].SegStartIdx; Idx < Bez.CtrlPts[NewIdx].SegStartIdx; Idx++)    //create the segs ahead of the marker but don't mesh them
        {
            RoadSegment seg = new RoadSegment();
            seg.Idx = Idx;
            seg.SectnIdx = NewIdx - 1;
            Rd.Sectns[NewIdx - 1].Segments.Add(seg);
            seg.CreateGameObjects();
            seg.goSeg.name = "RoadSeg" + Idx;
            Rd.Segments.Add(seg);
            seg.SetMaterial(RoadMat);
            seg.LFenceType = LFenceType;
            seg.RFenceType = RFenceType;
            seg.goSeg.transform.SetParent(Rd.Sectns[NewIdx - 1].goSectn.transform);
        }
        Rd.Sectns[NewIdx].SetMaterial(RoadMat);
        Rd.Sectns[NewIdx - 1].LFenceType = LFenceType;
        Rd.Sectns[NewIdx - 1].RFenceType = RFenceType;
        Rd.Sectns[NewIdx - 1].SetMaterial(RoadMat);
        XSecCalculator.AdjustHairpin(Bez, NewIdx - 2);
        if (NewIdx > 3) Road.Instance.Sectns[NewIdx - 3].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(NewIdx - 3);
        Bez.CtrlPts[NewIdx].goRdMkr.GetComponent<RoadMarker>().Select();
        //Update the tutorial
        if (SaveLoadModel.savedGames.Count < 3)
        {
            Tutorial Tut = _canvas.transform.Find("pnlTutorialBuild2(Clone)").GetComponent<Tutorial>();
            int _roadCost = BillOfRoadMaterials.Items.Sum(i => i.Cost);
            int _sceneryCost = BillOfSceneryMaterials.Items.Sum(i => i.Opt.Cost);
            int conesLeft = (UserDataManager.Instance.Data.Coins - _roadCost - _sceneryCost) / 5;
            Tut.ShowSpeech(string.Format("You have money for {0:0.} more cones\n\nThe track must be a loop", conesLeft), 3);
        }
    }

    /// <summary>
    /// Divides the Current Section in half
    /// </summary>
    public void InsertSection()
    {
        int CurrIdx = BezCtrlPt.Current.CtrlPtId;
        int newSectnIdx = CurrIdx + 1;
        iRoadSectn CurrSectn = Rd.Sectns[CurrIdx];
        int CurrSectnSegStartId = BezCtrlPt.Current.SegStartIdx;
        int CurrSectnSegCount = BezCtrlPt.Current.SegCount;
        int CurrSectnNewSegCount = CurrSectnSegCount / 2;
        int NewSectnSegStartId = CurrSectnSegStartId + CurrSectnNewSegCount;
        int NewSectnSegCount = CurrSectnSegCount - CurrSectnNewSegCount;
        int NxtSectnStartId = Bez.CtrlPts[newSectnIdx].SegStartIdx;

        //Insert Bezier Control Point
        Vector3 NewPos = Bez.Path[NewSectnSegStartId];
        BezCtrlPt NewCtrlPt = new BezCtrlPt(Bez, NewPos);
        NewCtrlPt.BankAngle = BezCtrlPt.Current.BankAngle;
        Bez.CtrlPts.Insert(newSectnIdx, NewCtrlPt);
        Bez.SetCtrlPtIds();
        //We dont have to move the path points from the old CtrlPt to the new one
        BezCtrlPt.Current.SegCount = CurrSectnNewSegCount;
        NewCtrlPt.SegCount = NewSectnSegCount;
        Bez.SetSegStartIds();
        NewCtrlPt.CreateRoadMarker();
        Bez.Interp(newSectnIdx - 1);
        Bez.Interp(newSectnIdx);
        Bez.Interp(newSectnIdx + 1);
        Bez.DrawLine();

        Bez.AlignAllRoadMarkers();
        //Insert Section
        iRoadSectn NewSectn = new RoadSectn();
        NewSectn.Chargeable = false;
        NewSectn.Idx = newSectnIdx;
        NewSectn.LFenceType = CurrSectn.LFenceType;
        NewSectn.RFenceType = CurrSectn.RFenceType;
        NewSectn.SetMaterial(CurrSectn.Segments[0].roadMaterial);
        NewSectn.CreateGameObjects();
        NewSectn.name = "RoadSection" + (newSectnIdx);
        NewSectn.goSectn.name = "RoadSection" + (newSectnIdx);
        Road.Instance.Sectns.Insert(newSectnIdx, NewSectn);    // Section1 comes after RoadMarker1

        Rd.OrganiseObjectsUsingBezierCtrlPtsAndPath();
        /*
        XSecCalculator.CalcXSecs(newSectnIdx-1, RoadWidth);
        XSecCalculator.CalcXSecs(newSectnIdx, RoadWidth);
        XSecCalculator.CalcXSecs(newSectnIdx+1, RoadWidth);

        CurrSectn.CalcFenceVerts();
        NewSectn.CalcFenceVerts();
        BuildQueue.Enqueue(newSectnIdx-1);
        BuildQueue.Enqueue(newSectnIdx);
        */
    }
    /*
    void AdjustHairpin(int MarkerIdx){
        List<XSec> Adjustments = Bez.AdjustHairpin(MarkerIdx);
        foreach (XSec Adj in Adjustments)
        {
            XSec XSc = Road.Instance.XSecs[Adj.Idx];
            XSc.Adjust(Adj);
        }
        foreach (XSec Adj in Adjustments)
        {
            AdjQueue.Enqueue(Adj);
        }
    }
     */

    public void DeleteRoadSectn()
    {
        if (BezCtrlPt.Current != null && Rd.IsCircular)
            ReindexSectns(BezCtrlPt.Current.CtrlPtId);
        Rd.DeleteLastSectn();
    }


    int BuildIdx = 0;
    IEnumerator BuildSegments()
    {
        do
        {
            if (BuildIdx > 0)
            {
                for (Int32 P = Bez.CtrlPts[BuildIdx].SegStartIdx; P < Bez.CtrlPts[BuildIdx].SegStartIdx + Bez.CtrlPts[BuildIdx].SegCount; P++)
                {
                    if (P < Bez.Path.Count - 1 || (Rd.IsCircular && P == Bez.Path.Count - 1))
                    {
                        try
                        {
                            SegVerts Verts = XSecCalculator.SegmentVertices(P);
                            RoadSegment seg = Road.Instance.Segments[P];
                            seg.BuildMeshes(Verts);
                            seg.GetTerrainHeights();
                            seg.AdjustTerrain();
                            seg.AddMeshes();
                            seg.SetMaterial();
                            if (P == Rd.StartingLineSegIdx) { PlaceStartingLine(P); }
                            if (P == Bez.CtrlPts[BuildIdx].SegStartIdx + Bez.CtrlPts[BuildIdx].SegCount - 1)
                            {
                                Road.Instance.Sectns[BuildIdx].DeleteFence();
                                Road.Instance.Sectns[BuildIdx].CreateFence();
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("BuildSegments Error BuildIndex = " + BuildIdx.ToString() + " P = " + P + " TotSegs = " + Road.Instance.Segments.Count + " Source = " + e.ToString());
                        }
                        yield return 0;
                        //TerrCtl.SetHeight(Bez.Path[P], Bez.Path[P - 1]);
                    }
                }
            }
            if (BuildQueue.Count == 0)
            {
                BuildIdx = 0;
            }
            else
            {
                BuildIdx = BuildQueue.Dequeue();
            }
            yield return 0;
        } while (true);
    }



    private void PlaceStartingLine(int SegNo)
    {
        Rd.StartingLineSegIdx = SegNo;
        if (!Rd.SegmentExists(SegNo)) return;
        if (!Rd.Segments[SegNo].HasMesh) return;
        StartingLine SL = (StartingLine)Scenery.Instance.Objects.Find(Obj => Obj.name == "StartingLine");
        if (SL == null)
        {
            SL = new StartingLine("StartingLine");
            Scenery.Instance.Objects.Add(SL);
        }

        SL.PlaceObject(Rd.XSecs[SegNo].MidPt, Rd.XSecs[SegNo+1].MidPt, Vector3.one);    //this won't create a duplicate

    }

    private void PlaceStartingLine(Vector3 pos, Quaternion rot)
    {
        StartingLine SL = (StartingLine)Scenery.Instance.Objects.Find(Obj => Obj.name == "StartingLine");
        if (SL == null)
        {
            SL = new StartingLine("StartingLine");
            Scenery.Instance.Objects.Add(SL);
        }
        else
        {

        }
        SL.PlaceObject(pos, rot, Vector3.one);    //this won't create a duplicate
        Rd.StartingLinePos = pos;
        Rd.StartingLineRot = rot;
    }


    public void DropMarker(RoadMarker Mrkr) 
    {

        //Recalc the XSecs
        XSecCalculator.CalcXSecs(Mrkr.Index - 1, RoadWidth);    //two before the marker
        XSecCalculator.CalcXSecs(Mrkr.Index, RoadWidth);    //before the marker
        XSecCalculator.CalcXSecs(Mrkr.Index + 1, RoadWidth);  //after the marker
        XSecCalculator.CalcXSecs(Mrkr.Index + 2, RoadWidth);  //two after the marker

        //Add to the build queue

        if ((Mrkr.Index - 2 < Bez.CtrlPts.Count - 4) || (Rd.IsCircular))
        {
            Rd.Sectns[Mrkr.Index - 2].CalcVisibleFenceVerts();
            BuildQueue.Enqueue(Mrkr.Index - 2);
        }
        if ((Mrkr.Index - 1 < Bez.CtrlPts.Count - 4) || (Rd.IsCircular))
        {
            Rd.Sectns[Mrkr.Index - 1].CalcVisibleFenceVerts();
            BuildQueue.Enqueue(Mrkr.Index - 1);
        }
        if ((Mrkr.Index < Bez.CtrlPts.Count - 4) || (Rd.IsCircular))
        {
            Rd.Sectns[Mrkr.Index].CalcVisibleFenceVerts();
            BuildQueue.Enqueue(Mrkr.Index);
        }
        if ((Mrkr.Index + 1 < Bez.CtrlPts.Count - 4) || (Mrkr.Index + 1 <= Bez.CtrlPts.Count - 3 && Rd.IsCircular))
        {
            Rd.Sectns[Mrkr.Index + 1].CalcVisibleFenceVerts();
            BuildQueue.Enqueue(Mrkr.Index + 1);
        }
        //circular road penultimate CtrlPt needs to rebuild Section 1
        if (Mrkr.Index + 1 == Bez.CtrlPts.Count - 3 && Rd.IsCircular)   //doesnt work todo - needs fixing
        {
            Rd.Sectns[1].CalcVisibleFenceVerts();
            BuildQueue.Enqueue(1);
        }
        Mrkr.DroppedPosition = Mrkr.transform.position;
        //Charge for this if they moved the marker more than 1 metre
        if(GameData.current.BezS.CtrlPts==null)
            Rd.Sectns[Mrkr.Index].Chargeable = true;
        else
        {
            if (GameData.current.BezS.CtrlPts[Mrkr.Index] == null)
                Rd.Sectns[Mrkr.Index].Chargeable = true;
            else
            {
                BezCtrlPtSerial bcps = GameData.current.BezS.CtrlPts[Mrkr.Index];
                Vector3 _oldMarkerPos = bcps.Pos.V3;
                if (Vector3.Distance(Bez.CtrlPts[Mrkr.Index].Pos, _oldMarkerPos) > 1 || Mathf.Abs(bcps.BankAngle- Bez.CtrlPts[Mrkr.Index].BankAngle)>10) Rd.Sectns[Mrkr.Index].Chargeable = true;
            }
        }
    }

    public void DropLoopMarker(RoadMarker Mrkr)
    {
        //Now we look backwards then forwards til we find a non-hairpin and adjust all these
        int PrevNoHPin = Bez.CtrlPts.Count - 3;
        int NxtNoHPin = Mrkr.Index + 1;
        //Find the previous non-hairpin
        do
        {
            float A = Bez.Angle(PrevNoHPin);
            //Debug.Log("CtrlPt" + PrevNoHPin.ToString() + " Angle = " + A);
            if (Mathf.Abs(Bez.Angle(PrevNoHPin)) > 80) { PrevNoHPin++; break; }
            PrevNoHPin--;
        } while (true);
        //Find the next non-hairpin
        do
        {
            float A = Bez.Angle(NxtNoHPin);
            if (Mathf.Abs(Bez.Angle(NxtNoHPin)) > 80) break;
            NxtNoHPin++;
        } while (true);

        //Recalc the XSecs
        for (int CtrlPtId = 1; CtrlPtId <= NxtNoHPin; CtrlPtId++)     //Between the non-hairpin markers
            XSecCalculator.CalcXSecs(CtrlPtId, RoadWidth);

        for (int CtrlPtId = PrevNoHPin + 1; CtrlPtId < Bez.CtrlPts.Count - 1; CtrlPtId++)     //Between the non-hairpin markers
            XSecCalculator.CalcXSecs(CtrlPtId, RoadWidth);


        //Add to the build queue
        Rd.Sectns[Bez.CtrlPts.Count - 3].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(Bez.CtrlPts.Count - 3);
        Rd.Sectns[Mrkr.Index].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(Mrkr.Index);
        Rd.Sectns[Mrkr.Index + 1].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(Mrkr.Index + 1);

        Mrkr.DroppedPosition = Mrkr.transform.position;
    }

    /// <summary>
    /// When you drop the marker onto the start marker
    /// It creates a smooth(ish) overlap by adding two extra control points
    /// and aligning them with points 2 and 3
    /// </summary>
    /// <param name="Point"></param>
    public void JoinSection(Vector3 Point)
    {
        if (Rd.Sectns.Count < 4) return;
        int SegCount = Mathf.CeilToInt(Vector3.Distance(Point, Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos) / 0.84f);
        Road.Instance.IsCircular = true;
        RoadSectn Sectn = new RoadSectn();
        int NewIdx = Road.Instance.Sectns.Count;
        Sectn.Idx = NewIdx;
        Sectn.name = "RoadSection" + (NewIdx);
        Rd.Sectns[NewIdx - 1].AddXSecs(SegCount);    //Add XSecs to the PREVIOUS Section
        Sectn.CreateGameObjects();
        Road.Instance.Sectns.Add(Sectn);

        //Move the hidden control point to the penultimate position:
        Bez.CtrlPts[0].Pos = Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos;  //this doesnt work any more - had to fix it in CalcXSecs
        //Add a control point at the join
        Bez.AddControlPoint(Point, 0, SegCount);
        //We add a couple more control points so we can recalculate the curve after the join
        //We will remove one of these two in a sec
        RoadSectn OverlapSectn = new RoadSectn();
        //OverlapSectn.AddXSecs(Rd.Sectns[1].Segments.Count);
        Bez.AddControlPoint(Bez.CtrlPts[2].Pos, Bez.CtrlPts[2].BankAngle, 20);
        Bez.AddControlPoint(Bez.CtrlPts[3].Pos, Bez.CtrlPts[3].BankAngle, 20);
        //Recalculate Section 1 that goes from CtrlPt1 to CtrlPt2
        Bez.Interp(1);  //It doesn't interp the Section before CtrlPt1
        //Interp the path two before the end
        Bez.Interp(NewIdx - 1);
        //Interp the path just before the end 
        Bez.Interp(NewIdx);

        Bez.AlignAllRoadMarkers();
        XSecCalculator.CalcXSecs(2, RoadWidth);


        //Bez.RemoveLastControlPoint();
        //Bez.CtrlPts[Bez.CtrlPts.Count - 1] = Bez.CtrlPts[Bez.CtrlPts.Count - 2];

        XSecCalculator.CalcXSecs(NewIdx - 1, RoadWidth);
        XSecCalculator.CalcXSecs(NewIdx, RoadWidth);
        //NOt sure we need this next bit - We only need to calculate the XSec for one path point.
        // Commented out because it crashed - XSecCalculator.CalcXSecs(NewIdx + 1, RoadWidth);
        //Remove the 2 extra control points and their paths
        Bez.RemoveLastControlPoint();
        Bez.RemoveLastControlPoint();
        Bez.CtrlPts[Bez.CtrlPts.Count - 1].Pos = Bez.CtrlPts[2].Pos;


        //new bit
        for (int Idx = Bez.CtrlPts[NewIdx - 1].SegStartIdx; Idx < Bez.CtrlPts[NewIdx].SegStartIdx; Idx++)    //create the segs ahead of the marker but don't mesh them
        {
            RoadSegment seg = new RoadSegment();
            seg.Idx = Idx;
            seg.SectnIdx = NewIdx - 1;
            Rd.Sectns[NewIdx - 1].Segments.Add(seg);
            seg.CreateGameObjects();
            seg.goSeg.name = "RoadSeg" + Idx;
            Rd.Segments.Add(seg);
            seg.SetMaterial(RoadMat);
            seg.LFenceType = LFenceType;
            seg.RFenceType = RFenceType;
            seg.goSeg.transform.SetParent(Rd.Sectns[NewIdx - 1].goSectn.transform);
        }
        Rd.Sectns[NewIdx - 1].SetMaterial(RoadMat); //bugfix 6/2/18 cos we now store RdMat in the section not the segment
        Rd.Sectns[NewIdx - 1].LFenceType = LFenceType;  //Longstanding bug fixed
        Rd.Sectns[NewIdx - 1].RFenceType = RFenceType;  //Longstanding bug fixed


        XSecCalculator.AdjustHairpin(Bez, NewIdx - 2);

        XSecCalculator.AdjustHairpin(Bez, NewIdx - 1);

        XSecCalculator.AdjustHairpin(Bez, 2);

        Road.Instance.Sectns[NewIdx - 3].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(NewIdx - 3);
        Road.Instance.Sectns[NewIdx - 2].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(NewIdx - 2);
        Road.Instance.Sectns[NewIdx - 1].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(NewIdx - 1);
        Road.Instance.Sectns[NewIdx].CalcVisibleFenceVerts();

        //Join the beginning of the first Segment to the end of the last

        //Road.Instance.XSecs[0].KerbL = Road.Instance.XSecs[(Road.Instance.Sectns.Count - 2) * 20].KerbL;
        //Road.Instance.XSecs[0].KerbR = Road.Instance.XSecs[(Road.Instance.Sectns.Count - 2) * 20].KerbR;


        Road.Instance.Sectns[1].CalcVisibleFenceVerts();
        BuildQueue.Enqueue(1);
        //Remove the Path points that were created when we added the second extra control point (The first one is needed because the SegVerts calculation needs it)
        //Bez.Path.RemoveRange(Bez.Path.Count - 20, 20);    //This line commentd out now because RemoveCtrlPt also removes the Path Points
        Bez.DrawLine();

    }

    private void AddScenery(Vector3 pos)
    {
        SceneryObject SObj = new SceneryObject(Toolbox.SceneryOpt);
        SObj.PlaceObject(pos, Quaternion.identity, Vector3.one);
        SObj.Chargeable = true;
        Scenery.Instance.Objects.Add(SObj);
        SObj.Select();
    }

    private void ReindexSectns(int StartSectnID)
    {
        //Bez.Path
        List<Vector3> PrevPaths = Bez.Path.GetRange(0, Bez.CtrlPts[StartSectnID].SegStartIdx);
        Bez.Path.RemoveRange(0, Bez.CtrlPts[StartSectnID].SegStartIdx);
        Bez.Path.AddRange(PrevPaths);

        //BezCtrlPts
        //remove the extra cp at starts and the two extra at the end
        Bez.CtrlPts.RemoveAt(0);
        Bez.CtrlPts.RemoveAt(Bez.CtrlPts.Count - 1);
        Bez.CtrlPts.RemoveAt(Bez.CtrlPts.Count - 1);
        //Copy the bit before the StartIdx
        List<BezCtrlPt> Prevs = Bez.CtrlPts.Where(c => c.CtrlPtId < StartSectnID).ToList();
        //Paste it at the end
        Bez.CtrlPts.AddRange(Prevs);
        Bez.CtrlPts.RemoveRange(0, StartSectnID-1);
        //Add the 3 extra CtrlPts
        BezCtrlPt p0 = new BezCtrlPt(Bez, Bez.CtrlPts.Last().Pos);
        Bez.CtrlPts.Insert(0, p0);
        BezCtrlPt p1 = new BezCtrlPt(Bez, Bez.CtrlPts[1].Pos);
        Bez.CtrlPts.Add(p1);
        BezCtrlPt p2 = new BezCtrlPt(Bez, Bez.CtrlPts[2].Pos);
        Bez.CtrlPts.Add(p2);

        Bez.CtrlPts[0].SegStartIdx = 0;
        Bez.SetCtrlPtIds();



        //Road Sections
        iRoadSectn LastSctn = Rd.Sectns.Last();
        Rd.Sectns.Remove(LastSctn);
        List<iRoadSectn> PrevSctns = Rd.Sectns.Where(s => s.Idx > 0 && s.Idx < StartSectnID).ToList();

        Rd.Sectns.AddRange(PrevSctns);
        Rd.Sectns.RemoveRange(1, StartSectnID - 1);
        Rd.Sectns.Add(LastSctn);
        Rd.SetSectionIds();
        for(int SectnID = 1; SectnID < Rd.Sectns.Count - 1; SectnID++)
        {
            Rd.Sectns[SectnID].goSectn.transform.SetSiblingIndex(SectnID);
        }

        //Rename RoadMarkers
        foreach (BezCtrlPt Ctl in Bez.CtrlPts)
        {
            if (Ctl.goRdMkr != null)
            {
                GameObject goRdMkr = Ctl.goRdMkr;
                RoadMarker RoadMarker = goRdMkr.gameObject.GetComponent<RoadMarker>();
                RoadMarker.name = "RoadMarker" + Ctl.CtrlPtId;
                RoadMarker.Index = Ctl.CtrlPtId;
                goRdMkr.transform.SetParent(Road.Instance.Sectns[Ctl.CtrlPtId].goSectn.transform);
            }
        }

        //Reindex Segments
        List<RoadSegment> PrevSegs = Rd.Segments.Where(s => s.SectnIdx < StartSectnID).ToList();
        //Rd.XSecs.Join(PrevSegs, x => x.Idx, i => i.Idx, (r) => new XSec);
        var px =
            (from x in Rd.XSecs
            join s in PrevSegs on
            x.Idx equals s.Idx
        select x);
        List<XSec> PrevXSecs = px.Cast<XSec>().ToList();
        Rd.Segments.AddRange(PrevSegs);
        Rd.Segments.RemoveRange(0, PrevSegs.Count);
        for(int i = 0; i<Rd.Segments.Count; i++)
        {
            Rd.Segments[i].Idx = i;
            Rd.Segments[i].goSeg.name = "RoadSeg" + i;
        }
        foreach(iRoadSectn Sec in Rd.Sectns)
        {
            foreach (RoadSegment seg in Sec.Segments)
            {
                seg.SectnIdx = Sec.Idx;
            }
        }

        Bez.SetSegStartIds();

        //Reindex XSecs
        Rd.XSecs.AddRange(PrevXSecs);
        Rd.XSecs.RemoveRange(0, PrevXSecs.Count);
        for (int i = 0; i < Rd.XSecs.Count; i++)
        {
            Rd.XSecs[i].Idx = i;
        }
    }

    public void TakePlanViewScreenshot()
    {
        UnityEngine.Object o = Resources.Load("prefabs/ScreenshotCam");
        goPlanCam = (GameObject)GameObject.Instantiate(o);
        Camera PlanCam = goPlanCam.GetComponent<Camera>();
        PlanCam.orthographic = true;
        //find the major axis (two Ctrlpts c1 and c2 furthest apart)
        float dist = 0;
        BezCtrlPt c1 = new BezCtrlPt(Bez, Vector3.zero), c2 = new BezCtrlPt(Bez, Vector3.zero);
        foreach (BezCtrlPt p1 in Bez.CtrlPts)
        {
            foreach (BezCtrlPt p2 in Bez.CtrlPts)
            {
                float thisdist = Mathf.Pow(p1.Pos.x - p2.Pos.x, 2) + Mathf.Pow(p1.Pos.z - p2.Pos.z, 2);
                if (thisdist > dist)
                {
                    dist = thisdist; c1 = p1; c2 = p2;
                }
            }
        }
        Vector3 c1Pos = new Vector3(c1.Pos.x, 500, c1.Pos.z);
        Vector3 c2Pos = new Vector3(c2.Pos.x, 500, c2.Pos.z);
        PlanCam.transform.position = (c1Pos + c2Pos) / 2;
        PlanCam.transform.LookAt(c1Pos);
        PlanCam.transform.Rotate(Vector3.up, 90);
        float _distFwd = 0, _distBwd = 0;

        foreach(BezCtrlPt p1 in Bez.CtrlPts)
        {
            Vector3 RelPos = PlanCam.transform.InverseTransformPoint(new Vector3(p1.Pos.x, 500, p1.Pos.z));
            if (RelPos.z > _distFwd) _distFwd = RelPos.z;
            if (RelPos.z < _distBwd) _distBwd = RelPos.z;
        }
        PlanCam.transform.Translate(0, 0, (_distFwd + _distBwd)/2, Space.Self);
        PlanCam.transform.Rotate(Vector3.right, 90, Space.Self);
        PlanCam.orthographicSize = Mathf.Pow(dist, 0.5f) * (float)Screen.height / (float)Screen.width / 2 * 1.1f;
        //_canvas.SetActive(false);
        goPlanCam.GetComponent<ScreenshotCam>().goCanvas = _canvas;
        goPlanCam.GetComponent<ScreenshotCam>().Grab = true;
        Bez.SetWidth(10);
        Rd.goRoad.SetActive(false);
        foreach(GameObject t in GameObject.FindGameObjectsWithTag("Terrain"))
        {
            t.GetComponent<Terrain>().enabled = false;
        }
        //Debug.Break();
    }


    void OnDestroy()
    {
        Rd.BuilderPos = transform.position;
        Rd.BuilderRot = transform.rotation;
        Toolbox.ToolOptionChanged -= ToolChange;
        if(Game.current.Dirty) Rd.CalculateBends();
    }

    private void PlaceMarker(Vector3 Pos, float height)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.transform.localScale = new Vector3(0.2f, height / 10, 0.2f);
        marker.name = "Path " + height;
        marker.transform.position = Pos;
    }

}


