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
        [SerializeField] private Sprite missed; public static Sprite Missed { get { return Instance.missed; } }
        [SerializeField] private Sprite occupied; public static Sprite Occupied { get { return Instance.occupied; } }
        [SerializeField] private Sprite occupiedPre; public static Sprite OccupiedPre { get { return Instance.occupiedPre; } }
        [SerializeField] private Sprite unoccupiable; public static Sprite Unoccupiable { get { return Instance.unoccupiable; } }
        [SerializeField] private Sprite attacked; public static Sprite Attacked { get { return Instance.attacked; } }
        [SerializeField] private Sprite destroyed; public static Sprite Destroyed { get { return Instance.destroyed; } }
        [SerializeField] private Sprite playerTurn; public static Sprite PlayerTurn { get { return Instance.playerTurn; } }
        [SerializeField] private Sprite opponentTurn; public static Sprite OpponentTurn { get { return Instance.opponentTurn; } }
        [SerializeField] private Sprite beri; public static Sprite Beri { get { return Instance.beri; } }
        [SerializeField] private Sprite[] bets; public static Sprite[] Bets { get { return Instance.bets; } }
        [SerializeField] private Sprite[] betsPVE; public static Sprite[] BetsPVE { get { return Instance.betsPVE; } }
        [SerializeField] private Sprite[] ranks; public static Sprite[] Ranks { get { return Instance.ranks; } }
        [SerializeField] private Sprite[] rewardLeaderBoard; public static Sprite[] RewardLeaderBoard { get { return Instance.rewardLeaderBoard; } }
        [SerializeField] private Sprite[] orderLeaderBoard; public static Sprite[] OrderLeaderBoard { get { return Instance.orderLeaderBoard; } }
        [SerializeField] private ListSprite[] achievements; public static ListSprite[] Achievements { get { return Instance.achievements; } }
        [SerializeField] private Sprite[] avatars; public static Sprite[] Avatars { get { return Instance.avatars; } }
        [SerializeField] private Sprite[] frames; public static Sprite[] Frames { get { return Instance.frames; } }
        [SerializeField] private Sprite[] tailframes; public static Sprite[] Tailframes { get { return Instance.tailframes; } }
        [SerializeField] private Sprite[] battleFields; public static Sprite[] BattleFields { get { return Instance.battleFields; } }
        [SerializeField] private ListSprite[] resourceIcons; public static ListSprite[] ResourceIcons { get { return Instance.resourceIcons; } }
        [SerializeField] private Sprite unKnowmAvatar; public static Sprite Unknown { get { return Instance.unKnowmAvatar; } }
        [SerializeField] private Sprite x; public static Sprite X { get { return Instance.x; } }
        [SerializeField] private Sprite questionMark; public static Sprite QuestionMark { get { return Instance.questionMark; } }
        [SerializeField] private Sprite win; public static Sprite Win { get { return Instance.win; } }
        [SerializeField] private Sprite lose; public static Sprite Lose { get { return Instance.lose; } }
        [SerializeField] private Sprite selectedAchievementBG; public static Sprite SelectedAchievementBG { get { return Instance.selectedAchievementBG; } }
        [SerializeField] private Sprite unselectedAchievementBG; public static Sprite UnselectedAchievementBG { get { return Instance.unselectedAchievementBG; } }
        [SerializeField] private Sprite selectedRankBG; public static Sprite SelectedRankBG { get { return Instance.selectedRankBG; } }
        [SerializeField] private Sprite unselectedRankBG; public static Sprite UnselectedRankBG { get { return Instance.unselectedRankBG; } }
        [SerializeField] private Sprite obtainableRankBG; public static Sprite ObtainableRankBG { get { return Instance.obtainableRankBG; } }
        [SerializeField] private Sprite unobtainableRankBG; public static Sprite UnobtainableRankBG { get { return Instance.unobtainableRankBG; } }
        [SerializeField] private Sprite shipLuckyShot; public static Sprite ShipLuckyShot { get { return Instance.shipLuckyShot; } }
        [SerializeField] private Sprite royalPassTreasure; public static Sprite RoyalPassTreasure { get { return Instance.royalPassTreasure; } }
        [SerializeField] private Sprite royalPassMilestone; public static Sprite RoyalPassMilestone { get { return Instance.royalPassMilestone; } }
        [SerializeField] private Sprite royalPassMilestoneOrder; public static Sprite RoyalPassMilestoneOrder { get { return Instance.royalPassMilestoneOrder; } }
        [SerializeField] private Sprite royalPassBeri; public static Sprite RoyalPassBeri { get { return Instance.royalPassBeri; } }
        [SerializeField] private Sprite royalPassGem; public static Sprite RoyalPassGem { get { return Instance.royalPassGem; } }
        [SerializeField] private Sprite royalPassOther; public static Sprite RoyalPassOther { get { return Instance.royalPassOther; } }
        [SerializeField] private Sprite trophy; public static Sprite Trophy { get { return Instance.trophy; } }
    }

    [System.Serializable]
    public class ListSprite
    {
        public Sprite[] sprites;
    }
}