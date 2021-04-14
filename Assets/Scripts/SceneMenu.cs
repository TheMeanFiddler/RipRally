using UnityEngine;
using UnityEngine.UI;
using System.Collections;



    public class SceneMenu:MonoBehaviour
    {
        string SelectedScene { get; set; }
        RectTransform trMud1;
        RectTransform trMud2;
    Transform trWipe;
    Transform trWiper;
    GameObject imgLeft;
    GameObject imgRight;
    float MouseDownPos;

    void Start()
        {
        Main.Instance.SelectedScene = "Canyon";
        trWipe = transform.Find("imgWipe");
        trWiper = transform.Find("imgWiper");
        trMud1 = trWipe.Find("imgMud1").GetComponent<RectTransform>();
        trMud2 = trWipe.Find("imgMud2").GetComponent<RectTransform>();
        imgLeft = GameObject.Find("imgLeftArrow");
        imgRight = GameObject.Find("imgRightArrow");
    }

    public void ChooseGlacial()
    {
        StopAllCoroutines();
        StartCoroutine(SplatAndWipe("Glacial"));
    }

    public void ChooseDesert()
    {
        StopAllCoroutines();
        StartCoroutine(SplatAndWipe("Desert"));
    }

    public void ChooseCanyon()
    {
        StopAllCoroutines();
        StartCoroutine(SplatAndWipe("Canyon"));
    }

    IEnumerator SplatAndWipe(string img)
    {
        float angl = -180;
        float alph = 1;

        trWipe.rotation = Quaternion.Euler(0, 0, angl);
        trMud1.rotation = Quaternion.Euler(0, 0, 0);
        trMud2.rotation = Quaternion.Euler(0, 0, 0);
        trMud1.gameObject.SetActive(true);
        trMud2.gameObject.SetActive(false);
        trMud1.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        trMud2.GetComponent<Image>().color = new Color(255, 255, 255, 1);


        //Splat1
        float scale = 0.1f;

        for (float d = 0;d<10;d++)
        {
            trMud1.localScale = new Vector3(scale*0.35f, scale * 0.35f, 1);
            scale += 0.1f;
            yield return new WaitForSeconds(0.02f);
        }
        //Splat2
        trMud2.gameObject.SetActive(true);
 
        scale = 0.1f;

        for (float d = 0; d < 10; d++)
        {
            trMud2.localScale = new Vector3(scale * 0.35f, scale * 0.35f, 1);
            scale += 0.1f;
            yield return new WaitForSeconds(0.01f);
        }

        //Wipe

        transform.Find("img"+ Main.Instance.SelectedScene).gameObject.SetActive(false);
        transform.Find("img" + img).gameObject.SetActive(true);
        imgLeft.SetActive(true);
        imgRight.SetActive(true);
        if (img == "Glacial") imgLeft.SetActive(false);
        if (img == "Desert") imgRight.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        Main.Instance.SelectedScene = img;
        while (angl < 0)
        {
            trWipe.rotation = Quaternion.Euler(0, 0, angl);
            trWiper.rotation = Quaternion.Euler(0, 0, angl+180);
            trMud1.rotation = Quaternion.Euler(0, 0, 0);
            trMud2.rotation = Quaternion.Euler(0, 0, 0);
            angl +=5;
            yield return new WaitForSeconds(0.02f);
        }
        //Wipe back
        while (angl > -181)
        {
            trWiper.rotation = Quaternion.Euler(0, 0, angl+180);
            angl -= 5;
            yield return new WaitForSeconds(0.02f);
        }

        //Fade
        while (alph >0)
        {
            trMud1.GetComponent<Image>().color = new Color(255, 255, 255, alph);
            trMud2.GetComponent<Image>().color = new Color(255, 255, 255, alph);
            alph-=0.02f;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    void Update()
    {
#if (UNITY_EDITOR)
        if (Input.GetMouseButtonUp(0))
            MouseUp(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
            MouseDown(Input.mousePosition);
#else
        if (Input.touchCount ==1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                MouseDown(Input.GetTouch(0).position);
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                MouseUp(Input.GetTouch(0).position);
        }
#endif
    }

    void MouseDown(Vector2 pos)
    {
        MouseDownPos = pos.x;
    }

    void MouseUp(Vector2 pos)
    {
        if(pos.x-MouseDownPos > Screen.width/4)
        {
            if (Main.Instance.SelectedScene == "Desert") ChooseCanyon();
            else if (Main.Instance.SelectedScene == "Canyon") ChooseGlacial();
        }
        else
        if(MouseDownPos - pos.x > Screen.width / 4)
        {
            if (Main.Instance.SelectedScene == "Canyon") ChooseDesert();
            else if (Main.Instance.SelectedScene == "Glacial") ChooseCanyon();
        }
    }

    public void BuildTrack()
        {
            PlayerManager.Type = "BuilderPlayer";
            Main.Instance.StartLoadingGameScene(Main.Instance.SelectedScene);
            //The restore of terrain and instantiation of the builder player is handled by Main.SceneChange()
        }

    }

