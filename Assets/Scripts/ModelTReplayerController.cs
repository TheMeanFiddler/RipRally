using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ModelTReplayerController : ModelTController
{

    public override void Init()
    {
        //Gps = new GPS(goCar);
        InputManager = new ReplayerInput();
    }

}
