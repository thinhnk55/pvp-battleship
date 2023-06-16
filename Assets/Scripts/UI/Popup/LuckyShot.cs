using DG.Tweening;
using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LuckyShot : Singleton<LuckyShot>
{
    public int indexShot;
    public List<GameObject> rockets;
    public List<Button> shots;
    public List<RewardInfo> rewards;

    [SerializeField] GameObject rocketRoot;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject shotRoot;
    [SerializeField] RewardCollection rewardCollection;
    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_LUCKY_SHOT, Instance.RecieveLuckyShot);
        double totalSecond = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - GameData.LastLuckyShot).TotalSeconds;

        var rocketC = Mathf.FloorToInt(Mathf.Clamp((float)totalSecond, 0, 10000000)) / GameData.RestoreRocketInterval;
        var rocketR = Mathf.FloorToInt(Mathf.Clamp((float)totalSecond, 0, 10000000)) % GameData.RestoreRocketInterval;
        GameData.RocketCount = Mathf.Clamp(GameData.RocketCount + rocketC, 0, 3);
        Debug.Log(rocketC +"_"+ GameData.RocketCount);
        GameData.LastLuckyShot = DateTime.UtcNow.Ticks - TimeSpan.FromSeconds((long)TimeSpan.FromTicks(DateTime.UtcNow.Ticks - GameData.LastLuckyShot).TotalSeconds / 60).Ticks;
        for (int i = 0; i < GameData.RocketCount; i++)
        {
            rockets.Add(Instantiate(rocketPrefab, rocketRoot.transform));
        }
        shots = shotRoot.GetComponentsInChildren<Button>().ToList();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        rewards = new List<RewardInfo>();
        for (int i = 0; i < GameData.LuckyShotResult.Count; i++)
        {
            rewards.Add(new RewardInfo()
            {
                GoodType = GoodType.BERI,
                Reward = new TransactionItemInfo()
                {
                    Amount = GameData.LuckyShotResult[i],
                    Icon = SpriteFactory.Beri,
                }
            });
        }
        rewardCollection.BuildUIs(rewards);
        for (int i = 0; i < GameData.LuckyShotConfig.Count; i++)
        {
            var list = new List<int>(GameData.LuckyShotConfig);
            list.Shuffle();
            shots[i].GetComponent<Image>().sprite = list[i] == 0 ? SpriteFactory.X : SpriteFactory.Beri;
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < GameData.LuckyShotConfig.Count; i++)
        {
            shots[i].GetComponent<Image>().sprite = SpriteFactory.Unknown;
        }
        yield return StartCoroutine(Suffle());
    }
    private void RecieveLuckyShot(JSONNode node)
    {
        Destroy(Instance.rockets[0]);
        Instance.rockets.Remove(rockets[0]);
        if (GameData.RocketCount == 3)
        {
            GameData.LastLuckyShot = DateTime.UtcNow.Ticks;
        }
        GameData.RocketCount--;

        int amount = GameData.LuckyShotConfig[int.Parse(node["index"])];
        Debug.Log(GameData.LuckyShotConfig[int.Parse(node["index"])]);
        GameData.LuckyShotConfig.Count();
        if (rewards.Count == 0)
        {
            GameData.LuckyShotResult.Add(amount);
            rewards.Add(new RewardInfo()
            {
                GoodType = GoodType.BERI,
                Reward = new TransactionItemInfo()
                {
                    Amount = GameData.LuckyShotResult[0],
                    Icon = SpriteFactory.Beri,
                }
            });
        }
        else
        {
            GameData.LuckyShotResult[0] = amount + GameData.LuckyShotResult[0];
            rewards[0] = new RewardInfo()
            {
                GoodType = GoodType.BERI,
                Reward = new TransactionItemInfo()
                {
                    Amount = GameData.LuckyShotResult[0],
                    Icon = SpriteFactory.Beri,
                }
            };
        }
        
        PResourceType.Beri.AddValue(amount);
        shots[indexShot].GetComponent<Image>().sprite = SpriteFactory.Attacked;
        rewardCollection.BuildUIs(rewards);
        StartCoroutine(Suffle());
    }

    public void Obtain()
    {
        rewards.Clear();
        rewardCollection.BuildUIs(rewards);
    }

    public IEnumerator Suffle()
    {
        yield return new WaitForSeconds(0.5f);
        shotRoot.GetComponent<GridLayoutGroup>().enabled = false;
        Vector3[] poses = new Vector3[shots.Count];
        for (int i = 0; i < shots.Count; i++)
        {
            poses[i] = shots[i].transform.position;
            shots[i].onClick.RemoveAllListeners();
            shots[i].transform.DOMove(shots[4].transform.position,1).SetEase(Ease.InCirc);
        }

        yield return new WaitForSeconds(1);
        for (int i = 0; i < shots.Count; i++)
        {
            shots[i].GetComponent<Image>().sprite = SpriteFactory.Occupied;
            shots[i].onClick.RemoveAllListeners();
            shots[i].transform.DOMove(poses[i], 1).SetEase(Ease.InCirc);
        }
        yield return new WaitForSeconds(1);
        shotRoot.GetComponent<GridLayoutGroup>().enabled = true;
        for (int i = 0; i < shots.Count; i++)
        {
            int _i = i;
            shots[i].onClick.AddListener(() =>
            {
                if (GameData.RocketCount>0)
                {
                    indexShot = _i;
                    WSClient.RequestShot();
                }
            });
        }
    }
}
