using UnityEngine;

public delegate void GizmoEventHandler();

/// <summary>
/// Abstract class inherits from Monobehaviour
/// Extended by GizmoMove, GizmoScale, GizmoRotate
/// </summary>
public abstract class aGizmo : MonoBehaviour
{
    public PlaceableObject AttachedObject { get; set; }
    public Transform AttachedTransform;
    protected float _hoverHeight = 3;
    protected Vector3 _hoverPos;
    protected Vector3 _pivotPos;
    protected Camera FPCam;
    public event GizmoEventHandler OnGizmoMoved;

    void Start()
    {
        FPCam = GameObject.Find("BuilderCamera").GetComponent<Camera>();
    }

    public void SetHoverPos(Vector3 p)
    {
        _hoverPos = p;
        SetPivotPos();
    }

    public abstract void SetPivotPos();

    protected virtual void FireGizmoMovedEvent()
    {
        if (OnGizmoMoved != null) { OnGizmoMoved(); }
    }

    void OnDestroy()
    {
        AttachedObject = null;
        AttachedTransform = null;
    }

}

