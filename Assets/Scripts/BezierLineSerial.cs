using UnityEngine;
using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class BezierLineSerial
{
    public Vector3Serial[] ControlPoints;
    public BezCtrlPtSerial[] CtrlPts;
    public Vector3Serial[] Path;

    public void Encode(BezierLine Bez)
    {
        Path = new Vector3Serial[Bez.Path.Count];
        for (int Idx = 0; Idx < Bez.Path.Count; Idx++)
        {
            Vector3Serial PS = new Vector3Serial(Bez.Path[Idx]);
            Path.SetValue(PS, Idx);
        }

        CtrlPts = new BezCtrlPtSerial[Bez.CtrlPts.Count];
        for (int Idx = 0; Idx < Bez.CtrlPts.Count; Idx++)
        {
            BezCtrlPtSerial BCPS = new BezCtrlPtSerial();
            BCPS.Encode(Bez.CtrlPts[Idx]);
            CtrlPts.SetValue(BCPS, Idx);
        }
    }

    public BezierLine Decode()
    {
        BezierLine Bez = BezierLine.Instance;
        Bez.LineId = 0;
        Bez.Path.Clear();
        Bez.CtrlPts.Clear();
        if (Path != null)   //in case we are loading a blank track
        {
            foreach (BezCtrlPtSerial BCPS in CtrlPts)
            {
                BezCtrlPt BCP = BCPS.Decode();
                Bez.CtrlPts.Add(BCP);
            }


            foreach (Vector3Serial PS in Path)
            {
                Bez.Path.Add(PS.V3);
            }
        }
        else { Bez.Init(); }    //blank track - put in the 2 dummies
        return Bez;

    }
}

[System.Serializable]
public class BezCtrlPtSerial
{
    public int CtrlPtId;
    public int LineId;
    public Vector3Serial Pos;
    public float BankAngle;
    public int SegStartIdx;
    public int SegCount;

    public void Encode(BezCtrlPt CtrlPt)
    {
        CtrlPtId = CtrlPt.CtrlPtId;
        LineId = CtrlPt.Line.LineId;
        Pos = new Vector3Serial(CtrlPt.Pos);
        BankAngle = CtrlPt.BankAngle;
        SegStartIdx = CtrlPt.SegStartIdx;
        SegCount = CtrlPt.SegCount;

    }

    public BezCtrlPt Decode()
    {
        BezCtrlPt BCP = new BezCtrlPt(BezierLine.Instance, Pos.V3);     //!!!!!!!Bugbugbug bug -fixed I think
        BCP.BankAngle = BankAngle;
        BCP.CtrlPtId = CtrlPtId;
        BCP.SegStartIdx = SegStartIdx;
        BCP.SegCount = SegCount;
        return BCP;
    }
}

