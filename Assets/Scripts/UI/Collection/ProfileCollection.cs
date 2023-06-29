using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ProfileInfo
{
    public int Avatar;
    public string Username;
    public int Rank;
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
        ProfileInfo info = new ProfileInfo()
        {
            Avatar = profile.Avatar.Data,
            Username = profile.Username.Data,
            Rank = profile.Rank,
            Wins = profile.Wins,
            Losts = profile.Losts,
            WinRate = profile.WinRate,
            WinStreak = profile.WinStreak,
            WinStreakMax = profile.WinStreakMax,
            PerfectGame = profile.PerfectGame,
            Achievement = profile.Achievement,
        };
        BuildUI(info);
    }
}
