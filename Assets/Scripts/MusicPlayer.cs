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
public class MusicPlayer : Singleton<MusicPlayer>
{
    AudioSource _softAudioSrc;
    AudioSource _hardAudioSrc;
    AudioSource _codaAudioSrc;

    AudioSource[] audioSrcs;
    int tgl = 0;
    double _currStart;
    double _nextStart = 100000;
    List<IResourceLocation> SoftCatalog = new List<IResourceLocation>();
    List<IResourceLocation> HardCatalog = new List<IResourceLocation>();
    List<IResourceLocation> CodaCatalog = new List<IResourceLocation>();
    List<AudioClip> CodaClips = new List<AudioClip>();
    string _theme = "Theme1";
    MusicType currType = MusicType.None;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _softAudioSrc = gameObject.AddComponent<AudioSource>();
        _hardAudioSrc = gameObject.AddComponent<AudioSource>();
        _codaAudioSrc = gameObject.AddComponent<AudioSource>();
        _softAudioSrc.loop = true;
        _hardAudioSrc.loop = true;
        if (Settings.Instance.MusicOn == false) { _softAudioSrc.mute = true; _hardAudioSrc.mute = true; }
        ResourceManager.ExceptionHandler = CustomExceptionHandler;
        if (Random.Range(0, 2) == 0) _theme = "Theme2";
    }

    IEnumerator Start()
    {
        AsyncOperationHandle<IList<IResourceLocation>> catHandle;
        //Returns any IResourceLocations that are mapped to the key "Soft" and "Gm"
        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Soft", _theme }, Addressables.MergeMode.Intersection);
        //catHandle = Addressables.LoadResourceLocationsAsync("Soft, Em");
        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            SoftCatalog.Add(k);
        }

        Addressables.Release(catHandle);

        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Hard", _theme }, Addressables.MergeMode.Intersection);
        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            HardCatalog.Add(k);
        }

        Addressables.Release(catHandle);

        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Coda", _theme }, Addressables.MergeMode.Intersection);

        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            AsyncOperationHandle<AudioClip> handle5 = Addressables.LoadAssetAsync<AudioClip>(k);
            yield return handle5;
            CodaClips.Add(handle5.Result);
            Addressables.Release(handle5);
        }

        Addressables.Release(catHandle);
        //Load the soft clip
        int rnd = Random.Range(0, SoftCatalog.Count);
        AsyncOperationHandle<AudioClip> hndl = Addressables.LoadAssetAsync<AudioClip>(SoftCatalog[rnd]);
        yield return hndl;
        if(hndl.Status == AsyncOperationStatus.Succeeded)
        {
            _softAudioSrc.clip = hndl.Result;
        }
        Addressables.Release(hndl);
        //Load the hard clip
        rnd = Random.Range(0, HardCatalog.Count);
        AsyncOperationHandle<AudioClip>  hndl2 = Addressables.LoadAssetAsync<AudioClip>(HardCatalog[rnd]);
        yield return hndl2;
        if (hndl2.Status == AsyncOperationStatus.Succeeded)
        {
            _hardAudioSrc.clip = hndl2.Result;
        }
        Addressables.Release(hndl2);

        //Load the coda clip
        rnd = Random.Range(0, CodaClips.Count);
        _codaAudioSrc.clip = CodaClips[rnd];
        
        _softAudioSrc.PlayScheduled(AudioSettings.dspTime + 0.1);
        _hardAudioSrc.PlayScheduled(AudioSettings.dspTime + 0.1);
        _hardAudioSrc.volume = 0;
        yield return 0;
    }

    private void CustomExceptionHandler(AsyncOperationHandle arg1, System.Exception exception)
    {
        Debug.Log(exception.GetType());
    }


    public enum MusicType { Soft, Hard, Coda, None}
    public enum PlaySched { Now, XFade, NextBar, End}
    

    public void SetMix(float val)
    {
        val = Mathf.Clamp01(val);
        _hardAudioSrc.volume = val;
        _softAudioSrc.volume = 1 - val;
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
        yield return 0;
    }
    
    public void ScheduleCoda()
    {
        Debug.Log(_hardAudioSrc.clip.samples);
        double len = (double)_hardAudioSrc.clip.samples / _hardAudioSrc.clip.frequency;
        int lastBarNo = _hardAudioSrc.timeSamples / 291972;
        int nextBarSample = (lastBarNo + 1) * 291972;
        double swapTime = _currStart + (double)nextBarSample / _hardAudioSrc.clip.frequency;
        // 72993 samples in a bar
        // freq = 44100 Hz
        Debug.Log("Sched");
        StartCoroutine(PlayCoda(swapTime));
    }




    IEnumerator PlayCoda(double swapTime = 0)
    {
        if (swapTime != 0)
        {
            while (AudioSettings.dspTime < swapTime)
            { yield return new WaitForEndOfFrame(); }
        }
        _hardAudioSrc.Stop();
        _codaAudioSrc.Play();

        yield return 0;
    }

    public void IsOn(bool isOn )
    {
        _softAudioSrc.mute = !isOn;
        _hardAudioSrc.mute = !isOn;
    }
}
