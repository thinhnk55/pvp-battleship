using FirebaseIntegration;
using Server;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : SingletonMono<LoadingScene>
{
    [SerializeField] float loadingDuration = 1;
    float currentLoadingTime = 0;
    [SerializeField] Slider loadingBar;
    AudioListener[] aL;
    EventSystem[] eS;
    AsyncOperation asynScene;
    private void Start()
    {
        loadingBar.maxValue = 100;
        loadingBar.onValueChanged.AddListener((value) =>
        {
            if (loadingBar.value == 100)
            {
                SceneManager.UnloadSceneAsync("Loading");
            }
        });
        InvokeRepeating("CheckMultipleAudioListenerAndEventSystem", 0, 0.1f);

        FirebaseInitialization.OnInitialized += AutoLogin;
        if (FirebaseInitialization.initialized == true)
        {
            AutoLogin();
        }
        else
        {
            FirebaseInitialization.Initialize();
        }

    }

    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable || (!WSClient.Instance.ws.IsAlive && currentLoadingTime > 5))
        {
            WSClientHandler.PopupReconnect();
            return;
        }
        currentLoadingTime += Time.deltaTime;
        if (asynScene != null)
        {
            loadingBar.value = (asynScene.progress + (FirebaseInitialization.initialized ? 1 : 0)) / 2 * 100 * Mathf.Clamp01(currentLoadingTime / loadingDuration);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        FirebaseIntegration.FirebaseInitialization.OnInitialized -= AutoLogin;
    }
    void CheckMultipleAudioListenerAndEventSystem()
    {
        aL = FindObjectsByType<AudioListener>(FindObjectsSortMode.InstanceID);
        eS = FindObjectsByType<EventSystem>(FindObjectsSortMode.InstanceID);
        if (aL.Length >= 2)
        {
            DestroyImmediate(aL[0].gameObject);
        }
        if (eS.Length >= 2)
        {
            DestroyImmediate(eS[0].gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Load " + sceneName);
        if (SceneManager.sceneCount >= 2)
        {
            try
            {
                if (SceneManager.GetSceneByName("Home").isLoaded)
                {
                    SceneManager.UnloadSceneAsync("Home");
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
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