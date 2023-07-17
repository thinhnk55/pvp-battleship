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
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    private void Awake()
    {
        SceneManager.LoadScene("PreHome", LoadSceneMode.Additive);
        loadingBar.maxValue = 100;
        DOTween.To(() => loadingBar.value, (value) => loadingBar.value = value, loadingBar.maxValue, loadingDuration);
        DOVirtual.DelayedCall(loadingDuration, () =>
        {
            SceneManager.UnloadSceneAsync("Loading");
        }, false);
        InvokeRepeating("CheckMultipleAudioListener", 0, 0.1f);
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
