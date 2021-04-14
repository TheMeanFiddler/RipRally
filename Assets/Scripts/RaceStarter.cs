using UnityEngine;
using UnityEngine.EventSystems;

public class RaceStarter: MonoBehaviour
{
    int LightCount = 0;
    float Timer;
    Transform FollowCam;
    Transform trPlayer;

    public void Init()
    {
        Timer = Time.time;
        //CamSelector.Instance.Init();
        Race.Current.StartEngines();
        LightCount = 0;
        Debug.Log("RaceStarterInit Sez FPCam = " + CamSelector.Instance.FPCam.name);
    }

    void Update()
    {
        if(CamSelector.Instance.FPCam==null) CamSelector.Instance.Init();
        if (Time.time - Timer > 1)
        {
            if (LightCount == 0)
            {

            }
            LightCount++;
            Timer = Time.time;
            
        }
        

        if (LightCount == 3) {
            CamSelector.Instance.SelectCam(Main.Instance.LastSelectedCam);
            Race.Current.Go(); }
    }
}

