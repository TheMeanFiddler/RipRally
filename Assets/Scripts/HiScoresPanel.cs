using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HiScoresPanel:MonoBehaviour
{

    List<HiScore> L;
    HiScore _hs;
    int _rank;
    Object HiScorePrefab;
    RectTransform trPanel;
    Transform _myTransform;
    Transform _trTrophy;
    LapStat _bestLap;

    public void Init(float totTime)
    {
        if (GameData.current.HiScores == null) GameData.current.HiScores = new List<HiScore>();
        L = GameData.current.HiScores;
        DrivingPlayManager.Current.ShowDrivingGUI(false);
        DrivingPlayManager.Current.ShowInputGUI(false);
        HiScorePrefab = Resources.Load("Prefabs/pnlHiScore");
        trPanel = transform.Find("pnlHiScoreList").GetComponent<RectTransform>();
        _trTrophy = transform.parent.Find("Trophy");
        _trTrophy.gameObject.SetActive(false);
        float _segTime = totTime / Race.Current.Laps / Road.Instance.Segments.Count;
        _rank = GetRank(_segTime);
        _hs = new HiScore();
        _hs.SegTime = _segTime;
        InsertHiScore(_hs);
        ShowBestLap();
        StartCoroutine(Darken());
        StartCoroutine (ShowHiScores());
    }

    public int GetRank(float t)
    {
        int rank = 1;
            int len = L.Count();
        if (len > 0)
        {
            for (int n = 0; n < len; n++) { if (t < L[n].SegTime) { rank = n + 1; break; } }
            if (t > L.Last().SegTime) rank = len + 1;
        }
        return rank;
    }

    private void InsertHiScore(HiScore _hs)
    {
        if (L.Count == 0)
        { L.Add(_hs); }
        else
        { L.Insert(_rank - 1, _hs); }
    }

    IEnumerator Darken()
    {
        Image i = GetComponent<Image>();
        float alpha = 0;
        while (alpha < 0.5f)
        {
            alpha += 0.02f;
            i.color = new Color(0, 0, 0, alpha);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
     }

    IEnumerator ShowHiScores()
    {
        int _startRank = _rank < 11 ? 1 : _rank - 4;
        float pos = trPanel.rect.min.y;
        float ScrHght = trPanel.rect.height;
        int ScrWdth = Screen.width;
        float ScoreHght = ScrHght / 11;
        yield return new WaitForSecondsRealtime(2);
        for(int n = 9; n >= 0 ; n-- )
        {
            int r = n + _startRank;
            GameObject goHiScore = (GameObject)GameObject.Instantiate(HiScorePrefab, trPanel);
            RectTransform tr = goHiScore.GetComponent<RectTransform>();
            tr.anchorMin = new Vector3(0.5f, 0);
            tr.anchorMax = new Vector3(0.5f, 0);
            tr.anchoredPosition = Vector2.zero;
            tr.localPosition = new Vector3(pos, pos, 0);
            tr.localScale = Vector3.one;
            tr.sizeDelta = new Vector2(0, ScoreHght);
            tr.Find("txtRank").GetComponent<Text>().text = r.ToString();
            if (r <= L.Count)
            {
                tr.Find("txtName").GetComponent<Text>().text = L[r-1].Name;
                tr.Find("txtTime").GetComponent<Text>().text = GenFunc.HMS(L[r - 1].SegTime*Road.Instance.Segments.Count);
            }
            if(r != _rank)
            {
                tr.Find("ipName").gameObject.SetActive(false);
            }
            else
            {
                _myTransform = tr;
                InputField ip = tr.Find("ipName").GetComponent<InputField>();
                ip.onEndEdit.AddListener(delegate { NameOnEndEdit(ip.text); });
                StartCoroutine(ShowBling(_trTrophy, tr.localPosition));
            }
            pos += ScoreHght;
            yield return new WaitForSecondsRealtime(0.3f);
        }
        yield break;
    }

    void ShowBestLap()
    {
        _bestLap = Race.Current.LapStats.OrderBy(ls => ls.time).First();
        transform.Find("txtBestLapTime").GetComponent<Text>().text = GenFunc.HMS(_bestLap.time);
    }

    public void NameOnEndEdit(string n)
    {
        _hs.Name = n;
        _myTransform.Find("ipName").GetComponent<InputField>().interactable = false;
        L[_rank - 1] = _hs;
        GameData.current.HiScores = L;
        SaveLoadModel.Save();
    }

    IEnumerator ShowBling(Transform tr, Vector3 pos)
    {
        tr.gameObject.SetActive(true);
        tr.localScale = new Vector3(0, 0, 0);
        tr.localRotation = Quaternion.Euler(20, 0, 0);
        if (_rank > 3) { tr.Find("Cup").gameObject.SetActive(false); }
        else {
            tr.Find("Medal").gameObject.SetActive(false);
            MeshRenderer R = tr.Find("Cup").GetComponent<MeshRenderer>();
            Material[] M = R.sharedMaterials;
            if(_rank==1)
                M[0] = (Material)Resources.Load("Prefabs/Materials/Gold");
            if(_rank == 2)
                M[0] = (Material)Resources.Load("Prefabs/Materials/Silver");
            if (_rank == 3)
                M[0] = (Material)Resources.Load("Prefabs/Materials/Bronze");

            R.sharedMaterials = M;
        }
        tr.GetComponentInChildren<Text>().text = GenFunc.OrdinalIndicator(_rank);
        for(int t = 0; t < 90; t++)
        {
            tr.localScale += new Vector3(0.16f, 0.16f, 0.16f);
            tr.Rotate(tr.parent.up, 5, Space.World);
            yield return new WaitForSeconds(0.02f);
        }
        for (int t = 0; t < 2000; t++)
        {
            tr.Rotate(tr.parent.up, 5, Space.World);
            yield return new WaitForSeconds(0.02f);
        }
        //07811713204
        yield break;
    }

    public void SaveGhostClick()
    {
        Recrdng.Current.SaveGhost(_bestLap);
    }

    public void btnRewardsClick()
    {
        DrivingPlayManager.Current.ShowDrivingGUI(true);
        DrivingPlayManager.Current.ShowInputGUI(true);
        GameObject goScore = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlScore"), Vector3.zero, Quaternion.identity, GameObject.Find("DrivingGUICanvas(Clone)").transform);
        RectTransform trScore = goScore.GetComponent<RectTransform>();
        trScore.anchoredPosition = Vector3.zero;
        goScore.GetComponent<ScorePanel>().ShowScore(Race.Current);
        Destroy(GameObject.Find("HiScoresCanvas(Clone)"));
    }
}

