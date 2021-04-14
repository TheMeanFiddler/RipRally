using UnityEngine;


    class InputPressPedal: MonoBehaviour
    {
    float TouchFingerId = -1;
    private BoxCollider2D Coll;
    private float TouchStartTime;
    private float TouchEndTime;
    private bool Touching;
    public float Value;
    public BoxCollider2D SteeringColl;


    void Start()
    {
        Coll = GetComponent<BoxCollider2D>();
    }


    float d;
    void Update()
    {
        //PPAD MOVEMENT
        Value = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (Coll.OverlapPoint(touch.position))
                {

                    if (SteeringColl != null)
                    {
                        if (SteeringColl.OverlapPoint(touch.position)) continue;
                    }
                    TouchStartTime = Time.time;
                    TouchFingerId = touch.fingerId;
                    Value = 0;
                    Touching = true;
                }
            }

            if (touch.phase == TouchPhase.Ended && TouchFingerId == touch.fingerId)
            {
                TouchEndTime = Time.time;
                Touching = false;
                TouchFingerId = -1;
            }
        }

        if (Touching == true && Value < 1)
        {
            Value = Mathf.Clamp01(Time.time - TouchStartTime);
        }
        if (Touching == false && Value > 0)
        {
            Value = Mathf.Clamp01(TouchEndTime - Time.time);
        }
    }

}

