using FirebaseIntegration;
using Framework;
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
    [SerializeField] Image claimImg;
    [SerializeField] Image watchAdsImg;
    [SerializeField] Sprite claimDefault;
    [SerializeField] Sprite watchAdsDefault;
    [SerializeField] Sprite btInteractable;


    private void Start()
    {
        AnalyticsHelper.SelectContent("daily_reward");
        ServerMessenger.AddListener<JSONNode>(ServerResponse._REWARD_ADS, OnGetAdsGift);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._GIFT, OnClickClaimBt);

        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        SetAmount();
        //claim.onClick.AddListener(OnClickClaimBt);
        HighLightRewards();
        resource.SetActive(false);
    }

    private void OnDestroy()
    {

        Timer<Gift>.Instance.OnTrigger -= OnTrigger;
        Timer<Gift>.Instance.OnElapse -= OnElapse;
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._REWARD_ADS, OnGetAdsGift);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._GIFT, OnClickClaimBt);
    }

    public void OnClickClaimBt(JSONNode data)
    {
        if (data["e"].AsInt != 0)
            return;
        else
        {

            StartCoroutine(ActiveResource());
            if (GameData.ProgressGift == 0)
            {
                CoinVFX.CoinVfx(resource.transform, rewards[^1].transform.position, rewards[^1].transform.position);
            }
            else
            {
                CoinVFX.CoinVfx(resource.transform, rewards[GameData.ProgressGift - 1].transform.position, rewards[GameData.ProgressGift - 1].transform.position);
            }

        }

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
                claimImg.sprite = claimDefault;
                watchAdsImg.sprite = watchAdsDefault;
                claim.interactable = true;
                watchAds.interactable = true;
            }
            else
            {
                claimImg.sprite = btInteractable;
                watchAdsImg.sprite = btInteractable;
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
