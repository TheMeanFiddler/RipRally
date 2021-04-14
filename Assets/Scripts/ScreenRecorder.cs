/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/
public delegate void StopRecHandler(string RecFilename);

namespace NatCorder.Examples {

#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;
    using Clocks;
    using Inputs;
    using System;
    using UnityEngine.Video;
    using System.Collections;
    using NatShareU;

    public class ScreenRecorder : MonoBehaviour {

        /**
        * ReplayCam Example
        * -----------------
        * This example records the screen using a `CameraRecorder`.
        * When we want mic audio, we play the mic to an AudioSource and record the audio source using an `AudioRecorder`
        * -----------------
        * Note that UI canvases in Overlay mode cannot be recorded, so we use a different mode (this is a Unity issue)
        */

        [Header("Recording")]
        public int videoWidth = 1280;
        public int videoHeight = 720;
        Canvas cnvPreview;
        Canvas cnvRR;

        [Header("Microphone")]
        public bool recordMicrophone;
        public AudioSource microphoneSource;

        private MP4Recorder videoRecorder;
        private IClock recordingClock;
        private CameraInput cameraInput;
        private AudioInput audioInput;

        private Camera _cam;
        private VideoPlayer vid;
        private string _path;

        public event StopRecHandler OnStopRec;
        public bool IsRecording = false;

        void Awake()
        {
            cnvPreview = transform.parent.Find("PreviewCanvas").GetComponent<Canvas>();
            GameObject goRR = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/cnvRR"));
            cnvRR = goRR.GetComponent<Canvas>();
            cnvRR.enabled = false;
        }

        public void SetCamera(Camera cam)
        {
            _cam = cam;
            cnvRR.worldCamera = cam;
        }

        public void StartRecording()
        {
            // Start recording
            var frameRate = 30;
            var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
            var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
            var clock = new RealtimeClock();
            videoRecorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount);
            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, clock, _cam);
            audioInput = recordMicrophone ? new AudioInput(videoRecorder, clock, microphoneSource, true) : null;
            // Unmute microphone
            //microphoneSource.mute = audioInput == null;
            IsRecording = true;
        }

        public async void StopRecording()
        {
            if (IsRecording)
            {
                // Mute microphone
                //microphoneSource.mute = true;
                // Stop recording
                audioInput?.Dispose();
                cameraInput.Dispose();
                var path = await videoRecorder.FinishWriting();
                // Playback recording
                Debug.Log($"Saved recording to: {path}");
                _path = path;
                var prefix = Application.platform == RuntimePlatform.IPhonePlayer ? "file://" : "";
            }
            IsRecording = false;
        }

 


        public void PreviewVideo()
        {
            if (vid == null) vid = GetComponent<VideoPlayer>();
            vid.enabled = true;
            vid.targetCamera = _cam;
            cnvPreview.enabled = true;
            cnvRR.enabled = true;
            vid.EnableAudioTrack(0, false);
            vid.source = VideoSource.VideoClip;
            vid.url = "file://" + _path;
            StartCoroutine(playVideo());
        }

        IEnumerator playVideo()
        {

            vid.Prepare();

            //Wait until video is prepared
            WaitForSeconds waitTime = new WaitForSeconds(1);
            while (!vid.isPrepared)
            {
                Debug.Log("Preparing Video");
                //Prepare/Wait for 5 sceonds only
                yield return waitTime;
                //Break out of the while loop after 5 seconds wait
            }

            //Assign the Texture from Video to RawImage to be displayed
            //image.texture = vid.texture;

            //Play Video
            vid.Play();

            //Play Sound
            //audioSource.Play();

            Debug.Log("Playing Video");
            while ((ulong)vid.frame < vid.frameCount)
            {
                //Debug.Log("Video Time: " + Mathf.FloorToInt((float)vid.time));
                yield return null;
            }
        }


        internal void ClosePreview()
        {
            vid.Stop();
            cnvPreview.enabled = false;
        }

        public void NativeShare()
        {
            vid.Stop();
            cnvPreview.enabled = false;
            NatShare.Share(_path, OnShare);
        }

        private void OnShare()
        {
            Debug.Log("Shared");
        }
    }
}