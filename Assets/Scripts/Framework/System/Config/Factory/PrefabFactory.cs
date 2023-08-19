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
        [SerializeField] private GameObject octileUI; public static GameObject OctileUI { get { return Instance.octileUI; } }
        [SerializeField] private GameObject[] ships; public static GameObject[] Ships { get { return Instance.ships; } }
        [SerializeField] private GameObject[] shipsUI; public static GameObject[] ShipsUI { get { return Instance.shipsUI; } }
        [SerializeField] private GameObject plane; public static GameObject Plane { get { return Instance.plane; } }
        [SerializeField] private GameObject missle; public static GameObject Missle { get { return Instance.missle; } }
        [SerializeField] private GameObject popupConfirm; public static GameObject PopupConfirm { get { return Instance.popupConfirm; } }
        [SerializeField] private GameObject popupMessage; public static GameObject PopupMessage { get { return Instance.popupMessage; } }
        [SerializeField] private GameObject popupGood; public static GameObject PopupGood { get { return Instance.popupGood; } }
        [SerializeField] private GameObject popupGoodApply; public static GameObject PopupGoodApply { get { return Instance.popupGoodApply; } }
        [SerializeField] private GameObject popupUserTerms; public static GameObject PopupUserTerms { get { return Instance.popupUserTerms; } }
        [SerializeField] private GameObject popupRPGood; public static GameObject PopupRPGood { get { return Instance.popupRPGood; } }
        [SerializeField] private GameObject popupStarter; public static GameObject PopupStarter { get { return Instance.popupStarter; } }
        [SerializeField] private GameObject popupQuestCompleted; public static GameObject PospupQuestCompleted { get { return Instance.popupQuestCompleted; } }
        [SerializeField] private GameObject popupShop; public static GameObject PopupShop { get { return Instance.popupShop; } }
        [SerializeField] private GameObject popupDisconnect; public static GameObject PopupDisconnect { get { return Instance.popupDisconnect; } }


    }
}

