using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickableObject : MonoBehaviour
{

    public PlaceableObject PlaceableObject;
    private RoadBuilder _rb;

    void Start()
    {
        Init();
    }
    public void Init()
    {
        GameObject bp = GameObject.Find("BuilderPlayer(Clone)");
        if(bp!=null)_rb = bp.GetComponent<RoadBuilder>();
    }
    void OnMouseUp()
    {
        _rb.Dragging = true;
        PlaceableObject.Select();
    }


    void OnDestroy()
    {
        PlaceableObject = null;
    }

}

