using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MaxSdkBase;


public class DailyRewardDisplay : MonoBehaviour
{
    [SerializeField] List<DailyReward> rewards = new();

    [SerializeField] GameObject resource;
    [SerializeField] TextMeshProUGUI countDown;
    [SerializeField] Button claim;
    [SerializeField] Button watchAds;
    


    private void Start()
    {     
        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;
        SetAmount();          
        claim.onClick.AddListener(OnClickClaimBt);
        DisplayRewards();
        /*if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            //countDown.text = "Collect";
            claim.interactable = true;
            watchAds.interactable = true;
            
        }
        else
        {
            claim.interactable = false;
            watchAds.interactable = false;
            //countDown.text = Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        }*/
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

    public void DisplayRewards()
    {  
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            foreach (var reward in rewards)
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
        else
        {
            foreach (var reward in rewards)
            {               
                reward.ChangeSprite(reward.defaultBroard);             
            }
        }
        

    }

    public IEnumerator ActiveResource()
    {
        resource.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        resource.SetActive(false);
    }

    public void SetAmount()
    {
        if(rewards != null)
        {
            foreach (var reward in rewards)
            {
                reward.amount.text = "x" + GameData.GiftConfig[reward.id].ToString();
                //Debug.Log($"GameData.GiftConfig[{reward.id}] = {GameData.GiftConfig[reward.id]} ");
            }
        }          
    }    

    public void ToggleButton()
    {
        if(Timer<Gift>.Instance.TriggersFromBegin >= 1)
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

    private void OnTrigger()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1() + "s";
        //reminder.UpdateObject();        
        DisplayRewards();
        ToggleButton();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" :  Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1() + "s";      
        DisplayRewards();
        ToggleButton();
    }
}
