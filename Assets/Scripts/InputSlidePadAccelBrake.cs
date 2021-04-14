using UnityEngine;
using UnityEngine.UI;

class InputSlidePadAccelBrake : InputAccelBrakeTouch
{
    //VARIABLES FOR PPAD MOVEMENT
    float _touchFingerId;
    private GameObject _pad;
    private BoxCollider2D Coll;
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos = new Vector2(-1, -1);
    private bool _accel = true;


    void Start()
    {
        _pad = this.gameObject;
        Coll = GetComponent<BoxCollider2D>();
    }

    Vector2 SwipeTop;
    Vector2 SwipeBtm;
    float d;
    void Update()
    {
        //PPAD MOVEMENT
        AccelValue = 0;
        BrakeValue = 0;
        foreach (Touch touch in Input.touches)
        {
            if (Coll.OverlapPoint(touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _touchStartPos = touch.position;
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.y > _touchStartPos.y)
                    {
                        AccelValue = (touch.position.y - _touchStartPos.y) / 200;
                        if (AccelValue > 1) AccelValue = 1;
                    }
                    else
                    {
                        BrakeValue = (_touchStartPos.y - touch.position.y) / 200;
                        if (BrakeValue > 1) { AccelValue = 1 - BrakeValue; BrakeValue = 0; }
                        }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    _touchEndPos = touch.position;
                }

            }
        }

    }
}

