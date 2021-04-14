using System;
using System.Collections.Generic;
using UnityEngine;


public interface IGamePanelController
{

}

class GamePanelController : IGamePanelController
{
    IMenuListView _gpv;

    public GamePanelController(IMenuListView gpv)
    {
        _gpv = gpv;
        SaveLoadModel.GetSavedGamesList();
        _gpv.GetData();
        _gpv.PopulateMenu<SavedGameMenuItem>();
    }


}

