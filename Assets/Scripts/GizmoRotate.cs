using System;
using UnityEngine;

/// <summary>
/// Inherits from aGizmo whichi inherits from Monobehavoiur
/// Attached as a component to the GizmoRotate prefab
/// </summary>
public class GizmoRotate : aGizmo
{
    Vector3 ObjStartPos;
    float Dist;
    string _axis;
    int _axisSign; //this is +1 or -1
    Plane DragPlane;
    Vector3 _horiz;
    float _mouseAngleOffset;
    Quaternion _startQuat;
    Vector3 GizmoLocalPos;
    Quaternion GizmoLocalRot;
    Vector3 AttachedTransformPosRelativeToGizmo;
    float d;
    Ray ray;
    bool Lerping;
    Quaternion LerpStartRot;
    Quaternion LerpEndRot;
    Vector3 LerpStartPt;
    Vector3 LerpEndPt;
    float LerpStartTime;
    bool MouseDown = false;


    public override void SetPivotPos()
    {
        _pivotPos = _hoverPos;
        GizmoLocalPos = transform.localPosition;
        GizmoLocalRot = transform.localRotation;
        AttachedTransformPosRelativeToGizmo = transform.InverseTransformPoint(AttachedTransform.position);
    }

    void OnMouseDown()
    {
        MouseDown = true;
        FPCam.transform.parent.GetComponent<RoadBuilder>().Dragging = true;
        RaycastHit hit;
        ray = FPCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (transform.InverseTransformPoint(hit.collider.bounds.center).x > 0.1) { _axis = "x"; _axisSign = 1; }
            if (transform.InverseTransformPoint(hit.collider.bounds.center).y > 0.1) { _axis = "y"; _axisSign = 1; }
            if (transform.InverseTransformPoint(hit.collider.bounds.center).z > 0.1) { _axis = "z"; _axisSign = 1; }
            if (transform.InverseTransformPoint(hit.collider.bounds.center).x < -0.1) { _axis = "x"; _axisSign = -1; }
            if (transform.InverseTransformPoint(hit.collider.bounds.center).y < -0.1) { _axis = "y"; _axisSign = -1; }
            if (transform.InverseTransformPoint(hit.collider.bounds.center).z < -0.1) { _axis = "z"; _axisSign = -1; }
        }
        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
        ObjStartPos = AttachedTransform.position;
        _startQuat = AttachedTransform.rotation;
        if (_axis == "x")
        {
            LerpEndPt = transform.position + transform.right * 20 * _axisSign;
        }
        if (_axis == "y")
        {
            LerpEndPt = transform.position + transform.up * 20 * _axisSign;
        }
        if (_axis == "z")
        {
            LerpEndPt = transform.position + transform.forward * 20 * _axisSign;
        }
        LerpStartRot = FPCam.transform.rotation;
        LerpStartPt = FPCam.transform.position;
        LerpStartTime = Time.time;
        Lerping = true;
        //transform.LookAt(FPCam.transform);
    }

    void OnMouseDrag()
    {
        if (Lerping) return;
        Vector2 Centre = new Vector2(Screen.width / 2, Screen.height / 2);
        float a = Vector2.Angle(-Vector2.up, (Vector2)Input.mousePosition - Centre);
        if (Input.mousePosition.x < Screen.width / 2) a = 360 - a;
        a = _mouseAngleOffset - a;
        if (a > 360) a = a - 360;
        if (a < 0) a = a + 360;
        if (_axis == "x")
        {
            transform.rotation = _startQuat;
            transform.Rotate(Vector3.right, a);
        }
        if (_axis == "y")
        {
            transform.rotation = _startQuat;
            transform.Rotate(Vector3.up, a);
        }
        if (_axis == "z")
        {
            transform.rotation = _startQuat;
            transform.Rotate(Vector3.forward, a);
        }
        Vector3 NewAttTranPos = transform.TransformPoint(AttachedTransformPosRelativeToGizmo);
        AttachedTransform.rotation = transform.rotation;
        AttachedTransform.position = NewAttTranPos;
        transform.localPosition = GizmoLocalPos;
        transform.localRotation = GizmoLocalRot;
    }

    void OnMouseUp()
    {
        MouseDown = false;
        if (!Lerping)
        {
            AttachedObject.Rot = transform.rotation;
            Game.current.Dirty = true;
            FireGizmoMovedEvent();
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
                if (_axis == "y") //for y we are looking down in it so we need to say which way is up
                    LerpEndRot = Quaternion.LookRotation(this.transform.position - FPCam.transform.position);
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
                if (!MouseDown)
                    FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = false;
                else
                {
                    Vector2 Centre = new Vector2(Screen.width / 2, Screen.height / 2);
                    _mouseAngleOffset = Vector2.Angle(-Vector2.up, (Vector2)Input.mousePosition - Centre);
                    if (Input.mousePosition.x < Screen.width / 2) _mouseAngleOffset = 360 - _mouseAngleOffset;
                    if (_mouseAngleOffset > 360) _mouseAngleOffset = _mouseAngleOffset - 360;
                    if (_mouseAngleOffset < 0) _mouseAngleOffset = _mouseAngleOffset + 360;
                }
            }
        }
    }
}

