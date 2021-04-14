using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public sealed class TerrainController
{
    GameObject[] AllTerrainObjects;
    public List<TerrainListItem> TerrainList = new List<TerrainListItem>();
    public List<HeightmapSerial> Heightmaps;
    public List<TerrainData> TerrainDataList = new List<TerrainData>();
    public bool LoadedNewTerrainHeightmaps = false;
    public bool RestoredTerrainBkp = false;
    int TerrainCount;
    public int BackedUpLevel;


    //internal instance
    static TerrainController _instance;
    static readonly object padlock = new object();

    //THis is where it instantiates itself
    public static TerrainController Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) { _instance = new TerrainController(); }
                return _instance;
            }
        }
    }
    // Constructor - Use this for initialization
    public TerrainController()
    {

        Heightmaps = new List<HeightmapSerial>();
        //HeightsBkp = Heights.Select(a => a.ToArray()).ToArray();

    }

    public void Init()
    {
        Debug.Log("TerrainCOntrollerInit");
        UnityEngine.Profiling.Profiler.BeginSample("TerrainInit");
        AllTerrainObjects = GameObject.FindGameObjectsWithTag("Terrain");
        System.Array.Sort(AllTerrainObjects, CompareObjNames);
        TerrainCount = AllTerrainObjects.Length;
        if (LoadedNewTerrainHeightmaps || RestoredTerrainBkp)
        {
            TerrainList.Clear();
            for (int i = 0; i < TerrainCount; i++)
            {
                TerrainListItem TLI = new TerrainListItem(AllTerrainObjects[i]);
                TLI.TerrainId = i;
                TerrainList.Add(TLI);
            }
        }
        else
        {
            for (int i = 0; i < AllTerrainObjects.Count(); i++)
            {
                TerrainList[i].T = AllTerrainObjects[i];
            }
        }

        if (LoadedNewTerrainHeightmaps) { ApplyTerrainHeightmaps(); if(GameData.current.Scene=="Canyon") RepaintSplats(); LoadedNewTerrainHeightmaps = false; }

        if (RestoredTerrainBkp) { ApplyTerrainBkps(); RestoredTerrainBkp = false; }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    private void RepaintSplats()
    {
        TerrainData TD0 = TerrainList[0].TData;
        //This is the cliff terrain
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        float[,,] splatmapData = new float[TD0.alphamapWidth, TD0.alphamapHeight, TD0.alphamapLayers];
        for (int y = 0; y < TD0.alphamapHeight; y++)
        {
            for (int x = 0; x < TD0.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)TD0.alphamapHeight;
                float x_01 = (float)x / (float)TD0.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = TD0.GetHeight(Mathf.RoundToInt(y_01 * TD0.heightmapResolution), Mathf.RoundToInt(x_01 * TD0.heightmapResolution));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = TD0.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = TD0.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[TD0.alphamapLayers];

                // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT
                
                // Texture[0] is stronger on Sloping terrain that faces the x axis
                //splatWeights[0] = Mathf.Clamp01(steepness * steepness / (TD0.heightmapHeight / 0.1f)) * Mathf.Clamp01(Mathf.Abs(normal.z));

                // Texture[1] stronger on flatter terrain
                // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
                // Subtract result from 1.0 to give greater weighting to flat surfaces
                //splatWeights[1] = 1.0f - Mathf.Clamp01(steepness * steepness / (TD0.heightmapHeight / 0.1f));

                // Texture[2] is stronger on Sloping terrain that faces the z axis
                //splatWeights[2] = Mathf.Clamp01(steepness * steepness / (TD0.heightmapHeight / 0.1f)) * Mathf.Clamp01(Mathf.Abs(normal.x));

                float SlopeThreshold = UnityEngine.Random.Range(1f, 1.5f);
                float slope = steepness * steepness / (TD0.heightmapResolution); //oops I meant Scale but its nearly the same
                if (slope < SlopeThreshold)
                    splatWeights[0] = 1;
                else if (slope < 2)
                    { splatWeights[1] = 1; }
                else if(slope<8)
                    {
                        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
                        { splatWeights[2] = 1; }
                        else
                        { splatWeights[3] = 1; }
                    }
                else // if (slope > 8)
                    {
                        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
                        { splatWeights[4] = 1; }
                        else
                        { splatWeights[5] = 1; }
                    }


                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < TD0.alphamapLayers; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the TD:
        TD0.SetAlphamaps(0, 0, splatmapData);

        if (TerrainList.Count == 1) return;
        TerrainData TD1 = TerrainList[1].TData;
        splatmapData = new float[TD1.alphamapWidth, TD1.alphamapHeight, TD1.alphamapLayers];
        for (int y = 0; y < TD1.alphamapHeight; y++)
        {
            for (int x = 0; x < TD1.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)TD1.alphamapHeight;
                float x_01 = (float)x / (float)TD1.alphamapWidth;

                // Sample the height at this location (note GetHeight expects int coordinates corrSetHeightesponding to locations in the heightmap array)
                float height = TD1.GetHeight(Mathf.RoundToInt(y_01 * TD1.heightmapResolution), Mathf.RoundToInt(x_01 * TD1.heightmapResolution));
                float CliffHeight = TD0.GetHeight(Mathf.RoundToInt(y_01 * TD0.heightmapResolution), Mathf.RoundToInt(x_01 * TD0.heightmapResolution));
                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = TD1.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = TD1.GetSteepness(y_01, x_01);
                float CliffSteepness = TD0.GetSteepness(y_01, x_01);

                //Get rid of those annoying z-fighting flashers
                if (Mathf.Abs(height - CliffHeight) < 0.01f)
                {
                    if (CliffHeight > height)
                    {
                        float[,] NewH = new float[1, 1] { { (height - 0.02f) / TD1.heightmapScale.y } };  //Double curleys - thats a nested array initializer
                                                                                                          //SetHeights takes a range 0-1 whereas GetHeight is 0-600        
                        TD1.SetHeights(Mathf.RoundToInt(y_01 * TD1.heightmapResolution), Mathf.RoundToInt(x_01 * TD1.heightmapResolution), NewH);
                    }
                    else
                    {
                        float[,] NewH = new float[1, 1] { { (CliffHeight - 0.02f) / TD0.heightmapScale.y } };  //Double curleys - thats a nested array initializer
                                                                                                               //SetHeights takes a range 0-1 whereas GetHeight is 0-600        
                        TD0.SetHeights(Mathf.RoundToInt(y_01 * TD1.heightmapResolution), Mathf.RoundToInt(x_01 * TD1.heightmapResolution), NewH);
                    }
                }
                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[TD1.alphamapLayers];
                splatWeights[0] = 0;
                splatWeights[1] = 0;
                splatWeights[2] = 0;

                if (CliffSteepness > 21 && height > 11.1f)
                {
                    splatWeights[1] = 1;    //1 = Scree
                }
                else
                if (steepness  > 21)
                {
                    splatWeights[2] = 1;   //2 = Rock
                }
                else
                {
                    splatWeights[0] = 1;
                }
                
                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < TD1.alphamapLayers; i++)
                {

                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the TD:
        TD1.SetAlphamaps(0, 0, splatmapData);
    }

    int CompareObjNames(GameObject x, GameObject y)
    {
        return x.name.CompareTo(y.name);
    }
    /// <summary>
    /// Projects a Vector3 worldspace point into a float[2] array in Terainspace
    /// </summary>
    /// <param name="Point"></param>
    /// <returns></returns>
    public float[] ConvertToTerrainCoords(Vector3 Point)
    {
        float TerrX = (((Point.x - TerrainList[0].TData.heightmapScale.x / 2f - TerrainList[0].T.transform.position.x) / TerrainList[0].TData.size.x) * TerrainList[0].xRes);
        float TerrZ = (((Point.z - TerrainList[0].TData.heightmapScale.z / 2f - TerrainList[0].T.transform.position.z) / TerrainList[0].TData.size.z) * TerrainList[0].zRes);
        float[] Rtn = new float[2] { TerrX, TerrZ };
        return Rtn;
    }
    public Vector2 ConvertToWorldCoords(int j, int k)
    {
        float WrldX = j * TerrainList[0].TData.size.x / TerrainList[0].xRes + TerrainList[0].T.transform.position.x + TerrainList[0].TData.heightmapScale.x / 2f;
        float WrldZ = k * TerrainList[0].TData.size.z / TerrainList[0].zRes + TerrainList[0].T.transform.position.z + TerrainList[0].TData.heightmapScale.z / 2f;
        Vector2 Rtn = new Vector2(WrldX, WrldZ);
        return Rtn;
    }
    //public float ConvertToTerrainHeight(float y)
    //{
    //    return (y - T.transform.position.y) / yRes;
    //}

    /*public float[] OldHeight(int j, int k)
    {
        float[] rtn = new float[TerrainCount];
        for (int i = 0; i < TerrainCount; i++)
        {
            rtn[i] = TerrainList[i].HeightsBkp[j, k];
        }
        return rtn;
    }*/



    public void ApplyMods(List<TerrainMod> Mods)
    {
        foreach (TerrainMod Mod in Mods)
        {
            TerrainListItem TLI = TerrainList[Mod.TerrainId];
            TLI.TData.SetHeights(Mod.TerrainXBase, Mod.TerrainYBase, Mod.Heights);
        }
        //LoadedNewTerrainHeightmaps = false;   //Took this out cos I think its depreciated
    }
    /// <summary>
    /// Used when opening an existing track. Copies the heights and alphas from the GameData serialised maps
    /// </summary>
    private void ApplyTerrainHeightmaps()
    {
        foreach (HeightmapSerial HMS in Heightmaps)
        {
            TerrainData TD = TerrainList[HMS.TerrainId].TData;
            TD.SetHeights(0, 0, HMS.Heights);
        }
    }
    /// <summary>
    /// Used when creating a new track. Copies the heights and alphas from a stored copy
    /// </summary>
    public void ApplyTerrainBkps()
    {
        foreach(TerrainListItem TLI in TerrainList)
        {
            TerrainData TDBkp = TerrainDataList[TLI.TerrainId];
            TLI.TData.SetHeights(0, 0, TDBkp.GetHeights(0, 0, TDBkp.heightmapResolution, TDBkp.heightmapResolution));
            float[,,] am = TDBkp.GetAlphamaps(0, 0, TDBkp.alphamapWidth, TDBkp.alphamapHeight);
            TLI.TData.SetAlphamaps(0, 0, am);
        }
        TerrainDataList.Clear();
        RestoredTerrainBkp = false;
    }


}

public class TerrainListItem
{
    public int TerrainId { get; set; }
    public GameObject T { get; set; }
    public TerrainData TData { get; set; }
    public int xRes { get; set; }
    public int zRes { get; set; }
    public float yRes { get; set; }
    public float[,] HeightsBkp { get; set; }

    //Constructor
    public TerrainListItem(GameObject TerrainGameObject)
    {
        T = TerrainGameObject;
        TData = T.GetComponent<Terrain>().terrainData;
        xRes = TData.heightmapResolution;
        zRes = TData.heightmapResolution;
        yRes = TData.heightmapScale.y;
        /*if (HeightsBkp == null)
        {
            HeightsBkp = TData.GetHeights(0, 0, xRes, zRes);
        }*/
    }
}

[System.Serializable]
public class TerrainHeight
{
    public int j { get; set; }
    public int k { get; set; }
    public float y { get; set; }
}

public class TerrainPoint
{
    private int _j;
    private int _k;
    public TerrainPoint(int j, int k)
    {

        _j = j;
        _k = k;
    }
    public int j
    {
        get { return _j; }
        set { _j = value; }
    }
    public int k
    {
        get { return _k; }
        set { _k = value; }
    }

}

public class TerrainMinimap
{
    public int TerrainXBase { get; set; }
    public int TerrainYBase { get; set; }
    public float[,] Heights { get; set; }
    public bool RaiseAllowed { get; set; }


    public List<TerrainMod> GetTerrainMods()
    {
        List<TerrainMod> rtn = new List<TerrainMod>();
        //find out which is the highest terrain - this is used if we raise the terrain so that only the highest one is raised
        TerrainListItem HighestTerrain = TerrainController.Instance.TerrainList.OrderByDescending(item => item.TData.GetHeight(this.TerrainXBase, this.TerrainYBase)).First();

        foreach (TerrainListItem TLI in TerrainController.Instance.TerrainList)
        {
            TerrainMod Mod = new TerrainMod();
            Mod.TerrainId = TLI.TerrainId;
            Mod.TerrainXBase = this.TerrainXBase;
            Mod.TerrainYBase = this.TerrainYBase;
            Mod.Heights = new float[this.Heights.GetLength(1), this.Heights.GetLength(0)];
            bool ThisTerrainWasCut = false;
            float[,] PrevHeights = TLI.TData.GetHeights(this.TerrainXBase, this.TerrainYBase, this.Heights.GetLength(0), this.Heights.GetLength(1));
            for (int j = 0; j < this.Heights.GetLength(0); j++)
            {
                for (int k = 0; k < this.Heights.GetLength(1); k++)
                {
                    if (this.Heights[j, k] == 0)
                    {
                        Mod.Heights[k, j] = PrevHeights[k, j];
                    }
                    else
                    {
                        float NewHeight = (this.Heights[j, k] - TLI.T.transform.position.y) / TLI.yRes;
                        if (!this.RaiseAllowed)
                        {
                            if (NewHeight < PrevHeights[k, j])
                            {
                                Mod.Heights[k, j] = NewHeight;
                                ThisTerrainWasCut = true;
                            }
                            else
                            {
                                Mod.Heights[k, j] = PrevHeights[k, j];
                            }
                        }
                        else  //if RaiseAllowed
                        {
                            if (TLI==HighestTerrain)
                                Mod.Heights[k, j] = NewHeight;
                            else
                                Mod.Heights[k, j] = NewHeight*0.95f;
                            ThisTerrainWasCut = true;
                        }
                    }
                }
            }
            if (ThisTerrainWasCut)
            {
                rtn.Add(Mod);
            }
        }
        return rtn;
    }
}

[System.Serializable]
public class TerrainMod
{
    public int TerrainId { get; set; }
    public int TerrainXBase { get; set; }
    public int TerrainYBase { get; set; }
    public float[,] Heights { get; set; }
}

[System.Serializable]
public class TerrainBackupSerial
{
    public string Scene;
    public HeightmapSerial[] HeightMapsSerial;
    public SplatMap[] SplatMaps;

}

[System.Serializable]
public class SplatMap
{
    public int TerrainId;
    //float[,,] SplatMapData;

    public void Encode(TerrainListItem tli)
    {
        int Width = tli.TData.alphamapWidth;
        int Height = tli.TData.alphamapHeight;
        int Layers = tli.TData.alphamapLayers;
        TerrainId = tli.TerrainId;
        //SplatMapData = tli.TData.GetAlphamaps(0, 0, Width, Height);
    }
}
