using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Doesn't Inherit from aGizmo because its quite a different animal
/// Attached as a component to the Roadmarker's GizmoMove prefab
/// </summary>
public class RoadMarkerGizmo: MonoBehaviour
    {
        Transform AttachedTransform;
        Camera FPCam;
        Vector3 GizmoStartPos;
        float Dist;
        string _dragDirection;
        Plane DragPlane;
        Vector3 _horiz;
        Vector3 _mouseOffset;
        float _hoverHeight = 8;
        Ray r;
        float d;
        Ray ray;
        BezierLine Bez;
        RoadMarker Mrkr;
        bool Lerping;
        Vector3 LerpStartPt;
        Vector3 LerpEndPt;
        Quaternion LerpStartRot;
        Quaternion LerpEndRot;
        float LerpStartTime;
        bool MouseDown = false;
        RoadBuilder _rb;

        void Start()
        {
            FPCam = GameObject.Find("BuilderCamera").GetComponent<Camera>();
            _rb = GameObject.Find("BuilderPlayer(Clone)").GetComponent<RoadBuilder>();
            AttachedTransform = transform.parent;
            Bez = BezierLine.Instance;
            Mrkr = AttachedTransform.GetComponent<RoadMarker>();
    }

        void OnMouseDown()
        {
            MouseDown = true;
            _rb.Dragging = true;
            RaycastHit hit;
            ray = FPCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.bounds.extents.y * AttachedTransform.localScale.y < 1) _dragDirection = "xz"; else _dragDirection = "y";
            }
            FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
            GizmoStartPos = transform.position;
            if (_dragDirection == "xz")
            {
                LerpEndPt = transform.position + Vector3.up * 40;
            }
            else
            {
                //Decide where to move the camera to... This is the nearest point perpendicular to the road and 30m from it
                Vector3 Perp = AttachedTransform.right;
                Vector3 Dest1 = AttachedTransform.position + Perp * 30;
                Vector3 Dest2 = AttachedTransform.position - Perp * 30;
                if (Vector3.Distance(FPCam.transform.position, Dest1) < Vector3.Distance(FPCam.transform.position, Dest2))
                    LerpEndPt = Dest1;
                else
                    LerpEndPt = Dest2;
            }
            LerpStartRot = FPCam.transform.rotation;
            LerpStartPt = FPCam.transform.position;
            LerpStartTime = Time.time;
            Lerping = true;
            Debug.Log("Lerp to " + LerpEndPt);
            //transform.LookAt(FPCam.transform);
        }
        void OnMouseDrag()
        {
            if (Lerping) return;
            r = FPCam.ScreenPointToRay(Input.mousePosition);
            float entry;
            Vector3 newpos;
            if (_dragDirection == "xz")
            {
                DragPlane.Raycast(r, out entry);
                d = entry;
                newpos = r.GetPoint(d) - _mouseOffset;
            }
            else
            {
                DragPlane.Raycast(r, out entry);
                d = entry;
                newpos = new Vector3(GizmoStartPos.x, r.GetPoint(d).y - _mouseOffset.y, GizmoStartPos.z);
            }
            AttachedTransform.position = newpos - AttachedTransform.up * _hoverHeight;
            if (Mrkr.Index == 1 && Road.Instance.IsCircular) {
                Bez.CtrlPts[Bez.CtrlPts.Count - 2].Pos = Mrkr.transform.position;
                //Bez.CtrlPts[Bez.CtrlPts.Count - 2].goRdMkr.transform.position = Mrkr.transform.position;
                Bez.Interp(Bez.CtrlPts.Count - 2);
            }
            Bez.CtrlPts[Mrkr.Index].Pos = Mrkr.transform.position;
            Bez.Interp(Mrkr.Index - 1);
            Bez.Interp(Mrkr.Index);
            Bez.Interp(Mrkr.Index + 1);
            //Bez.DrawLine();
        }

        void OnMouseUp()
        {
            MouseDown = false;
            if (!Lerping)
            {
                Mrkr.DropMarker();
                transform.Find("YArrow").GetComponent<Renderer>().enabled = true;
                transform.Find("XZPlane").GetComponent<Renderer>().enabled = true;
            }
            LerpEndPt = LerpStartPt;
            LerpStartPt = FPCam.transform.position;
            LerpStartTime = Time.time;
            Lerping = true;
        }

        void Update()
        {
            if (Lerping)
            {
                Vector3 NewPos = Vector3.Slerp(LerpStartPt, LerpEndPt, (Time.time - LerpStartTime));
                FPCam.transform.position = NewPos;
                if (MouseDown)  //we're moving towards the marker
                {   //Dont look straight at the marker but lerp the camera towards it
                    if (_dragDirection == "xz") //for xz we are looking down in it so we need to say which way is up
                        LerpEndRot = Quaternion.LookRotation(this.transform.position - FPCam.transform.position, this.transform.position - FPCam.transform.parent.position);
                    else
                        LerpEndRot = Quaternion.LookRotation(this.transform.position - FPCam.transform.position);
                }
                else  //we're returning towards the builder
                    LerpEndRot = LerpStartRot;

                Quaternion NewRot = Quaternion.Slerp(FPCam.transform.rotation, LerpEndRot, (Time.time - LerpStartTime));
                FPCam.transform.rotation = NewRot;
                if (Vector3.Distance(NewPos, LerpEndPt) < 0.02f)
                {
                    Lerping = false;
                    if (MouseDown)
                    {
                        RaycastHit hit;
                        float entry;
                        ray = FPCam.ScreenPointToRay(Input.mousePosition);
                        Physics.Raycast(ray, out hit);
                        if (_dragDirection == "xz")
                        {
                            //create a horizontal plane 
                            DragPlane = new Plane(Vector3.up, transform.position);
                        }
                        else
                        {
                            //create a plane that faces the camera
                            Vector3 p = FPCam.transform.position - transform.position;
                            _horiz = new Vector3(p.x, 0, p.z);
                            _horiz.Normalize();
                            DragPlane = new Plane(_horiz, transform.position);

                        }
                        DragPlane.Raycast(ray, out entry);
                        d = entry;
                        _mouseOffset = ray.GetPoint(d) - transform.position;
                    }
                    else
                    {
                        FPCam.transform.localPosition = Vector3.zero;
                        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = false;
                    }
                }
            }
        }

    }

