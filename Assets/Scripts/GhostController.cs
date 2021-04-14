using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GhostController : MonoBehaviour, iVehicleController
{
    public bool EndSkidmarks { get; set; }
    public GPS Gps {get; set;}
    public iInputManager InputManager{get;set;}
    public float motorForce { get; set; }
    GhostData[] _gd;
    bool Going;
    int FrameCount;
    public int Frame;

    public void Init()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/" + Game.current.Filename + ".gst"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + Game.current.Filename + ".gst", FileMode.Open);
            GhostDataSerial[] gds = (GhostDataSerial[])bf.Deserialize(file);
            file.Close();
            FrameCount = gds.Length;
            _gd = new GhostData[FrameCount];
            for (int f = 0; f < gds.Length; f++)
                _gd[f] = gds[f].Decode();
        }
        else
        {
            Main.Instance.Ghost = false;
            Destroy(this.gameObject);
        }
    }

    public void StartEngine()
    {
        
    }
    public void Go()
    {
        Going = true;
    }

    void FixedUpdate()
    {
            transform.position = _gd[Frame].Pos;
            transform.rotation = _gd[Frame].Rot;
            if (Going && Frame<FrameCount-1)
            Frame++;
    }

        void Destroy()
    {
        Debug.Log("GControlerDestr");
        Gps = null;
    }
}
