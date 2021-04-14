/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/
//public delegate void StopRecHandler(string RecFilename);

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

    public class ScreenRecorderOld : MonoBehaviour {

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

        public void StartRecording () {
            // Start recording
            recordingClock = new RealtimeClock();
            videoRecorder = new MP4Recorder(
                videoWidth,
                videoHeight,
                30,
                recordMicrophone ? AudioSettings.outputSampleRate : 0,
                recordMicrophone ? (int)AudioSettings.speakerMode : 0);
                //OnStopRecording
            //);
            
            // Create recording inputs
            cameraInput = new CameraInput(videoRecorder, recordingClock, _cam);
            if (recordMicrophone) {
                StartMicrophone();
                audioInput = new AudioInput(videoRecorder, recordingClock, microphoneSource, true);
            }
            IsRecording = true;
        }

        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
            microphoneSource.clip = Microphone.Start(null, true, 60, 48000);
            while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
            microphoneSource.timeSamples = Microphone.GetPosition(null);
            microphoneSource.loop = true;
            microphoneSource.Play();
            #endif
        }

        public void StopRecording () {
            if (IsRecording)
            {
                // Stop the recording inputs
                if (recordMicrophone)
                {
                    StopMicrophone();
                    audioInput.Dispose();
                }
                cameraInput.Dispose();
                // Stop recording
                //videoRecorder.Dispose();
            }
            IsRecording = false;
        }

        private void StopMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR
            Microphone.End(null);
            microphoneSource.Stop();
            #endif
        }

        void OnStopRecording(string path)
        {
            _path = path;
            if (OnStopRec != null) OnStopRec(path);
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