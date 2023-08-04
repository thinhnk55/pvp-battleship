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
    public List<Image> rockets;
    public List<Button> shots;
    [SerializeField] GameObject earnButton;
    [SerializeField] GameObject adsButton;
    [SerializeField] GameObject rocketRoot;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject shotRoot;
    [SerializeField] GameObject resourceUI;
    [SerializeField] TextMeshProUGUI countDown;

    [SerializeField] Sprite emptyRocket;
    [SerializeField] Sprite rocket;
    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, Instance.RecieveLuckyShot);
        Timer<LuckyShot>.Instance.Init(Instance.OnTriggerTimer, Instance.OnElapseTimer);
        GameData.RocketCount.Data = Mathf.Clamp(GameData.RocketCount.Data, 0, 3);
        int count = Instance.rockets.Count;
        if (GameData.RocketCount.Data == 3)
        {
            Instance.earnButton.SetActive(false);
            Instance.adsButton.SetActive(false);
        }
        for (int i = 0; i < rockets.Count; i++)
        {
            if (i< GameData.RocketCount.Data)
            {
                rockets[i].SetSprite(rocket);
            }
            else
            {
                rockets[i].SetSprite(emptyRocket);
            }
        }
        GameData.RocketCount.OnDataChanged += Instance.OnRocketChange;
        // To do
        Instance.shots = shotRoot.GetComponentsInChildren<Button>().ToList();
        StartCoroutine(Instance.Init());


    }
    private void Update()
    {
        Timer<LuckyShot>.Instance.Elasping();
    }
    protected override void OnDestroy()
    {

        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, Instance.RecieveLuckyShot);
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
        if (node["e"].AsInt == 0)
        {
            GameData.RocketCount.Data = node["d"]["l"]["r"].AsInt;
            int amount = int.Parse(node["d"]["b"]);
            PConsumableType.BERI.AddValue(amount);
            if (amount == 0)
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

    }

    public void OnRocketChange(int oldValue, int newValue)
    {
        if (newValue > oldValue)
        {
            if (newValue == 3)
            {
                Instance.earnButton.SetActive(false);
                Instance.adsButton.SetActive(false);
            }
            for (int i = 0; i < newValue - oldValue; i++)
            {
                Debug.Log("New Rocket");
                //
                rockets[oldValue + i].SetSprite(rocket);
            }
        }
        else
        {
            if (oldValue == 3)
            {
                Instance.earnButton.SetActive(true);
                Instance.adsButton.SetActive(true);
            }
            for (int i = 0; i < oldValue - newValue; i++)
            {
                //
                rockets[newValue + i].SetSprite(emptyRocket);
            }
        }
    }
    public void OnTriggerTimer()
    {

    }
    public void OnElapseTimer()
    {
        if (GameData.RocketCount.Data >= 3)
        {
            Instance.countDown.text = "Full";
        }
        else
        {
            if (Timer<LuckyShot>.Instance.TriggersFromBegin>=1)
            {
                Instance.countDown.text = $"Collect";
            }
            else
            {
                Instance.countDown.text = $"{Timer<LuckyShot>.Instance.RemainTime_Sec.Hour_Minute_Second_1()}";
            }
        }
    }
    public IEnumerator Suffle()    // To do
    {
        yield return new WaitForSeconds(0.5f);
        Instance.shotRoot.GetComponent<GridLayoutGroup>().enabled = false;
        Vector3[] poses = new Vector3[Instance.shots.Count];
        for (int i = 0; i < Instance.shots.Count; i++)
        {
            Instance.shots[i].onClick.RemoveAllListeners();
            poses[i] = Instance.shots[i].transform.position;
            Instance.shots[i].transform.DOMove(Instance.shots[4].transform.position,1).SetEase(Ease.InCirc);
        }

        yield return new WaitForSeconds(1);
        for (int i = 0; i < shots.Count; i++)
        {
            Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.Unknown;
            Instance.shots[i].onClick.RemoveAllListeners();
            Instance.shots[i].transform.DOMove(poses[i], 1).SetEase(Ease.InCirc);
        }
        yield return new WaitForSeconds(1);
        Instance.shotRoot.GetComponent<GridLayoutGroup>().enabled = true;
        for (int i = 0; i < Instance.shots.Count; i++)
        {
            int _i = i;
            Instance.shots[i].onClick.AddListener(() =>
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

    public void Earn()
    {
        WSClient.LuckyShotEarn();
    }
}
