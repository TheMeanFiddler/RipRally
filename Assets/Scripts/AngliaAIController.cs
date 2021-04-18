using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class AngliaAIController : AngliaController
{
    public override void Init()
    {
        GameObject goPlayer = this.gameObject;
        InputManager = goPlayer.AddComponent<AngliaAIInput>();
        Gps = new GPS(goPlayer);
        ((AngliaAIInput)InputManager).Gps = Gps;
        IdleAudioSource.spatialBlend = 1f;    //so cant hear it when its a long way off
        RevAudioSource.spatialBlend = 1f;
        AccelAudioSource.spatialBlend = 1f;
        DecelAudioSource.spatialBlend = 1f;
        SkidAudioSource.spatialBlend = 1f;
    }

}


