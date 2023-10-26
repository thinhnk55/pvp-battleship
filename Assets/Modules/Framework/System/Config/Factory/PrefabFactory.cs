using System;
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
            GameObject.Instantiate(CoreDontDestroyOnLoad);
        }
        #region PrimitiveAsset
        [SerializeField] private GameObject textPrefab; public static GameObject TextPrefab { get { return Instance.textPrefab; } }
        [SerializeField] private GameObject audioSourcePrefab; public static GameObject AudioSourcePrefab { get { return Instance.audioSourcePrefab; } }
        [SerializeField] private GameObject coreDontDestroyOnLoad; public static GameObject CoreDontDestroyOnLoad { get { return Instance.coreDontDestroyOnLoad; } }
        #endregion
        [SerializeField] private GameObject octile; public static GameObject Octile { get { return Instance.octile; } }
        [SerializeField] private GameObject octileUI; public static GameObject OctileUI { get { return Instance.octileUI; } }
        [SerializeField] private GameObject[] ships; public static GameObject[] Ships { get { return Instance.ships; } }
        [SerializeField] private GameObject[] shipsUI; public static GameObject[] ShipsUI { get { return Instance.shipsUI; } }
        [SerializeField] private GameObject[] shipsPVE; public static GameObject[] ShipsPVE { get { return Instance.shipsPVE; } }
        [SerializeField] private GameObject plane; public static GameObject Plane { get { return Instance.plane; } }
        [SerializeField] private GameObject missle; public static GameObject Missle { get { return Instance.missle; } }
        [SerializeField] private GameObject popupAchie; public static GameObject PopupAchie { get { return Instance.popupAchie; } }
        [SerializeField] private GameObject popupConfirm; public static GameObject PopupConfirm { get { return Instance.popupConfirm; } }
        [SerializeField] private GameObject popupOutOfResource; public static GameObject PopupOutOfResource { get { return Instance.popupOutOfResource; } }
        [SerializeField] private GameObject popupMessage; public static GameObject PopupMessage { get { return Instance.popupMessage; } }
        [SerializeField] private GameObject popupInvalidFormation; public static GameObject PopupInvalidFormation { get { return Instance.popupInvalidFormation; } }
        [SerializeField] private GameObject popupGood; public static GameObject PopupGood { get { return Instance.popupGood; } }
        [SerializeField] private GameObject popupGoodApply; public static GameObject PopupGoodApply { get { return Instance.popupGoodApply; } }
        [SerializeField] private GameObject popupUserTerms; public static GameObject PopupUserTerms { get { return Instance.popupUserTerms; } }
        [SerializeField] private GameObject popupPrivacyPolicy; public static GameObject PopupPrivacyPolicy { get { return Instance.popupPrivacyPolicy; } }
        [SerializeField] private GameObject popupRPGood; public static GameObject PopupRPGood { get { return Instance.popupRPGood; } }
        [SerializeField] private GameObject popupStarter; public static GameObject PopupStarter { get { return Instance.popupStarter; } }
        [SerializeField] private GameObject popupQuestCompleted; public static GameObject PospupQuestCompleted { get { return Instance.popupQuestCompleted; } }
        [SerializeField] private GameObject popupShop; public static GameObject PopupShop { get { return Instance.popupShop; } }
        [SerializeField] private GameObject popupDisconnect; public static GameObject PopupDisconnect { get { return Instance.popupDisconnect; } }
        [SerializeField] private GameObject popupTuTorHome; public static GameObject PopupTuTorHome { get { return Instance.popupTuTorHome; } }
        [SerializeField] private GameObject popupTuTorBet; public static GameObject PopupTuTorBet { get { return Instance.popupTuTorBet; } }
        [SerializeField] private GameObject popupTuTorFormation; public static GameObject PopupTuTorFormation { get { return Instance.popupTuTorFormation; } }
        [SerializeField] private GameObject popupTuTorPlay; public static GameObject PopupTuTorPlay { get { return Instance.popupTuTorPlay; } }
        [SerializeField] private GameObject popupReceiveGift; public static GameObject PopupReceiveGift { get { return Instance.popupReceiveGift; } }
        [SerializeField] private GameObject popupLossPVE; public static GameObject PopupLossPVE { get { return Instance.popupLossPVE; } }
        [SerializeField] private GameObject popupRetreatPVE; public static GameObject PopupRetreatPVE { get { return Instance.popupRetreatPVE; } }
        [SerializeField] private GameObject popupResourcePVE; public static GameObject PopupResourcePVE { get { return Instance.popupResourcePVE; } }
        [SerializeField] private GameObject popupReceiveRewardCompletePVE; public static GameObject PopupReceiveRewardCompletePVE { get { return Instance.popupReceiveRewardCompletePVE; } }
        [SerializeField] private GameObject popupMissTurn; public static GameObject PopupMissTurn { get { return Instance.popupMissTurn; } }
        [SerializeField] private GameObject popupLuckyshot; public static GameObject PopupLuckyshot { get { return Instance.popupLuckyshot; } }
        [SerializeField] private GameObject popupLinkAccountMessage; public static GameObject PopupLinkAccountMessage { get { return Instance.popupLinkAccountMessage; } }
        [SerializeField] private GameObject popupDeleteAccount; public static GameObject PopupDeleteAccount { get { return Instance.popupDeleteAccount; } }
        [SerializeField] private GameObject popupDeleteConfirm; public static GameObject PopupDeleteConfirm { get { return Instance.popupDeleteConfirm; } }

        [SerializeField] private GameObject popupRPConfirm; public static GameObject PopupRPConfirm { get { return Instance.popupRPConfirm; } }
    }
}

