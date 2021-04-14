using System;
using UnityEngine;

/// <summary>
/// Doesn't Inherit from aGizmo because its quite a different animal
/// Attached as a component to the Roadmarker's GizmoTilt prefab
/// </summary>
public class RoadMarkerGizmoTilt_SingleArrowNotUsed: MonoBehaviour
    {
        Transform AttachedTransform;
        Camera FPCam;
        float _startAngle;
        Vector3 _untiltedPos;
        Vector3 _otherHandStartPos;
        string _hand;
        Transform OtherHandGizmo;
        float Dist;
        BoxCollider Collider;
        Vector3 _horiz;
        float _mouseOffsetY;
        float _hoverHeight = 8;
        Ray r;
        float d;
        Ray ray;
        BezierLine Bez;
        RoadMarker Mrkr;
        int XSecId;
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
            XSecId = Bez.CtrlPts[Mrkr.Index].SegStartIdx;
            
            if (name == "LeftTiltArrow")
            {
                _hand = "L";
                //OtherHandGizmo = Mrkr.GizmoTiltRight.transform;
            }
            else
            {
                _hand = "R";
                //OtherHandGizmo = Mrkr.GizmoTiltLeft.transform;
            }
            _otherHandStartPos = OtherHandGizmo.position;
        }

        void OnMouseDown()
        {
            MouseDown = true;
            _rb.Dragging = true;
            FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
            _startAngle = Bez.CtrlPts[Mrkr.Index].BankAngle * Mathf.Deg2Rad;
            Vector3 _bankDirection = AttachedTransform.forward * _startAngle;
            _untiltedPos =  VectGeom.RotatePointAroundPivot(transform.position, OtherHandGizmo.position, -_bankDirection);
            Debug.Log("Start angle = " + Bez.CtrlPts[Mrkr.Index].BankAngle);
            Debug.Log("UntiltedPos = " + _untiltedPos);

                //Decide where to move the camera to... This is the nearest point perpendicular to the road and 30m from it
                Vector2 Perp2d = XSecCalculator.Perpndclr((AttachedTransform.GetComponent<RoadMarker>().Index - 1) * 20);
                Vector3 Perp = new Vector3(Perp2d.x, 0, Perp2d.y);
                Vector3 Dest1 = AttachedTransform.position + Perp * 30;
                Vector3 Dest2 = AttachedTransform.position - Perp * 30;
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
            if (_hand == "L")
                NewAngle = _startAngle + Mathf.Atan2(MouseY, Screen.width / 2);
            else
                NewAngle = _startAngle - Mathf.Atan2(MouseY, Screen.width/2);
            NewAngle = NewAngle * Mathf.Rad2Deg;
            //Debug.Log("New angle = " + NewAngle);
            Vector3 NewPos;
            Vector3 _bankDirection = AttachedTransform.forward * NewAngle;
            NewPos = VectGeom.RotatePointAroundPivot(_untiltedPos, OtherHandGizmo.position, _bankDirection);
            Bez.CtrlPts[Mrkr.Index].Pos = (NewPos + _otherHandStartPos) / 2;
            AttachedTransform.position = Bez.CtrlPts[Mrkr.Index].Pos;
            Bez.CtrlPts[Mrkr.Index].BankAngle = NewAngle;
            Bez.Interp(Mrkr.Index - 1);
            Bez.Interp(Mrkr.Index);
            Bez.Interp(Mrkr.Index + 1);
            Bez.DrawLine();
            AttachedTransform.rotation = Quaternion.identity;
            try
            {
                AttachedTransform.LookAt(BezierLine.Instance.Path[Bez.CtrlPts[Mrkr.Index].SegStartIdx + 1]);
                AttachedTransform.Rotate(Vector3.forward, NewAngle);
            }
            catch (System.Exception e)
            {//Todo Find something for it to look at
            }
            transform.position = NewPos;
            OtherHandGizmo.position = _otherHandStartPos;
            transform.LookAt(OtherHandGizmo, AttachedTransform.up);
            OtherHandGizmo.LookAt(transform, AttachedTransform.up);
        }

        void OnMouseUp()
        {
            MouseDown = false;
            if (!Lerping)
            {
                Mrkr.DropMarker();
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

