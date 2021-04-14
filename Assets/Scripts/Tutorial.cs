using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;


public class Tutorial:MonoBehaviour
{
    Canvas _canvas;
    GameObject _speechBubble;
    VideoPlayer vid;
    RawImage img;
    RectTransform _finger;

    void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        try
        {
            _speechBubble = transform.Find("imgTutor/SpeechBubble").gameObject;
            _speechBubble.SetActive(false);
            _finger = transform.Find("imgHole").GetComponent<RectTransform>() ;
        }
        catch { }
        if (name == "pnlTutorialReplayer(Clone)") StartCoroutine(ReplayTut());
    }
    public void IntroPage2()
    {
        GameObject Tut = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlTutorialIntro2"), new Vector2(500, 500), Quaternion.identity, _canvas.transform);
        Tut.transform.localScale = Vector3.one;
        Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(this.gameObject);
    }

    public void IntroPage1()
    {
        GameObject Tut = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlTutorialIntro1"), new Vector2(500, 500), Quaternion.identity, _canvas.transform);
        Tut.transform.localScale = Vector3.one;
        Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(this.gameObject);
    }

    public void IntroLetsPlay()
    {
        if (SaveLoadModel.savedGames.Count < 2)
        {
            Object objPnl = Resources.Load("Prefabs/pnlTutorialTrackSelector");
            GameObject Tut = (GameObject)GameObject.Instantiate(objPnl, _canvas.transform);
            Tut.transform.localScale = Vector3.one;
            Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        Destroy(this.gameObject);
    }

    public void PlayBuildVideo1()
    {
        GameObject cam = GameObject.Find("BuilderCamera");
        if (vid == null) vid = transform.Find("vid").GetComponent<VideoPlayer>();
        vid.enabled = true;
        vid.targetCamera = cam.GetComponent<Camera>();
        transform.Find("pnlVid").gameObject.SetActive(true);
        Transform canv = transform.parent;
        transform.Find("pnlTutor").gameObject.SetActive(false);
        canv.Find("DPad(Clone)").gameObject.SetActive(false);
        canv.Find("ToolboxPanel").gameObject.SetActive(false);
        canv.Find("RoadPanel").gameObject.SetActive(false);
        canv.Find("MenuToggle").gameObject.SetActive(false);
        canv.Find("DeleteButton").gameObject.SetActive(false);


        vid.EnableAudioTrack(0, false);
        vid.source = VideoSource.VideoClip;

        StartCoroutine(playVideo());

    }

    IEnumerator playVideo()
    {
        
        vid.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!vid.isPrepared)
        {
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
        }

        //Assign the Texture from Video to RawImage to be displayed
        //image.texture = vid.texture;

        //Play Video
        vid.Play();

        //Play Sound
        //audioSource.Play();

        while (vid.frame<(long)vid.frameCount-1)
        {
            //Debug.Log("Video Time: " + Mathf.FloorToInt((float)vid.time));
            yield return null;
        }
        HideVid();
    }

    public void HideVid()
    {
        vid.targetCamera = null;
        vid.enabled = false;
        transform.Find("pnlVid").gameObject.SetActive(false);
        Transform canv = transform.parent;
        canv.Find("DPad(Clone)").gameObject.SetActive(true);
        canv.Find("ToolboxPanel").gameObject.SetActive(true);
        canv.Find("RoadPanel").gameObject.SetActive(true);
        canv.Find("MenuToggle").gameObject.SetActive(true);
        canv.Find("DeleteButton").gameObject.SetActive(true);
        transform.Find("pnlTutor").gameObject.SetActive(true);
    }

    public void VideoPlus()
    {
        if (vid.frame < 250) vid.frame = 250;           //Move Around
        else if (vid.frame < 800) vid.frame = 800;      //Slide
        else if (vid.frame < 1250) vid.frame = 1250;    //Cut thro
        else if (vid.frame < 1790) vid.frame = 1790;    //Delete
        else if (vid.frame < 2100) vid.frame = 2100;    //Bridge
        else if (vid.frame < 2500) vid.frame = 2500;    //Smooth
        else if (vid.frame < 2880) vid.frame = 2880;    //Bank
        else if (vid.frame < 3130) vid.frame = 3130;    //Insert
        else if (vid.frame < 3330) vid.frame = 3330;    //Road and Fence
        else if (vid.frame < 3530) vid.frame = 3530;    //Test drive
        else if (vid.frame < 3920) vid.frame = 3920;    //Adjust
        else if (vid.frame < 4050) vid.frame = 4050;    //Save
        else if (vid.frame < 4200) vid.frame = 4200;    //Pay
    }

    float MinusTime;
    public void VideoMinus()
    {
        if (Time.time - MinusTime < 1)
        {
            if (vid.frame < 270) vid.frame = 1;           //Move Around
            else if (vid.frame < 800) vid.frame = 1;      //Slide
            else if (vid.frame < 1250) vid.frame = 250;    //Cut thro
            else if (vid.frame < 1790) vid.frame = 800;    //Delete
            else if (vid.frame < 2100) vid.frame = 1250;    //Bridge
            else if (vid.frame < 2500) vid.frame = 1790;    //Smooth
            else if (vid.frame < 2880) vid.frame = 2100;    //Bank
            else if (vid.frame < 3130) vid.frame = 2500;    //Insert
            else if (vid.frame < 3330) vid.frame = 2880;    //Road and Fence
            else if (vid.frame < 3530) vid.frame = 3130;    //Test drive
            else if (vid.frame < 3920) vid.frame = 3330;    //Adjust
            else if (vid.frame < 4050) vid.frame = 3530;    //Save
            else if (vid.frame < 4200) vid.frame = 3920;    //Pay
        }
            else
        {
            if (vid.frame < 270) vid.frame = 1;           //Move Around
            else if (vid.frame < 800) vid.frame = 250;      //Slide
            else if (vid.frame < 1250) vid.frame = 800;    //Cut thro
            else if (vid.frame < 1790) vid.frame = 1250;    //Delete
            else if (vid.frame < 2100) vid.frame = 1790;    //Bridge
            else if (vid.frame < 2500) vid.frame = 2100;    //Smooth
            else if (vid.frame < 2880) vid.frame = 2500;    //Bank
            else if (vid.frame < 3130) vid.frame = 2880;    //Insert
            else if (vid.frame < 3330) vid.frame = 3130;    //Road and Fence
            else if (vid.frame < 3530) vid.frame = 3330;    //Test drive
            else if (vid.frame < 3920) vid.frame = 3530;    //Adjust
            else if (vid.frame < 4050) vid.frame = 3920;    //Save
            else if (vid.frame < 4200) vid.frame = 4050;    //Pay
        }
        MinusTime = Time.time;
    }

    public void BuildPage2()
    {
        transform.parent.Find("pnlTutorialBuild2(Clone)").gameObject.SetActive(true);
        ClosePanel();
    }

    IEnumerator ReplayTut()
    {
        transform.parent.Find("pnlReplayer(Clone)/pnlBuyCameras").gameObject.SetActive(false);
        float LerpStartTime;
        Vector2 LerpStartPos;
        Vector2 LerpEndPos;
        float LerpFrac = 0;
        ShowSpeech("Let me show you how to replay the race", 4);
        LerpStartTime = Time.time; LerpStartPos = new Vector2(-110, -20); LerpEndPos = new Vector2(110, -20); ; LerpFrac = 0;
        while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 4; yield return new WaitForEndOfFrame(); }

        while (true)
        {
            ShowSpeech("Use the buttons to start and stop", 4);
            LerpStartTime = Time.time; LerpStartPos = new Vector2(-110, -20); LerpEndPos = new Vector2(110, -20); ; LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time-LerpStartTime)/4; yield return new WaitForEndOfFrame(); }
            ShowSpeech("Tap on the timeline to move to a frame", 4);
            LerpStartTime = Time.time; LerpStartPos = _finger.anchoredPosition; LerpEndPos = new Vector2(0, -60); LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 0.5f; yield return new WaitForEndOfFrame(); }
            yield return new WaitForSeconds(3.5f);
            ShowSpeech("Slide the red markers to set the start and finish frame", 4);
            LerpStartTime = Time.time; LerpStartPos = _finger.anchoredPosition; LerpEndPos = new Vector2(-200, -60); LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 0.5f; yield return new WaitForEndOfFrame(); }
            yield return new WaitForSeconds(3.5f);
            ShowSpeech("Replay cams give stunning camera angles", 4);
            LerpStartTime = Time.time; LerpStartPos = new Vector2(355, -50); LerpEndPos = new Vector2(355, -320); ; LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 4; yield return new WaitForEndOfFrame(); }
            ShowSpeech("Tap on them or drag them onto the timeline", 4);
            LerpStartTime = Time.time; LerpStartPos = new Vector2(355, -320); LerpEndPos = new Vector2(355, -50); ; LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 4; yield return new WaitForEndOfFrame(); }

            ShowSpeech("Save your replay to play with it later", 4);
            LerpStartTime = Time.time; LerpStartPos = _finger.anchoredPosition; LerpEndPos = new Vector2(-190, -20); LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 0.5f; yield return new WaitForEndOfFrame(); }
            yield return new WaitForSeconds(3.5f);

            ShowSpeech("or make a video to share with your friends", 4);
            LerpStartTime = Time.time; LerpStartPos = _finger.anchoredPosition; LerpEndPos = new Vector2(-280, -20); LerpFrac = 0;
            while (LerpFrac < 1) { _finger.anchoredPosition = Vector2.Lerp(LerpStartPos, LerpEndPos, LerpFrac); LerpFrac = (Time.time - LerpStartTime) / 0.5f; yield return new WaitForEndOfFrame(); }
            yield return new WaitForSeconds(3.5f);
        }
    }
    void Update()
    {
        if((Input.GetMouseButtonDown(0)) && gameObject.name == "pnlTutorialTrackSelector(Clone)")
            Destroy(this.gameObject);
    }
    /// <summary>
    /// Shows a speech bubble for d seconds
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="duration"></param>
    public void ShowSpeech(string msg, int duration)
    {
        _speechBubble.GetComponentInChildren<Text>().text = msg;
        _speechBubble.SetActive(true);
        Invoke("CloseSpeechBubble", duration);
    }

    void CloseSpeechBubble()
    {
        _speechBubble.SetActive(false);
    }

    public void VisitShop()
    {
        GameObject Tut = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/pnlShop"), new Vector2(500, 500), Quaternion.identity, _canvas.transform);
        Tut.transform.localScale = Vector3.one;
        Tut.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        Destroy(this.gameObject);
    }

    public void ClosePanel()
    {
        if(this.name == "pnlTutorialReplayer(Clone)")
        _canvas.transform.Find("pnlReplayer(Clone)/pnlBuyCameras").gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

    public void IntroHide(bool Hide)
    {
        Settings.Instance.TutorialIntroHide = Hide;
        Settings.Instance.SaveToFile();
    }

    public void ShopHide(bool Hide)
    {
        Settings.Instance.TutorialShopHide = Hide;
        Settings.Instance.SaveToFile();
    }

}

