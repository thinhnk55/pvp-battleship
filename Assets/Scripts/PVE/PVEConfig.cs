using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEConfig :SingletonScriptableObjectModulized<PVEConfig>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (_instance == null)
        {
            Instance.ToString();
        }
    }
    [SerializeField] private string userAgreement; public static string UserAgreement { get { return Instance.userAgreement; } set { Instance.userAgreement = value; } }
}
