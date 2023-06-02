using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXFactory : SingletonScriptableObject<VFXFactory>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if (_instance == null)
        {
            Instance.ToString();
        }
    }
    [SerializeField] private GameObject explosion; public static GameObject Explosion { get { return Instance.explosion; } }
    [SerializeField] private GameObject smoke; public static GameObject Smoke { get { return Instance.smoke; } }
    [SerializeField] private GameObject splashWater; public static GameObject SplashWater { get { return Instance.splashWater; } }
}
