using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class HotrodPlayerController : HotrodController
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
        goPacjk = GameObject.Instantiate(objPacjk, Vector3.up * 150, Quaternion.identity, trCanvas) as GameObject;
        Pacjk = goPacjk.GetComponent<PacejkaDisplay>();
        Pacjk.WC = WCFL;
        */

        InputManager = InputFactory.ChooseInputManager();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputManager = null;
    }

}
