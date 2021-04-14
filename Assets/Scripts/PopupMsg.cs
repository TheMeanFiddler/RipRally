using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupMsg:MonoBehaviour
{
    public Text MsgText;
    public Text Shadow;
    public RectTransform MsgPanel;
    float y = 50;
    Color _col;
    Color _shadcol;

    public void Init(string msg, Color col, bool OKButton)
    {
        MsgText.text = msg;
        Shadow.text = msg;
        _col = col;
        _shadcol = new Color(0, 0, 0);
        if (!OKButton)
            transform.Find("btnOK").gameObject.SetActive(false);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float Alpha = 1;
        
        do
        {
            //y+=2;
            //MsgPanel.localPosition = new Vector3(0, y, 0);
            Alpha -=0.02f;
            _col.a = Alpha;
            _shadcol.a = Alpha;
            MsgText.color = _col;
            Shadow.color = _shadcol;
            yield return null;
        } while (Alpha > 0);
        Destroy(gameObject);
        yield break;
    }
}

