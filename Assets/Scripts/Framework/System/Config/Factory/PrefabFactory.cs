using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public class PrefabFactory : SingletonScriptableObject<PrefabFactory>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            if (_instance == null)
            {
                Instance.ToString();
            }
        }
        #region PrimitiveAsset
        [SerializeField] private GameObject textPrefab; public static GameObject TextPrefab { get { return Instance.textPrefab; } }
        [SerializeField] private GameObject audioSourcePrefab; public static GameObject AudioSourcePrefab { get { return Instance.audioSourcePrefab; } }
        #endregion

        [SerializeField] private GameObject octile; public static GameObject Octile { get { return Instance.octile; } }
        [SerializeField] private GameObject[] ships; public static GameObject[] Ships { get { return Instance.ships; } }
        [SerializeField] private GameObject[] popupConfirm; public static GameObject[] PopupConfirm { get { return Instance.popupConfirm; } }


    }
}

