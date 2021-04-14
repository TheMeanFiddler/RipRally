using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class SuspensionBridge : aSceneryComponent
{
    Transform _model;
    StringBuilder sb;
    List<Transform> LCables = new List<Transform>();
    List<Transform> RCables = new List<Transform>();
    List<Vector3> LCablePoses = new List<Vector3>();
    List<Vector3> RCablePoses = new List<Vector3>();

    public override void Init(PlaceableObject p)
    {
        _model = transform.Find("SuspensionBridgeModel");
        //List all the cables and their local positions
        for (int i = 0; i < 15; i++)
        {
            sb = new StringBuilder("CableL", 8);
            string c = sb.Append((14 - i).ToString()).ToString();
            Transform Cable = _model.Find(c);
            LCables.Add(Cable);
            LCablePoses.Add(Cable.localPosition);
            Cable.SetParent(null);
            sb = new StringBuilder("CableR", 8);
            c = sb.Append((14 - i).ToString()).ToString();
            Cable = _model.Find(c);
            RCables.Add(Cable);
            RCablePoses.Add(Cable.localPosition);
            Cable.SetParent(null);

        }
            FenceCables();
        _placeableObj = p;
        _placeableObj.OnGizmoMoved += FenceCables;
    }

    public void FenceCables()
    {
        Ray r;
        RaycastHit hit;
        int SegIdx;
        sb = new StringBuilder("Cable", 8);
        r = new Ray(_model.Find("RayCaster").position, Vector3.down);
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.name.StartsWith("RoadSeg"))
            {
                SegIdx = System.Convert.ToInt16(hit.collider.name.Substring(7));
                foreach (Transform Cable in LCables)
                {
                    int idx = LCables.IndexOf(Cable);
                    Vector3 kerbL = Road.Instance.XSecs[SegIdx + 12 + idx * 6].KerbL;
                    Cable.position = transform.TransformPoint(LCablePoses[idx]);
                    float Dist = Vector3.Distance(Cable.position, kerbL);
                    Cable.localScale = new Vector3(1, 1, Dist);
                    Cable.LookAt(kerbL);
                }
                foreach (Transform Cable in RCables)
                {
                    int idx = RCables.IndexOf(Cable);
                    Vector3 kerbR = Road.Instance.XSecs[SegIdx + 12 + idx*6].KerbR;
                    Cable.position = transform.TransformPoint(RCablePoses[idx]);
                    float Dist = Vector3.Distance(Cable.position, kerbR);
                    Cable.localScale = new Vector3(1, 1, Dist);
                    Cable.LookAt(kerbR);
                }
            }
            else
            {
                foreach(Transform Cable in LCables)
                {
                    Cable.localScale = Vector3.zero;
                }
                foreach (Transform Cable in RCables)
                {
                    Cable.localScale = Vector3.zero;
                }
            }
        }
    }


    void PlaceMarker(Vector3 Pt)
    {
        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        marker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        marker.transform.position = Pt;
    }

    void OnDestroy()
    {
        _placeableObj.OnGizmoMoved -= FenceCables;

    }
}

