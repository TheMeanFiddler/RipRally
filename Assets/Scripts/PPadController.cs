using UnityEngine;
using System.Collections;
using UnityEngine.UI;

class PPadController : MonoBehaviour
{
    //VARIABLES FOR PPAD MOVEMENT
    float pPadTouchFingerId;
    float BrakeTouchFingerId;
    private GameObject pPad;
    private Collider2D pPadColliderAccel;
    private Collider2D pPadColliderBrake;
    private Image AccelImage;

    public Vector2 displacement;
    public bool Brake = false;
    public float BrakeForce = 0;
    public float Accel = 0;

    void Start()
    {
        pPad = this.gameObject;
        foreach (Collider2D Coll in pPad.GetComponents<Collider2D>())
        {
            if (Coll.bounds.center.x < pPad.transform.position.x)
            {
                pPadColliderBrake = Coll;
            }
            else if (Coll.bounds.center.x > pPad.transform.position.x)
            {
                pPadColliderAccel = Coll;
            }
        }
        Sprite AccelSprite = pPad.GetComponentsInChildren<Image>()[0].sprite;
    }

    void Update()
    {
        //PPAD MOVEMENT
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began) {
                if (pPadColliderAccel.OverlapPoint(touch.position))
                {
                    Vector2 PedalBtm = pPadColliderAccel.bounds.min;
                    float Ac = (touch.position.y - PedalBtm.y)/500;
                    Accel = Ac;
                    pPadTouchFingerId = touch.fingerId;
                }
                if (pPadColliderBrake.OverlapPoint(touch.position))
                {
                    Brake = true;
                    BrakeTouchFingerId = touch.fingerId;
                }
                
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.fingerId == pPadTouchFingerId)
                {
                    Vector2 PedalBtm = pPadColliderAccel.bounds.min;
                    float Ac = (touch.position.y - PedalBtm.y) / 233;
                    if (Ac > 1.0f) Ac = 1.0f;
                    Accel = Ac;
                }
                if (touch.fingerId == BrakeTouchFingerId)
                {
                    Vector2 PedalBtm = pPadColliderBrake.bounds.min;
                    float Br = (touch.position.y - PedalBtm.y) / 233;
                    if (Br > 1.0f) Br = 1.0f;
                    BrakeForce = Br;
                    
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (touch.fingerId == pPadTouchFingerId)
                {
                    Accel = 0;
                    pPadTouchFingerId = 0;
                }
                if (touch.fingerId == BrakeTouchFingerId)
                {
                    Brake = false;
                    BrakeTouchFingerId = 0;
                }
            }
        }
    }
}

