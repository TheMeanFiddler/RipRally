using UnityEngine;
using UnityEngine.UI;

class SteeringWheelPadController : MonoBehaviour
{
    //VARIABLES FOR PPAD MOVEMENT
    float _touchFingerId;
    Transform SteeringWheel;
    BoxCollider2D coll;

    private Vector2 _touchStartPos;
    RectTransform rt;
    public float Value = 0f;
    float _angle = 0;
    float _prevAngle;
    Vector2 SWPos;

    void Start()
    {
        SteeringWheel = transform.Find("SteeringWheel");
        rt = GetComponent<RectTransform>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //PPAD MOVEMENT

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (coll.OverlapPoint(touch.position))
                {
                    Main.Instance.PopupMsg("Steer");
                    Value = 0f;
                    _angle = 0;
                    SWPos = SteeringWheel.position;
                    _touchStartPos = touch.position - SWPos;
                    _touchFingerId = touch.fingerId;
                    
                }
            }
            if (touch.fingerId == _touchFingerId)
            {
                if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                {
                    Vector2 P1 = _touchStartPos;
                    Vector2 P2 = touch.position - SWPos;
                    _angle = Vector2.Angle(P1, P2);
                    float sign = Mathf.Sign(P1.x * P2.y - P1.y * P2.x);
                    _angle = _angle * sign;
                    if (Mathf.Abs(_angle - _prevAngle) > 50) _angle = _prevAngle;   //cos it jumps from 180 to -180 and viceversa
                    _prevAngle = _angle;
                    Value = -_angle / 120f;  //arbitrary sensitivity
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Value = 0f;
                    _angle = 0;
                    _touchFingerId = 0;
                }
            }
            SteeringWheel.rotation = Quaternion.Euler(0, 0, _angle);
            _prevAngle = _angle;
        }


    }
}

