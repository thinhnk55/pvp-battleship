using CartoonFX;
using DG.Tweening;
using Framework;
using SimpleJSON;
using Spine.Unity;
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
    [SerializeField] Slider countDownSlider;

    [SerializeField] Sprite emptyRocket;
    [SerializeField] Sprite rocket;
    [SerializeField] SkeletonGraphic anim;
    [SerializeField] Sequence sequence;
    private void Start()
    {
        //ServerMessenger.AddListener<JSONNode>(ServerResponse.RECIEVE_REWARD_ROCKET, RewardAds);
        AudioHelper.PauseMusic();
        Instance.anim.timeScale = 0.9f;
        ServerMessenger.AddListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, Instance.LuckyShotFire);
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
                Instance.rockets[i].SetSprite(rocket);
            }
            else
            {
                Instance.rockets[i].SetSprite(emptyRocket);
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
        //ServerMessenger.RemoveListener<JSONNode>(ServerResponse.RECIEVE_REWARD_ROCKET, RewardAds);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._LUCKYSHOT_FIRE, Instance.LuckyShotFire);
        GameData.RocketCount.OnDataChanged -= Instance.OnRocketChange;
        Timer<LuckyShot>.Instance.OnTrigger -= Instance.OnTriggerTimer;
        Timer<LuckyShot>.Instance.OnElapse -= Instance.OnElapseTimer;
        Timer<LuckyShot>.Instance.MarkedPoint = DateTime.UtcNow.Ticks;
        AudioHelper.ResumeMusic();
        base.OnDestroy();
    }
    IEnumerator Init()
    {
        indexShot = -1;
        for (int i = 0; i < Instance.shots.Count; i++)
        {
            if (PRandom.Bool(0.25f))
            {
                Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.ShipLuckyShot;
            }
            else
            {
                Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.X;
            }
            int _i = i;
            Instance.shots[i].onClick.AddListener(() =>
            {
                if (GameData.RocketCount.Data > 0)
                {
                    indexShot = _i;
                    WSClientHandler.RequestShot();
                }
            });
        }
        yield return StartCoroutine(Instance.Door());
    }
    private void LuckyShotFire(JSONNode node)
    {
        if (node["e"].AsInt == 0)
        {
            GameData.RocketCount.Data = node["d"]["l"]["r"].AsInt;
            int amount = int.Parse(node["d"]["b"]);
            PConsumableType.BERI.AddValue(amount);
            if (amount == 0)
            {
                Instance.shots[indexShot].GetComponent<Image>().sprite = SpriteFactory.X;
                Instance.shots[indexShot].GetComponent<Image>().SetNativeRatioFixedWidth();
                SoundType.LUCKYSHOT_FALSE.PlaySound();
            }
            else
            {
                Instance.shots[indexShot].GetComponent<Image>().sprite = SpriteFactory.ShipLuckyShot;
                Instance.shots[indexShot].GetComponent<Image>().SetNativeRatioFixedWidth();
                {// explosion
                    var vfx = ObjectPoolManager.SpawnObject<ParticleSystem>(VFXFactory.Explosion, Instance.shots[indexShot].transform.position);
                    var mainVfx = vfx.main;
                    var renVfx = vfx.GetComponent<ParticleSystemRenderer>();
                    var cfrx = vfx.GetComponent<CFXR_Effect>().animatedLights;
                    cfrx[0].colorGradient.colorKeys = new GradientColorKey[2] { new GradientColorKey(new Color(0, 1f, 0.9f), 0), new GradientColorKey(new Color(0, 0.9f, 0.8f), 1) };
                    renVfx.sortingLayerName = "UI";
                    mainVfx.startColor = Color.cyan;
                    mainVfx.startSizeMultiplier = 0.5f;
                    SoundType.SHIP_EXPLOSION.PlaySound();
                }
                {//text
                    TextMeshProUGUI text = (TextMeshProUGUI)ObjectPoolManager.SpawnObject<TextBase>(PrefabFactory.TextPrefab, Instance.shots[indexShot].transform.position + Vector3.up/2, null, true).text;
                    text.text = amount.ToString();
                    text.fontSize = 36f;
                    Debug.Log(Instance.shots[indexShot].transform.position);
                    text.transform.DOMoveY(Instance.shots[indexShot].transform.position.y + 1.5f, 1f).OnComplete(() =>
                    {
                        text.gameObject.SetActive(false);
                    });
                }

                DOVirtual.DelayedCall(0.5f, ()=> CoinVFX.CoinVfx(Instance.resourceUI.transform, Instance.shots[indexShot].transform.position, Instance.shots[indexShot].transform.position));
            }
            StartCoroutine(Instance.Door());
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
                Instance.rockets[oldValue + i].SetSprite(rocket);
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
                Instance.rockets[newValue + i].SetSprite(emptyRocket);
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
            Instance.countDownSlider.value = 0;
        }
        else
        {
            if (Timer<LuckyShot>.Instance.TriggersFromBegin>=1)
            {
                Instance.countDown.text = $"Collect";
                Instance.countDownSlider.value = 0;
            }
            else
            {
                Instance.countDown.text = $"{Timer<LuckyShot>.Instance.RemainTime_Sec.Hour_Minute_Second_1()}";
                Instance.countDownSlider.value = (float)Timer<LuckyShot>.Instance.RemainTime_Sec / Timer<LuckyShot>.Instance.TriggerInterval_Sec;
            }
        }
    }
    public IEnumerator Suffle() 
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
                    WSClientHandler.RequestShot();
                }
            });
        }
    }
    public IEnumerator Door() 
    {
        if (indexShot >=0)
        {
            Vector3 initScale = Instance.shots[indexShot].transform.localScale;
            Instance.sequence = DOTween.Sequence();
            Instance.sequence.Append(Instance.shots[indexShot].transform.DOScale(initScale * 0.9f, 0.25f))
                .Append(Instance.shots[indexShot].transform.DOScale(initScale * 1.1f, 0.25f))
                .SetLoops(6);
            Instance.shots[indexShot].image.SetAlpha(1f);
        }
        for (int i = 0; i < Instance.shots.Count; i++)
        {
            Instance.shots[i].enabled = false;
        }
        yield return new WaitForSeconds(indexShot>=0 ? 1f : 0.5f);
        Instance.anim.SetAnimation("animation", false);
        Instance.anim.Initialize(false);
        SoundType.DOOR.PlaySound();
        yield return new WaitForSeconds(anim.GetDuration("animation") * 1 / 2);
        if (indexShot >= 0)
            Instance.shots[indexShot].image.SetAlpha(0);
        yield return new WaitForSeconds(anim.GetDuration("animation")* 1/ 2);
        for (int i = 0; i < Instance.shots.Count; i++)
        {
            Instance.shots[i].GetComponent<Image>().sprite = SpriteFactory.Unknown;
            Instance.shots[i].enabled = true;
        }
    }
    public void Earn()
    {
        WSClientHandler.LuckyShotEarn();
    }
}
