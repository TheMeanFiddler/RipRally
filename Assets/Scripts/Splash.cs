using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    AsyncOperation mao;
    bool _musicDone;
    bool _gDPRDone;
    void Start()
    {
        MusicPlayer mp = MusicPlayer.Instance;
        mp.OnLoadResult += Mp_OnLoadResult;

        StartCoroutine(LoadMenu());
    }

    private void Mp_OnLoadResult(bool Success)
    {
        _musicDone = true;

    }

    IEnumerator LoadMenu()
    {
        PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("npa"))
            _gDPRDone = true;
        else
        {
            yield return new WaitForSeconds(1);
            transform.Find("pnlConsent").gameObject.SetActive(true);
        }
        yield return new WaitUntil(() => _musicDone && _gDPRDone);
        SceneManager.LoadScene("TrackSelector");
        Adverts.Instance.LoadAd();
        yield return null;
    }

    public void GDPRDecision(bool Agree)
    {
        _gDPRDone = true;
        
         if (Agree)
            PlayerPrefs.SetInt("npa", 0);
        else
            PlayerPrefs.SetInt("npa", 1);
        PlayerPrefs.Save();
        transform.Find("pnlConsent").gameObject.SetActive(false);
    }

    public void ClickPrivacyPolicy()
    {
        Application.OpenURL("www.google.com");
    }
}

