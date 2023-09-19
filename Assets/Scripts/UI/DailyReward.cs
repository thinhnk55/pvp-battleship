using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public TextMeshProUGUI amount;

    public Image imageBoard;
    public Image rewardCoin;

    public int id;
    public bool received;

    /*private void Start()
    {
        amount.text = GameData.GiftConfig[id].ToString();
    }*/
}
