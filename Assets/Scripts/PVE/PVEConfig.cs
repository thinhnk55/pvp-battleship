using Framework;
using UnityEngine;

public class PVEConfig : SingletonScriptableObjectModulized<PVEConfig>
{
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (_instance == null)
        {
            Instance.ToString();
        }
    }
#endif
    [SerializeField] private string userAgreement; public static string UserAgreement { get { return Instance.userAgreement; } set { Instance.userAgreement = value; } }
}
