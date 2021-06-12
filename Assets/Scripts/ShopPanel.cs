using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Assets.Scripts;

public class ShopPanel:MonoBehaviour
{
    Transform trContentPanel;
    UnityEngine.Object objShopItem;
    List<ShopItem> Basket;
    GameObject btnPayNow;
    GameObject btnShowBasket;
    GameObject btnBackToShop;
    GameObject btnBackToGame;
    GameObject btnBackToGame2;
    GameObject btnGetCoins;
    GameObject pnlGetCoins;
    GameObject txtInstructions;
    GameObject pnlToggles;
    Text txtTotal;
    private ShopItemType _typeFilter;

    void Awake()
    {
        trContentPanel = transform.Find("scrlToys/Mask/Content");
        btnPayNow = transform.Find("btnPayNow").gameObject;
        btnGetCoins = transform.Find("btnGetCoins").gameObject;
        btnShowBasket = transform.Find("btnShowBasket").gameObject;
        btnBackToShop = transform.Find("btnBackToShop").gameObject;
        btnBackToGame = transform.Find("btnBackToGame").gameObject;
        btnBackToGame2 = transform.Find("btnBackToGame2").gameObject;
        txtInstructions = transform.Find("txtInstructions").gameObject;
        txtTotal = transform.Find("pnlToys/txtTotal").GetComponent<Text>();
        pnlToggles = transform.Find("pnlToys/Toggles").gameObject;
        objShopItem = Resources.Load("Prefabs/pnlShopItem");
        Basket = new List<ShopItem>();
        ShowCoins();
    }

    public void SwitchToggle(String TogName)
    {
        transform.Find("pnlToys/Toggles/tgl" + TogName).GetComponent<Toggle>().isOn = true;
    }

    public void SetTrackFilter(bool IsOn)
    {
        if (IsOn)
        { _typeFilter = ShopItemType.Track; ShowShopItems(); }
    }
    public void SetRoadFilter(bool IsOn)
    {
        if (IsOn)
        { _typeFilter = ShopItemType.Road; ShowShopItems();}
    }
    public void SetFenceFilter(bool IsOn)
    {
        if (IsOn)
        { _typeFilter = ShopItemType.Fence; ShowShopItems();}
    }
    public void SetSceneryFilter(bool IsOn)
    {
        if(IsOn)
        { _typeFilter = ShopItemType.Scenery; ShowShopItems();}
    }
    public void SetCarFilter(bool IsOn)
    {
        if (IsOn)
        { _typeFilter = ShopItemType.Car; ShowShopItems(); }
    }
    public void SetCamFilter(bool IsOn)
    {
        if (IsOn)
        { _typeFilter = ShopItemType.Camera; ShowShopItems(); }
    }


    public void ShowShopItems()
    {
        ClearScrollView();
        foreach (ShopItem itm in Shop.Items.Where(i => i.Type == _typeFilter).OrderBy(i=>i.Licence))
        {
            GameObject goShopItem = (GameObject)Instantiate(objShopItem, trContentPanel);
            goShopItem.transform.localScale = Vector3.one;
            goShopItem.transform.Find("txtName").GetComponent<Text>().text = itm.Name;
            goShopItem.transform.Find("Image").GetComponent<Image>().sprite = itm.Image;
            goShopItem.transform.Find("btnBuy/txtBuy").GetComponent<Text>().text = "Buy <color=#ffaa00ff>$ " + itm.Licence.ToString() + "</color>";
            goShopItem.transform.Find("txtCost").GetComponent<Text>().text = itm.Cost.ToString();
            int _itmId = itm.Id;
            if (Basket.Where(b => b.Id == _itmId).Count() != 0) { goShopItem.transform.Find("btnBuy").gameObject.SetActive(false); }
            if(UserDataManager.Instance.Data.Purchases.Where(p=>p==_itmId).Count()!=0) { goShopItem.transform.Find("btnBuy").gameObject.SetActive(false); goShopItem.transform.Find("txtAdded").gameObject.SetActive(false); goShopItem.transform.Find("txtAddedShadow").gameObject.SetActive(false); }
            else { goShopItem.transform.Find("btnBuy").GetComponent<Button>().onClick.AddListener(delegate { AddToBasket(_itmId); }); }
        }
        txtTotal.text = BasketTotal().ToString();
        btnPayNow.SetActive(false);
        btnBackToShop.SetActive(false);
        btnBackToGame.SetActive(true);
        btnBackToGame2.SetActive(false);
        btnGetCoins.SetActive(false);
        btnShowBasket.SetActive(true);
        pnlToggles.SetActive(true);
        if (BasketTotal() > UserDataManager.Instance.Data.Coins)
        {
            txtInstructions.GetComponent<Text>().text = "You need to get more coins";
        }
        else
        {
            txtInstructions.GetComponent<Text>().text = "";
        }
    }

    private void AddToBasket(int _itmId)
    {
        Basket.Add(Shop.Items[_itmId]);
        ShowShopItems();
    }

    public void ShowBasket()
    {
        ClearScrollView();
        pnlToggles.SetActive(false);
        foreach (ShopItem itm in Basket)
        {
            GameObject goShopItem = (GameObject)Instantiate(objShopItem, trContentPanel);
            goShopItem.transform.localScale = Vector3.one;
            goShopItem.transform.Find("txtName").GetComponent<Text>().text = itm.Name;
            goShopItem.transform.Find("Image").GetComponent<Image>().sprite = itm.Image;
            goShopItem.transform.Find("btnBuy/txtBuy").GetComponent<Text>().text = "Remove";
            goShopItem.transform.Find("txtCost").GetComponent<Text>().text = itm.Cost.ToString();
            goShopItem.transform.Find("btnBuy").GetComponent<Button>().onClick.AddListener(delegate { RemoveFromBasket(itm); });
        }
        txtTotal.text = BasketTotal().ToString();
        if (BasketTotal() > UserDataManager.Instance.Data.Coins)
        {
            btnGetCoins.SetActive(true);
            btnPayNow.SetActive(false);
        }
        else
        {
            btnGetCoins.SetActive(false);
            btnPayNow.SetActive(true);
        }
        btnBackToGame.SetActive(false);
        if (Basket.Count == 0)
        {
            btnPayNow.SetActive(false);
        }
        btnBackToShop.SetActive(true);
        btnShowBasket.SetActive(false);
        if (BasketTotal() < UserDataManager.Instance.Data.Coins)
        { 
            txtInstructions.GetComponent<Text>().text = "You have $ " + (UserDataManager.Instance.Data.Coins - BasketTotal()).ToString() + " left to spend";
        }
    }

    private void ClearScrollView()
    {
        while (trContentPanel.childCount > 0)
        {
            Transform child = trContentPanel.GetChild(0);
            child.Find("btnBuy").GetComponent<Button>().onClick.RemoveAllListeners();
            child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }
    }

    private void RemoveFromBasket(ShopItem itm)
    {
        Basket.Remove(itm);
        ShowBasket();
    }

    private int BasketTotal()
    {
        return Basket.Sum(b => b.Licence);
    }

    public void PayNow()
    {
        UserDataManager.Instance.Data.Coins -= BasketTotal();
        foreach (ShopItem itm in Basket)
        {
            if (itm.Type == ShopItemType.Track)
            {
                GetComponent<PurchaseDownloader>().StartDownload(itm.Name, "Track");
            }
            UserDataManager.Instance.Data.Purchases.Add(itm.Id);
        }
        UserDataManager.Instance.SaveToFile();
        ClearScrollView();
        try { transform.GetComponentInParent<ToolboxController>().LoadToolOptions(); if(Basket.Count>0) Main.Instance.PopupMsg("New toys added", Color.red); } catch { }
        try {GameObject.Find("ReplayCamController").GetComponent<ReplayCamControllerFactory>().GetPurchasedCameras();} catch { }
        Basket.Clear();
        Destroy(this.gameObject);
    }



    private void ShowCoins()
    {
        try
        {
            transform.Find("btnCoins/txtCoins").GetComponent<Text>().text = UserDataManager.Instance.Data.Coins.ToString();
        }
        catch { }
    }

    public void AddCoins(int numCoins)
    {
        int _prevCoins = UserDataManager.Instance.Data.Coins;
        StartCoroutine(CountCoins(_prevCoins, _prevCoins + numCoins));
    }

    IEnumerator CountCoins(int prevCoins, int newCoins)
    {
        Text t = transform.Find("btnCoins/txtCoins").GetComponent<Text>();
        int incr = (newCoins - prevCoins) / 100;
        for (float c = prevCoins; c <= newCoins; c += incr)
        {
            t.text = c.ToString();
            yield return null;
        }
        yield break;
    }

    public void ShowGetCoins()
    {
        UnityEngine.Object objPnl = Resources.Load("Prefabs/pnlGetCoins");
        pnlGetCoins = (GameObject)GameObject.Instantiate(objPnl, Vector3.zero, Quaternion.identity, this.transform);
        pnlGetCoins.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void BackToGame()
    {
        Destroy(this.gameObject);
    }
}

