using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;


class GearShiftController : MonoBehaviour
{
    private Collider2D GearShiftCollider;
    private bool _prevOverlap;
    public string Gear = "F";
    private Transform Knob;

    void Start()
    {
        GearShiftCollider = GetComponent<Collider2D>();
        Knob = transform.GetChild(0);
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (GearShiftCollider.OverlapPoint(touch.position))
                {
                    if (Gear == "F")
                    {
                        Gear = "R";
                        Knob.Translate(Vector3.down * 200);
                    }
                    else
                    {
                        Gear = "F";
                        Knob.Translate(Vector3.up * 200);
                    }
                }
            }
            /*
            if (touch.phase == TouchPhase.Moved)
            {
                bool _isTouching = GearShiftCollider.OverlapPoint(touch.position);
                bool _wasTouching = GearShiftCollider.OverlapPoint(touch.position - touch.deltaPosition);
                if (_isTouching && !_wasTouching)
                {
                    Gear = "R";
                    Knob.Translate(Vector3.down * 200);
                }
                if (_wasTouching && !_isTouching)
                {
                    Gear = "F";
                    Knob.Translate(Vector3.up * 200);
                }

            }
            */
        }

    }
}

