using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TextEditStarter : MonoBehaviour
{
    GameObject EditingInputField;
    float MouseDownPosx;
    float MouseDownTime;

    void Awake()
    {
        //InputField = transform.parent.FindChild("InputField").gameObject; //Didnt work it kept crashing
        //EditingInputField = GameObject.Find("RenameInputField");
    }

    public void OnMouseDown()
    {
        MouseDownPosx = Input.mousePosition.x;
        MouseDownTime = Time.time;
    }

    public void OnMouseUp()
    {
        if (Mathf.Abs(Input.mousePosition.x - MouseDownPosx) < 10)
        {
            EditStart();
        }
    }

    public void OnPointerClick(BaseEventData eventData)
    {
        PointerEventData PEV = (PointerEventData)eventData;
        if (PEV.clickCount == 2)
        EditStart();
    }


    //if (guiText.HitTest(touch.position))

    public void EditStart()
    {
        /*
        UnityEngine.Object objTrackPanel = Resources.Load("Prefabs/pnlTrackDetail");
        GameObject goTrackPanel = (GameObject)GameObject.Instantiate(objTrackPanel,Vector3.zero, Quaternion.identity, GameObject.Find("MenuCanvas").transform);
        goTrackPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        goTrackPanel.GetComponent<TrackDetail>().Init();
        EditingInputField = goTrackPanel.transform.Find("txtName").gameObject;
        //EditingInputField.SetActive(true);
        EditingInputField.GetComponent<InputField>().text = this.GetComponent<Text>().text;
        InputFieldInitializedEventArgs args = new InputFieldInitializedEventArgs { Submitter = EditingInputField };
        OnInputFieldInitialized(args);
        //EventSystem.current.SetSelectedGameObject(InputField, null);
        //this.gameObject.SetActive(false);
*/
    }

    //THis event tells the terget that the InputField is there so it can subscribe to the InputSubmitted event
    public delegate void InputFieldInitializedEventHandler(GameObject sender, InputFieldInitializedEventArgs e);
    public event InputFieldInitializedEventHandler InputFieldInitialized;
    protected virtual void OnInputFieldInitialized(InputFieldInitializedEventArgs e)
    {
        if (InputFieldInitialized != null)
        {
            InputFieldInitialized(this.gameObject, e);
        }
    }

}




