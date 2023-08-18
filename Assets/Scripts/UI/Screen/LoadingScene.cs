using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : CacheMonoBehaviour
{
    [SerializeField] float loadingDuration = 1;
    float currentLoadingTime = 0;
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    AsyncOperation asyn;
    private void Awake()
    {
        asyn = SceneManager.LoadSceneAsync("PreHome", LoadSceneMode.Additive);
        asyn.allowSceneActivation = false;
        loadingBar.maxValue = 100;
        loadingBar.onValueChanged.AddListener((value) =>
        {
            if (value == 100)
            {
                SceneManager.UnloadSceneAsync("Loading");
            }
        });
        InvokeRepeating("CheckMultipleAudioListener", 0, 0.1f);
    }
    private void Update()
    {
        currentLoadingTime += Time.deltaTime;
        loadingBar.value = Mathf.Pow(asyn.progress * 100, currentLoadingTime / loadingDuration);
        Debug.Log(loadingBar.value);

    }
    public void CheckMultipleAudioListener()
    {
        aL = FindObjectsOfType<AudioListener>();
        if (aL.Length >= 2)
        {
            DestroyImmediate(aL[0]);
        }
    }

}