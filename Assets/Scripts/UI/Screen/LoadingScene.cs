using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : SingletonMono<LoadingScene>
{
    [SerializeField] float loadingDuration = 1;
    float currentLoadingTime = 0;
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    AsyncOperation asyn;
    private void Start()
    {
        loadingBar.maxValue = 100;
        loadingBar.onValueChanged.AddListener((value) =>
        {
            if (loadingBar.value == 100)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
            }
        });
        InvokeRepeating("CheckMultipleAudioListener", 0, 0.1f);
        WSClient.Instance.Connect(Authentication.DataAuth.AuthData.userId, Authentication.DataAuth.AuthData.token);
    }

    private void Update()
    {
        if(asyn == null) { return; }

        currentLoadingTime += Time.deltaTime;
        loadingBar.value = asyn.progress * 100 * Mathf.Clamp01(currentLoadingTime /loadingDuration);
    }
    void CheckMultipleAudioListener()
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
        asyn.allowSceneActivation = true;
    }

}