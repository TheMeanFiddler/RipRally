using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

class Adverts : MonoBehaviour
{ 
    public int CoinsReward { get; set; }
    public bool Recover { get; set; }
    private RewardedAd rewardedAd;
    string recoverAdUnit_ID;

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
#if UNITY_ANDROID
        recoverAdUnit_ID = "ca-app-pub-1062887910651588/8581588982";
#endif
#if UNITY_IOS || UNITY_IPHONE
        recoverAdUnit_ID = "ca-app-pub-1062887910651588/8940681634";
#endif
        //this is the admob test advert
        recoverAdUnit_ID = "ca-app-pub-3940256099942544/1712485313";
    }



    public void PlayVideo100Coins()
    {

            CoinsReward = 100;
    }

    public void PlayVideoDoubleCoins(int coinsWon)
    {


            CoinsReward = coinsWon;


    }

    public void PlayVideoRecover()
    {
        Main.Instance.PopupMsg("recover");
        CoinsReward = 0;
        Recover = true;
        this.rewardedAd = new RewardedAd(recoverAdUnit_ID);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
        this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Main.Instance.PopupMsg("Ad Failed to Show");
    }

    private void RewardedAd_OnAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Main.Instance.PopupMsg("Faild Load " + e.Message);
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Main.Instance.PopupMsg("show");
        this.rewardedAd.Show();
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        Debug.Log(type);
        switch (type)
        {
            case "Reward":
                Debug.Log(type);
                DrivingPlayManager.Current.PlayerCarManager.Recover();
                Recover = false;
                break;
        }
    }


    /*
    public void OnUnityAdsDidFinish(string surfacingId, ShowResult result)
    {
        Debug.Log("Ad finished");
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown." + "CoinsReward" + CoinsReward);
                if (CoinsReward > 0)
                {
                    ScorePanel sp = GetComponent<ScorePanel>();
                    if (sp != null) sp.ShowDoubleCoins();
                    GameObject goShop = GameObject.Find("pnlShop(Clone)");
                    if (goShop != null) {
                        goShop.GetComponent<ShopPanel>().AddCoins(CoinsReward);
                    }
                    UserDataManager.Instance.Data.Coins += CoinsReward;
                    UserDataManager.Instance.SaveToFile();
                }
                if (Recover)
                {
                    Main.Instance.PopupMsg("Ad Watched");
                    DrivingPlayManager.Current.PlayerCarManager.Recover();
                    Recover = false;
                }
                break;
            case ShowResult.Skipped:
                if (Recover)
                {
                    Main.Instance.PopupMsg("Ad Skipped");
                    DrivingPlayManager.Current.PlayerCarManager.DontRecover();
                    Recover = false;
                }
                break;
            case ShowResult.Failed:
                if (Recover)
                {
                    Main.Instance.PopupMsg("Ad Failed");
                    DrivingPlayManager.Current.PlayerCarManager.DontRecover();
                    Recover = false;
                }
                break;
        }
        Recover = false;
    }
    */
    public void ClosePanel()
    {
        this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        Destroy(this.gameObject);
    }
}

/*
Set up app-ads.txt for your apps
=================================
The app-ads.txt initiative helps fight fraud and safeguard your app ads earnings. Use these instructions to set up app-ads.txt for your AdMob apps.

If you haven't already, create an app-ads.txt file using the spec provided by IAB Tech Lab.
Copy and paste the following code snippet into your app-ads.txt file:
google.com, pub-1062887910651588, DIRECT, f08c47fec0942fa0
content_copy
Publish your app-ads.txt on the root of your developer website (for example, sampledomain.com/app-ads.txt). Make sure the domain is entered exactly as listed on Google Play or the App Store.
Wait at least 24 hours for AdMob to crawl and verify your app-ads.txt file.
Open AdMob, go to your apps, and click the app-ads.txt tab to check your app-ads.txt status.

Setting Test Devices
======================
I've set up iPhone 6 as a test device. Havent done Galaxy s8 yet
 
Ads in test mode
================
Ads served by the AdMob Network will display a label that lets you know you’re in test mode. Look for the test mode label before clicking on an ad. Clicking on production ads can result in a policy violation for invalid traffic.

Using mediation and test devices
-----------------------
If you’re using AdMob mediation or Open Bidding, test mode applies to ads served from the AdMob Network only and does not apply to ads served by other networks. 

Be sure that you've instructed your third-party ad source will serve ads in test mode. 
     */
