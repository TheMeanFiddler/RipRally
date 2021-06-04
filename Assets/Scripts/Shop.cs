using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public static class Shop
{
    public static List<ShopItem> Items;

    public static void FillItems()
    {
        Items = new List<ShopItem>();
        Items.Add(new ShopItem(ShopItemType.Road, "Tarmac", "", 0, 5));                     //0
        Items.Add(new ShopItem(ShopItemType.Road, "Washboard", "", 100, 5));                //1
        Items.Add(new ShopItem(ShopItemType.Road, "DirtyRoad", "", 100, 5));                //2
        Items.Add(new ShopItem(ShopItemType.Road, "Dirt", "", 100, 5));                     //3
        Items.Add(new ShopItem(ShopItemType.Road, "Air", "", 100, 0));                      //4
        Items.Add(new ShopItem(ShopItemType.Fence, "Fence0", "", 0, 0));                    //5
        Items.Add(new ShopItem(ShopItemType.Fence, "Fence1", "", 0, 0));                    //6
        Items.Add(new ShopItem(ShopItemType.Fence, "Fence2", "", 100, 0));                  //7
        Items.Add(new ShopItem(ShopItemType.Scenery, "Tree", "Nature", 0, 50));             //8
        Items.Add(new ShopItem(ShopItemType.Scenery, "Rock1", "Nature", 100, 20));          //9
        Items.Add(new ShopItem(ShopItemType.Scenery, "WoodStrut", "Buildings", 200, 50));   //10
        Items.Add(new ShopItem(ShopItemType.Scenery, "CurvedRamp", "Gadgets", 400, 100));   //11
        Items.Add(new ShopItem(ShopItemType.Scenery, "Spectators", "People", 2000, 200 ));  //12
        Items.Add(new ShopItem(ShopItemType.Scenery, "RomanArch", "Buildings", 300, 100));  //13
        Items.Add(new ShopItem(ShopItemType.Scenery, "SeeSaw", "Gadgets", 1000, 500));      //14
        Items.Add(new ShopItem(ShopItemType.Scenery, "Platform", "Gadgets", 200, 50));      //15
        Items.Add(new ShopItem(ShopItemType.Camera, "Follow Cam", "Cameras", 0, 0));        //16
        Items.Add(new ShopItem(ShopItemType.Camera, "Wheelarch Cam", "Cameras", 400, 0));   //17
        Items.Add(new ShopItem(ShopItemType.Camera, "Driver Cam", "Cameras", 600, 0));     //18
        Items.Add(new ShopItem(ShopItemType.Camera, "Zoom Cam", "Cameras", 800, 0));        //19
        Items.Add(new ShopItem(ShopItemType.Camera, "Drone Cam", "Cameras", 800, 0)); //20
        Items.Add(new ShopItem(ShopItemType.Camera, "RoadRunner Cam", "Cameras", 1000, 0));      //21
        Items.Add(new ShopItem(ShopItemType.Camera, "Hedgehog Cam", "Cameras", 1000, 0));   //22
        Items.Add(new ShopItem(ShopItemType.Camera, "Side Cam", "Cameras", 200, 0));       //23
        Items.Add(new ShopItem(ShopItemType.Scenery, "Loop", "Gadgets", 1400, 700));        //24
        Items.Add(new ShopItem(ShopItemType.Fence, "StoneWall", "", 200, 0));               //25
        Items.Add(new ShopItem(ShopItemType.Scenery, "PineTree", "Nature", 100, 50));       //26
        Items.Add(new ShopItem(ShopItemType.Scenery, "SuspensionBridge", "Buildings", 300, 200));       //27
        Items.Add(new ShopItem(ShopItemType.Scenery, "Sign Left Bend", "Buildings", 0, 2));       //28
        Items.Add(new ShopItem(ShopItemType.Scenery, "Sign Left Hairpin", "Buildings", 0, 2));       //29
        Items.Add(new ShopItem(ShopItemType.Scenery, "Sign Right Bend", "Buildings", 0, 2));       //30
        Items.Add(new ShopItem(ShopItemType.Scenery, "Sign Right Hairpin", "Buildings", 0, 2));       //31
        Items.Add(new ShopItem(ShopItemType.Scenery, "Plank", "Buildings", 20, 20));       //32
        Items.Add(new ShopItem(ShopItemType.Scenery, "Rock2", "Nature", 100, 20));          //33
        Items.Add(new ShopItem(ShopItemType.Camera, "Ice Cam", "Cameras", 1000, 0)); //34
        Items.Add(new ShopItem(ShopItemType.Car, "Slocus", "Car", 600, 0)); //35
        Items.Add(new ShopItem(ShopItemType.Car, "HotRod", "Car", 0, 0)); //36
        Items.Add(new ShopItem(ShopItemType.Car, "Banglia", "Car", 500, 0)); //37
        Items.Add(new ShopItem(ShopItemType.Car, "Push", "Car", 700, 0)); //38
        Items.Add(new ShopItem(ShopItemType.Scenery, "House 1", "Buildings", 20, 20));       //39
        Items.Add(new ShopItem(ShopItemType.Track, "Tenner", "Tracks", 500, 0));       //40
        Items.Add(new ShopItem(ShopItemType.Track, "Flat Rabbit", "Tracks", 500, 0));       //41
        FillAddressableItems();
    }


    public static async Task FillAddressableItems()
    {
        List<ShopItemSerial> Itms = await ShopItemReader.ListtShopItems();
        Debug.Log("Here");
        //await AddressableLocationLoader.GetLocations("Tracks", Locations);
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
}
    public class ShopItem
{
    public int Id { get; set; }
    public ShopItemType Type { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public int Licence { get; set; }
    public int Cost { get; set; }
    public Sprite Image { get; set; }

    //constructor
    public ShopItem(ShopItemType type, String name, string category, int licence, int cost)
    {
        Id = Shop.Items.Count;
        Type = type;
        Name = name;
        Category = category;
        Licence = licence;
        Cost = cost;
        if (type == ShopItemType.Scenery || type == ShopItemType.Road || type == ShopItemType.Fence)
            Image = Resources.Load<Sprite>("Sprites/ToolIcons/ToolType" + name);
        else if (type == ShopItemType.Camera)
            Image = Resources.Load<Sprite>("Sprites/ToolIcons/CamType" + name);
        else if (type == ShopItemType.Car)
            Image = Resources.Load<Sprite>("Sprites/ToolIcons/CarType" + name);

    }

    public static explicit operator ToolOption(ShopItem itm)    // so we can convert ShopItems into ToolOptions
    {
        ToolOption Opt = new ToolOption { Name = itm.Name, Cost = itm.Cost, Image = itm.Image };
        switch (itm.Type)
        {
            case ShopItemType.Scenery:
                Opt.Type = ToolboxController.ToolType.Scenery;
                break;
            case ShopItemType.Fence:
                Opt.Type = ToolboxController.ToolType.Fence;
                break;
            case ShopItemType.Road:
                Opt.Type = ToolboxController.ToolType.Road;
                break;
        }
        return Opt;
    }
}


public enum ShopItemType { Scenery, Fence, Road, Car, Camera, Track }

