using UnityEngine;
using System.Collections.Generic;


public class AILesson
{

    int _segIdx;
    AIInputBuffer _inputBuffer;
    public AIResult Result;

    public AILesson(int segIdx, AIInputBuffer inputBuffer, AIResult result)
    {
        _segIdx = segIdx;
        _inputBuffer = inputBuffer;
        Result = result;
    }
}

public class AIInputBuffer
{
    Queue<AIInput> AIInputs = new Queue<AIInput>();
    float PrevAccel = 0;
    float PrevBrake = 0;
    float PrevSteer = 0;

    public void RecordInput(float Accel, float Brake, float Steer, float Time)
    {
        if(Mathf.Abs(Accel - PrevAccel) > 0.1f) {
            AIInputs.Enqueue(new AIInput { Type = AIInputType.Accel, Value = Accel, Time = Time });
            PrevAccel = Accel;
            if (AIInputs.Count > 30) AIInputs.Dequeue();
        }
        if (Mathf.Abs(Brake - PrevBrake) > 0.1f)
        {
            AIInputs.Enqueue(new AIInput { Type = AIInputType.Brake, Value = Brake, Time = Time });
            PrevBrake = Brake;
            if (AIInputs.Count > 30) AIInputs.Dequeue();
        }
        if (Mathf.Abs(Steer - PrevSteer) > 2f)
        {
            AIInputs.Enqueue(new AIInput { Type = AIInputType.Steer, Value = Steer, Time = Time });
            PrevSteer = Steer;
            if (AIInputs.Count > 30) AIInputs.Dequeue();
        }
    }

    public void Dump()
    {
        AIInput i;
        while (AIInputs.Count > 0)
        { i = AIInputs.Dequeue();
            Debug.Log(i.Type + "   " + i.Value + " Time= " + i.Time);
        }
    }
    public List<AIInput> ToList()
    {
        List<AIInput> rtn = new List<AIInput>();
        while (AIInputs.Count > 0)
            rtn.Add(AIInputs.Dequeue());
        return rtn;
    }
}

public struct AIInput
{
    public AIInputType Type;
    public float Value;
    public float Time;
}

public enum  AIResult { OK = 0, HitLFence = 1, HitRFence = 2, Stuck = 3, Fly = 4}

public enum AIInputType {  Accel = 0, Brake = 1, Steer = 2}
