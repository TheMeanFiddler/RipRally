using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;




public class ModelTPlayerController : ModelTController
{

    public override void Init()
    {
        Gps = new GPS(this.gameObject);


        /*
        //For testing
        UnityEngine.Object objPacjk = Resources.Load("Prefabs/PacejkaDisplay");
        GameObject goPacjk = GameObject.Instantiate(objPacjk, Vector3.zero, Quaternion.identity, trCanvas) as GameObject;
        PacejkaDisplay Pacjk = goPacjk.GetComponent<PacejkaDisplay>();
        Pacjk.WC = WCRL;
        goPacjk = GameObject.Instantiate(objPacjk, Vector3.up * 50, Quaternion.identity, trCanvas) as GameObject;
        Pacjk = goPacjk.GetComponent<PacejkaDisplay>();
        Pacjk.WC = WCFL;
        */

        InputManager = InputFactory.ChooseInputManager();
    }

    void OnDestroy()
    {
        Destroy(GameObject.Find("DrivingGUICanvas(Clone)"));
        InputManager = null;
    }

}
