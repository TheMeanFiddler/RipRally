using UnityEngine;

    [System.Serializable]
    public class AllHeightmapsSerial {
        private HeightmapSerial[] AHMS;

        public void Encode()
        {
        Debug.Log("EncodeHeightMaps");
            TerrainController TC = TerrainController.Instance;
            AHMS = new HeightmapSerial[TC.TerrainList.Count];
            for(int Idx = 0; Idx<TC.TerrainList.Count;Idx++){
                HeightmapSerial HMS = new HeightmapSerial();
                HMS.Encode(TC.TerrainList[Idx]);
                AHMS[Idx] = HMS;
            }
        }

        public void Decode()
        {
            TerrainController TC = TerrainController.Instance;
            TC.Heightmaps.Clear();
        if (AHMS != null)
        { //In case we are loading a blank track
            foreach (HeightmapSerial HMS in AHMS)
            {
                TC.Heightmaps.Add(HMS);
            }
        }
            TC.LoadedNewTerrainHeightmaps = true;
        }
    }

    [System.Serializable]
    public class HeightmapSerial {
        public int TerrainId;
        public float[,] Heights;

        public void Encode(TerrainListItem tli)
        {
            TerrainId = tli.TerrainId;
            int xRes = tli.TData.heightmapResolution;
            int zRes = tli.TData.heightmapResolution;
            Heights = tli.TData.GetHeights(0, 0, xRes, zRes);
        }
    }