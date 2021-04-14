using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class GPadController : MonoBehaviour {
	private RawImage gPad;
	private Collider2D gPadCollider;
	private float gPadTouchFingerId;
	public Vector2 glanceVector;
	public delegate void TapAction();
	public static event TapAction OnGPadTap;
	public delegate void DragAction(Vector2 dragPos);
	public static event DragAction OnGPadDrag;
	
	// Use this for initialization
	void Start () {
		gPadTouchFingerId = -1;
		gPad = this.gameObject.GetComponent <RawImage> ();
		gPadCollider = this.GetComponent <Collider2D> ();
		glanceVector = Vector2.zero;
	}

	void Update(){
				foreach (Touch touch in Input.touches) {
						if (touch.phase == TouchPhase.Began) {
								if (gPadCollider.OverlapPoint (touch.position)) {
								gPadTouchFingerId = touch.fingerId;
								}
						}
						if (touch.phase == TouchPhase.Ended && touch.fingerId == gPadTouchFingerId) {
							if (gPadCollider.OverlapPoint (touch.position)) {
								if(OnGPadTap != null) {
									OnGPadTap();
								}
							} else {
								if(OnGPadDrag != null) {
									OnGPadDrag(touch.position);
								}
							}
							gPadTouchFingerId = -1;
						}
				}
		}

}
