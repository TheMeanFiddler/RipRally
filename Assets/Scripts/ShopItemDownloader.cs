using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class ShopItemDownloader:MonoBehaviour
{
    string _assetName;
    public IList<IResourceLocation> Locations = new List<IResourceLocation>();

    public void Download(string assetName, string label)
    {
        _assetName = assetName;
        _ = LoadLocation(assetName, label);
    }
    public async Task LoadLocation(string assetName, string label)
    {
        Locations.Clear();
        string strPath = new StringBuilder("Assets/Shop/").Append(assetName).Append(".bytes").ToString();
        await AddressableLocationLoader.GetLocations(label, Locations);
        IResourceLocation loc = Locations.SingleOrDefault(l => l.PrimaryKey.Equals(assetName));
        //StartCoroutine(LoadAsset(loc));
        AsyncOperationHandle<UnityEngine.Object> h = Addressables.LoadAssetAsync<UnityEngine.Object>(strPath);
        h.Completed += AssetLoad_Completed;
    }

    public async Task LoadLocations(string label)
    {
        Locations.Clear();
        await AddressableLocationLoader.GetLocations(label, Locations);
    }

    void AssetLoad_Completed(AsyncOperationHandle<UnityEngine.Object> h)
    {
        SaveAddressable(h.Result);
    }
    public void SaveAddressable(UnityEngine.Object obj)
    {
        TextAsset DemoTextAsset;
        DemoTextAsset = obj as TextAsset;
        string _filePath = new StringBuilder(Application.persistentDataPath).Append("/" + _assetName + ".rip").ToString();
        Debug.Log(_filePath);
        File.WriteAllBytes(_filePath, DemoTextAsset.bytes);
        //
        //Debug.Log(filePath);
        //System.IO.File.Copy(filePath, Application.persistentDataPath + "/Demo Track.rip");
    }


}

