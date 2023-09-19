using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DailyRewardDisplay : MonoBehaviour
{
    [SerializeField] List<DailyReward> rewards = new();

    [SerializeField] GameObject resource;

    [SerializeField] Button claim;

    [SerializeField] int preValue;

    private void Start()
    {
        preValue = GameData.ProgressGift;
        SetStatusRewards(0);
        StartCoroutine(DisplayRewards(0f));
        claim.onClick.AddListener(OnClickClaimBt);
        Debug.Log("Start" + GameData.ProgressGift);
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
}
