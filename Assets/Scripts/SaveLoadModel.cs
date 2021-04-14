using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System;


public delegate void GameDataEncodedEventHandler(byte[] data);
public delegate void GameDataDecodedEventHandler();

//using System.Xml.Serialization;
public static class SaveLoadModel
{
    public static List<Game> savedGames = new List<Game>();
    public static event GameDataEncodedEventHandler OnGameDataEncoded;

    public static void Save()
    {
        GameData.current.Encode();
        BinaryFormatter bf = new BinaryFormatter();
        //Save the GameData in its own file
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/" + Game.current.Filename + ".rip"); //you can call it anything you want

        GameData.current.Scene = Main.Instance.SelectedScene;                              //Todo: Remove this and retest
        GameData.current.MacId = SystemInfo.deviceUniqueIdentifier;
        //GameData.current.Encode();
        bf.Serialize(file, GameData.current);
        file.Close();
        //This is how you save to a byteararay to pass across the network
        //bytearray[] MByteArray
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    bf.Serialize(ms, obj);
        //    MByteArray = ms.ToArray();
        //}

        //Update and save the savedgames.gd file 
        if (Game.current.Saved == false) SaveLoadModel.savedGames.Add(Game.current);
        Game.current.Dirty = false;
        Game.current.Saved = true;
        Game sg = savedGames.FirstOrDefault(g => g.GameId == Game.current.GameId);
        sg.IsMine = Game.current.IsMine;
        SaveSavedGames();
    }

    public static int GetRank(float t)
    {
        int rank = -1;
        if (GameData.current.HiScores == null) GameData.current.HiScores = new List<HiScore>();
        else
        {
            List<HiScore> L = GameData.current.HiScores;
            int len = L.Count();
            if (len > 0)
                for (int n = 0; n < len; n++) { if (t < L[n].SegTime) { rank = n + 1; break; } }
            if (t > L.Last().SegTime) rank = len + 1;
        }
        return rank;
    }

    public static void Delete(int GameId)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Game GameToDelete = savedGames.Find(g => g.GameId == GameId);
        string Filename = GameToDelete.Filename;
        savedGames.Remove(GameToDelete);
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, savedGames);
        file.Close();
        if (File.Exists(Application.persistentDataPath + "/" + Filename + ".rip"))
        {
            File.Delete(Application.persistentDataPath + "/" + Filename + ".rip");
        }
    }

    public static void GetSavedGamesList()
    {
        savedGames.Clear();
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoadModel.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
        //look for games that are in the folder but aren't in the .gd file
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.rip");
        foreach (FileInfo f in info)
        {
            string Trackname = f.Name.Substring(0, f.Name.Length - 4);
            if (SaveLoadModel.savedGames.Exists(i => i.Filename == Trackname) == false)
            {
                Game g = new Game();
                g.Saved = true;
                g.Dirty = false;
                g.Filename = Trackname;
                g.IsMine = (GamefileIsMine(f));
                savedGames.Add(g);
            }
        }

        //look for SavedGames that are in the list but no file exists
        List<Game> GamesToDelete = new List<Game>();
        foreach (Game g in savedGames)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + g.Filename + ".rip"))
                GamesToDelete.Add(g);  
        }
        foreach(Game g in GamesToDelete)
        {
            savedGames.Remove(g);
        }
    }

    private static bool GamefileIsMine(FileInfo f)
    {
        bool rtn;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/" + f.Name, FileMode.Open);
        GameData gd = (GameData)bf.Deserialize(file);
        if (gd.MacId == SystemInfo.deviceUniqueIdentifier)
            rtn = true;
        else
            rtn = false;
        file.Close();
        bf = null;
        rtn = true;//remove this
        return rtn;

    }

    public static byte[] ByteEncodeSavedGamesList()
    {
        byte[] MByteArray;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, savedGames);
            MByteArray = ms.ToArray();
        }
        return MByteArray;
    }

    public static void ByteDecodeSavedGamesList(byte[] data)
    {
        savedGames.Clear();
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data))
        {
            savedGames = (List<Game>)bf.Deserialize(ms);
        }

    }

    public static byte[] ByteEncodeGame(Game _game)
    {
        byte[] MByteArray = new byte[0];
        if (File.Exists(Application.persistentDataPath + "/" + _game.Filename + ".rip"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + Game.current.Filename + ".rip", FileMode.Open);
            int Len = Convert.ToInt32(file.Length);
            MByteArray = new byte[Len];
            file.Read(MByteArray, 0, Len);

        }

        return MByteArray;
    }
    public static void ByteDecodeGameData(byte[] data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data))
        {
            GameData.current = (GameData)bf.Deserialize(ms);
            GameData.current.Decode();
        }
    }

    public static void LoadGameData(int Idx)
    {
        if (Idx == -1)
        {      //It says "New Game"
        }
        else
        {
            if (Game.current == null || Game.current.GameId != Idx)
            {
                Game MyGame = savedGames.Find(g => g.GameId == Idx);
                LoadFromFile(MyGame);
            }
            //Otherwise we are already on the selected game so we don't have to re-load it
        }
    }

    public static void LoadFromFile(Game game)
    {

        Game.current = game;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/" + Game.current.Filename + ".rip"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + Game.current.Filename + ".rip", FileMode.Open);
            try
            {
                GameData.current = (GameData)bf.Deserialize(file);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                GameDataBackwardCompat _gdb = (GameDataBackwardCompat)bf.Deserialize(file);
                GameData.current = _gdb.Convert();
            }
            file.Close();
            //file.SafeFileHandle.Close();
            //bf = null;
            Main.Instance.SelectedScene = GameData.current.Scene;
            Main.Instance.GameLoadedFromFileNeedsDecoding = true;
        }
    }

    public static void DecodeGameScene()
    {
        UnityEngine.Profiling.Profiler.BeginSample("Decode");
        //LoadTerrainBackup(GameData.current.Scene); //What to do with this?????
        GameData.current.Decode();
        UnityEngine.Profiling.Profiler.EndSample();
    }



    public static void LoadTerrainBackup(string Scene)
    {
        //Create a Terrain Backup by going to the Root Asset Folder where the "New Terrain x" file is. SHow in explorer and copy and paste it into Resources/TerrainBackups. THen Rename is as per this function
        int TerrainId = 0;
        do
        {
            TerrainData TD = Resources.Load("TerrainBackup/" + Scene + "Terrain" + TerrainId.ToString() + "Bkp") as TerrainData;
            if (TD != null)
            {
                TerrainController.Instance.TerrainDataList.Add(TD);
                TerrainId++;
            }
            else { break; }
        } while (true);
        if (TerrainId > 0) TerrainController.Instance.RestoredTerrainBkp = true;
        else
        {
            Debug.Log("TerrainBackup not found - " + "TerrainBackup/" + Scene + "Terrain0Bkp");
        }
    }

    public static void Rename(int GameId, string NewName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        Game GameToRename = savedGames.Find(g => g.GameId == GameId);
        if (Game.current != null)
        {
            //if (Game.current.GameId == GameToRename.GameId) Game.current.Filename = NewName; //No Idea what this was
        }
        string Filename = GameToRename.Filename;
        GameToRename.Filename = NewName;
        SaveSavedGames();
        if (File.Exists(Application.persistentDataPath + "/" + Filename + ".rip"))
        {
            File.Move(Application.persistentDataPath + "/" + Filename + ".rip", Application.persistentDataPath + "/" + NewName + ".rip");
        }
    }

    public static void SaveSavedGames()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, savedGames);
        file.Close();
        file = null;
        bf = null;
    }

    public static void SaveDemoTrack()
    {
        TextAsset DemoTextAsset;
        DemoTextAsset = Resources.Load("DemoTrack/Demo Track") as TextAsset;
        File.WriteAllBytes(Application.persistentDataPath + "/Demo Track.rip", DemoTextAsset.bytes);
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Demo Track.rip");
        //Debug.Log(filePath);
        //System.IO.File.Copy(filePath, Application.persistentDataPath + "/Demo Track.rip");
    }

}

