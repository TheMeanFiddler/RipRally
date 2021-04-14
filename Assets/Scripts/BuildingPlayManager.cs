using UnityEngine;


public class BuildingPlayManager
{
    public static BuildingPlayManager Current;

    //constructors
    public BuildingPlayManager()
    {
    }
    public BuildingPlayManager(iMain MainScript, int _selectedGameId)
    {

    }

    public void PlayOffline()
    {

        RenderSettings.fog = true;
        //RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogDensity = 0.005f;
        GameObject goPlyr;

        UnityEngine.Object Plyr = Resources.Load("Prefabs/BuilderPlayer");
        goPlyr = (GameObject)GameObject.Instantiate(Plyr, new Vector3(235, 50f, -315f), Quaternion.identity);
        goPlyr.GetComponent<BuilderController>().Init();
        goPlyr.GetComponentInChildren<Camera>().farClipPlane = 300;
    }

    public void PlayAsServer()
    {

    }
}

