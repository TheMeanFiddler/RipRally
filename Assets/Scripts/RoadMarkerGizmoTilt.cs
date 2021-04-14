using System;
using UnityEngine;

/// <summary>
/// Doesn't Inherit from aGizmo because its quite a different animal
/// Attached as a component to the Roadmarker's GizmoTilt prefab
/// </summary>
public class RoadMarkerGizmoTilt: MonoBehaviour
    {
        Transform AttachedTransform;
        Camera FPCam;
        float _startAngle;
        Quaternion _startRot;
        Vector3 _startPos;
        float Dist;
        BoxCollider ColliderL;
        BoxCollider ColliderR;
        string _dragHandle;
        Vector3 _pivot;
        Vector3 _horiz;
        float _mouseOffsetY;
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


        public void Init()
        {
            FPCam = GameObject.Find("BuilderCamera").GetComponent<Camera>();
            _rb = GameObject.Find("BuilderPlayer(Clone)").GetComponent<RoadBuilder>();
            AttachedTransform = transform.parent;
            Bez = BezierLine.Instance;
            Mrkr = AttachedTransform.GetComponent<RoadMarker>();
            int XSecId = Bez.CtrlPts[Mrkr.Index].SegStartIdx;
            Transform trl = transform.Find("LeftTiltArrow");
            Transform trr = transform.Find("RightTiltArrow");
            //transform.LookAt(Road.Instance.XSecs[XSecId+1].MidPt);
            //transform.Rotate(Vector3.forward, Bez.CtrlPts[Mrkr.Index].BankAngle);
        }

        void OnMouseDown()
        {
            MouseDown = true;
            _rb.Dragging = true;
            RaycastHit hit;
            ray = FPCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (transform.InverseTransformPoint(hit.collider.bounds.center).x < 1) _dragHandle = "L"; else _dragHandle = "R";
            }
            if (_dragHandle == "L") 
                _pivot = Road.Instance.XSecs[Bez.CtrlPts[Mrkr.Index].SegStartIdx].KerbR;
            else 
                _pivot = Road.Instance.XSecs[Bez.CtrlPts[Mrkr.Index].SegStartIdx].KerbL;
            FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
            _startAngle = Bez.CtrlPts[Mrkr.Index].BankAngle * Mathf.Deg2Rad;
            Debug.Log("Start angle = " + Bez.CtrlPts[Mrkr.Index].BankAngle + "\n_dragHandle = " + _dragHandle);
            _startRot = AttachedTransform.rotation;
            _startPos = AttachedTransform.position;

                //Decide where to move the camera to... This is the nearest point perpendicular to the road and 30m from it
                Vector3 Perp = AttachedTransform.forward;
                Vector3 Dest1 = AttachedTransform.position + Perp * 30 + Vector3.up * 3;
                Vector3 Dest2 = AttachedTransform.position - Perp * 30 + Vector3.up * 3;
                if (Vector3.Distance(FPCam.transform.position, Dest1) < Vector3.Distance(FPCam.transform.position, Dest2))
                    LerpEndPt = Dest1;
                else
                    LerpEndPt = Dest2;

            LerpStartRot = FPCam.transform.rotation;
            LerpStartPt = FPCam.transform.position;
            LerpStartTime = Time.time;
            Lerping = true;
            //Debug.Log("Lerp to " + LerpEndPt);
        }
        void OnMouseDrag()
        {
            if (Lerping) return;
            float MouseY = Input.mousePosition.y - _mouseOffsetY;
            float NewAngle;
            float AngleIncrement;
            if (_dragHandle == "L")
            {
                AngleIncrement = -Mathf.Atan2(MouseY, Screen.width / 2) * Mathf.Rad2Deg;
            }
            else
            {
                AngleIncrement = Mathf.Atan2(MouseY, Screen.width / 2) * Mathf.Rad2Deg;
            }
            NewAngle = _startAngle + AngleIncrement;
            Bez.CtrlPts[Mrkr.Index].BankAngle = NewAngle;
            //Rotate the marker - We do this by rotating the transform about the kerb pivot
            Vector3 _markerPos = Bez.CtrlPts[Mrkr.Index].Pos;
            AttachedTransform.position = _startPos;
            AttachedTransform.rotation = _startRot;
            AttachedTransform.RotateAround(_pivot, AttachedTransform.forward, AngleIncrement);    //this function was depreciated but I really struggled to do it any other way
            Bez.CtrlPts[Mrkr.Index].Pos = AttachedTransform.position;
            Bez.Interp(Mrkr.Index - 1);
            Bez.Interp(Mrkr.Index);
            Bez.Interp(Mrkr.Index + 1);
            Bez.DrawLine();
        }

        void OnMouseUp()
        {
            MouseDown = false;
            if (!Lerping)
            {
                Mrkr.DropMarker();
                Debug.Log("DropMarker");
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
                    if (_dragHandle == "xz") //for xz we are looking down in it so we need to say which way is up
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
                        _mouseOffsetY = Input.mousePosition.y;
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

