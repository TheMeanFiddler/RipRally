using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System.Collections.Generic;

public class FinishPanel:MonoBehaviour
{
    int _rank;
    float _segTime;

    public void ShowRank(float TotTime)
    {
        _segTime = TotTime / Road.Instance.Segments.Count / Race.Current.Laps;
        _rank = SaveLoadModel.GetRank(_segTime);
        StringBuilder sb = new StringBuilder();
        if (_rank != -1)
        {
            sb.Append(_rank);
            switch (_rank % 100)
            {
                case 11:
                case 12:
                case 13:
                    sb.Append("th"); break;
            }

            switch (_rank % 10)
            {
                case 1:
                    sb.Append("st"); break;
                case 2:
                    sb.Append("nd"); break;
                case 3:
                    sb.Append("rd"); break;
                default:
                    sb.Append("th"); break;
            }
        }
        else { sb.Append("---"); }
        transform.Find("txtRank").GetComponent<Text>().text = sb.ToString();
        transform.Find("txtLapTime").GetComponent<Text>().text = GenFunc.HMS(TotTime / Race.Current.Laps);
    }

    public void NameOnEnter(string n)
    {
        HiScore _hs = new HiScore();
        _hs.Name = n;
        _hs.SegTime = _segTime;
        List<HiScore> L = GameData.current.HiScores;
        if (L.Count == 0)
        { L.Add(_hs); }
        else
        { L.Insert(_rank - 1, _hs); }
        GameData.current.HiScores = L;
        SaveLoadModel.Save();
        GameObject goHiScores = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlHiScores"), Vector3.zero, Quaternion.identity, GameObject.Find("DrivingGUICanvas(Clone)").transform);
        RectTransform trHiScores = goHiScores.GetComponent<RectTransform>();
        trHiScores.anchoredPosition = Vector3.zero;
        //goHiScores.GetComponent<ScorePanel>().ShowScore(Race.Current);
        Destroy(this.gameObject);
    }

    public void Rewards()
    {
        GameObject goScore = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlScore"), Vector3.zero, Quaternion.identity, GameObject.Find("DrivingGUICanvas(Clone)").transform);
        RectTransform trScore = goScore.GetComponent<RectTransform>();
        trScore.anchoredPosition = Vector3.zero;
        goScore.GetComponent<ScorePanel>().ShowScore(Race.Current);
        Destroy(this.gameObject);
    }
}

