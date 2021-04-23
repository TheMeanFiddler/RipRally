using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetReferenceUtility : MonoBehaviour
{
    AudioSource _music;
    public AssetReference ClipToLoad;
    public AssetReference objectToLoad;
    private GameObject instantiatedObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartMusic()
    {
        Main.Instance.PopupMsg("Start Music");
        _music = GetComponent<AudioSource>();
        //Addressables.LoadAssetAsync<AudioClip>(ClipToLoad).Completed += AudioLoaded;
        Addressables.LoadAssetAsync<GameObject>(objectToLoad).Completed += ObjectLoadDone;
    }

    private void AudioLoaded(AsyncOperationHandle<AudioClip> obj)
    {
        Main.Instance.PopupMsg("Audio Loaded");
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            AudioClip loadedClip = obj.Result;
            _music.clip = loadedClip;
            _music.Play();
        }
    }

    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = obj.Result;
            Debug.Log("Successfully loaded object.");
            instantiatedObject = Instantiate(loadedObject);
            instantiatedObject.transform.position = new Vector3(1000, -298, 25);
            Debug.Log("Successfully instantiated object.");
        }
    }
    /*
    private void ObjectLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject loadedObject = obj.Result;
            Debug.Log("Successfully loaded object.");
            instantiatedObject = Instantiate(loadedObject);
            Debug.Log("Successfully instantiated object.");
            if (accessoryObjectToLoad != null)
            {
                accessoryObjectToLoad.InstantiateAsync(instantiatedObject.transform).Completed += op =>
                {
                    if (op.Status == AsyncOperationStatus.Succeeded)
                    {
                        instantiatedAccessoryObject = op.Result;
                        Debug.Log("Successfully loaded and instantiated accessory object.");
                    }
                };
            }
        }
    }
    */
}
