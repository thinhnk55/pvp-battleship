using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCollection : CardCollectionBase<ProfileData>
{
    [SerializeField] bool isPlayer;
    [SerializeField] Image avatar;
    [SerializeField] Image frame;
    [SerializeField] Image frameTail;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] Image rankIcon;
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] Slider rankProgress;
    [SerializeField] TextMeshProUGUI battles;
    [SerializeField] TextMeshProUGUI shipDestroyed;
    [SerializeField] TextMeshProUGUI perfectGame;
    [SerializeField] TextMeshProUGUI winStreak;
    [SerializeField] TextMeshProUGUI winStreakMax;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI losts;
    [SerializeField] TextMeshProUGUI winRate;
    private void Awake()
    {
        if (isPlayer)
        {
            BuildUI(GameData.Player);
        }
        else
        {
            BuildUI(GameData.Opponent);
        }
    }

    public void BuildUI(ProfileData infos)
    {
        if (avatar)
        {
            if (infos.Avatar == -1)
            {
                avatar.sprite = SpriteFactory.Unknown;
            }
            else
            {
                avatar.sprite = SpriteFactory.Avatars[infos.Avatar];

            }
        }
        if (username)
        {
            username.text = infos.Username;
        }
        if (rank)
        {
            rank.text = infos.Rank.ToString();
        }
        if (rankIcon)
        {
            rankIcon.sprite = SpriteFactory.Ranks[infos.Rank%12];
        }
        if (rankProgress)
        {
            rankProgress.value = 0;
        }
        if (battles)
        {
            battles.text = (infos.Wins + infos.Losts).ToString();
        }
        if (shipDestroyed)
        {
            shipDestroyed.text = infos.Achievement[(int)AchievementType.ENVOVY_OF_WAR].ToString();
        }
        if (perfectGame)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (winStreak)
        {
            winStreak.text = infos.WinStreak.ToString();
        }
        if (winStreakMax)
        {
            winStreakMax.text = infos.WinStreakMax.ToString();
        }
        if (wins)
        {
            wins.text = infos.Wins.ToString();
        }
        if (losts)
        {
            losts.text = infos.Losts.ToString();
        }
        if (winRate)
        {
            winRate.text = (infos.WinRate * 100).ToString("F1") + "%";
        }
    }
}
