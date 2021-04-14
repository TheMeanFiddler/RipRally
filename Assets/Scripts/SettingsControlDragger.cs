using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsControlDragger: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform _rectTransform;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (name == "imgSteer")
            _rectTransform.position = eventData.position;
        else
            _rectTransform.position = new Vector2(eventData.position.x,100);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (name == "imgAccel")
        {
            Settings.Instance.AccelPos = GetComponent<RectTransform>().anchoredPosition.x;
            Settings.Instance.SaveToFile();
        }
        if (name == "imgBrake")
        {
            Settings.Instance.BrakePos = GetComponent<RectTransform>().anchoredPosition.x;
            Settings.Instance.SaveToFile();
        }
        if (name == "imgSteer")
        {
            Settings.Instance.SteeringPosX = GetComponent<RectTransform>().anchoredPosition.x;
            Settings.Instance.SteeringPosY = GetComponent<RectTransform>().anchoredPosition.y;
            Settings.Instance.SaveToFile();
        }
    }

}

