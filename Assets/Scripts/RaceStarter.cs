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
    }

    void Update()
    {
        if(CamSelector.Instance.FPCam==null) CamSelector.Instance.Init();
        if (Time.time - Timer > 1)
        {
            if (LightCount == 2)
            {
                MusicPlayer.Instance.SchedulePlay(MusicPlayer.State.Hard, 1);
            }
            LightCount++;
            Timer = Time.time;
            
        }
        

        if (LightCount == 3) {
            CamSelector.Instance.SelectCam(Main.Instance.LastSelectedCam);
            Race.Current.Go(); }
    }
}

