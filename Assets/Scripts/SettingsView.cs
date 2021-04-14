using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class SettingsView:MonoBehaviour
{
    GameObject pnlSoundSettings;
    GameObject pnlControlSettings;
    RectTransform trSteer;
    RectTransform trAccel;
    RectTransform trBrake;

    void Awake()
    {
        pnlSoundSettings = transform.Find("pnlSoundSettings").gameObject;
        pnlControlSettings = transform.Find("pnlControlSettings").gameObject;
        trSteer = transform.Find("pnlControlSettings/pnlControls/imgSteer").GetComponent<RectTransform>();

        Settings.Instance.LoadFromFile();
        switch(Settings.Instance.SteerControl)
        {
            case "Tilt":
            GameObject.Find("tglSteerTilt").GetComponent<Toggle>().isOn = true;
            trSteer.gameObject.SetActive(false);
            break;
            case "SteeringWheel":
            GameObject.Find("tglSteeringWheel").GetComponent<Toggle>().isOn = true;
            break;
        }

        switch (Settings.Instance.AccelControl)
        {
            case "ThumbPress":
                GameObject.Find("tglAccelThumbPress").GetComponent<Toggle>().isOn = true;
                break;
            case "ThumbSlide":
                GameObject.Find("tglAccelThumbSlide").GetComponent<Toggle>().isOn = true;
                break;
            case "Tilt":
                GameObject.Find("tglAccelTilt").GetComponent<Toggle>().isOn = true;
                break;
        }

        switch (Settings.Instance.CamType)
        {
            case "FollowCamFly":
                GameObject.Find("tglFollowCamFly").GetComponent<Toggle>().isOn = true;
                break;
            case "FollowCamSwing":
                GameObject.Find("tglFollowCamSwing").GetComponent<Toggle>().isOn = true;
                break;
        }

        GameObject.Find("sldSoundFXVolume").GetComponent<Slider>().value = Settings.Instance.SFXVolume;
        GameObject.Find("tglZip").GetComponent<Toggle>().isOn = Settings.Instance.Zip;
        pnlSoundSettings.SetActive(false);

        transform.Find("pnlControlSettings/pnlControls/imgAccel").GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.AccelPos, 50);
        transform.Find("pnlControlSettings/pnlControls/imgBrake").GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.Instance.BrakePos, 50);
        trSteer.anchoredPosition = new Vector2(Settings.Instance.SteeringPosX, Settings.Instance.SteeringPosY);
    }

    public void SwitchTabSound(bool selected)
    {
        if (selected)
        {
            pnlSoundSettings.SetActive(true);
            pnlControlSettings.SetActive(false);
        }
    }

    public void SwitchTabControl(bool selected)
    {
        if (selected)
        {
            pnlSoundSettings.SetActive(false);
            pnlControlSettings.SetActive(true);
        }
    }

    public void Close()
    {
        Settings.Instance.SaveToFile();
        Destroy(this.gameObject);
    }

    public void SteerTilt(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.SteerControl = "Tilt";
            trSteer.gameObject.SetActive(false);
        }
    }

    public void SteeringWheel(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.SteerControl = "SteeringWheel";
            trSteer.gameObject.SetActive(true);
        }
    }
    public void AccelThumbPress(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.AccelControl = "ThumbPress";
        }
    }
    public void AccelThumbSlide(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.AccelControl = "ThumbSlide";
        }
    }
    public void AccelTilt(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.AccelControl = "Tilt";
        }
    }
    public void CamFly(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.CamType = "FollowCamFly";
        }
    }
    public void CamSwing(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.CamType = "FollowCamSwing";
        }
    }


    public void ZipFiles(bool Selected)
    {
        Settings.Instance.Zip = Selected;
        Settings.Instance.SaveToFile();
    }

    public void CamFP(bool Selected)
    {
        if (Selected)
        {
            Settings.Instance.CamType = "FPCam";
        }
    }

    public void SlideSFXVol(float vol)
    {
        Settings.Instance.SFXVolume = vol;

    }
}

