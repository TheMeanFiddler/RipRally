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

    AudioSource _codaAudioSrc;
    AudioSource _crescAudioSrc;
    AudioSource _softAudioSrc;
    AudioSource[] audioSrcs;
    double startTime = 100000;
    double _clipLen;
    double hardLen;
    double softLen;
    bool Transitioning;
    List<IResourceLocation> SoftCatalog = new List<IResourceLocation>();
    List<IResourceLocation> HardCatalog = new List<IResourceLocation>();
    List<IResourceLocation> CodaCatalog = new List<IResourceLocation>();
    List<AudioClip> CodaClips = new List<AudioClip>();
    string _theme = "Theme1";
    State _state = State.Silent;
    State _prevState = State.Silent;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _softAudioSrc = gameObject.AddComponent<AudioSource>();
        _crescAudioSrc = gameObject.AddComponent<AudioSource>();
        _codaAudioSrc = gameObject.AddComponent<AudioSource>();
        _softAudioSrc.loop = true;
        _crescAudioSrc.loop = true;
        if (Settings.Instance.MusicOn == false) { _softAudioSrc.mute = true; _crescAudioSrc.mute = true; _codaAudioSrc.mute = true; }
        ResourceManager.ExceptionHandler = CustomExceptionHandler;
        
    }
    private void Start()
    {
        StartCoroutine(ChooseTheme());
    }

    internal void ApplySettings(bool isOn)
    {
        audioSrcs[0].mute = !isOn; audioSrcs[1].mute = !isOn; _codaAudioSrc.mute = !isOn;
    }

    IEnumerator ChooseTheme()
    {
        AsyncOperationHandle<IList<IResourceLocation>> catHandle;
        _theme = "Theme" + (Random.Range(1, 4));
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
            _clipLen = (double)hndl.Result.samples / hndl.Result.frequency;

        }
        Addressables.Release(hndl);
        //Load the hard clip
        rnd = Random.Range(0, HardCatalog.Count);
        AsyncOperationHandle<AudioClip>  hndl2 = Addressables.LoadAssetAsync<AudioClip>(HardCatalog[rnd]);
        yield return hndl2;
        if (hndl2.Status == AsyncOperationStatus.Succeeded)
        {
            _crescAudioSrc.clip = hndl2.Result;
        }
        Addressables.Release(hndl2);

        //Load the coda clip
        rnd = Random.Range(0, CodaClips.Count);
        if(CodaClips.Count>0)
        _codaAudioSrc.clip = CodaClips[rnd];

        Debug.Log("StartSoft");
        _softAudioSrc.PlayScheduled(AudioSettings.dspTime + 0.1);
        startTime = AudioSettings.dspTime + 0.1;
        ScheduleStateChange(State.Soft, AudioSettings.dspTime + 0.1);
        _state = State.Toggling;
        yield return 0;
    }

    private void CustomExceptionHandler(AsyncOperationHandle arg1, System.Exception exception)
    {
        Debug.Log(exception.GetType());
    }

    private void Update()
    {
        if (_prevState != _state) Debug.Log("Change from " + _prevState.ToString() + " to " + _state);
        _prevState = _state;

        if (_state != State.Silent && _state != State.Toggling)
        {
            if (AudioSettings.dspTime>startTime + 30)
            {
                StepDown();
            }
            
        }
        if (_state == State.Silent && AudioSettings.dspTime > startTime + 10)
        {
            if (Random.value > 0.5) SchedulePlay(State.Hard, 0.1f);
            else SchedulePlay(State.Soft, 0.1f);
        }
    }

    public enum State { Soft, Hard, Toggling, Fading, Silent}

    public void StepDown()
    {
        if (_state==State.Hard) SchedulePlay(State.Soft, 0, false);
        else
            Fade();
    }


    public void SchedulePlay(State newState, float secs = 0, bool nextBar = false)
    {
        //if (_state == State.Fading) { StopCoroutine("FadeSoft"); _softAudioSrc.volume = 1; Debug.Log("Stop FadeSoftCoRou"); } ;
        if (_state == State.Toggling) return;
        Debug.Log("SchedulePlay " + newState.ToString());

        AudioSource newAudioSrc = _codaAudioSrc;
        if (newState == State.Soft) newAudioSrc = _softAudioSrc;
        if (newState == State.Hard) newAudioSrc = _crescAudioSrc;
        AudioSource currAudioSrc = _codaAudioSrc;
        if (_state == State.Soft) currAudioSrc = _softAudioSrc;
        if (_state == State.Hard) currAudioSrc = _crescAudioSrc;

        if (secs == 0)
        {
            double RunTime = (double)currAudioSrc.timeSamples / currAudioSrc.clip.frequency;
            double clipStartTime = AudioSettings.dspTime - RunTime;
            if (nextBar)
            {
                // 72993 samples in a bar
                // freq = 44100 Hz
                int lastBarNo = currAudioSrc.timeSamples / 72993;
                int nextBarSample = (lastBarNo + 1) * 72993;
                startTime = clipStartTime + (double)nextBarSample / currAudioSrc.clip.frequency;
            }
            else
                startTime = clipStartTime + (double)currAudioSrc.clip.samples / currAudioSrc.clip.frequency;
        }
        else
        {
            startTime = AudioSettings.dspTime + secs;
        }
        _state = State.Toggling;
        newAudioSrc.PlayScheduled(startTime);
        currAudioSrc.SetScheduledEndTime(startTime);
        StartCoroutine(ScheduleStateChange(newState, startTime));
    }

    IEnumerator ScheduleStateChange(State state, double schedTime)
    {
        yield return new WaitUntil(() => AudioSettings.dspTime > schedTime);
        _state = state;
        yield return 0;
    }

    public void Fade()
    {
        if (_state != State.Fading)
        {
            Debug.Log("Start Fade");
            StartCoroutine("FadeSoft");
            _state = State.Fading;
        }
    }
    IEnumerator FadeSoft()
    {
        float v = 1;
        while (v > 0f)
        {
            v -= 0.001f;
            _softAudioSrc.volume = v;
            yield return new WaitForEndOfFrame();
        }
        _softAudioSrc.Stop();
        _softAudioSrc.volume = 1;
        if (_state == State.Fading)
        {
            _state = State.Silent;
            Debug.Log("EndFade");
            startTime = AudioSettings.dspTime;
        }
        yield return null;

    }

}
