using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    bool PointerIsDown = false;
    float PointerDownTime;
    float PointerDownPosY;

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<ScrollRectSnap>().OnDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<ScrollRectSnap>().DragEnd();
        PointerIsDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDownTime = Time.time;
        PointerDownPosY = eventData.pressPosition.y;
        PointerIsDown = true;
    }

    void Update()
    {
        //if(PointerIsDown)
        if (PointerIsDown && Time.time - PointerDownTime > 1 && Mathf.Abs(Input.mousePosition.y - PointerDownPosY) < 10)
        {
            Transform SelItm = GetComponent<ScrollRectSnap>().SelectedScrollItem;
            SelItm.GetComponentInChildren<TextEditStarter>().EditStart();
            PointerIsDown = false;
        }
    }


}
