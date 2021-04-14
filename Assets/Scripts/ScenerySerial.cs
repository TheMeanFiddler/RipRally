using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ScenerySerial
{
    public PlaceableObjectSerial[] Objects;
    public void Encode()
    {
        Scenery Sc = Scenery.Instance;
        Objects = new PlaceableObjectSerial[Sc.Objects.Count];
        for (int Idx = 0; Idx < Sc.Objects.Count; Idx++)
        {
            PlaceableObjectSerial POS = new PlaceableObjectSerial();
            POS.Encode(Sc.Objects[Idx]);
            Objects[Idx] = POS;
        }
    }

    public void Decode()
    {
        Scenery Sc = Scenery.Instance;
        Sc.RemoveObjects();
        if (Objects == null) return;  //In case we are loading a blank track
        foreach (PlaceableObjectSerial POS in Objects)
        {
            PlaceableObject PO = POS.Decode();
            Sc.Objects.Add(PO);
        }
    }
}

[System.Serializable]
public class PlaceableObjectSerial
{
    public string Type { get; set; }
    public string prefab { get; set; }
    public string name { get; set; }
    public Vector3Serial pos;
    public Vector3Serial Scale;
    public QuaternionSerial rot;

    public void Encode(PlaceableObject Obj)
    {
        Type = Obj.GetType().ToString();
        prefab = Obj.prefab;
        name = Obj.name;
        pos = new Vector3Serial(Obj.Pos);
        Scale = new Vector3Serial(Obj.Scale);
        rot = new QuaternionSerial(Obj.Rot);
    }

    public PlaceableObject Decode()
    {
        PlaceableObject rtn;
        switch (Type)
        {
            case "StartingLine":
                rtn = new StartingLine(name);
                break;
            default:
                rtn = new SceneryObject(name);
                break;
        }

        rtn.prefab = prefab;
        rtn.Pos = pos.V3;
        try { rtn.Scale = Scale.V3; } catch { rtn.Scale = Vector3.one; }
        rtn.Rot = rot.Decode;
        return rtn;
    }
}
