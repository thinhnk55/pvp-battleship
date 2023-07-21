using DG.Tweening;
using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LuckyShot : SingletonMono<LuckyShot>
{

    public int indexShot;
    public List<GameObject> rockets;
    public List<Button> shots;

    [SerializeField] GameObject rocketRoot;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject shotRoot;
    [SerializeField] GameObject resourceUI;
    [SerializeField] TextMeshProUGUI countDown;

    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_LUCKY_SHOT, Instance.RecieveLuckyShot);
        GameData.RocketCount.OnDataChanged += Instance.OnRocketChange;
        Timer<LuckyShot>.Instance.Init(Instance.OnTriggerTimer, Instance.OnElapseTimer);
        GameData.RocketCount.Data = Mathf.Clamp(GameData.RocketCount.Data + Timer<LuckyShot>.Instance.TriggersFromMark, 0, 3);
        int count = rockets.Count;
        for (int i = 0; i < GameData.RocketCount.Data - count; i++)
        {
            Instance.rockets.Add(Instantiate(rocketPrefab, rocketRoot.transform));
        }
        Instance.shots = shotRoot.GetComponentsInChildren<Button>().ToList();
        StartCoroutine(Instance.Init());

        ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_REWARD_ROCKET, RewardAds);

    }
    private void Update()
    {
        Timer<LuckyShot>.Instance.Elasping();
    }
    protected override void OnDestroy()
    {

        ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_LUCKY_SHOT, Instance.RecieveLuckyShot);
        GameData.RocketCount.OnDataChanged -= Instance.OnRocketChange;
        Timer<LuckyShot>.Instance.OnTrigger -= Instance.OnTriggerTimer;
        Timer<LuckyShot>.Instance.OnElapse -= Instance.OnElapseTimer;
        Timer<LuckyShot>.Instance.MarkedPoint = DateTime.UtcNow.Ticks;
        base.OnDestroy();
    }
    IEnumerator Init()
    {
        var list = new List<int>();
        list.AddRange(GameData.LuckyShotConfig);
        list.Shuffle();
        for (int i = 0; i < GameData.LuckyShotConfig.Count; i++)
        {
            if (list[i] == 0)
            {
                Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.ShipLuckyShot;

            }
            else
            {
                Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.X;
            }
        }

        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Instance.Suffle());
    }
    private void RecieveLuckyShot(JSONNode node)
    {
        GameData.RocketCount.Data--;
        int amount = GameData.LuckyShotConfig[int.Parse(node["index"])];
        Debug.Log(GameData.LuckyShotConfig[int.Parse(node["index"])]);
        
        PConsumableType.BERI.AddValue(amount);
        if(amount == 0)
        {
            Instance.shots[indexShot].GetComponent<Image>().sprite = SpriteFactory.X;
        }
        else
        {
            Instance.shots[indexShot].GetComponent<Image>().sprite = SpriteFactory.ShipLuckyShot;
            CoinVFX.CoinVfx(Instance.resourceUI.transform, Instance.shots[indexShot].transform.position, Instance.shots[indexShot].transform.position);
        }
        StartCoroutine(Instance.Suffle());
    }

    public void OnRocketChange(int oldValue, int newValue)
    {
        if (newValue > oldValue)
        {
            for (int i = 0; i < newValue - oldValue; i++)
            {
                Debug.Log("New Rocket");
                rockets.Add(Instantiate(rocketPrefab, rocketRoot.transform));
            }
        }
        else
        {
            if (oldValue == 3)
            {
                Timer<LuckyShot>.Instance.BeginPoint = DateTime.UtcNow.Ticks; 
            }
            for (int i = 0; i < oldValue - newValue; i++)
            {
                Destroy(Instance.rockets[0]);
                Instance.rockets.RemoveAt(0);
            }
        }
    }
    public void OnTriggerTimer()
    {
        if (GameData.RocketCount.Data<3)
        {
            GameData.RocketCount.Data++;

        }
    }
    public void OnElapseTimer()
    {
        if (GameData.RocketCount.Data >= 3)
        {
            countDown.text = "Full";
        }
        else
        {
            countDown.text = $"Free Rocket - {Timer<LuckyShot>.Instance.RemainTime_Sec.Hour_Minute_Second_1()}";
        }
    }
    public IEnumerator Suffle()
    {
        yield return new WaitForSeconds(0.5f);
        shotRoot.GetComponent<GridLayoutGroup>().enabled = false;
        Vector3[] poses = new Vector3[shots.Count];
        for (int i = 0; i < shots.Count; i++)
        {
            shots[i].onClick.RemoveAllListeners();
            poses[i] = shots[i].transform.position;
            shots[i].transform.DOMove(shots[4].transform.position,1).SetEase(Ease.InCirc);
        }

        yield return new WaitForSeconds(1);
        for (int i = 0; i < shots.Count; i++)
        {
            shots[i].GetComponent<Image>().sprite = SpriteFactory.Unknown;
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
                if (GameData.RocketCount.Data > 0)
                {
                    indexShot = _i;
                    WSClient.RequestShot();
                }
            });
        }
    }

    public void RewardAds(JSONNode json)
    {
        GameData.RocketCount.Data++;
    }
}
