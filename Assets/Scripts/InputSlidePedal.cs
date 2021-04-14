using UnityEngine;
using UnityEngine.UI;

class InputSlidePedal: MonoBehaviour
{
    //VARIABLES FOR PPAD MOVEMENT
    private BoxCollider2D Coll;
    private float TouchFingerId = -1;
    private Vector2 _touchStartPos;
    public float Value;
    public float DefaultValue = 0;
    public float TouchValue;
    public float SwipeLength;
    public BoxCollider2D SteeringColl;

    void Start()
    {
        Coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (Coll.OverlapPoint(touch.position))
                {
                    {
                        if (SteeringColl != null)
                        {
                            if (SteeringColl.OverlapPoint(touch.position)) continue;
                        }
                        TouchFingerId = touch.fingerId;
                        _touchStartPos = touch.position;
                        Value = TouchValue;
                    }
                }
            }

            if (TouchFingerId == touch.fingerId && touch.phase == TouchPhase.Moved)
            {

                Value = Mathf.Clamp(TouchValue + (touch.position.y - _touchStartPos.y) / SwipeLength, -1f, 1f);
            }

            if (TouchFingerId == touch.fingerId && touch.phase == TouchPhase.Ended)
            {
                Value = DefaultValue;
                TouchFingerId = -1;
            }
        }
    }
}

