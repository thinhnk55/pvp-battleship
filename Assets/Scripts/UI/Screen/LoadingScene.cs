using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class LoadingScene : SingletonMono<LoadingScene>
{
    [SerializeField] float loadingDuration = 1;
    float currentLoadingTime = 0;
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    AsyncOperation asyn;
    bool flag; // Sau khi hàm LoadScene được gọi thì mới chạy update

    protected override void Awake()
    {
        //base.Awake();
        //WSClientHandler.Instance.Connect();
    }

    private void Start()
    {
        WSClientHandler.Instance.Connect();
    }

    private void Update()
    {
        if (!flag)
            return;

        currentLoadingTime += Time.deltaTime;
        loadingBar.value = Mathf.Pow(asyn.progress * 100, currentLoadingTime /loadingDuration);

    }
    public void CheckMultipleAudioListener()
    {
        aL = FindObjectsOfType<AudioListener>();
        if (aL.Length >= 2)
        {
            DestroyImmediate(aL[0]);
        }
    }

    public void LoadScene(string sceneName)
    {
        asyn = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyn.allowSceneActivation = false;
        loadingBar.maxValue = 100;
        flag = true;
        loadingBar.onValueChanged.AddListener((value) =>
        {
            if (value == 100)
            {
                SceneManager.UnloadSceneAsync("Loading");
            }
        });
        InvokeRepeating("CheckMultipleAudioListener", 0, 0.1f);
    }

}