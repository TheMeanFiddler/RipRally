using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class MachineCarController : CarController
{
    public override void Init()
    {
        GameObject goPlayer = this.gameObject;
        goPlayer.AddComponent<CarAIInput>();
        InputManager = goPlayer.GetComponent<CarAIInput>();
        CarAIInput AI = (CarAIInput)InputManager;
        Rigidbody rb = goPlayer.GetComponent<Rigidbody>();
        Gps = new GPS(goPlayer);
        AI.Gps = Gps;
        EngineAudioSource.spatialBlend = 1f;    //so cant hear it when its a long way off
    }

}


