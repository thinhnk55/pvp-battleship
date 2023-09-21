using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public TextMeshProUGUI amount;

    public GameObject tick;
    public Image imageBoard;
    public Image rewardCoin;

    public Sprite defaultBroard;
    public Sprite highlightBroard;

    public int id;

    public void ChangeSprite(Sprite _sprite)
    {
        if (imageBoard != null && _sprite != null)
        {
            imageBoard.sprite = _sprite;
        }    
        
    }   
    
    public void Received()
    {
        if(tick != null && imageBoard != null && rewardCoin != null)
        {
            tick.SetActive(true);
            imageBoard.color = Color.Lerp(Color.black, Color.white, 0.5f);
            rewardCoin.color = Color.Lerp(Color.black, Color.white, 0.5f);
        }    
        
    }  
    
    public void NotReceived()
    {
        if (tick != null && imageBoard != null && rewardCoin != null)
        {
            tick.SetActive(false);
            imageBoard.color = Color.white;
            rewardCoin.color = Color.white;
        }               
    }    
}
