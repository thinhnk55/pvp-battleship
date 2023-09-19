using Framework;
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

    

    private void Start()
    {
        Timer<Gift>.Instance.OnTrigger += OnTrigger;
        Timer<Gift>.Instance.OnElapse += OnElapse;

        SetStatusRewards(0);
        StartCoroutine(DisplayRewards(0f));
        claim.onClick.AddListener(OnClickClaimBt);
        Debug.Log("Start" + GameData.ProgressGift);


        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            countDown.text = "Collect";
        }
        else
        {
            countDown.text = Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        }
    }

    public void OnClickClaimBt()
    {
        if (Timer<Gift>.Instance.TriggersFromBegin >= 1)
        {
            Debug.Log("Onclick" + GameData.ProgressGift);
            SetStatusRewards(1);
            StartCoroutine(DisplayRewards(1f));
            StartCoroutine(ActiveResource());
            CoinVFX.CoinVfx(resource.transform, rewards[GameData.ProgressGift].transform.position, rewards[GameData.ProgressGift].transform.position);

        }
    }

    


    public void SetStatusRewards(int number)
    {
        foreach (var reward in rewards)
        {
            if (reward.id < GameData.ProgressGift + number)
            {
                reward.received = true;
            }
            else
            {
                reward.received = false;
            }
        }    
    }

    public IEnumerator DisplayRewards(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (var reward in rewards)
        {
            if (reward.received)
            {
                reward.imageBoard.color = Color.Lerp(Color.white, Color.black, 0.5f); ;
                reward.rewardCoin.color = Color.Lerp(Color.white, Color.black, 0.5f); ;
            }
            else
            {
                reward.imageBoard.color = Color.white;
                reward.rewardCoin.color = Color.white;
            }
        }
        
    }

    public IEnumerator ActiveResource()
    {
        resource.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        resource.SetActive(false);

    }

    private void OnTrigger()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" :"next reward: " +Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
        //reminder.UpdateObject();
    }

    private void OnElapse()
    {
        countDown.text = Timer<Gift>.Instance.TriggersFromBegin >= 1 ? "Collect" : "next reward: " + Timer<Gift>.Instance.RemainTime_Sec.Hour_Minute_Second_1();
    }
}
