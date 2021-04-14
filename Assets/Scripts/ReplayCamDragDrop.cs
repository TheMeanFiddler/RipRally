using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReplayCamDragDrop: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool IsDragging = false;
    RectTransform trDragImg;
    RectTransform trArrow;
    RectTransform _trCamTimeline;
    Image imgArrow;

    public Replayer Replayer;
    public int Frame;
    public int CamId;
    

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Toggle>() != null)
        {
            GameObject goDragImg = new GameObject("DragImg");
            goDragImg.AddComponent<Image>().sprite = GetComponent<Image>().sprite;
            trDragImg = (RectTransform)goDragImg.transform;
            trDragImg.localScale = Vector3.one * 0.25f;
            _trCamTimeline = (RectTransform)Replayer.transform.Find("pnlCamTimeline");
            trDragImg.SetParent(_trCamTimeline, false);
            trDragImg.position = transform.position;
            trDragImg.anchorMin = new Vector2(0.5f, 0); trDragImg.anchorMax = new Vector2(0.5f, 0); trDragImg.pivot = new Vector2(0.5f, 1);
            GameObject goArrow = new GameObject("Arrow");
            imgArrow = goArrow.AddComponent<Image>();
            trArrow = (RectTransform)goArrow.transform;
            trArrow.SetParent(trDragImg,false);
            trArrow.pivot = new Vector2(0.5f, 0);
            trArrow.anchorMin = new Vector2(0.5f, 1); trArrow.anchorMax = new Vector2(0.5f, 1);
            trArrow.sizeDelta = new Vector2(5, 100);
            imgArrow.color = Color.yellow;
        }
        else
        {
            trDragImg = this.GetComponent<RectTransform>();
            trDragImg.anchorMin = new Vector2(0.5f, 0); trDragImg.anchorMax = new Vector2(0.5f, 0); trDragImg.pivot = new Vector2(0.5f, 1);
            trArrow = (RectTransform)trDragImg.GetChild(0);
            _trCamTimeline = (RectTransform)transform.parent;
            Replayer.DeleteCam(Frame);
        }
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        float LineLength;
        trDragImg.position = eventData.position + new Vector2(0, 20);
            LineLength = -trDragImg.localPosition.y * 4;
        trArrow.sizeDelta= new Vector2(5,LineLength);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        ReplayCamDragDrop newDD;
        Frame = Replayer.CamDrop(trDragImg.anchoredPosition, CamId);
        if (Frame == -1) { Destroy(trDragImg.gameObject); return; }
        if (gameObject.GetComponent<Toggle>() != null)
        {
            newDD = trDragImg.gameObject.AddComponent<ReplayCamDragDrop>(); // so you can drag it again
            newDD.Replayer = Replayer;
            newDD.Frame = Frame;
            Debug.Log(Frame);
            newDD.CamId = CamId;
        }
    }
}

