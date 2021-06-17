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



public class PurchaseDownloader:MonoBehaviour
{



    public void StartDownload(string assetName, string label)
    {
        _ = Download(assetName, label);
    }
    public async Task Download(string assetName, string label)
    {
        IList<IResourceLocation> Locations = new List<IResourceLocation>();
        //each shop item is a listing
        StringBuilder sb = new StringBuilder("Assets/Shop/").Append(assetName);
        if ("Track" == label) sb.Append(".bytes");
        string strPath = sb.ToString();
        await AddressableLocationLoader.GetLocations(label, Locations);
        IResourceLocation loc = Locations.SingleOrDefault(l => l.PrimaryKey.Equals(assetName));
        //StartCoroutine(LoadAsset(loc));
        AsyncOperationHandle<UnityEngine.Object> h = Addressables.LoadAssetAsync<UnityEngine.Object>(strPath);
        h.Completed += handle =>
        {
            if (h.Status == AsyncOperationStatus.Succeeded)
                SaveAddressable(h.Result, assetName, label);
            if (h.Status == AsyncOperationStatus.Failed)
                MarkAddressableForDownload(h.Result, assetName, label);
            Debug.Log(assetName);
        };
    }

        public void SaveAddressable(UnityEngine.Object obj, string assetName, string label)
    {
        TextAsset DemoTextAsset;
        DemoTextAsset = obj as TextAsset;
        if ("Track".Equals(label))
        {
            string _filePath = new StringBuilder(Application.persistentDataPath).Append("/" + "assetName" + ".rip").ToString();
            File.WriteAllBytes(_filePath, DemoTextAsset.bytes);
        }

    }

    void MarkAddressableForDownload(UnityEngine.Object obj, string assetName, string label)
    {

    }


}

