using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCollection : CardCollectionBase<ProfileData>
{
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] Image rankIcon;
    [SerializeField] TextMeshProUGUI rank;
    [SerializeField] Slider rankProgress;
    [SerializeField] TextMeshProUGUI shipDestroyed;
    [SerializeField] TextMeshProUGUI perfectGame;
    [SerializeField] TextMeshProUGUI winStreak;
    [SerializeField] TextMeshProUGUI winStreakMax;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI losts;
    [SerializeField] TextMeshProUGUI winRate;
    private void Awake()
    {

        BuildUI(GameData.Player);
    }

    protected void BuildUI(ProfileData infos)
    {
        if (username)
        {
            username.text = infos.Username;
        }
        if (rank)
        {
            rank.text = infos.Point.ToString();
        }
        if (rankIcon)
        {
            rankIcon.sprite = null;
        }
        if (rankProgress)
        {
            rankProgress.value = 0;
        }
        if (shipDestroyed)
        {
            shipDestroyed.text = infos.ShipDestroyed.ToString();
        }
        if (perfectGame)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (winStreak)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (winStreakMax)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (wins)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (losts)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
        if (winRate)
        {
            perfectGame.text = infos.PerfectGame.ToString();
        }
    }
}
