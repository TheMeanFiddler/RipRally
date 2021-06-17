using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Serialization;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ShopItemReader:IDisposable
{  
    private IResourceLocation _spriteLoc;
    private bool disposedValue;

    //ShopItems are id, name, type, category, cost, licence, image, Addressable
    public async Task<List<ShopItem>> ListShopItemsAsync()
    {
        Sprite _icon = null;
        //get the location of the ShopIcons AssetBundle;
        IList<IResourceLocation> IconLocations = new List<IResourceLocation>();
        Task GetLocsTask = AddressableLocationLoader.GetLocations("ShopIcon", IconLocations);
        await GetLocsTask;
        _spriteLoc = IconLocations.FirstOrDefault();// foreach(IResourceLocation l in IconLocations)
        AsyncOperationHandle<IList<Sprite>> hs = Addressables.LoadAssetsAsync<Sprite>(_spriteLoc, obj => { Debug.Log("Loaded" + obj.name); });
        //Get the ShopItems json file
        Task<TextAsset> MyTask =  Addressables.LoadAssetAsync<TextAsset>("ShopItems").Task;
        await MyTask;
        string jsn = MyTask.Result.text;
        List<ShopItemSerial> _shopItems = JsonHelper.FromJson<ShopItemSerial>(jsn).ToList();
        List<ShopItem> List = new List<ShopItem>();
        //await hs.Task;
        foreach (var i in _shopItems)
        {
            ShopItemType _type = (ShopItemType)Enum.Parse(typeof(ShopItemType), i.ShopItemType);
            if (i.Name == "CarTypeBanglia")

                _icon = hs.Result.FirstOrDefault(s => s.name == i.Name);
            //}
            else
            {
                if (_type == ShopItemType.Scenery || _type == ShopItemType.Road || _type == ShopItemType.Fence)
                    _icon = Resources.Load<Sprite>("Sprites/ToolIcons/ToolType" + i.Name);
                else if (_type == ShopItemType.Camera)
                    _icon = Resources.Load<Sprite>("Sprites/ToolIcons/CamType" + i.Name);
                else if (_type == ShopItemType.Car)
                    _icon = Resources.Load<Sprite>("Sprites/ToolIcons/CarType" + i.Name);
            }
            /*
            else
            {
                if (type == ShopItemType.Scenery || type == ShopItemType.Road || type == ShopItemType.Fence)
                    Image = Resources.Load<Sprite>("Sprites/ToolIcons/ToolType" + name);
                else if (type == ShopItemType.Camera)
                    Image = Resources.Load<Sprite>("Sprites/ToolIcons/CamType" + name);
                else if (type == ShopItemType.Car)
                {
                    if (name == "Banglia")
                        Image = ShopItemReader.GetIcon(name).Result;
                    else
                        Image = Resources.Load<Sprite>("Sprites/ToolIcons/CarType" + name);
                }
            }
            */
            List.Add(new ShopItem(_type, i.Name, i.Category, i.Licence, i.Cost, i.Addressable, _icon));
        }
        return List;
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ShopItemReader()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

[Serializable]
public class ShopItemSerial
{
    public int Id;
    public string ShopItemType;
    public string Name;
    public string Category;
    public int Licence;
    public int Cost;
    [OptionalField]
    public bool Addressable = true;
}

