using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoviePlayer : MonoBehaviour {

    string imageFolderName = "Video/TutorialBuild1";
    List<Sprite> pictures = new List<Sprite>();
    public bool loop = false;
    public bool IsPlaying { get; set; }
    int counter = 0;
    bool Film = true;
    float PictureRateInSeconds;
    private float nextPic = 0;
 
 void Start()
    {
        if (Film == true)
        {
            PictureRateInSeconds = 0.04166666666666666666f;
        }
        IsPlaying = true;
        Object[] textures = Resources.LoadAll<Sprite>(imageFolderName);
        for (int i = 0; i < textures.Length; i++)
        {
            pictures.Add((Sprite)textures[i]);
        }
    }

    void Update()
    {
        if (IsPlaying)
        {
            if (Time.time > nextPic)
            {
                nextPic = Time.time + PictureRateInSeconds;
                GetComponent<Image>().sprite = pictures[counter];
                counter++;
            }
            if (counter == pictures.Count)
            {
                Debug.Log("End Of Video");
                if (loop)
                {
                    counter = 0;
                }
                else
                {
                    IsPlaying = false;
                }
            }
        }
    }
}