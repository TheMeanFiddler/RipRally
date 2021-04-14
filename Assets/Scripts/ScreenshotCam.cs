using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

class ScreenshotCam:MonoBehaviour
    {
    public GameObject goCanvas;
    public bool Grab = false;

    IEnumerator OnPostRender()
    {
        if (Grab)
        {
            Debug.Log("takeshot");
            Texture2D Screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            Screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            Screenshot.Apply();
            GameData.current.Img = Screenshot.GetRawTextureData();
            DestroyObject(Screenshot);
            //Debug.Break();
            Destroy(this.gameObject);
            //DestroyObject(rt);
            BezierLine.Instance.SetWidth(0.5f);
            if (goCanvas != null) goCanvas.SetActive(true);
            Road.Instance.goRoad.SetActive(true);
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("Terrain"))
            {
                t.GetComponent<Terrain>().enabled = true;
            }
            Grab = false;
            yield return 0;
        }
    }
    }

