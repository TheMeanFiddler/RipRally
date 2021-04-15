using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public static class VectGeom
{
    public static bool ContainsPoint(Vector2[] PolyPoints, Vector2 p)
    {
        int j = PolyPoints.Length - 1;
        bool inside = false;
        for (int i = 0; i < PolyPoints.Length; i++)
        {
            if ((PolyPoints[i].y < p.y && PolyPoints[j].y >= p.y || PolyPoints[j].y < p.y && PolyPoints[i].y >= p.y) &&
                (PolyPoints[i].x + (p.y - PolyPoints[i].y) / (PolyPoints[j].y - PolyPoints[i].y) * (PolyPoints[j].x - PolyPoints[i].x) < p.x))
                inside = !inside;


            /*
            if (((PolyPoints[i].y <= p.y && p.y < PolyPoints[j].y) || (PolyPoints[j].y <= p.y && p.y < PolyPoints[i].y)) &&
               (p.x < (PolyPoints[j].x - PolyPoints[i].x) * (p.y - PolyPoints[i].y) / (PolyPoints[j].y - PolyPoints[i].y) + PolyPoints[i].x))
                inside = !inside;
            */
            j = i;
        }
        return inside;
    }

    static float distsq(Vector2 v, Vector2 w)
    {
        return (v.x - w.x) * (v.x - w.x) + (v.y - w.y) * (v.y - w.y);
    }


    public static float DistPointToLine2D(Vector2 p, Vector2 v, Vector2 w)
    {
        // Return minimum distance between line segment vw and point p
        float l2 = distsq(v, w);  // i.e. |w-v|^2 -  avoid a sqrt
        if (l2 == 0.0) return Vector2.Distance(p, v);   // v == w case
        // Consider the line extending the segment, parameterized as v + t (w - v).
        // We find projection of point p onto the line. 
        // It falls where t = [(p-v) . (w-v)] / |w-v|^2
        float t = Vector2.Dot(p - v, w - v) / l2;
        if (t < 0.0) return Vector2.Distance(p, v);       // Beyond the 'v' end of the segment
        else if (t > 1.0) return Vector2.Distance(p, w);  // Beyond the 'w' end of the segment
        Vector2 projection = v + t * (w - v);  // Projection falls on the segment
        return Vector2.Distance(p, projection);
    }

    public static float DistancePointToLine(Vector3 LineStart, Vector3 LineEnd, Vector3 point)
    {
        Vector3 Direction = (LineEnd - LineStart).normalized;
        return Vector3.Cross(Direction, point - LineStart).magnitude;
    }



    public static Vector3 InversePlan(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector2 Convert2d(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    /// <summary>
    /// Find the intersection point(out Intersection param)
    /// From http://wiki.unity3d.com/index.php/3d_Math_functions
    /// </summary>
    /// <param name="intersection"></param>
    /// <param name="linePoint1"></param>
    /// <param name="lineVec1"></param>
    /// <param name="linePoint2"></param>
    /// <param name="lineVec2"></param>
    /// <returns>false if they dont intersect</returns>
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {
        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

    public static bool LinesIntersect2d(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;
        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;
        bool doIntersect = true;
        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {
            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }
            if (betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;


    }

    public static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion Quat)
    {
        return Quat * (point - pivot) + pivot;
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }


    //linePnt - point the line passes through
    //lineDir - unit vector in direction of line, either direction works
    //pnt - the point to find nearest on line for
    public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
    {
        lineDir.Normalize();//this needs to be a unit vector
        var v = pnt - linePnt;
        var d = Vector3.Dot(v, lineDir);
        return linePnt + lineDir * d;
    }

    /// <summary>
    /// Calculates an angle between three points in relation a normal.
    /// </summary>
    /// <param name="p1">First point.</param>
    /// <param name="p2">common point.</param>
    /// <param name="p3">Second point.</param>
    /// <param name="n">normal</param>
    /// <returns>Angle in degrees.</returns>
    public static float SignedAngle(Vector3 Pt1, Vector3 Pt2, Vector3 Pt3, Vector3 n)
    {
        return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(Pt2 - Pt1, Pt2 - Pt3)), Vector3.Dot(Pt2 - Pt1, Pt2 - Pt3)) * Mathf.Rad2Deg;
    }

    //Unity Vector Geometry Library functions 
    //https://wiki.unity3d.com/index.php/3d_Math_functions

    /// <summary>
    /// Signed angle between 2 vectors
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static float SignedAngle(Vector3 v1, Vector3 v2)
    {
        float rtn = Vector3.Angle(v1, v2);
        if (Vector3.Cross(v1, v2).y < 0) rtn = -rtn;
        return rtn;
    }

    public static Vector3 PerpNrm(Vector3 v1, Vector3 v2)
    {
        return Vector3.Cross(v1, v2).normalized;
    }

    public static Vector3 Reciprocal(Vector3 v3)
    {
        return new Vector3(1 / v3.x, 1 / v3.y, 1 / v3.z);
    }

    public static void CircleFrom3Pts(Vector3 p1, Vector3 p2, Vector3 p3, out float Rad, out Vector3 Centre)
    {
        Centre = p1;
        Vector3 Perp12 = Convert2d(Vector3.Cross(p1, p2));
        Vector3 Perp23 = Convert2d(Vector3.Cross(p2, p3));
        Vector2 q1 = Convert2d(p1);
        Vector2 q2 = Convert2d(p2);
        Vector2 q3 = Convert2d(p3);
        Vector2 Mid12 = (q1 + q2) / 2;
        Vector2 Mid23 = (q2 + q3) / 2;
        Vector3 Intersection;
        if (LineLineIntersection(out Intersection, Mid12, Perp12, Mid23, Perp23))
        {
            Centre = new Vector3(Intersection.x, 0, Intersection.y);
        }
        Rad = Vector3.Distance(p1, Centre);
    }


}

