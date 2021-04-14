using UnityEngine;
using System.Collections.Generic;

    [System.Serializable]
    public class RoadSerial
    {
        public RoadSectionSerial[] Sectns;
        public RoadSegmentSerial[] Segs;
        public XSecSerial[] XSecs;
        public Vector3Serial StartingLinePos;
        public QuaternionSerial StartingLineRot;
        public int StartingLineSegIdx;
        public Vector3Serial BuilderPos;
        public QuaternionSerial BuilderRot;
        public bool IsCircular;

        public void Encode()
        {
            Road Rd = Road.Instance;
            StartingLineSegIdx = Rd.StartingLineSegIdx;
            BuilderPos = new Vector3Serial(Rd.BuilderPos);
            BuilderRot = new QuaternionSerial(Rd.BuilderRot);
            IsCircular = Rd.IsCircular;
            XSecs = new XSecSerial[Rd.XSecs.Count];
            for (int Idx = 0; Idx < Rd.XSecs.Count; Idx++)
            {
                XSecSerial XS = new XSecSerial();
                XS.Encode(Rd.XSecs[Idx]);
                XSecs[Idx] = XS;
            }

            Sectns = new RoadSectionSerial[Rd.Sectns.Count];
            for(int Idx =0; Idx < Rd.Sectns.Count; Idx++)
            {
                RoadSectionSerial RSS = new RoadSectionSerial();
                RSS.Encode(Rd.Sectns[Idx]);
                Sectns[Idx] = RSS;
            }

            Segs = new RoadSegmentSerial[Rd.Segments.Count];
            for (int Idx = 0; Idx < Rd.Segments.Count; Idx++)
            {
                RoadSegmentSerial SegS = new RoadSegmentSerial();
                SegS.Encode(Rd.Segments[Idx]);
                Segs[Idx] = SegS;
            }
        }

        public void Decode()
        {
            Road Rd = Road.Instance;
            Rd.Segments.Clear();
            Rd.XSecs.Clear();
            Rd.Sectns.Clear();
        if (XSecs == null)
        {  //in case we are loading a blank track
            Rd.Init();
        }
        else
        {
            Rd.StartingLineSegIdx = StartingLineSegIdx;
            if (BuilderPos != null) { Rd.BuilderPos = BuilderPos.V3; } else { Rd.BuilderPos = new Vector3(0, 50f, 0); }
            if (BuilderRot != null) { Rd.BuilderRot = BuilderRot.Decode; }
            Rd.IsCircular = IsCircular;
            foreach (XSecSerial XS in XSecs)
            {
                XSec X = XS.Decode();
                Rd.XSecs.Add(X);
            }

            foreach (RoadSectionSerial RSS in Sectns)
            {
                RoadSectn RS = RSS.Decode();
                Rd.Sectns.Add(RS);
            }


            foreach (RoadSegmentSerial SegS in Segs)
            {
                RoadSegment Seg = SegS.Decode();
                Rd.Segments.Add(Seg);
            }

            //Build the meshes for all the segments
            for (int Idx = 0; Idx < Segs.Length; Idx++)
            {
                RoadSegment seg = Road.Instance.Segments[Idx];
                if (seg.HasMesh)
                {
                    SegVerts Verts = XSecCalculator.SegmentVertices(Idx);
                    seg.BuildMeshes(Verts);
                }
                //seg.GetTerrainHeights();
                //seg.AdjustTerrain();
                //seg.SetMaterial();
                //seg.DeleteFence();
                //seg.CreateFence();
            }
            Rd.CalculateBends();
        }
      }

    }

    [System.Serializable]
    public class XSecSerial
    {
        public int Idx;
        public Vector3Serial MidPt;
        public Vector3Serial KerbL;
        public Vector3Serial KerbR;
        public Vector3Serial TerrainL;
        public Vector3Serial TerrainR;

        public void Encode(XSec X)
        {
            Idx = X.Idx;
            KerbL = new Vector3Serial(X.KerbL);
            KerbR = new Vector3Serial(X.KerbR);
            TerrainL = new Vector3Serial(X.TerrainL);
            TerrainR = new Vector3Serial(X.TerrainR);
            MidPt = new Vector3Serial(X.MidPt);
        }

        public XSec Decode()
        {
            XSec rtn = new XSec(Idx);
            rtn.KerbL = KerbL.V3;
            rtn.KerbR = KerbR.V3;
            rtn.TerrainL = TerrainL.V3;
            rtn.TerrainR = TerrainR.V3;
            rtn.MidPt = MidPt.V3;
            return rtn;
        }
    }

    [System.Serializable]
    public class RoadSectionSerial
    {
        public int Idx;
        public string LFenceType;
        public string RFenceType;
        public string Mat;
        public Vector3Serial[] LFenceVerts;
        public Vector3Serial[] RFenceVerts;
        //[System.Runtime.Serialization.OptionalField]
        //public int[] XSecIds;
        [System.Runtime.Serialization.OptionalField]
        public float Width;

    public void Encode(iRoadSectn RS)
        {
            Idx = RS.Idx;
            LFenceType = RS.LFenceType;
            RFenceType = RS.RFenceType;
            Mat = RS.RoadMaterial;
            LFenceVerts = new Vector3Serial[RS.LFenceVerts.Count];
            for (int LFVId = 0; LFVId < RS.LFenceVerts.Count; LFVId++)
            {
                LFenceVerts[LFVId] = new Vector3Serial(RS.LFenceVerts[LFVId]);
            }
            RFenceVerts = new Vector3Serial[RS.RFenceVerts.Count];
            for (int RFVId = 0; RFVId < RS.RFenceVerts.Count; RFVId++)
            {
                RFenceVerts[RFVId] = new Vector3Serial(RS.RFenceVerts[RFVId]);
            }
        //XSecIds = new int[RS.XSecs.Count];
        //for (int XSId = 0; XSId < RS.XSecs.Count; XSId++)
        //{
        //    XSecIds[XSId] = RS.XSecs[XSId].Idx;
        //}
        }

        public RoadSectn Decode()
        {
            RoadSectn rtn = new RoadSectn();
            rtn.Idx = Idx;
            rtn.name = "RoadSection" + Idx;
            rtn.LFenceType = LFenceType;
            rtn.RFenceType = RFenceType;
            rtn.RoadMaterial = Mat;
        foreach (Vector3Serial  VS in LFenceVerts)
            {
                rtn.LFenceVerts.Add(VS.V3);
            }
            foreach (Vector3Serial VS in RFenceVerts)
            {
                rtn.RFenceVerts.Add(VS.V3);
            }
            return rtn;
        }
    }

    [System.Serializable]
    public class RoadSegmentSerial
    {
        public int Idx;
        public int SectnIdx;
//        [System.Runtime.Serialization.OptionalField]    //says this field can be missing
        public string roadMaterial;
        public bool HasMesh;

        public void Encode(RoadSegment Seg)
        {
            Idx = Seg.Idx;
            SectnIdx = Seg.SectnIdx;
            //roadMaterial = Road.Instance.Sectns[SectnIdx].RoadMaterial;
            HasMesh = Seg.HasMesh;
        }

        public RoadSegment Decode()
        {
            RoadSegment rtn = new RoadSegment();
            rtn.Idx = Idx;
            rtn.SectnIdx = SectnIdx;
            Road.Instance.Sectns[SectnIdx].Segments.Add(rtn);
            rtn.roadMaterial = Road.Instance.Sectns[SectnIdx].RoadMaterial;
            rtn.HasMesh = HasMesh;
            return rtn;
         }
    }



