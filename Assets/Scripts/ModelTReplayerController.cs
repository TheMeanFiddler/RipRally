using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ModelTReplayerController : CarController
{

    public override void Init()
    {

        //Gps = new GPS(goCar);
        InputManager = new ReplayerInput();
        foreach (WheelController WC in goCar.GetComponentsInChildren<WheelController>()) { WC.Replayer = true; }
    }

}
