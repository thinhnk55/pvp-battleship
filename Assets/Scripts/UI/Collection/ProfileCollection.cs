using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public struct ProfileInfo
{
    public int Avatar;
    public int Frame;
    public string Username;
    public int Rank;
    public int Point;
    public int Wins;
    public int Losts;
    public float WinRate;
    public int WinStreak;
    public int WinStreakMax;
    public int PerfectGame;
    public List<int> Achievement;
}
public class ProfileCollection : CardCollectionBase<ProfileInfo>
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
        UpdateUIs();
        GameData.Player.Avatar.OnDataChanged += OnDataChanged;
        GameData.Player.FrameAvatar.OnDataChanged += OnDataChanged;
        GameData.Player.Username.OnDataChanged += OnDataChanged;
    }
    private void OnDestroy()
    {
        GameData.Player.Avatar.OnDataChanged -= OnDataChanged;
        GameData.Player.FrameAvatar.OnDataChanged -= OnDataChanged;
        GameData.Player.Username.OnDataChanged -= OnDataChanged;
    }
    private void OnDataChanged(int arg1, int arg2)
    {
        UpdateUIs();
    }
    private void OnDataChanged(string arg1, string arg2)
    {
        UpdateUIs();
    }
    public void BuildUI(ProfileInfo infos)
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
        if (frame)
        {
            if (infos.Frame >= 0)
            {
                frame.sprite = SpriteFactory.ResourceIcons[(int)PNonConsumableType.AVATAR_FRAME].sprites[infos.Frame];
                frame.SetAlpha(1);
            }
            else
            {
                frame.SetAlpha(0);
            }
        }
        if (frameTail)
        {
            frameTail.sprite = SpriteFactory.Tailframes[infos.Frame];
        }
        if (username)
        {
            username.text = infos.Username;
        }
        if (rank)
        {
            rank.text = GameData.RankConfigs[infos.Rank].Title;
        }
        if (rankIcon)
        {
            rankIcon.sprite = SpriteFactory.Ranks[infos.Rank];
        }
        if (rankProgress)
        {
            rankProgress.maxValue = GameData.RankConfigs[infos.Rank].Point;
            rankProgress.value = infos.Point;
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

    public override void UpdateUIs()
    {
        var profile = new ProfileData();
        if (isPlayer)
        {
            profile = GameData.Player;
        }
        else
        {
            profile = GameData.Opponent;
        }
        if (profile != null)
        {
            ProfileInfo info = new ProfileInfo()
            {
                Avatar = profile.Avatar.Data,
                Frame = profile.FrameAvatar.Data,
                Username = profile.Username.Data,
                Rank = profile.Rank,
                Point = profile.Point,
                Wins = profile.Wins,
                Losts = profile.Losts,
                WinRate = profile.WinRate,
                WinStreak = profile.WinStreak,
                WinStreakMax = profile.WinStreakMax,
                PerfectGame = profile.PerfectGame,
                Achievement = profile.AchievementProgress,
            };
            BuildUI(info);
        }

    }
}
