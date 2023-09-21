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
}
