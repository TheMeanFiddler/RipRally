using UnityEngine;

class GizmoBank:MonoBehaviour
{
    Transform AttachedTransform;
    Camera FPCam;
    float _startAngle;
    Quaternion _startRot;
    bool _viewFromFront;
    [SerializeField]
    string _hand;
    [SerializeField]
    Transform _oppositeGizmoBank;
    Vector3 _pivot;
    Vector3 _horiz;
    float _mouseOffsetAngle;
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
        //transform.LookAt(Road.Instance.XSecs[XSecId+1].MidPt);
        //transform.Rotate(Vector3.forward, Bez.CtrlPts[Mrkr.Index].BankAngle);
    }

    void OnMouseDown()
    {
        MouseDown = true;
        _rb.Dragging = true;
        if (_hand == "L")
            _pivot = Road.Instance.XSecs[Bez.CtrlPts[Mrkr.Index].SegStartIdx].MidPt;
        else
            _pivot = Road.Instance.XSecs[Bez.CtrlPts[Mrkr.Index].SegStartIdx].MidPt;
        FPCam.transform.parent.GetComponent<BuilderCamController>().FreezeTilt = true;
        _startAngle = Bez.CtrlPts[Mrkr.Index].BankAngle;
        _startRot = transform.localRotation;

        //Decide where to move the camera to... This is the nearest point perpendicular to the road and 30m from it
        Vector3 Perp = AttachedTransform.forward;
        Plane p = new Plane(Perp, AttachedTransform.position);
        _viewFromFront = p.GetSide(FPCam.transform.position);
        if (_viewFromFront)
            LerpEndPt = AttachedTransform.position + Perp * 30 + Vector3.up * 3;
        else
            LerpEndPt = AttachedTransform.position - Perp * 30 + Vector3.up * 3;

        LerpStartRot = FPCam.transform.rotation;
        LerpStartPt = FPCam.transform.position;
        LerpStartTime = Time.time;
        Lerping = true;
    }

    void OnMouseDrag()
    {
        if (Lerping) return;
        float NewAngle;
        float MouseAngle = Mathf.Atan2(Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2) * Mathf.Rad2Deg;
        //Signed angle measured anticlockwise from the right
        MouseAngle = MouseAngle - _mouseOffsetAngle;
        if (MouseAngle > 180) MouseAngle = MouseAngle - 360;
        if (MouseAngle < -180) MouseAngle = MouseAngle + 360;
        if (_viewFromFront)
        {
            MouseAngle = -MouseAngle;
        }

        NewAngle = _startAngle + MouseAngle;
        if (_hand == "R")
        {
            if (NewAngle > 0)
            { transform.localRotation = Quaternion.Euler(0, 0, NewAngle); _oppositeGizmoBank.localRotation = Quaternion.identity; }
            else
            { _oppositeGizmoBank.localRotation = Quaternion.Euler(0, 0, NewAngle); transform.localRotation = Quaternion.identity; }
        }
        else
        {
            if (NewAngle > 0)
            { _oppositeGizmoBank.localRotation = Quaternion.Euler(0, 0, NewAngle); transform.localRotation = Quaternion.identity; }
            else
            { transform.localRotation = Quaternion.Euler(0, 0, NewAngle); _oppositeGizmoBank.localRotation = Quaternion.identity; }
        }
        Bez.CtrlPts[Mrkr.Index].BankAngle = NewAngle;
        //transform.RotateAround(_pivot, AttachedTransform.forward, AngleIncrement);
        //Bez.CtrlPts[Mrkr.Index].Pos = AttachedTransform.position;
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
                if (_hand == "xz") //for xz we are looking down in it so we need to say which way is up
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
                    _mouseOffsetAngle = Mathf.Atan2(Input.mousePosition.y - Screen.height / 2, Input.mousePosition.x - Screen.width / 2) * Mathf.Rad2Deg;
                    Debug.Log(_mouseOffsetAngle);
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

