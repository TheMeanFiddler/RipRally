using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

/*How it works
Runs a random Soft clip on repeat until the RaceSelector loads
Then it fades out
It loads the Hard music into the second AudioSource
When the player car accelerates it plays the hard music
When the car stops it plays the Soft


*/
//https://gamedevbeginner.com/ultimate-guide-to-playscheduled-in-unity/
public class MusicPlayer : MonoBehaviour
{
    AudioSource[] audioSrcs;
    int tgl = 0;
    double _currStart;
    double _nextStart = 100000;
    List<IResourceLocation> SoftCatalog = new List<IResourceLocation>();
    List<IResourceLocation> HardCatalog = new List<IResourceLocation>();
    List<IResourceLocation> CodaCatalog = new List<IResourceLocation>();
    List<AudioClip> CodaClips = new List<AudioClip>();
    string _key = "Gm";
    MusicType currType = MusicType.None;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        audioSrcs = GetComponents<AudioSource>();
        ResourceManager.ExceptionHandler = CustomExceptionHandler;
        if (Random.Range(0, 2) == 0) _key = "Em";
    }

    IEnumerator Start()
    {
        AsyncOperationHandle<IList<IResourceLocation>> catHandle;
        //Returns any IResourceLocations that are mapped to the key "Soft" and "Gm"
        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Soft", _key }, Addressables.MergeMode.Intersection);
        //catHandle = Addressables.LoadResourceLocationsAsync("Soft, Em");
        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            SoftCatalog.Add(k);
        }

        Addressables.Release(catHandle);

        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Hard", _key }, Addressables.MergeMode.Intersection);
        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            HardCatalog.Add(k);
        }

        Addressables.Release(catHandle);

        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Coda", _key }, Addressables.MergeMode.Intersection);

        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            AsyncOperationHandle<AudioClip> handle5 = Addressables.LoadAssetAsync<AudioClip>(k);
            yield return handle5;
            CodaClips.Add(handle5.Result);
            Addressables.Release(handle5);
        }

        Addressables.Release(catHandle);


        Play(MusicType.Soft, PlaySched.Now);
        yield return 0;
    }

    private void CustomExceptionHandler(AsyncOperationHandle arg1, System.Exception exception)
    {
        Debug.Log(exception.GetType());
    }


    public enum MusicType { Soft, Hard, Coda, None}
    public enum PlaySched { Now, XFade, NextBar, End}
    
    public void Play(MusicType type, PlaySched t)
    {
        if (currType == type) return;
        if (type == MusicType.Soft)
        {
            int rnd = Random.Range(0, SoftCatalog.Count);
            StartCoroutine(LoadAndPlay(SoftCatalog[rnd], t));
        }
        if (type == MusicType.Hard)
        {
            int rnd = Random.Range(0, HardCatalog.Count);
            StartCoroutine(LoadAndPlay(HardCatalog[rnd], t));
        }
        currType = type;
    }


    IEnumerator LoadAndPlay(IResourceLocation loc, PlaySched t)
    {
        Debug.Log("LoadAndPlay");
        AsyncOperationHandle<AudioClip> hndl = Addressables.LoadAssetAsync<AudioClip>(loc);
        yield return hndl;
        if (hndl.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip loadedClip = hndl.Result;
            audioSrcs[1-tgl].clip = loadedClip;
            double swapTime = 0.01;
            if (t == PlaySched.Now)
            {
                Debug.Log(loadedClip.name + "Now on " + (1 - tgl).ToString());
                swapTime = AudioSettings.dspTime + 0.01;
                audioSrcs[1 - tgl].PlayScheduled(AudioSettings.dspTime + 0.01);
            }
            if (t == PlaySched.End)
            {
                swapTime = AudioSettings.dspTime + 10;
                audioSrcs[1 - tgl].PlayScheduled(AudioSettings.dspTime + 10);
            }

            Debug.Log("OK");
            StartCoroutine(SwapSrcs(0.01));

        }
        Addressables.Release(hndl);
        yield return 0;
    }



    public void FadeOut(float secs)
    {
        StartCoroutine (Fade(secs));
    }
    IEnumerator Fade(float secs)
    {
        float vol = audioSrcs[tgl].volume;
        float incr = vol / 50;
        while (vol > 0)
        {
            vol -= incr;
            audioSrcs[tgl].volume = vol;
            yield return new WaitForSeconds(secs/50);
        }
        audioSrcs[tgl].Stop();
        audioSrcs[tgl].volume = 1;
        StartCoroutine(SwapSrcs());
        yield return 0;
    }
    
    public void SheduleCoda()
    {
        Debug.Log(audioSrcs[tgl].clip.samples);
        Debug.Log(audioSrcs[tgl].clip.frequency);
        double len = (double)audioSrcs[tgl].clip.samples / audioSrcs[tgl].clip.frequency;
        int lastBarNo = audioSrcs[tgl].timeSamples / 291972;
        int nextBarSample = (lastBarNo + 1) * 291972;
        double swapTime = _currStart + (double)nextBarSample / audioSrcs[tgl].clip.frequency;
        // 72993 samples in a bar
        // freq = 44100 Hz
        Debug.Log(CodaClips.Count);
        int rnd = Random.Range(0, CodaClips.Count);
        audioSrcs[1 - tgl].clip=CodaClips[rnd];
        audioSrcs[1 - tgl].loop = false;
        audioSrcs[1 - tgl].PlayScheduled(swapTime);
        StartCoroutine(SwapSrcs(swapTime));
    }




    IEnumerator SwapSrcs(double swapTime = 0)
    {
        if (swapTime != 0)
        {
            while (AudioSettings.dspTime < swapTime)
            { yield return new WaitForEndOfFrame(); }
        }
        audioSrcs[tgl].Stop();
        tgl = 1 - tgl;
        Debug.Log("Swap to " + tgl);
        yield return 0;
    }
}
