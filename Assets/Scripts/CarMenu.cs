using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

class CarMenu: BumpMenu
{

    void Start()
    {
        Init();
        GetData();
        base.PopulateMenu<CarMenuItem>();
        MenuItems[0].Select();
        SelectedItem = MenuItems[0];
        NextItem = MenuItems.Count==1?null:MenuItems[1];
        PrevItem = null;
    }

    public override void Init()
    {
        base.Init();
        mainscript = Main.Instance;

        //GamePanelController gpc = new GamePanelController(this, nwm);
        PlayerManager.Type = "CarPlayer";
    }

    public override void GetData()
    {
        data = new List<MenuDataItem>();
        foreach (int Itm in UserDataManager.Instance.Data.Purchases)
        {
            if (Itm==35)
                data.Add(new MenuDataItem { Id = 0, Name = "Car", Type = mainscript.Color });
            if (Itm == 36)
                data.Add(new MenuDataItem { Id = 1, Name = "Hotrod", Type = mainscript.Color });
            if (Itm == 37)
                data.Add(new MenuDataItem { Id = 2, Name = "Anglia", Type = mainscript.Color });
            if (Itm == 38)
                data.Add(new MenuDataItem { Id = 3, Name = "Porsche", Type = mainscript.Color });
        }
    }

    public override void PopulateMenu<T>()
    {
        base.PopulateMenu<T>();
    }




    //Called when you click one of the colour buttons:
    public void ChangeColor(string color)
    {
        SelectedItem.SetType(color);
    }
}


