using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Ionic.BZip2;

public interface iRaceRecorder
{
    List<iVehicleManager> Recordables { get; set; }
    void RecFrame();
}

public class RaceRecorder : iRaceRecorder
{
    public static RaceRecorder current;
    public List<iVehicleManager> Recordables { get; set; }
    public string State { get; set; }
    public RaceRecorder(List<iVehicleManager> Recbls)
    {
        Recordables = Recbls;
        State = "Paused";
    }


    public void RecFrame()
    {
        if (State != "Paused")
            Recrdng.Current.RecFrame();
    }

}

public interface iRecrdng
{
    int FrameCount { get; set; }
    int CurrFrame { get; set; }
    int PlayerCarId { get; set; }
    string Filename { get; set; }
    List<List<RecFrameData>> Data { get; set; }
    List<RecordableVehicle> Recordables { get; set; }
    int StartFrame { get; set; }
    int EndFrame { get; set; }
    void RecFrame();
    void Pause();
    void Resume(int FrameNo);
    void Resume();
    void Save();
    void SaveAs(string name);
    void SaveGhost(LapStat ls);
    void RecordDamage(byte VId, RecEventType type, string partname, object Crunchname);
}

public class Recrdng : iRecrdng
{
    public static iRecrdng Current;
    public int FrameCount { get; set; }
    public int CurrFrame { get; set; }
    public int StartFrame { get; set; }
    public int EndFrame { get; set; }
    public List<RecordableVehicle> Recordables { get; set; }
    public int PlayerCarId { get; set; }
    public string Filename { get; set; }
    public List<List<RecFrameData>> Data { get; set; }
    private string State = "Recording";
    public Recrdng()
    {
        Filename = Game.current.Filename + "-"
            + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString("00") + System.DateTime.Now.Day.ToString("00") + "_"
            + System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00") + ".rcd";
        Data = new List<List<RecFrameData>>();
        Recordables = DrivingPlayManager.Current.VehicleManagers.Select(vm => new RecordableVehicle(vm)).ToList();
        PlayerCarId = DrivingPlayManager.Current.VehicleManagers.IndexOf(DrivingPlayManager.Current.PlayerCarManager);
    }

    public Recrdng(string filename)
    {
        Filename = filename;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/" + Filename))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + Filename, FileMode.Open);
            RecrdngSerial rs = (RecrdngSerial)bf.Deserialize(file);
            int _vehCount = rs.Data.GetLength(1);
            Main.Instance.Vehicles = rs.Vehicles.ToList();
            FrameCount = rs.Data.GetLength(0);
            Data = new List<List<RecFrameData>>();
            for (int f = 0; f < FrameCount; f++)
            {
                List<RecFrameData> lf = new List<RecFrameData>();
                for (int v = 0; v < _vehCount; v++)
                {
                    RecFrameData rfd = new RecFrameData();
                    RecFrameDataSerial rfds = rs.Data[f, v];
                    rfd.VId = rfds.VId;
                    rfd.FAV = (sbyte)rfds.FAngularVelocity;
                    rfd.RAV = (sbyte)rfds.RAngularVelocity;
                    rfd.FLSprL = rfds.FLSprL;
                    rfd.FRSprL = rfds.FRSprL;
                    rfd.RLSprL = rfds.RLSprL;
                    rfd.RRSprL = rfds.RRSprL;
                    rfd.FLGnd = rfds.FLGnd;
                    rfd.FRGnd = rfds.FRGnd;
                    rfd.RLGnd = rfds.RLGnd;
                    rfd.RRGnd = rfds.RRGnd;
                    rfd.Pos = rfds.Pos.V3;
                    rfd.Rot = rfds.Rot.Decode;
                    rfd.XMovement = rfds.XMovement;
                    if (rfds.Events!=null)
                    {
                        rfd.Events = rfds.Events.ToList(); 
                    }
                    //rfd.Event = new RecEvent {Type = (RecEventType)rfds.EventType, Data = rfds.EventData };
                    lf.Add(rfd);
                }
                Data.Add(lf);
            }
            file.Close();
            CurrFrame = 0;
        }
    }


    public void RecFrame()
    {
        if (State == "Recording")
        {
            List<RecFrameData> FrameData = Recordables.Select(item => new RecFrameData(item)).ToList<RecFrameData>();
            Data.Add(FrameData);
            FrameCount = Data.Count;
        }
    }

    public void RecordDamage(byte VId, RecEventType type, string partname, object Crunchname)
    {
        //Cant have two events on one recframe so just put it on the last empty one
        RecEvent evt = new RecEvent { Type = type, Data = partname, Data2 = Crunchname };
        Data.Last().FirstOrDefault(fd => fd.VId == VId).Events.Add(evt); // = new RecEvent { Type = type, Data = partname, Data2 = Crunchname };
    }

    public void Pause()
    {
        CurrFrame = FrameCount - 1;
        State = "Paused";
    }

    public void Resume(int FrameNo)
    {
        State = "Recording";
        Data.RemoveRange(FrameNo, FrameCount - FrameNo);
        FrameCount = FrameNo;
    }

    public void Resume()
    {
        State = "Recording";
        CurrFrame = FrameCount - 1;
    }

    public RecrdngSerial Serialize()
    {
        RecrdngSerial rtn = new RecrdngSerial();
        rtn.Vehicles = new RecordableVehicleSerial[Recordables.Count()];
        int i = 0;
        foreach(RecordableVehicle rv in  Recordables)
        {
            rtn.Vehicles[i] = rv.Serialize();
            i++;
        }
        int SavedFrameCount = EndFrame - StartFrame;
        rtn.Data = new RecFrameDataSerial[SavedFrameCount, Recordables.Count()];

        for (int f = 0; f < SavedFrameCount; f++)
        {
            for (int r = 0; r < Recordables.Count(); r++)
            {
                rtn.Data[f, r] = this.Data[f+StartFrame][r].Serialize();
            }
        }
        return rtn;
    }

    public void Save()
    {
        string nameOfTheFileToSave = Application.persistentDataPath + "/" + Game.current.Filename + "-"
    + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString("00") + System.DateTime.Now.Day.ToString("00") + "_" + System.DateTime.Now.Hour.ToString("00") + System.DateTime.Now.Minute.ToString("00") + System.DateTime.Now.Second.ToString("00");

        if (!Settings.Instance.Zip)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            file = File.Create(nameOfTheFileToSave + ".rcd"); //you can call it anything you want
            bf.Serialize(file, this.Serialize());
            file.Close();
        }

        else
        {

            using (FileStream fs = new FileStream(nameOfTheFileToSave + ".zip", FileMode.Create))
            {
                using (MemoryStream objectSerialization = new MemoryStream())
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    bformatter.Serialize(objectSerialization, this.Serialize());

                    using (BinaryWriter binaryWriter = new BinaryWriter(fs))
                    {
                        binaryWriter.Write(objectSerialization.GetBuffer().Length); //write the length first

                        using (BZip2OutputStream osBZip2 = new BZip2OutputStream(fs))
                        {
                            osBZip2.Write(objectSerialization.GetBuffer(), 0, objectSerialization.GetBuffer().Length); //write the compressed file
                        }
                    }
                }
            }
        }
    }

    public void SaveAs(string name)
    {
        string Fullname = Application.persistentDataPath + "/" + Game.current.Filename + "-" + name;
        BinaryFormatter bf = new BinaryFormatter();
        //Save the recording in its own file
        FileStream file;
        file = File.Create(Fullname + ".rcd"); //you can call it anything you want
        bf.Serialize(file, this.Serialize());
        file.Close();
        return;

        //This zipper takes 5 times as long and shrinks the file to 1/3 size
        using (FileStream fs = new FileStream(Fullname + ".zip", FileMode.Create))
        {
            using (MemoryStream objectSerialization = new MemoryStream())
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(objectSerialization, this.Serialize());

                using (BinaryWriter binaryWriter = new BinaryWriter(fs))
                {
                    binaryWriter.Write(objectSerialization.GetBuffer().Length); //write the length first

                    using (BZip2OutputStream osBZip2 = new BZip2OutputStream(fs))
                    {
                        osBZip2.Write(objectSerialization.GetBuffer(), 0, objectSerialization.GetBuffer().Length); //write the compressed file
                    }
                }
            }
        }
    }

    public void SaveGhost(LapStat ls)
    {
        GhostDataSerial[] Gst;
        int SavedFrameCount = FrameCount;
        Gst = new GhostDataSerial[ls.finFr - ls.stFr];
        for (int f = ls.stFr; f < ls.finFr; f++)
        {
            GhostDataSerial gf = new GhostDataSerial();
            RecFrameData rfdg = this.Data[f][PlayerCarId];
            gf.Pos = new Vector3Serial(rfdg.Pos);
            gf.Rot = new QuaternionSerial(rfdg.Rot);
            Gst[f-ls.stFr] = gf;
        }
        string nameOfTheFileToSave = Application.persistentDataPath + "/" + Game.current.Filename + ".gst";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(nameOfTheFileToSave);
        bf.Serialize(file, Gst);
        file.Close();
    }

}

public class RecordableVehicle
{
    public byte VId;
    public string Vehicle { get; set; }
    public string Color { get; set; }
    public Transform Transform { get; set; }
    public iInputManager InputManager { get; set; }
    public WheelController WCFL { get; set; }
    public WheelController WCRL { get; set; }
    public WheelController WCFR { get; set; }
    public WheelController WCRR { get; set; }
    public DamageController DMC { get; set; }

    public RecordableVehicle(iVehicleManager vm)
    {
        VId = vm.VId;
        Vehicle = vm.Vehicle;
        Color = vm.Color;
        Transform = vm.goCar.transform;
        InputManager = vm.VehicleController.InputManager;
        DMC = vm.goCar.GetComponent<DamageController>();
        WCFL = Transform.Find("WheelColliders/WCFL").GetComponent<WheelController>();
        WCRL = Transform.Find("WheelColliders/WCRL").GetComponent<WheelController>();
        WCFR = Transform.Find("WheelColliders/WCFR").GetComponent<WheelController>();
        WCRR = Transform.Find("WheelColliders/WCRR").GetComponent<WheelController>();
    }

    public RecordableVehicleSerial Serialize()
    {
        return new RecordableVehicleSerial { Vehicle = Vehicle, Color = Color };
    }
}

[System.Serializable]
public class RecordableVehicleSerial
{
    public string Vehicle { get; set; }
    public string Color { get; set; }
}

[System.Serializable]
public class RecrdngSerial
{
    public RecordableVehicleSerial[] Vehicles;
    public RecFrameDataSerial[,] Data;

}

public class RecFrameData
{
    public byte VId;
    public Vector3 Pos;
    public Quaternion Rot;
    public float XMovement;
    //public float ZMovement;
    public sbyte FAV;
    public sbyte RAV;
    public sbyte FLSprL;
    private sbyte fRSprL;
    public sbyte RLSprL;
    public sbyte RRSprL;
    public bool FLGnd;
    public bool FRGnd;
    public bool RLGnd;
    public bool RRGnd;
    public List<RecEvent> Events;

    public sbyte FRSprL
    {
        get
        {
            return fRSprL;
        }

        set
        {
            fRSprL = value;
        }
    }

    public RecFrameData()
    {

    }

    public RecFrameData(RecordableVehicle recbl)
    {
        VId = recbl.VId;
        Pos = recbl.Transform.position;
        Rot = recbl.Transform.rotation;
        XMovement = recbl.WCFR.steerAngle;
        //ZMovement = recbl.InputManager.ZMovement();
        FAV = (sbyte)recbl.WCFR.angularVelocity;
        RAV = (sbyte)recbl.WCRR.angularVelocity;
        FLSprL = (sbyte)(recbl.WCFL.springLengthInst * 100);
        FRSprL = (sbyte)(recbl.WCFR.springLengthInst * 100);
        RLSprL = (sbyte)(recbl.WCRL.springLengthInst * 100);
        RRSprL = (sbyte)(recbl.WCRR.springLengthInst * 100);
        FLGnd = recbl.WCFL.isGrounded;
        FRGnd = recbl.WCFR.isGrounded;
        RLGnd = recbl.WCRL.isGrounded;
        RRGnd = recbl.WCRR.isGrounded;
        Events = new List<RecEvent>();
    }

    public RecFrameDataSerial Serialize()
    {
        RecFrameDataSerial rtn = new RecFrameDataSerial();
        rtn.VId = VId;
        rtn.Pos = new Vector3Serial(Pos);
        rtn.Rot = new QuaternionSerial(Rot);
        rtn.XMovement = XMovement;
        //rtn.ZMovement = ZMovement;
        rtn.FAngularVelocity = FAV;
        rtn.RAngularVelocity = RAV;
        rtn.FLSprL = FLSprL;
        rtn.FRSprL = FRSprL;
        rtn.RLSprL = RLSprL;
        rtn.RRSprL = RRSprL;
        rtn.FLGnd = FLGnd;
        rtn.FRGnd = FRGnd;
        rtn.RLGnd = RLGnd;
        rtn.RRGnd = RRGnd;
        if (Events!=null) rtn.Events = Events.ToArray();
        return rtn;
    }
}

[System.Serializable]
public class RecFrameDataSerial
{
    public byte VId;
    public Vector3Serial Pos;
    public QuaternionSerial Rot;
    public float XMovement;
    //public float ZMovement;
    //public float BrakeForce;
    public sbyte FAngularVelocity;
    public sbyte RAngularVelocity;
    public sbyte FLSprL;
    public sbyte FRSprL;
    public sbyte RLSprL;
    public sbyte RRSprL;
    public bool FLGnd;
    public bool FRGnd;
    public bool RLGnd;
    public bool RRGnd;
    public RecEvent[] Events;
}


[System.Serializable]
public class RecEvent
{
    public RecEventType Type { get; set; }
    public object Data { get; set; }
    public object Data2 { get; set; }
}

public class GhostData
{
    public Vector3 Pos;
    public Quaternion Rot;

    public GhostDataSerial Serialize()
    {
        GhostDataSerial rtn = new GhostDataSerial();
        rtn.Pos = new Vector3Serial(Pos);
        rtn.Rot = new QuaternionSerial(Rot);
        return rtn;
    }
}

[System.Serializable]
public struct GhostDataSerial
{
    public Vector3Serial Pos;
    public QuaternionSerial Rot;

    public GhostData Decode()
    {
        GhostData rtn = new GhostData();
        rtn.Pos = Pos.V3;
        rtn.Rot = Rot.Decode;
        return rtn;
    }
}
