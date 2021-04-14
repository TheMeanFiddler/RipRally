using System;
using UnityEngine;

/// <summary>
/// Inherits from aGizmo whichi inherits from Monobehavoiur
/// Attached as a component to the GizmoMove prefab
/// </summary>
    public class GizmoMove: aGizmo
    {
        Vector3 ObjStartPos;
        float Dist;
        string _dragDirection;
        Plane DragPlane;
        Vector3 _horiz;
        Vector3 _mouseOffset;
        Ray r;
        float d;
        Ray ray;
        bool Lerping;
        Quaternion LerpStartRot;
        Quaternion LerpEndRot;
        Vector3 LerpStartPt;
        Vector3 LerpEndPt;
        float LerpStartTime;
        bool MouseDown = false;
        Vector2 CentreScreen = new Vector2(Screen.width / 2, Screen.height / 2);

    //create a plane that faces the camera
    //Vector3 p = FPCam.transform.InverseTransformPoint(transform.position);
    //_horiz = new Vector3(p.x, 0, p.z);
    //_horiz.Normalize();
    //_yDragPlane = new Plane(_horiz, transform.position);

    public override void SetPivotPos()
    {
        _pivotPos = new Vector3(_hoverPos.x, AttachedTransform.position.y, _hoverPos.z);
    }

    void OnMouseDown()
        {
        float camdist = Vector3.Distance(FPCam.transform.parent.position, transform.position);
            MouseDown = true;
            FPCam.transform.parent.GetComponent<RoadBuilder>().Dragging = true;
            RaycastHit hit;
            ray = FPCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.bounds.extents.y < 1) _dragDirection = "xz"; else _dragDirection = "y";
            }
            FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
            ObjStartPos = AttachedTransform.position;
            if (_dragDirection == "xz")
            {
            LerpEndPt = AttachedTransform.position + _hoverPos + Vector3.up * camdist;
            }
            else
            {
                //WOrk out where to lerp the cam to
                Vector3 Offset = (FPCam.transform.position - transform.position).normalized;
            LerpEndPt = FPCam.transform.position; // transform.position + Offset * _hoverHeight*3;
            }
            LerpStartRot = FPCam.transform.rotation;
            LerpStartPt = FPCam.transform.position;
            LerpStartTime = Time.time;
            Lerping = true;
            Debug.Log("Lerp to " + LerpEndPt);
            AttachedObject.DisableClickColliders();
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
                newpos = r.GetPoint(d) - _hoverPos - _mouseOffset;
                if (Mathf.Abs(Input.mousePosition.x - Screen.width / 2) > Screen.width * 0.45f
                || Mathf.Abs(Input.mousePosition.y - Screen.height / 2) > Screen.height * 0.45f)
                {
                    Vector2 Dir = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - CentreScreen).normalized;
                    Vector3 FPCamPos = FPCam.transform.position;
                    FPCam.transform.parent.Translate(new Vector3(Dir.x, Dir.y, 0), FPCam.transform);
                    Vector3 TranslationIncrement = FPCam.transform.position - FPCamPos;
                    LerpStartPt += TranslationIncrement;
                }
            }
            else
            {
                DragPlane.Raycast(r, out entry);
                d = entry;
                newpos = new Vector3(ObjStartPos.x, r.GetPoint(d).y - _hoverPos.y - _mouseOffset.y, ObjStartPos.z);
            }
            AttachedTransform.position = newpos; // -Vector3.up * _hoverHeight;
        }

        void OnMouseUp()
        {
            MouseDown = false;
            if (!Lerping)
            {
                AttachedObject.Pos = AttachedTransform.position;
                transform.Find("YArrow").GetComponent<Renderer>().enabled = true;
                transform.Find("XZPlane").GetComponent<Renderer>().enabled = true;
                Game.current.Dirty = true;
                FireGizmoMovedEvent();
            }
            LerpEndPt = LerpStartPt;
            LerpStartPt = FPCam.transform.position;
            LerpStartTime = Time.time;
            Lerping = true;
            AttachedObject.EnableClickColliders();
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
                            Vector3 p = FPCam.transform.InverseTransformPoint(transform.position);
                            _horiz = new Vector3(p.x, 0, p.z);
                            _horiz.Normalize();
                            DragPlane = new Plane(_horiz, transform.position);

                        }
                        DragPlane.Raycast(ray, out entry);
                        d = entry;
                        _mouseOffset = ray.GetPoint(d) - transform.position;
                    }
                    else
                        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = false;
                }
            }
        }

}

