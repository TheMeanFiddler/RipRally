using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

class SavedGameMenu : SpinMenu
{

    private Button btnChooseCar;
    public override void Init()
    {
        base.Init();
        mainscript = Main.Instance; // goMain.GetComponent<Main>();
        btnChooseCar = GameObject.Find("btnCar").GetComponent<Button>();
        SaveLoadModel.GetSavedGamesList();
        GetData();
        PopulateMenu<SavedGameMenuItem>();
        mainscript.ShowCoins();
    }

    public override void GetData()
    {
        data = new List<IMenuDataItem>();
        if (SaveLoadModel.savedGames.Count == 0) GetDemoTrack();    //First time - put in the demo track
        foreach (Game g in SaveLoadModel.savedGames)
        {
            MenuDataItem i = new MenuDataItem { Id = g.GameId, Name = g.Filename, IsMine = g.IsMine };
            data.Add(g);
        }
    }

    public override void PopulateMenu<T>()
    {
        //Adds all the data driven menu items
        ExtraItemAtStart = true;



        //Adds the NewTrack Menu Item, then call the base PopulateMenu
        IMenuItem FirstItm = new T();
        MenuItems.Add(FirstItm);
        FirstItm.ContainingMenu = this;
        FirstItm.CreateGameObjects("New Track", "");        //This one will also set the caption
        Transform trFirstItem = FirstItm.OuterGameObject.transform;
        trFirstItem.SetParent(this.transform);
        trFirstItem.localPosition = -Vector3.forward * 900;
        trFirstItem.localScale = Vector3.one * 3;
        FirstItm.Id = -1;
        Angles.Add(0);
        mainscript.SelectedGameID = -1;
        FirstItm.Permanent = true;
        FirstItm.ScrollIdx = 0;

        base.PopulateMenu<T>();

        int MenuId;

        if (Game.current != null)
        {
            MenuId = MenuItems.IndexOf(MenuItems.Find(m => m.Id == Game.current.GameId));
            SnapTo(MenuId);
            if (Game.current.Dirty)
            {
                SavedGameMenuItem i = (SavedGameMenuItem)SelectedItem;
                i.EnableSaveButton(true);
            }
        }
        else
        {
            MenuId = MenuItems.IndexOf(MenuItems.Find(m => m.Id == -1));
            SnapTo(MenuId);
        }
    }

    public override void Select(IMenuItem itm)
    {
        mainscript.SelectedGameID = itm.Id;
        btnChooseCar.interactable = (itm.Id != -1);
        base.Select(itm);
    }

    void GetDemoTrack()
    {
        SaveLoadModel.SaveDemoTrack();
        Game DemoGame = new Game { Filename = "Demo Track" };
        SaveLoadModel.savedGames.Add(DemoGame);
    }


    public void BuildTrack()
    {
        if (SelectedItem == null) return;
        if (SelectedItem.Id == -1)
            Main.Instance.StartLoadingMenuScene("SceneSelector");
        else
        {           //N.B. the SaveLoadModel decides whether to load the game or keep the current one
            if (Game.current != null)
            {
                if (SelectedItem.Id != Game.current.GameId)
                {
                    //TerrainController.Instance.RestoreBackup();
                }
            }
            PlayerManager.Type = "BuilderPlayer";
            SaveLoadModel.LoadGameData(SelectedItem.Id);
            Main.Instance.StartLoadingGameScene(GameData.current.Scene); // Once the scene is loaded it activates and moves the EventSystem across
        }
    }
    public void ChooseCar()
    {
        PlayerManager.Type = "CarPlayer";
        SaveLoadModel.LoadGameData(SelectedItem.Id);
        if (GameData.current.RdS.Sectns == null) { Main.Instance.PopupMsg("This track has no road"); return; }
        Main.Instance.StartLoadingMenuScene("VehicleSelector");
    }
}

