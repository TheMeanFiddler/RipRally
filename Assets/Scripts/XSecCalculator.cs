using UnityEngine;
//Alex Giffgaff sim card - He's sending texts and deleting them so I wont see them
//End of 8th Jan 416 Texts

public static class XSecCalculator
{
    /// <summary>
    /// Calculates the Kerb positions for the segments leading up to the controlpoint
    /// <para>Ascertains if its possible first so it never crashes</para>
    /// </summary>
    public static void CalcXSecs(int CtrlPtIdx, int RoadWidth, bool FrstHlf = true, bool SecndHlf = true)
    {
        BezierLine Bez = BezierLine.Instance;
        Road Rd = Road.Instance;
        //If this is Idx1 then calculate the last visible one
        if (CtrlPtIdx == 1 && Rd.IsCircular) CtrlPtIdx = Bez.CtrlPts.Count - 2;
        //if (CtrlPtIdx == Bez.CtrlPts.Count-1 && Rd.IsCircular) CtrlPtIdx = 1;   //doesnt work 
        if (CtrlPtIdx < 2) return;
        if (CtrlPtIdx > Bez.CtrlPts.Count-2) return;
        int CP1XId = Bez.CtrlPts[CtrlPtIdx - 2].SegStartIdx;
        int CP2XId = Bez.CtrlPts[CtrlPtIdx - 1].SegStartIdx;
        int CP3XId = Bez.CtrlPts[CtrlPtIdx].SegStartIdx;
        int CP4XId = Bez.CtrlPts[CtrlPtIdx + 1].SegStartIdx;

        if (Bez.CtrlPts[CtrlPtIdx - 1].SegCount == 0) CP2XId = CP3XId;
        if (Bez.CtrlPts[CtrlPtIdx - 2].SegCount == 0) CP1XId = CP2XId;
        if (Rd.IsCircular && CP2XId == Rd.XSecs.Count) { CP2XId = Bez.CtrlPts[2].SegStartIdx; CP3XId = Bez.CtrlPts[2].SegStartIdx; CP4XId = Bez.CtrlPts[3].SegStartIdx; } //because of the overlap
        if (Rd.IsCircular && CP3XId == Rd.XSecs.Count) { CP3XId = Bez.CtrlPts[1].SegStartIdx; CP4XId = Bez.CtrlPts[2].SegStartIdx; } //because of the overlap
        if (Rd.IsCircular && CtrlPtIdx == 2) CP1XId = Bez.CtrlPts[Rd.Sectns.Count-2].SegStartIdx;
        //if (Bez.CtrlPts[CtrlPtIdx + 1].SegCount == 0) CP4XId = CP3XId;
        
        XSec XSec1 = CalcXSecPerp(CP1XId, RoadWidth);
        XSec XSec2 = CalcXSecPerp(CP2XId, RoadWidth);
        XSec XSec3 = CalcXSecPerp(CP3XId, RoadWidth);
        XSec XSec4 = CalcXSecPerp(CP4XId, RoadWidth);

        Vector3 RKerb1;
        Vector3 RKerb2;
        Vector3 RKerb3;
        Vector3 RKerb4;
        Vector3 LKerb1;
        Vector3 LKerb2;
        Vector3 LKerb3;
        Vector3 LKerb4;

        RKerb1 = XSec1.KerbR;
        RKerb2 = XSec2.KerbR;
        RKerb3 = XSec3.KerbR;
        RKerb4 = XSec4.KerbR;

        LKerb1 = XSec1.KerbL;
        LKerb2 = XSec2.KerbL;
        LKerb3 = XSec3.KerbL;
        LKerb4 = XSec4.KerbL;


        //this worked on the flat road. Now we have to bank the road
        Vector3 _bankDirection;
        try
        {
        //This will fail at some point if you are moving a cone at the end - its OK tho
        _bankDirection = (Bez.PathPlusOne(CP1XId) - Bez.Path[CP1XId]).normalized * Bez.CtrlPts[CtrlPtIdx - 2].BankAngle;
            if(Bez.CtrlPts[CtrlPtIdx - 2].BankAngle < 0)
                LKerb1 = VectGeom.RotatePointAroundPivot(LKerb1, Bez.Path[CP1XId], _bankDirection);
            else 
                RKerb1 = VectGeom.RotatePointAroundPivot(RKerb1, Bez.Path[CP1XId], _bankDirection);
        _bankDirection = (Bez.PathPlusOne(CP2XId) - Bez.Path[CP2XId]).normalized * Bez.CtrlPts[CtrlPtIdx - 1].BankAngle;
            if (Bez.CtrlPts[CtrlPtIdx - 1].BankAngle < 0)
                LKerb2 = VectGeom.RotatePointAroundPivot(LKerb2, Bez.Path[CP2XId], _bankDirection);
            else
                RKerb2 = VectGeom.RotatePointAroundPivot(RKerb2, Bez.Path[CP2XId], _bankDirection);
        _bankDirection = (Bez.PathPlusOne(CP3XId) - Bez.Path[CP3XId]).normalized * Bez.CtrlPts[CtrlPtIdx].BankAngle;
            if (Bez.CtrlPts[CtrlPtIdx].BankAngle < 0)
                LKerb3 = VectGeom.RotatePointAroundPivot(LKerb3, Bez.Path[CP3XId], _bankDirection);
            else
                RKerb3 = VectGeom.RotatePointAroundPivot(RKerb3, Bez.Path[CP3XId], _bankDirection);
        _bankDirection = (Bez.PathPlusOne(CP4XId) - Bez.Path[CP4XId]).normalized * Bez.CtrlPts[CtrlPtIdx + 1].BankAngle;
            if (Bez.CtrlPts[CtrlPtIdx+1].BankAngle < 0)
                LKerb4 = VectGeom.RotatePointAroundPivot(LKerb4, Bez.Path[CP4XId], _bankDirection);
            else
                RKerb4 = VectGeom.RotatePointAroundPivot(RKerb4, Bez.Path[CP4XId], _bankDirection);
        }
        catch (System.Exception e) { Debug.Log(e.ToString()); }


        int NumXSecsToAdjust;
        //Catmull Rom up to the control pt
        //RIGHT KERB
        try { NumXSecsToAdjust = Bez.CtrlPts[CtrlPtIdx - 1].SegCount; }
        catch (System.Exception e) { NumXSecsToAdjust = 0; }
        int AdjustStartIdx = CP2XId;
        Vector3 a = RKerb1;
        Vector3 b = RKerb2;
        Vector3 c = RKerb3;
        Vector3 d = RKerb4;

        float t = 0.5f;
        //if (Vector3.Angle(c - b, d - c) > 135 || Vector3.Angle(b - a, c - b) > 135) t = 2.5f;
        for (int p = 0; p < NumXSecsToAdjust; p++)
        {
            float u = (float)p / NumXSecsToAdjust;

            //Coeffs for the catmullrom equation
            Vector3 c0 = b;
            Vector3 c1 = -t * a + t * c;
            Vector3 c2 = 2f * t * a + (t - 3f) * b + (3f - 2f * t) * c - t * d;
            Vector3 c3 = -t * a + (2f - t) * b + (t - 2f) * c + t * d;
            Vector3 temp = c0 + c1 * u + c2 * u * u + c3 * u * u * u;
            //Vector3 temp = .5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
            Vector3 newKerb = temp;
            //PlaceMarker(temp);
            Rd.XSecs[AdjustStartIdx + p].KerbR = temp;
        }

        //Catmull Rom up to the control pt
        //LEFT KERB
        a = LKerb1;
        b = LKerb2;
        c = LKerb3;
        d = LKerb4;

        //if (Vector3.Angle(c - b, d - c) > 135 || Vector3.Angle(b - a, c - b) > 135) t = 2.5f;
        for (int p = 0; p < NumXSecsToAdjust; p++)
        {
            float u = (float)p / NumXSecsToAdjust;

            //Coeffs for the catmullrom equation
            Vector3 c0 = b;
            Vector3 c1 = -t * a + t * c;
            Vector3 c2 = 2f * t * a + (t - 3f) * b + (3f - 2f * t) * c - t * d;
            Vector3 c3 = -t * a + (2f - t) * b + (t - 2f) * c + t * d;
            Vector3 temp = c0 + c1 * u + c2 * u * u + c3 * u * u * u;
            //Vector3 temp = .5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
            Vector3 newKerb = temp;
            //PlaceMarker(temp);
            Rd.XSecs[AdjustStartIdx + p].KerbL = temp;
            Rd.XSecs[AdjustStartIdx + p].TerrainL = (Rd.XSecs[AdjustStartIdx + p].KerbL - Bez.Path[AdjustStartIdx + p]) * 1.3f + Bez.Path[AdjustStartIdx + p];
            Rd.XSecs[AdjustStartIdx + p].TerrainR = (Rd.XSecs[AdjustStartIdx + p].KerbR - Bez.Path[AdjustStartIdx + p]) * 1.3f + Bez.Path[AdjustStartIdx + p];

            Rd.XSecs[AdjustStartIdx + p].MidPt = (Rd.XSecs[AdjustStartIdx + p].KerbL + Rd.XSecs[AdjustStartIdx + p].KerbR) / 2;
            Rd.XSecs[AdjustStartIdx + p].MidPt = new Vector3(Rd.XSecs[AdjustStartIdx + p].MidPt.x, Bez.Path[AdjustStartIdx + p].y, Rd.XSecs[AdjustStartIdx + p].MidPt.z);
        }
    }

    public static XSec CalcXSecPerp(int PathId, int RoadWidth)
    {
        XSec rtn = new XSec(PathId);
        BezierLine Bez = BezierLine.Instance;
        Vector3 Path3d;
        if (PathId == Bez.Path.Count)
            Path3d = Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos;
         else 
            Path3d = Bez.Path[PathId];
        Vector2 Path2d = Convert2d(Path3d);
        Vector2 Perp1 = Perpndclr(PathId);
        Vector2 KerbR2d = Path2d + Perp1 * RoadWidth;
        Vector2 KerbL2d = Path2d - Perp1 * RoadWidth;

        Vector3 KerbR = new Vector3(KerbR2d.x, Path3d.y, KerbR2d.y);
        Vector3 KerbL = new Vector3(KerbL2d.x, Path3d.y, KerbL2d.y);


        rtn.TerrainL = (KerbL - Path3d) * 1.3f + Path3d;         //Here's where ther terrain is slightly wider than the road
        rtn.TerrainR = (KerbR - Path3d) * 1.3f + Path3d;         //Here's where ther terrain is slightly wider than the road
        rtn.KerbL = KerbL;
        rtn.KerbR = KerbR;
        rtn.MidPt = Path3d;
        return rtn;

    }

    private static Vector2 Convert2d(Vector3 V)
    {
        return new Vector2(V.x, V.z);
    }

    public static Vector2 Perpndclr(int Idx)
    //Returns the vector2 that is perpendiclr to the path 
    {
        BezierLine Bez = BezierLine.Instance;
        Vector2 p0;
        Vector2 p1;
        Vector2 p2;
        if (Idx == Bez.Path.Count)
            p1 = new Vector2(Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos.x, Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos.z);
        else
            p1 = new Vector2(Bez.Path[Idx].x, Bez.Path[Idx].z);

        if (Idx > 0)
        {
            p0 = new Vector2(Bez.Path[Idx - 1].x, Bez.Path[Idx - 1].z);
        }
        else
        {
            p0 = p1;
        }
        if (Idx < Bez.Path.Count - 1)
        {
            p2 = new Vector2(Bez.Path[Idx + 1].x, Bez.Path[Idx + 1].z);
        }
        else
        {
            p2 = p1;
        }
        if (Road.Instance.IsCircular && Idx == Road.Instance.XSecs.Count) { p2 = new Vector2(Bez.Path[1].x, Bez.Path[1].z); }
        //p0.Normalize();
        //p1.Normalize();
        //p2.Normalize();

        Vector2 Dir1 = p1 - p0;
        Vector2 Dir2 = p2 - p1;
        Vector2 Tangent = (Dir1 + Dir2) / 2;
        Vector2 Perpndclr = new Vector2(Tangent.y, -Tangent.x);
        Perpndclr.Normalize();
        return Perpndclr;
    }

    public static void AdjustHairpin(BezierLine Bez, int CtlPtIdx)
    {
        return;
        if (CtlPtIdx < 2) return;
        //Adjusts the Road.Instance.XSecs around the controlpoint
        string Hand;
        Road Rd = Road.Instance;
        //SegAdj=new List<SegVerts>();
        Vector2 PivotStart2d = Vector2.zero;
        Vector2 PivotEnd2d = Vector2.zero;
        int PrevCtrlPtSegId = Bez.CtrlPts[CtlPtIdx-1].SegStartIdx;
        int PivotStartIdx = Bez.CtrlPts[CtlPtIdx].SegStartIdx;
        int PivotEndIdx = Bez.CtrlPts[CtlPtIdx].SegStartIdx;
        int NxtCtrlPtSegId = Bez.CtrlPts[CtlPtIdx + 1].SegStartIdx;
        if (CtlPtIdx < 2) return;
        //See if its a right or a left hand curve
        Vector2 Path2d = Convert2d(Rd.XSecs[PivotStartIdx].MidPt);
        Vector2 PrvPath2d = Convert2d(Rd.XSecs[PivotStartIdx - 1].MidPt);
        Vector2 NxtPath2d = Convert2d(Rd.XSecs[PivotStartIdx + 1].MidPt);
        Vector3 cross = Vector3.Cross(PrvPath2d - Path2d, NxtPath2d - Path2d);
        if (cross.z > 0) Hand = "R"; else Hand = "L";

        //Right Edge
        if (Hand == "R")
        {
            //Catmull Rom up to the control pt
            int NumXSecsToAdjust = Bez.CtrlPts[CtlPtIdx - 1].SegCount/2;
            int AdjustStartIdx = (PrevCtrlPtSegId + PivotStartIdx) / 2;
            Vector3 a = Rd.XSecs[PrevCtrlPtSegId].KerbR;
            Vector3 b = Rd.XSecs[(PrevCtrlPtSegId + PivotStartIdx)/2].KerbR;
            Vector3 c = Rd.XSecs[PivotStartIdx].KerbR;
            Vector3 d = Rd.XSecs[(PivotStartIdx + NxtCtrlPtSegId) / 2].KerbR;
            c = new Vector3(c.x, (b.y + d.y) / 2, c.z);
            float t = 0.5f;
            //if (Vector3.Angle(c - b, d - c) > 135 || Vector3.Angle(b - a, c - b) > 135) t = 2.5f;
            for (int p = 0; p < NumXSecsToAdjust; p++)
            {
                float u = (float)p / NumXSecsToAdjust;

                //Coeffs for the catmullrom equation
                Vector3 c0 = b;
                Vector3 c1 = -t * a + t * c;
                Vector3 c2 = 2f * t * a + (t - 3f) * b + (3f - 2f * t) * c - t * d;
                Vector3 c3 = -t * a + (2f - t) * b + (t - 2f) * c + t * d;
                Vector3 temp = c0 + c1 * u + c2 * u * u + c3 * u * u * u;
                //Vector3 temp = .5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
                Vector3 newKerb = temp;
                XSec X = new XSec((CtlPtIdx - 1) * 20 - 10 + p);
                X.KerbR = temp - Rd.XSecs[AdjustStartIdx + p].KerbR;
                Road.Instance.XSecs[AdjustStartIdx + p].Adjust(X);
            }

            //Catmull ROm after the control point
            NumXSecsToAdjust = Bez.CtrlPts[CtlPtIdx].SegCount / 2;
            AdjustStartIdx = (PivotStartIdx) / 2;
            a = b;
            b = c;
            c = d;
            d = Rd.XSecs[NxtCtrlPtSegId].KerbR;
            //if (Vector3.Angle(c - b, d - c) > 135 || Vector3.Angle(b - a, c - b) > 135) t = 2.5f;
            for (int p = 0; p < NumXSecsToAdjust; p++)
            {
                float u = (float)p / NumXSecsToAdjust;

                //Coeffs for the catmullrom equation
                Vector3 c0 = b;
                Vector3 c1 = -t * a + t * c;
                Vector3 c2 = 2f * t * a + (t - 3f) * b + (3f - 2f * t) * c - t * d;
                Vector3 c3 = -t * a + (2f - t) * b + (t - 2f) * c + t * d;
                Vector3 temp = c0 + c1 * u + c2 * u * u + c3 * u * u * u;
                //Vector3 temp = .5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
                Vector3 newKerb = temp;
                XSec X = new XSec((CtlPtIdx - 1) * 20 + p);
                X.KerbR = temp - Rd.XSecs[AdjustStartIdx + p].KerbR;
                Road.Instance.XSecs[AdjustStartIdx + p].Adjust(X);
            }
        }
        if (Hand == "L")
        {
            for (int Idx1 = (CtlPtIdx - 2) * 20 + 1; Idx1 < (CtlPtIdx - 1) * 20 - 1; Idx1++)
            {
                for (int Idx2 = (CtlPtIdx) * 20 - 1; Idx2 > (CtlPtIdx - 1) * 20; Idx2--)
                {
                    Vector2 PrevKerb12d = Convert2d(XSecCalculator.CalcXSecPerp(Idx1 - 1, 8).KerbL);
                    Vector2 PrevKerb22d = Convert2d(XSecCalculator.CalcXSecPerp(Idx2 - 1, 8).KerbL);
                    Vector2 Kerb12d = Convert2d(XSecCalculator.CalcXSecPerp(Idx1, 8).KerbL);
                    Vector2 Kerb22d = Convert2d(XSecCalculator.CalcXSecPerp(Idx2, 8).KerbL);
                    if (VectGeom.LinesIntersect2d(PrevKerb12d, Kerb12d, PrevKerb22d, Kerb22d))
                    {
                        //We've found the crossing point
                        //                                                   where Hand = "L";
                        //Take 2 steps back cos the curve would be too tight 
                        PivotStartIdx = Idx1 - 2;
                        PivotEndIdx = Idx2 + 1;
                        PivotStart2d = Convert2d(Road.Instance.XSecs[PivotStartIdx].KerbL);
                        PivotEnd2d = Convert2d(Road.Instance.XSecs[PivotEndIdx].KerbL);
                        //Now move the closest pivot back so its the same distance from the control point
                        //Which is the closest?
                        Vector2 CtrlPt2d = Convert2d(Bez.CtrlPts[CtlPtIdx].Pos);
                        float DistS = Vector2.Distance(PivotStart2d, CtrlPt2d);
                        float DistE = Vector2.Distance(PivotEnd2d, CtrlPt2d);
                        PlaceMarker(CtrlPt2d);
                        if (DistS > DistE)
                        {
                            //Debug.Log("ClosestPivotIdx = PivotEndIdx");
                            PivotEnd2d = Vector2.Lerp(CtrlPt2d, PivotEnd2d, DistS / DistE);
                            Rd.XSecs[PivotEndIdx].KerbL = new Vector3(PivotEnd2d.x, 0, PivotEnd2d.y);           //Todo1
                        }
                        else
                        {
                            //Debug.Log("ClosestPivotIdx = PivotStartIdx");
                            PivotStart2d = CtrlPt2d + (PivotStart2d - CtrlPt2d) * DistE / DistS;
                            Rd.XSecs[PivotStartIdx].KerbL = new Vector3(PivotStart2d.x, 0, PivotStart2d.y);     //Todo1
                        }
                        PlaceMarker(PivotStart2d);
                        PlaceMarker(PivotEnd2d);
                    }
                }
            }
        }
        if (PivotStartIdx != PivotEndIdx)
        {
            //Adjust all the right kerbs around the pivot
            //lets just lerp the kerb from pivotstart to pivotend
            //Todo: would be nice to do a catmul-rom curve around the pivot
            float TotLerp = (float)(PivotEndIdx - PivotStartIdx);
            for (int Idx = PivotStartIdx + 1; Idx < PivotEndIdx; Idx++)
            {
                float LerpFrac = (float)(Idx - PivotStartIdx) / TotLerp;
                Vector2 Kerb2d;
                if (Hand == "R")
                {                                                           //Todo1
                    Kerb2d = Vector2.Lerp(Convert2d(Road.Instance.XSecs[PivotStartIdx].KerbR), (Convert2d(Road.Instance.XSecs[PivotEndIdx].KerbR)), LerpFrac);
                    Rd.XSecs[Idx].KerbR = new Vector3(Kerb2d.x, Bez.Path[Idx].y, Kerb2d.y);         //Todo1
                }
                else
                {                                                           //Todo1
                    Kerb2d = Vector2.Lerp(Convert2d(Road.Instance.XSecs[PivotStartIdx].KerbL), (Convert2d(Road.Instance.XSecs[PivotEndIdx].KerbL)), LerpFrac);
                    Rd.XSecs[Idx].KerbL = new Vector3(Kerb2d.x, Bez.Path[Idx].y, Kerb2d.y);         //Todo1
                }

            }
        }
        //Even if there is no hairpin we still might have a steep inner slope
        //So find where the steepness starts and finishes
        if (PivotStartIdx == PivotEndIdx)
        {
            Vector3 K1;
            Vector3 K2;
            bool PivotStarted = false;
            for (int i = (CtlPtIdx - 1) * 20 - 5; i < (CtlPtIdx - 1) * 20 + 5; i++)
            {
                if (Hand == "L")
                {
                    K1 = Road.Instance.XSecs[i].KerbL;
                    K2 = Road.Instance.XSecs[i + 1].KerbL;
                }
                else
                {
                    K1 = Road.Instance.XSecs[i].KerbR;
                    K2 = Road.Instance.XSecs[i + 1].KerbR;
                }
                if ((K2.y - K1.y) / Vector2.Distance(Convert2d(K2), Convert2d(K1)) > 0.4f)
                {
                    if (PivotStarted == false)
                    {
                        PivotStartIdx = i;
                        PivotStarted = true;
                    }
                    if (PivotStarted == true)
                    {
                        PivotEndIdx = i;
                    }
                }
                else
                {
                    if (PivotStarted == true) break;
                }
            }
        }
        //Adjust the heights so we don't get steps in the road
        //we adjust the inner slope before and after the pivot uaing a smoothstep function
        int NoOfSegmentsToAdjust = Mathf.CeilToInt(Mathf.Abs((Bez.Path[PivotEndIdx].y - Bez.Path[PivotStartIdx].y) / 2));
        int AdjStartIdx = PivotStartIdx - NoOfSegmentsToAdjust;
        if (AdjStartIdx < 0) AdjStartIdx = 0;
        int AdjEndIdx = PivotEndIdx + NoOfSegmentsToAdjust;
        float AdjStarty;
        float AdjEndy;
        float TotAdjDist = 0;
        if (Hand == "L")
        {
            AdjStarty = Road.Instance.XSecs[AdjStartIdx].KerbL.y;
            AdjEndy = Road.Instance.XSecs[AdjEndIdx].KerbL.y;
            for (int i = AdjStartIdx + 1; i <= AdjEndIdx; i++)
            {
                TotAdjDist += Vector2.Distance(Convert2d(Road.Instance.XSecs[i].KerbL), Convert2d(Road.Instance.XSecs[i - 1].KerbL));
            }
            float delta = 0;
            for (int i = AdjStartIdx + 1; i <= AdjEndIdx; i++)
            {
                delta += Vector2.Distance(Convert2d(Road.Instance.XSecs[i].KerbL), Convert2d(Road.Instance.XSecs[i - 1].KerbL));
                float NewY = Mathf.SmoothStep(AdjStarty, AdjEndy, delta / TotAdjDist);
                float AdjY = NewY - Road.Instance.XSecs[i].KerbL.y;
                XSec X = new XSec(i);
                X.KerbL = new Vector3(0, AdjY, 0);
                Road.Instance.XSecs[i].Adjust(X);
            }
        }
        if (Hand == "R")
        {
            AdjStarty = Road.Instance.XSecs[AdjStartIdx].KerbR.y;
            AdjEndy = Road.Instance.XSecs[AdjEndIdx].KerbR.y;
            for (int i = AdjStartIdx + 1; i <= AdjEndIdx; i++)
            {
                TotAdjDist += Vector2.Distance(Convert2d(Road.Instance.XSecs[i].KerbR), Convert2d(Road.Instance.XSecs[i - 1].KerbR));
            }
            float delta = 0;
            for (int i = AdjStartIdx + 1; i <= AdjEndIdx; i++)
            {
                delta += Vector2.Distance(Convert2d(Road.Instance.XSecs[i].KerbR), Convert2d(Road.Instance.XSecs[i - 1].KerbR));
                float NewY = Mathf.SmoothStep(AdjStarty, AdjEndy, delta / TotAdjDist);
                float AdjY = NewY - Road.Instance.XSecs[i].KerbR.y;
                XSec X = new XSec(i);
                X.KerbR = new Vector3(0, AdjY, 0);
                Road.Instance.XSecs[i].Adjust(X);
            }
        }
    }

    public static SegVerts SegmentVertices(int Idx)
    {
        SegVerts rtn = new SegVerts();
        try
        {
            if (Road.Instance.IsCircular && Idx==Road.Instance.XSecs.Count-1)
            {
                rtn.MidPtF = Road.Instance.XSecs[0].MidPt;
                rtn.KerbFL = Road.Instance.XSecs[0].KerbL;
                rtn.KerbFR = Road.Instance.XSecs[0].KerbR;
                rtn.TerrainFL = Road.Instance.XSecs[0].TerrainL;
                rtn.TerrainFR = Road.Instance.XSecs[0].TerrainR;
            }
            else
            {
                rtn.MidPtF = Road.Instance.XSecs[Idx + 1].MidPt;
                rtn.KerbFL = Road.Instance.XSecs[Idx+1].KerbL;
                rtn.KerbFR = Road.Instance.XSecs[Idx+1].KerbR;
                rtn.TerrainFL = Road.Instance.XSecs[Idx+1].TerrainL;
                rtn.TerrainFR = Road.Instance.XSecs[Idx+1].TerrainR;
            }
            rtn.MidPtB = Road.Instance.XSecs[Idx].MidPt;
            rtn.KerbBL = Road.Instance.XSecs[Idx].KerbL;
            rtn.KerbBR = Road.Instance.XSecs[Idx].KerbR;
            rtn.TerrainBL = Road.Instance.XSecs[Idx].TerrainL;
            rtn.TerrainBR = Road.Instance.XSecs[Idx].TerrainR;

        }
        catch (System.Exception e)
        {
            Debug.Log("Error " + e.ToString());
        }
        return rtn;

    }

    private static void PlaceMarker(Vector3 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        marker.transform.position = Pos;
    }

    private static void PlaceBigMarker(Vector3 Pos, string Name = "Marker")
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.name = Name;
        //marker.transform.localScale = new Vector3(f, 0.2f, 0.2f);
        marker.transform.position = Pos;
    }

    private static void PlaceMarker(Vector2 Pos)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marker.transform.localScale = new Vector3(0.3f, 20f, 0.3f);
        marker.transform.position = new Vector3(Pos.x, 10f, Pos.y);
    }
}