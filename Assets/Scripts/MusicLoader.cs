using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

/*How it works
Runs a random ambient clip on repeat until the DPM loads
Then it gets to the end of the bar and plays a coda
It loads the Hard music into the second AudioSource
When the player car accelerates it plays the hard music
When the car stops it plays the coda


*/
//https://gamedevbeginner.com/ultimate-guide-to-playscheduled-in-unity/
public class MusicLoader : MonoBehaviour
{
    AudioSource[] audioSrcs;
    int tgl = 0;
    double _currStart;
    double _nextStart = 100000;
    List<IResourceLocation> AmbientCatalog = new List<IResourceLocation>();
    List<IResourceLocation> HardCatalog = new List<IResourceLocation>();
    List<IResourceLocation> CodaCatalog = new List<IResourceLocation>();
    List<AudioClip> CodaClips = new List<AudioClip>();
    string _key = "Gm";

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
        //Returns any IResourceLocations that are mapped to the key "Ambient" and "Gm"
        catHandle = Addressables.LoadResourceLocationsAsync(new string[] { "Ambient", _key }, Addressables.MergeMode.Intersection);
        //catHandle = Addressables.LoadResourceLocationsAsync("Ambient, Em");
        yield return catHandle;

        foreach (var k in catHandle.Result)
        {
            AmbientCatalog.Add(k);
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


        int rnd = Random.Range(0, AmbientCatalog.Count);
        AsyncOperationHandle<AudioClip> handle4 = Addressables.LoadAssetAsync<AudioClip>(AmbientCatalog[rnd]);
        yield return handle4;
        audioSrcs[0].clip = handle4.Result;
        _currStart = AudioSettings.dspTime;
        audioSrcs[0].PlayScheduled(AudioSettings.dspTime + 0.5);
        _currStart = AudioSettings.dspTime + 0.5;
        yield return 0;
    }

    private void CustomExceptionHandler(AsyncOperationHandle arg1, System.Exception exception)
    {
        Debug.Log(exception.GetType());
    }

    void Update()
    {

        if (AudioSettings.dspTime > _nextStart - 1)
        {
            PlayAmbient();
        }
    }

    public void PlayAmbient()
    {
        int rnd = Random.Range(0, AmbientCatalog.Count);
        Addressables.LoadAssetAsync<AudioClip>(AmbientCatalog[rnd]).Completed += AudioLoaded;
    }

    public void PlayHard()
    {
        int rnd = Random.Range(0, HardCatalog.Count);
        Addressables.LoadAssetAsync<AudioClip>(HardCatalog[rnd]).Completed += AudioLoaded;
    }

    private void AudioLoaded(AsyncOperationHandle<AudioClip> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Main.Instance.PopupMsg("Audio Loaded");
            AudioClip loadedClip = obj.Result;
            audioSrcs[tgl].clip = loadedClip;
            audioSrcs[tgl].PlayScheduled(AudioSettings.dspTime + 0.1);
            _currStart = AudioSettings.dspTime + 0.1;
        }
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




    IEnumerator SwapSrcs(double swapTime)
    {
        while (AudioSettings.dspTime < swapTime)
        { yield return new WaitForEndOfFrame(); }
        audioSrcs[tgl].Stop();
        tgl = 1 - tgl;

        yield return 0;
    }
}
