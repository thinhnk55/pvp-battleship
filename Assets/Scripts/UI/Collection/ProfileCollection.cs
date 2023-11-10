using Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct ProfileInfo
{
    public int UserId;
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
    [SerializeField] TextMeshProUGUI userId;
    [SerializeField] Image avatar;
    [SerializeField] Image frame;
    [SerializeField] Image frameTail;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] Image rankIcon;
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] TextMeshProUGUI pointRank;
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
        if (isPlayer)
        {
            GameData.Player.Avatar.OnDataChanged += OnDataChanged;
            GameData.Player.FrameAvatar.OnDataChanged += OnDataChanged;
            GameData.Player.Username.OnDataChanged += OnDataChanged;
        }
        else
        {
            GameData.Opponent.Avatar.OnDataChanged += OnDataChanged;
            GameData.Opponent.FrameAvatar.OnDataChanged += OnDataChanged;
            GameData.Opponent.Username.OnDataChanged += OnDataChanged;
        }

    }
    private void OnDestroy()
    {
        if (isPlayer)
        {
            GameData.Player.Avatar.OnDataChanged -= OnDataChanged;
            GameData.Player.FrameAvatar.OnDataChanged -= OnDataChanged;
            GameData.Player.Username.OnDataChanged -= OnDataChanged;
        }
        else
        {
            GameData.Opponent.Avatar.OnDataChanged -= OnDataChanged;
            GameData.Opponent.FrameAvatar.OnDataChanged -= OnDataChanged;
            GameData.Opponent.Username.OnDataChanged -= OnDataChanged;
        }
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
        if (userId)
        {
            userId.text = "#" + infos.UserId.ToString();
        }
        if (avatar)
        {
            if (infos.Avatar == -1)
            {
                avatar.sprite = SpriteFactory.Unknown;
            }
            else
            {
                avatar.sprite = SpriteFactory.ResourceIcons[(int)PNonConsumableType.AVATAR].sprites[infos.Avatar];
            }
        }
        if (frame)
        {
            if (infos.Frame >= 0)
            {
                frame.sprite = SpriteFactory.ResourceIcons[(int)PNonConsumableType.AVATAR_FRAME].sprites.GetLoop(infos.Frame);
                frame.SetAlpha(1);
            }
            else
            {
                frame.SetAlpha(0);
            }
        }
        if (frameTail)
        {
            if (infos.Avatar == -1)
            {
                avatar.SetAlpha(0);
            }
            else
            {
                avatar.SetAlpha(1);
                frameTail.sprite = SpriteFactory.Tailframes.GetLoop(infos.Frame);
            }
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
            rankProgress.maxValue = GameData.RankConfigs.GetClamp(infos.Rank + 1).Point;
            rankProgress.value = infos.Point;
            pointRank?.SetText(rankProgress.value + "/" + rankProgress.maxValue);
        }
        if (battles && !infos.Achievement.IsNullOrEmpty())
        {
            battles.text = (infos.Wins + infos.Losts).ToString();
        }
        if (shipDestroyed && !infos.Achievement.IsNullOrEmpty())
        {
            shipDestroyed.text = infos.Achievement[(int)AchievementType.DESTROY_SHIP].ToString();
        }
        if (perfectGame && !infos.Achievement.IsNullOrEmpty())
        {
            perfectGame.text = infos.Achievement[(int)AchievementType.PERFECT_GAME].ToString();
        }
        if (winStreak && !infos.Achievement.IsNullOrEmpty())
        {
            winStreak.text = infos.Achievement[(int)AchievementType.WIN_STREAK_MAX].ToString();
        }
        winStreakMax?.SetText(infos.WinStreakMax.ToString());
        if (wins)
        {
            wins.text = infos.Achievement[(int)AchievementType.WIN].ToString();
        }
        if (losts)
        {
            //losts.text = infos.Achievement[(int)AchievementType.].ToString() - infos.Achievement[(int)AchievementType.WIN].ToString();
        }
        if (winRate && !infos.Achievement.IsNullOrEmpty())
        {
            winRate.text = (infos.WinRate * 100).ToString("F1") + "%";
        }
    }

    public override void UpdateUIs()
    {
        ProfileData profile = null;
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
            if (profile.Avatar == null)
            {
                profile.Avatar = new PDataUnit<int>(-1);
            }
            if (profile.FrameAvatar == null)
            {
                profile.FrameAvatar = new PDataUnit<int>(-1);
            }
            if (profile.Username == null)
            {
                profile.Username = new PDataUnit<string>("");
            }
            ProfileInfo info = new ProfileInfo()
            {
                UserId = profile.UserId,
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
