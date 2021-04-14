using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MeshTest: MonoBehaviour
{
    GameObject Part;
    MeshCollider coll;
    List<Vector3> verts;
    List<int> tris;
    GameObject txtLbl;

    private void Start()
    {
        Part = GameObject.Find("RLWing");
        txtLbl = GameObject.Find("txtLbl");
        coll = Part.GetComponent<MeshCollider>();
        verts = Part.GetComponent<MeshFilter>().sharedMesh.vertices.ToList();
        tris = Part.GetComponent<MeshFilter>().sharedMesh.triangles.ToList();

        foreach (Vector3 v in verts)
        {
            GameObject lbl = (GameObject)GameObject.Instantiate(txtLbl, Part.transform);
            lbl.name = (verts.IndexOf(v)).ToString();
            lbl.transform.localPosition = v;
            lbl.GetComponent<TextMesh>().text = lbl.name;
        }
        //foreach(int tr in tris) {if (tr == 325) Debug.Log(tris.IndexOf(tr));}
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 LocalPt;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r,out hit))
            {
                LocalPt = Part.transform.InverseTransformPoint(hit.point);
                List<Vector3> vs = verts.OrderBy(v => (LocalPt - v).sqrMagnitude).ToList();
                for (int i = 0; i < 50; i++)
                {
                    Debug.Log(i + " " + vs[i]);
                }

            }
        }
    }

}

