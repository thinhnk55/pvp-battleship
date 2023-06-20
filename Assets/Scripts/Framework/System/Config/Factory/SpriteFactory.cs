using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SpriteFactory : SingletonScriptableObject<SpriteFactory>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            if (_instance == null)
            {
                Instance.ToString();
            }
        }
        [SerializeField] private Sprite octile; public static Sprite Octile { get { return Instance.octile; } }
        [SerializeField] private Sprite occupied; public static Sprite Occupied { get { return Instance.occupied; } }
        [SerializeField] private Sprite unoccupiable; public static Sprite Unoccupiable { get { return Instance.unoccupiable; } }
        [SerializeField] private Sprite attacked; public static Sprite Attacked { get { return Instance.attacked; } }
        [SerializeField] private Sprite destroyed; public static Sprite Destroyed { get { return Instance.destroyed; } }
        [SerializeField] private Sprite playerTurn; public static Sprite PlayerTurn { get { return Instance.playerTurn; } }
        [SerializeField] private Sprite opponentTurn; public static Sprite OpponentTurn { get { return Instance.opponentTurn; } }
        [SerializeField] private Sprite beri; public static Sprite Beri { get { return Instance.beri; } }
        [SerializeField] private Sprite[] ranks; public static Sprite[] Ranks { get { return Instance.ranks; } }
        [SerializeField] private ListSprite[] achievements; public static ListSprite[] Achievements { get { return Instance.achievements; } }
        [SerializeField] private Sprite[] avatars; public static Sprite[] Avatars { get { return Instance.avatars; } }
        [SerializeField] private Sprite unKnowmAvatar; public static Sprite Unknown { get { return Instance.unKnowmAvatar; } }
        [SerializeField] private Sprite x; public static Sprite X { get { return Instance.x; } }
        [SerializeField] private Sprite win; public static Sprite Win { get { return Instance.win; } }
        [SerializeField] private Sprite lose; public static Sprite Lose { get { return Instance.lose; } }
        [SerializeField] private Sprite selectedAchievementBG; public static Sprite SelectedAchievementBG { get { return Instance.selectedAchievementBG; } }
        [SerializeField] private Sprite unselectedAchievementBG; public static Sprite UnselectedAchievementBG { get { return Instance.unselectedAchievementBG; } }


    }

    [System.Serializable]
    public class ListSprite
    {
        public Sprite[] sprites;
    }
}