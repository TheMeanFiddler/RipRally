using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash: MonoBehaviour
{
    AsyncOperation mao;
    void Start()
    {
        SceneManager.LoadScene("TrackSelector");
    }

    IEnumerator LoadMenuScene(string SceneName)
    {
        mao = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        mao.allowSceneActivation = false;
        while (!mao.isDone)
        {
            if (mao.progress == 0.9f)
                mao.allowSceneActivation = true;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.UnloadScene("Splash");
    }
}

