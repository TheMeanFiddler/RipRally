using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[System.Serializable]
public sealed class Settings
{
    private static Settings _instance;
    static readonly object padlock = new object();
    public string CamType { get; set; }
    public string SteerControl { get; set; }
    public bool HorizonTilt { get; set; }
    public string AccelControl { get; set; }
    public bool TutorialIntroHide = false;
    public bool TutorialBuildHide = false;
    public bool TutorialShopHide = false;
    public float SFXVolume = 1;
    public float SteeringPosX = -200;
    public float SteeringPosY = 100;
    public float AccelPos = 200;
    public float BrakePos = -200;
    public bool Zip;


    public static Settings Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new Settings(); }
                return _instance;
            }
        }
    }

    public void SaveToFile()
    {
        Debug.Log(_instance.CamType);
        BinaryFormatter bf = new BinaryFormatter();
        //Save the GameData in its own file
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/Settings.dat"); //you can call it anything you want
        bf.Serialize(file, _instance);
        file.Close();
    }

    public void LoadFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Settings.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Settings.dat", FileMode.Open);
            _instance = (Settings)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.Log("Filenotfound");
            _instance.CamType = "FollowCamSwing";
            _instance.SteerControl = "Tilt";
            _instance.HorizonTilt = true;
            _instance.AccelControl = "ThumbPress";
        }
    }
}

public class UserDataManager
{
    private static UserDataManager _instance;
    static readonly object padlock = new object();
    public UserData Data { get; set; }

    public static UserDataManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new UserDataManager();
                    _instance.Data = new UserData();
                }
                return _instance;
            }
        }
    }

    public void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/Config.dat"); //you can call it anything you want
        bf.Serialize(file, Data.Encode());
        file.Close();
    }

    public void LoadFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Config.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/Config.dat", FileMode.Open);
            UserDataSerial usd = (UserDataSerial)bf.Deserialize(file);
            file.Close();
            Data = new UserData(usd);
            Data.Coins = 989898;    //comment out when live
            Debug.Log("Config Loaded");
            if (Data.MacId != SystemInfo.deviceUniqueIdentifier || Data.MacId ==null)
            {
                Data.Coins = 100;
                Data.Purchases = new List<int>();
                Data.Purchases.Add(0);  //Tarmac
                Data.Purchases.Add(5);  //Fence0
                Data.Purchases.Add(6);  //Fence1
                Data.Purchases.Add(7);  //Fence2
                Data.Purchases.Add(8);  //Tree
                Data.Purchases.Add(16); //rotating cam
                Data.Purchases.Add(36); //Hotrod
            }
        }
        else
        {
            Data.Coins = 100;
            Data.Purchases = new List<int>();
            Data.Purchases.Add(0);
            Data.Purchases.Add(5);
            Data.Purchases.Add(6);
            Data.Purchases.Add(7);
            Data.Purchases.Add(8);
            Data.Purchases.Add(16); //rotating cam
            Data.Purchases.Add(36); //Hotrod
        }
        Data.MacId = SystemInfo.deviceUniqueIdentifier;
    }
}


[System.Serializable]
public class UserData
{
    public string MacId;
    public int Coins { get; set; }
    public List<int> Purchases { get; set; }
    //constructor

    public UserData() { }

    public UserData(UserDataSerial usd)
    {
        MacId = usd.MacId;
        Coins = usd.Coins;
        Purchases = usd.Purchases.ToList();
    }
    public UserDataSerial Encode()
    {
        UserDataSerial usd = new UserDataSerial();
        usd.MacId = SystemInfo.deviceUniqueIdentifier;
        usd.Coins = Coins;
        usd.Purchases = Purchases.ToArray<int>();
        return usd;
    }
}

[System.Serializable]
public class UserDataSerial
{
    public string MacId;
    public int Coins { get; set; }
    public int[] Purchases { get; set; }
}

