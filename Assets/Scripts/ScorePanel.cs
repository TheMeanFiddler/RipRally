using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePanel:MonoBehaviour
{
    DrivingMenuController _dmc;
    int TotalScore;
    int PrevCoins;
    int NewCoins;

    void Start()
    {
        _dmc = GetComponentInParent<Canvas>().GetComponent<DrivingMenuController>();
        _dmc.ShowPanel(false);
        transform.Find("ReplayButton").GetComponent<Button>().onClick.AddListener(() => Replay());
        transform.Find("MenuButton").GetComponent<Button>().onClick.AddListener(() => ShowMenuPanel());
    }

    public void ShowScore(iRace race)
    {
        transform.Find("ptsLapRec").GetComponent<Text>().text = race.LapRecBonus.ToString();
        transform.Find("ptsHog").GetComponent<Text>().text = race.HogBonus.ToString();
        transform.Find("ptsDrift").GetComponent<Text>().text = race.DriftBonus.ToString();
        transform.Find("ptsAir").GetComponent<Text>().text = race.AirBonus.ToString();
        float Mult = Road.Instance.XSecs.Count * (float)race.Laps / 6000f;
        float MultRounded = Mathf.RoundToInt(Mult * 10) / 10f;
        transform.Find("ptsMult").GetComponent<Text>().text = "X " + MultRounded.ToString();
        TotalScore = Mathf.RoundToInt((race.LapRecBonus + race.HogBonus + race.DriftBonus + race.AirBonus) * MultRounded);
        transform.Find("ptsTotal").GetComponent<Text>().text = TotalScore.ToString();
        PrevCoins = UserDataManager.Instance.Data.Coins;
        NewCoins = PrevCoins + TotalScore;
        transform.Find("btnCoins/txtCoins").GetComponent<Text>().text = PrevCoins.ToString();
        UserDataManager.Instance.Data.Coins = NewCoins;
        UserDataManager.Instance.SaveToFile();
        StartCoroutine(CountCoins());
    }

    IEnumerator CountCoins()
    {
        for (float c = PrevCoins; c <= NewCoins; c+=0.5f)
        {
            int cn = Mathf.RoundToInt(c);
            transform.Find("btnCoins/txtCoins").GetComponent<Text>().text = cn.ToString();
            yield return null;
        }
        yield break;
    }
    public void DoubleCoins_Click()
    {
        GetComponent<Adverts>().PlayVideoDoubleCoins(TotalScore);
        transform.Find("pnlDouble").gameObject.SetActive(false);
    }

    public void ShowDoubleCoins()
    {
        Debug.Log("ShowDOubleCoins");
        PrevCoins = NewCoins;
        NewCoins = PrevCoins + TotalScore;
        StartCoroutine(CountCoins());
    }

    public void DoubleCoinsNoThanks_Click()
    {
        transform.Find("pnlDouble").gameObject.SetActive(false);
    }

    void Replay()
    {
        _dmc.ReplayButtonClick();
        Destroy(this.gameObject);
    }

    void ShowMenuPanel()
    {
        _dmc.ShowPanel(true);
        _dmc.ShowMenuPanel(true);
        Destroy(this.gameObject);
    }


}

