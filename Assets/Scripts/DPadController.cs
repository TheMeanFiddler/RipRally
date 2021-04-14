using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DPadController : MonoBehaviour {
	//VARIABLES FOR DPAD MOVEMENT
	float dPadTouchFingerId;
	private GameObject dPad;
	private Collider2D dPadColliderUp;
    private Collider2D dPadColliderRight;
    private Collider2D dPadColliderDown;
    private Collider2D dPadColliderLeft;
    private Collider2D dPadColliderFly;
	public Vector2 displacement;
    private bool Fly = false;
    public float Strafe;
    public float x;
    public float y;
    public float z;


	// Use this for initialization
	void Start () {

#if UNITY_EDITOR
		//this.GetComponentInParent<CanvasGroup>().alpha = 0;
#endif
		dPadTouchFingerId = -1;
		dPad = this.gameObject;
        foreach (Collider2D Coll in dPad.GetComponents<Collider2D>())
        {
            if (Coll.bounds.center == dPad.transform.position)
            {
                dPadColliderFly = Coll;
            }
            else if (Coll.bounds.center.x > dPad.transform.position.x+10)
            {
                dPadColliderRight = Coll;
            }
            else if (Coll.bounds.center.x < dPad.transform.position.x-10)
            {
                dPadColliderLeft = Coll;
            }
            else if (Coll.bounds.center.y > dPad.transform.position.y+10)
            {
                dPadColliderUp = Coll;
            }
            else if (Coll.bounds.center.y < dPad.transform.position.y-10)
            {
                dPadColliderDown = Coll;
            }
        }

	}

    private Collider2D TouchedCollider(Vector2 TouchPosition)
    {
        foreach (Collider2D Coll in dPad.GetComponents<Collider2D>())
        {
            if (Coll.OverlapPoint(TouchPosition)) return Coll;
        }
        return null;
    }
	// Update is called once per frame
	void Update () {
		//DPAD MOVEMENT
		foreach (Touch touch in Input.touches) {
                if (touch.phase == TouchPhase.Began)
                {
                    dPadTouchFingerId = touch.fingerId;
                    
                    //Camera FPCam = GameObject.Find("FPCamera").GetComponent<Camera>();
                    //Vector2 v = new Vector2(FPCam.ScreenToWorldPoint(touch.position).x, FPCam.ScreenToWorldPoint(touch.position).y);
                    if (dPadColliderFly.OverlapPoint(touch.position))
                    {
                        Fly = true;
                    }
                    if (dPadColliderUp.OverlapPoint(touch.position))
                    {
                        z = 1;
                    }
                    if (dPadColliderDown.OverlapPoint(touch.position))
                    {
                        z = -1;
                    }
                    if (dPadColliderRight.OverlapPoint(touch.position))
                    {
                        x = 1;
                    }
                    if (dPadColliderLeft.OverlapPoint(touch.position))
                    {
                        x = -1;
                    }
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    if (touch.fingerId == dPadTouchFingerId)
                    {
                        if (Fly && dPadColliderUp.OverlapPoint(touch.position))
                        {
                            y = 1;
                        }
                        if (Fly && dPadColliderDown.bounds.Contains(touch.position))
                        {
                            y = -1;
                        }
                        if (Fly && dPadColliderRight.OverlapPoint(touch.position))
                        {
                            Strafe = 1;
                        }
                        if (Fly && dPadColliderLeft.bounds.Contains(touch.position))
                        {
                            Strafe = -1;
                        }
                }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touch.fingerId == dPadTouchFingerId)
                    {
                        Fly = false;
                        Strafe = 0;
                        x = 0;
                        y = 0;
                        z = 0;
                        dPadTouchFingerId = -1;
                    }
                }
		}
	}
}
