using UnityEngine;
using UnityEngine.Advertisements;

class Adverts : MonoBehaviour, IUnityAdsListener
{

    public int CoinsReward { get; set; }
    public bool Recover { get; set; }

    void Start()
    {
#if UNITY_ANDROID
        Advertisement.Initialize("1490467", false);
#endif
#if UNITY_IOS || UNITY_IPHONE
        Advertisement.Initialize("1490468", false);
#endif
        Advertisement.AddListener(this);
    }

public void PlayVideo100Coins()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            CoinsReward = 100;
            Advertisement.Show("rewardedVideo");
        }
        else
        {
            Main.Instance.PopupMsg("Sorry offline no advert");
        }
    }

    public void PlayVideoDoubleCoins(int coinsWon)
    {

        if (Advertisement.IsReady("rewardedVideo"))
        {
            CoinsReward = coinsWon;
            Advertisement.Show("rewardedVideo");
        }
        else
        {
            Main.Instance.PopupMsg("Sorry offline no advert");
        }
    }

    public void PlayVideoRecover()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            CoinsReward = 0;
            Recover = true;
            Advertisement.Show("rewardedVideo");
        }
        else
        {
            Main.Instance.PopupMsg("Sorry offline no advert");
        }
    }


    public void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == "rewardedVideo")
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
       Debug.Log("Ad Errored");
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        Debug.Log("Ad Started");
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }

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

    public void ClosePanel()
    {
        Destroy(this.gameObject);
    }
}