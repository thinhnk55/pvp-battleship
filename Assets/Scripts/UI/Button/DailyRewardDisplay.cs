using Framework;
using Monetization;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DailyRewardDisplay : MonoBehaviour
{
    [SerializeField] List<DailyReward> rewards = new();

    [SerializeField] GameObject resource;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] Button claim;
    [SerializeField] Button watchAds;



    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REWARD_ADS, OnGetAdsGift);


        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        SetAmount();
        claim.onClick.AddListener(OnClickClaimBt);
        HighLightRewards();

    }

    private void OnDestroy()
    {
        
        Timer<Gift>.Instance.OnTrigger -= OnTrigger;
        Timer<Gift>.Instance.OnElapse -= OnElapse;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REWARD_ADS, OnGetAdsGift);
    }

    public void OnClickClaimBt()
    {
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            //StartCoroutine(DisplayRewards(0f));
            StartCoroutine(ActiveResource());
            CoinVFX.CoinVfx(resource.transform, rewards[GameData.ProgressGift].transform.position, rewards[GameData.ProgressGift].transform.position);
        }
    }

    public void OnClickWatchAdsBt()
    {
        
    }



   

    public void HighLightRewards()
    {
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            if (rewards != null)
            {
                foreach (DailyReward reward in rewards)
                {
                    if (reward.id == GameData.ProgressGift)
                    {
                        reward.ChangeSprite(reward.highlightBroard);
                    }
                    else
                    {
                        reward.ChangeSprite(reward.defaultBroard);
                    }
                }
            }

        }
        else
        {
            if (rewards != null)
            {
                foreach (DailyReward reward in rewards)
                {
                    reward.ChangeSprite(reward.defaultBroard);
                }
            }

        }
    }

    public void DisplayReward()
    {
        if (rewards != null)
        {
            foreach (DailyReward reward in rewards)
            {
                if (reward.id < GameData.ProgressGift)
                {
                    reward.Received();
                }
                else
                {
                    reward.NotReceived();
                }
            }
        }

    }

    public IEnumerator ActiveResource()
    {
        if (resource != null)
        {
            resource.SetActive(true);
            yield return new WaitForSeconds(2.5f);
            resource.SetActive(false);
        }

    }

    public void SetAmount()
    {
        if (rewards != null)
        {
            foreach (DailyReward reward in rewards)
            {
                reward.amount.text = "x" + GameData.GiftConfig[reward.id].ToString();
                //Debug.Log($"GameData.GiftConfig[{reward.id}] = {GameData.GiftConfig[reward.id]} ");
            }
        }
    }

    public void ToggleButton()
    {
        if (claim != null && watchAds != null)
        {
            if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
            {

                claim.interactable = true;
                watchAds.interactable = true;
            }
            else
            {   
                claim.interactable = false;
                watchAds.interactable = false;           
            }
        }
        else
        {      
            return;
            
        }       
    }

    private void OnTrigger()
    {
        
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : "<color=#FFFFFF>Next rewards: </color>" + Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1() + "s";
        //reminder.UpdateObject();        
        HighLightRewards();
        ToggleButton();
        DisplayReward();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : "<color=#FFFFFF>Next rewards: </color>" + Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1() + "s";
        HighLightRewards();
        ToggleButton();
        DisplayReward();
    }

    public void OnGetAdsGift(JSONNode data)
    {
       
        PConsumableType.BERRY.AddValue(int.Parse(data["d"]["x"]["g"]));
        Timer<Gift>.Instance.BeginPoint = DateTime.UtcNow.Ticks;
        
        StartCoroutine(ActiveResource());
        if (resource != null)
        {
            CoinVFX.CoinVfx(resource.transform, rewards[GameData.ProgressGift].transform.position, rewards[GameData.ProgressGift].transform.position);
            Debug.Log("coinfly");
        }
        GameData.ProgressGift++;
    }
}
