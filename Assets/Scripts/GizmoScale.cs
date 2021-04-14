using System;
using UnityEngine;

/// <summary>
/// Inherits from aGizmo whichi inherits from Monobehavoiur
/// Attached as a component to the GizmoRotate prefab
/// </summary>
public class GizmoScale : aGizmo
{
    Vector3 GizmoStartPos;
    Vector3 GizmoStartScale;
    Vector3 AttachedTransformStartScale;
    float Dist;
    string _axis;
    int _axisSign; //this is +1 or -1
    Plane DragPlane;
    Vector3 _horiz;
    float _mouseAngleOffset;
    Vector3 _mouseStartPos;
    float d;
    Ray ray;
    Quaternion LerpStartRot;
    Quaternion LerpEndRot;
    Vector3 LerpStartPt;
    Vector3 LerpEndPt;
    float LerpStartTime;

    public override void SetPivotPos()
    {

    }

    void OnMouseDown()
    {

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
        Debug.Log(_axisSign.ToString() + _axis);
        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
        GizmoStartPos = transform.position;
        GizmoStartScale = transform.localScale;
        AttachedTransformStartScale = AttachedTransform.localScale;
        ray = FPCam.ScreenPointToRay(Input.mousePosition);
        float entry;
        if (_axis == "x")
        {
            DragPlane = new Plane(transform.forward, transform.position);   //dragplane is an xy plane
        }
        if (_axis == "y")
        {
            DragPlane = new Plane(transform.right, transform.position); //dragplane is an zy plane
        }
        if (_axis == "z")
        {
            DragPlane = new Plane(transform.up, transform.position);    //dragplane is an xz plane
        }

        DragPlane.Raycast(ray, out entry);
        d = entry;
        _mouseStartPos = ray.GetPoint(d);
    }

    void OnMouseDrag()
    {
        ray = FPCam.ScreenPointToRay(Input.mousePosition);
        float entry;
        DragPlane.Raycast(ray, out entry);
        d = entry;
        Vector3 _mouseDragVector = (ray.GetPoint(d) - _mouseStartPos) * _axisSign;

        if (_axis == "x")
        {
            float _mouseDragDist = Vector3.Dot(_mouseDragVector, transform.right);
            AttachedTransform.localScale = AttachedTransformStartScale + Vector3.right * _mouseDragDist * 0.1f;
        }
        if (_axis == "y")
        {
            float _mouseDragDist = Vector3.Dot(_mouseDragVector, transform.up);
            AttachedTransform.localScale = AttachedTransformStartScale + Vector3.up * _mouseDragDist * 0.1f;
        }
        if (_axis == "z")
        {
            float _mouseDragDist = Vector3.Dot(_mouseDragVector, transform.forward);
            AttachedTransform.localScale = AttachedTransformStartScale + Vector3.forward * _mouseDragDist * 0.1f;
        }
    }

    void OnMouseUp()
    {
        //transform.parent = AttachedTransform;
        AttachedObject.Scale = AttachedTransform.localScale;
        //transform.localScale = new Vector3(1 / AttachedObject.Scale.x, 1 / AttachedObject.Scale.y, 1 / AttachedObject.Scale.z);
        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = false;
        Game.current.Dirty = true;
        FireGizmoMovedEvent();
    }


    /*
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
    */
}

