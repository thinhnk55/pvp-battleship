using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    Color darkColor = Color.Lerp(Color.white, Color.black, 0.5f);
    Color whiteColor = Color.white;

    [SerializeField] Image imageBoard;
    [SerializeField] Image rewardCoin;

    public int id;
    public bool received;

    private void Start()
    {
        SetStatusReward();
        DisplayReward();
    }

    public void SetStatusReward()
    {
        if(id <= GameData.ProgressGift) {
            received = true;
        }
        else
        {
            received = false;
        }
    }    

    public void DisplayReward()
    {
        if (received)
        {
            imageBoard.color = darkColor;
            rewardCoin.color = darkColor;
        }
        else
        {
            imageBoard.color = whiteColor;
            rewardCoin.color = whiteColor;
        }    
    }    
}
