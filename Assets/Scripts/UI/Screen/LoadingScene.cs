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
    private void Awake()
    {
        SceneManager.LoadScene("PreHome", LoadSceneMode.Additive);
        GetComponent<Image>().DOFade(0, loadingDuration).OnComplete(() =>
        {
            SceneManager.UnloadSceneAsync("Loading");
        });
    }
}
