using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    

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
            imageBoard.color = Color.Lerp(Color.white, Color.black, 0.5f); ;
            rewardCoin.color = Color.Lerp(Color.white, Color.black, 0.5f); ;
        }
        else
        {
            imageBoard.color = Color.white;
            rewardCoin.color = Color.white;
        }    
    }    
}
