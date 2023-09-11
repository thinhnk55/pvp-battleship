using Server;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : SingletonMono<LoadingScene>
{
    [SerializeField] float loadingDuration = 1;
    float currentLoadingTime = 0;
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    AsyncOperation asynScene;
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
        FirebaseIntegration.FirebaseInitialization.OnInitialized += AutoLogin;
    }

    private void Update()
    {
        if (asynScene == null) { return; }

        currentLoadingTime += Time.deltaTime;
        loadingBar.value = asynScene.progress * 100 * Mathf.Clamp01(currentLoadingTime / loadingDuration);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        FirebaseIntegration.FirebaseInitialization.OnInitialized -= AutoLogin;
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
        if (SceneManager.sceneCount >= 2)
        {
            SceneManager.UnloadSceneAsync("Home");
        }
        asynScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asynScene.allowSceneActivation = true;
    }

    public void AutoLogin()
    {
        MainThreadDispatcher.ExecuteOnMainThread(() =>
        {
            WSClient.Instance.Connect(Authentication.DataAuth.AuthData.userId, Authentication.DataAuth.AuthData.token);
        });
    }
}