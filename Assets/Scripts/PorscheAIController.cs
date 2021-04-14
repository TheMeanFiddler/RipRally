using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class PorscheAIController : PorscheController
{
    public override void Init()
    {
        Transform trPlayer = goCar.transform.parent;
        GameObject goPlayer = trPlayer.gameObject;
        InputManager = goPlayer.AddComponent<HotrodAIInput>();
        HotrodAIInput AI = (HotrodAIInput)InputManager;
        Gps = new GPS(goPlayer);
        AI.Gps = Gps;
        EngineAudioSource.spatialBlend = 1f;    //so cant hear it when its a long way off
    }

}


