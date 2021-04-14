using UnityEngine;
//using UnityEngine.UI;

class InputPressPadAccelBrake : InputAccelBrakeTouch
{
    //VARIABLES FOR PPAD MOVEMENT
    float _accelTouchFingerId;
    float _brakeTouchFingerId;
    private GameObject _pad;
    private BoxCollider2D CollBrake;
    private BoxCollider2D CollAccel;
    private bool _braking = false;
    private bool _accel = false;
    private bool _reversing = false;
    private float TouchStartTime;
    private float TouchEndTime;


    void Start()
    {
        _pad = this.gameObject;
        CollBrake = GetComponents<BoxCollider2D>()[0];
        CollAccel = GetComponents<BoxCollider2D>()[1];
    }


    float d;
    void Update()
    {
        //PPAD MOVEMENT
        AccelValue = 0;
        BrakeValue = 0;
        foreach (Touch touch in Input.touches)
        {
            if (CollBrake.OverlapPoint(touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    TouchStartTime = Time.time;
                    _accelTouchFingerId = touch.fingerId;
                    _braking = true;
                    BrakeValue = 0;
                }
            }
            if (CollAccel.OverlapPoint(touch.position))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    TouchStartTime = Time.time;
                    _accelTouchFingerId = touch.fingerId;
                    _accel = true;
                    AccelValue = 0;
                }
            }
            if (touch.phase == TouchPhase.Ended && _accelTouchFingerId == touch.fingerId)
            {
                _accel = false;
                TouchEndTime = Time.time;
            }
            if (touch.phase == TouchPhase.Ended && _brakeTouchFingerId == touch.fingerId)
            {
                _braking = false;
                TouchEndTime = Time.time;
            }
        }

        if (_accel == true && AccelValue < 1)
        {
            AccelValue = Mathf.Clamp01(Time.time - TouchStartTime);
        }
        if (_accel == false && _braking == false && AccelValue !=0)
        {
            AccelValue = Mathf.Clamp01(TouchEndTime - Time.time);
        }
        if (_braking == true)
        {
            //Brake pressed for more than a second
            if (Time.time > TouchStartTime+1)
            {
                Main.Instance.PopupMsg("Reverse " + AccelValue);
                BrakeValue = 0;
                _reversing = true;
                AccelValue = -1; // -Mathf.Clamp01(Time.time - TouchStartTime-1); too slow
            }
            else   //brake pressed for less than sec
                BrakeValue = Mathf.Clamp01(Time.time - TouchStartTime);

        }

        //Foot off the brake before reverse has started
        if (_braking == false && _accel == false && BrakeValue > 0)
        {
            BrakeValue = Mathf.Clamp01(TouchEndTime - Time.time);
        }

        //Foot off the brake once reverse has started
        if (_braking == false && AccelValue<0)
        {
            AccelValue = -Mathf.Clamp01(Time.time - TouchEndTime);
        }

    }
}

