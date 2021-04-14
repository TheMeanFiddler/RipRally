using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.EventSystems;
using System;

class InputFieldSubmitter: MonoBehaviour
{
    //define the delegate;
    public delegate void InputSubmittedEventHandler(GameObject sender, InputSubmittedEventArgs e);
    public event InputSubmittedEventHandler InputSubmitted;

    //This method loads the args object with the correct stuff
    public void SubmitString(string n)
    {
        InputSubmittedEventArgs args = new InputSubmittedEventArgs();
        args.value = n;
        OnInputSubmitted(args);
        this.gameObject.SetActive(false);
    }

    //This fires the event. It is virtual because derived classes can override it
    protected virtual void OnInputSubmitted(InputSubmittedEventArgs e)
    {
        if (InputSubmitted != null)
        {
            InputSubmitted(this.gameObject, e);
        }
    }
}

public class InputSubmittedEventArgs : EventArgs
{
    public string value { get; set; }
}

public class InputFieldInitializedEventArgs : EventArgs
{
    public GameObject Submitter { get; set; }
}

