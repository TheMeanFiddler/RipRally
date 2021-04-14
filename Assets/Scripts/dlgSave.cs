using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System;

public class dlgSave:MonoBehaviour
{
    RectTransform pnlInvoice;
    PlaceableObject[] Chargeables;
    public SavedGameMenuItem MenuItemToSave { get; set; }
    int _totalCost = 0;
    GameObject pnlShortfall;
    GameObject pnlSave;


    void Start()
    {

        pnlShortfall = transform.Find("pnlShortfall").gameObject;
        pnlSave = transform.Find("pnlSave").gameObject;

        UnityEngine.Object objInvoiceItem = Resources.Load("Prefabs/InvoiceItem");
        
        pnlInvoice = transform.Find("ScrollView/Mask/Grid").GetComponent<RectTransform>();

        foreach(ShopItem i in BillOfRoadMaterials.Items)
        {
            GameObject goLine = (GameObject)GameObject.Instantiate(objInvoiceItem, Vector3.zero, Quaternion.identity, pnlInvoice);
            UnityEngine.Object objSprite = Resources.Load("Sprites/ToolIcons/ToolType" + i.Name);
            Sprite S = Sprite.Create((Texture2D)objSprite, new Rect(0, 0, 40, 40), Vector2.zero);
            goLine.transform.Find("Image").GetComponent<Image>().sprite = S;
            goLine.transform.Find("txtCost").GetComponent<Text>().text = i.Cost.ToString();
            _totalCost += i.Cost;
        }

        foreach (PlaceableObject Ch in BillOfSceneryMaterials.Items)
        {
            GameObject goLine = (GameObject)GameObject.Instantiate(objInvoiceItem, Vector3.zero, Quaternion.identity, pnlInvoice);
            UnityEngine.Object objSprite = Resources.Load("Sprites/ToolIcons/ToolType" + Ch.Opt.Name);
            Sprite S = Sprite.Create((Texture2D)objSprite, new Rect(0, 0, 40, 40), Vector2.zero);
            goLine.transform.Find("Image").GetComponent<Image>().sprite = S;
            goLine.transform.Find("txtCost").GetComponent<Text>().text = Ch.Opt.Cost.ToString();
            _totalCost += Ch.Opt.Cost;
        }
        transform.Find("txtTotalCost").GetComponent<Text>().text = _totalCost.ToString();
        if(_totalCost > UserDataManager.Instance.Data.Coins)
        {
            pnlShortfall.SetActive(true);
            pnlSave.SetActive(false);
        }
        else
        {
            pnlShortfall.SetActive(false);
            pnlSave.SetActive(true);
            if (_totalCost == 0) transform.Find("pnlSave/txtSave").GetComponent<Text>().text = "Save these changes for free";
        }

        ShowCoins();
    }

    private void ShowCoins() { 
        try
        {
            transform.Find("btnCoins/txtCoins").GetComponent<Text>().text = UserDataManager.Instance.Data.Coins.ToString();
        }
        catch { }
    }

    public void Save()
    {
        SaveLoadModel.Save();
        UserDataManager.Instance.Data.Coins -= _totalCost;
        UserDataManager.Instance.SaveToFile();
        foreach(iRoadSectn s in Road.Instance.Sectns.Where(s => s.Chargeable == true)) { s.Chargeable = false; }
        foreach (PlaceableObject Ch in BillOfSceneryMaterials.Items) { Ch.Chargeable = false; }
        Main.Instance.ShowCoins();
        if(MenuItemToSave!=null) MenuItemToSave.EnableSaveButton(false);
        Destroy(this.gameObject);
        
    }

    public void BackToGame()
    {
        Destroy(this.gameObject);
    }
}

