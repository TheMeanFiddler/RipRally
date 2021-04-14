using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[System.Serializable]
public class Game:IMenuDataItem

{

    public static Game current;
    public bool Saved = false;
    public bool Dirty = false;
    public int GameId;
    public int Hashcode; //used for comparing files by fileinfo.getHashCode()
    public Guid GUID;
    public int ScrollIdx;
    public string Filename;
    public bool IsMine;
    public float? BestTime;
    public float? BestLap;


    public Game()
    {
        int MaxIdx = 0;
        foreach (Game sg in SaveLoadModel.savedGames) { if (sg.GameId > MaxIdx)MaxIdx = sg.GameId; }
        GameId = MaxIdx+1;
        Filename = "Track " + GameId.ToString();
    }

}

[System.Serializable]
public class GameData
{
    public static GameData current;
    public int Idx;
    public string Filename;
    public Guid GUID;
    public string MacId;
    public string Scene;
    public BezierLineSerial BezS = new BezierLineSerial();
    public RoadSerial RdS = new RoadSerial();
    public ScenerySerial ScS = new ScenerySerial();
    public AllHeightmapsSerial AHMS = new AllHeightmapsSerial();
    public List<HiScore> HiScores;
    public byte[] Img;

    public void Encode()
    {
        if (SystemInfo.deviceUniqueIdentifier != MacId) return; //stops you trying to save a game that isnt yours
        GUID = new Guid();                              // Unique every time you save the game. Replay files are only relevant to this incarnation. 
        Idx = SaveLoadModel.savedGames.Count;
        Filename = Game.current.Filename;
        Game.current.IsMine = true;
        BezS.Encode(BezierLine.Instance);
        RdS = new RoadSerial();
        RdS.Encode();
        ScS.Encode();
        AHMS.Encode();
    }

    public void Decode()
    {
        BezS.Decode();
        RdS.Decode();
        ScS.Decode();
        AHMS.Decode();
    }


}

[System.Serializable]
public class Vector3Serial
{
    public float x;
    public float y;
    public float z;

    public Vector3Serial(Vector3 v3)
    {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }


    public Vector3 V3 { get { return new Vector3(x, y, z); } }

}

[System.Serializable]
public class Vector2Serial
{
    public float x;
    public float y;

    public Vector2Serial(Vector2 v2)
    {
        x = v2.x;
        y = v2.y;
    }

    public Vector2 V2 { get { return new Vector3(x, y); } }

}

[System.Serializable]
public class QuaternionSerial
{
    public float w;
    public float x;
    public float y;
    public float z;

    public QuaternionSerial(Quaternion Q)
    {
        w = Q.w;
        x = Q.x;
        y = Q.y;
        z = Q.z;
    }


    public Quaternion Decode {
        get {
        Quaternion rtn = new Quaternion();
        rtn.w = w;
        rtn.x = x;
        rtn.y = y;
        rtn.z = z;
        return rtn; }
    }

}

[System.Serializable]
public struct HiScore
{
    public float SegTime;
    public string Name;
}

[System.Serializable]
public class GameDataBackwardCompat
{
    public static GameData current;
    public int Idx;
    public string Filename;
    public Guid GUID;
    public string MacId;
    public string Scene;
    public BezierLineSerial BezS = new BezierLineSerial();
    public RoadSerial RdS = new RoadSerial();
    public ScenerySerial ScS = new ScenerySerial();
    public AllHeightmapsSerial AHMS = new AllHeightmapsSerial();
    [System.Runtime.Serialization.OptionalField]
    public float[] SegTimes;
    [System.Runtime.Serialization.OptionalField]
    public HiScore[] HiScores;

    public GameData Convert()
    {
        GameData _gd = new GameData();
        _gd.Idx = Idx;
        _gd.Filename = Filename;
        _gd.GUID = GUID;
        _gd.MacId = MacId;
        _gd.Scene = Scene;
        Debug.Log("Converted Old Format");
        return _gd;

    }
}