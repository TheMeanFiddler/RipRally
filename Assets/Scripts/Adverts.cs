using UnityEngine;
//using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
using System;
using System.Collections;

class Adverts : MonoBehaviour
{

    public int CoinsReward { get; set; }
    private bool? CoinsEarned = null;
    private bool? RecoveryEarned = null;
    string recoverAdUnit_ID;
    int npaValue = -1;

    private RewardedAd rewardedAd;
    void Init()
    {
        npaValue = PlayerPrefs.GetInt("npa", 0);    //non personalised ads
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { AdInitialize(initStatus); });
        recoverAdUnit_ID = "ca-app-pub-3940256099942544/5224354917";
#if UNITY_ANDROID
        recoverAdUnit_ID = "ca-app-pub-1062887910651588/8581588982";
        //This is the admob test advert:
        recoverAdUnit_ID = "ca-app-pub-3940256099942544/5224354917";
        //Comment this line out when you go live 
#endif
#if UNITY_IOS || UNITY_IPHONE
    recoverAdUnit_ID = "ca-app-pub-1062887910651588/8940681634";
    //This is the admob test advert:
    recoverAdUnit_ID = "ca-app-pub-3940256099942544/1712485313";
    //Comment this line out when you go live 
#endif

    }

    private void AdInitialize(InitializationStatus initStatus)
    {
    }

    public void PlayVideo100Coins()
    {
        Init();
        CoinsReward = 100;
        CoinsEarned = null;
        this.rewardedAd = new RewardedAd(recoverAdUnit_ID);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
        this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedCoins;
        StartCoroutine(WaitForCoins());
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddExtra("npa",npaValue.ToString()).Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void PlayVideoDoubleCoins(int coinsWon)
    {
        Init();
        CoinsReward = coinsWon;
        CoinsEarned = null;
    }

    public void PlayVideoRecover()
    {
        Init();
        CoinsReward = 0;
        RecoveryEarned = null;
        this.rewardedAd = new RewardedAd(recoverAdUnit_ID);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
        this.rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedRecovery;
        StartCoroutine(WaitForRecovery());
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

    }
    
    private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        CoinsEarned = false;
        RecoveryEarned = false;
    }

    private void RewardedAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        CoinsEarned = false;
        RecoveryEarned = false;
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        this.rewardedAd.Show();
    }

    private void HandleUserEarnedCoins(object sender, Reward args)
    {
        CoinsEarned = true;
    }

    private void HandleUserEarnedRecovery(object sender, Reward args)
    { 
        RecoveryEarned = true;
    }
    

    IEnumerator WaitForRecovery()
    {
        yield return new WaitUntil(()=>RecoveryEarned!= null);
        if (RecoveryEarned == true)
            DrivingPlayManager.Current.PlayerCarManager.Recover();
        else
            Debug.Log("No recovery");
            ClosePanel();
        yield return 0;
    }

    IEnumerator WaitForCoins()
    {
        yield return new WaitUntil(() => CoinsEarned != null);
        if (CoinsEarned == true)
        {
            ScorePanel sp = GetComponent<ScorePanel>();
            if (sp != null) sp.ShowDoubleCoins();
            GameObject goShop = GameObject.Find("pnlShop(Clone)");
            if (goShop != null)
            {
                goShop.GetComponent<ShopPanel>().AddCoins(CoinsReward);
            }
            UserDataManager.Instance.Data.Coins += CoinsReward;
            UserDataManager.Instance.SaveToFile();
        }
        yield return 0;
    }



    public void ClosePanel()
    {
        Destroy(this.gameObject);
    }
    public void Destroy()
    {
        if (rewardedAd != null)
        {
            this.rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
            this.rewardedAd.OnAdFailedToLoad -= RewardedAd_OnAdFailedToLoad;
            this.rewardedAd.OnAdFailedToShow -= RewardedAd_OnAdFailedToShow;
            this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedRecovery;
        }
        
        
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
