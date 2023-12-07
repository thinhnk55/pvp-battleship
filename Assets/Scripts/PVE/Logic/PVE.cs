using DG.Tweening;
using FirebaseIntegration;
using Framework;
using Monetization;
using Server;
using SimpleJSON;
//using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PVE : SingletonMono<PVE>
{
    public PDataUnit<int> CurrentStep;
    public bool IsRevived;
    [SerializeField] PVEStageCollection stagesView;
    [SerializeField] ShipPVE player;
    List<ShipPVE> shipPVEs;
    [SerializeField] Transform enemyRoot;
    [SerializeField] Button Retreat;
    public int selectedEnemy;
    [SerializeField] GameObject resource;

    protected override void Awake()
    {
        base.Awake();
        CurrentStep.Data = -1;
        CurrentStep.OnDataChanged += stagesView.OnStageChange;
    }

    private void Start()
    {
        NewGameTreasure();
        player.leanSelectable.enabled = false;
        shipPVEs = new List<ShipPVE>();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._FIRE_TREASURE, Attack);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._DATA_TREASURE, GetData);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._NEWGAME_TREASURE, NewGameTreasure);
    }
    protected override void OnDestroy()
    {
        Debug.LogWarning("PVE ONDestroy");
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._FIRE_TREASURE, Attack);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._DATA_TREASURE, GetData);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._NEWGAME_TREASURE, NewGameTreasure);
        CurrentStep.OnDataChanged -= stagesView.OnStageChange;
        PVEData.IsDeadPlayer.OnDataChanged -= PlayerRevival;
        base.OnDestroy();
    }

    public void RemoveListener()
    {
        PVEData.IsDeadPlayer.OnDataChanged -= PlayerRevival;
    }

    #region Server_Request_Response
    public void GetData()
    {
        new JSONClass()
        {
            { "id", ServerRequest._DATA_TREASURE.ToJson() }
        }.RequestServer();
    }

    private void GetData(JSONNode data)
    {
        PVEData.TypeBoard = int.Parse(data["d"]["t"]);
        CurrentStep.Data = int.Parse(data["d"]["s"]);
        player.point.Data = int.Parse(data["d"]["p"]);
        PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["r"]) == 1 ? true : false;
        StartCoroutine(InitTurn());
        PVEData.IsDeadPlayer.OnDataChanged += PlayerRevival;
    }


    public void NewGameTreasure()
    {
        new JSONClass()
        {
            {"id", ServerRequest._NEWGAME_TREASURE.ToJson()},
            {"t", PVEData.TypeBoard.Value.ToJson()},
        }.RequestServer();
    }

    private void NewGameTreasure(JSONNode data)
    {
        if (int.Parse(data["e"]) != 0) // old game
        {
            GetData();
            return;
        }
        // new game
        PVEData.TypeBoard = int.Parse(data["d"]["t"]);
        CurrentStep.Data = int.Parse(data["d"]["s"]);
        player.point.Data = int.Parse(data["d"]["p"]);
        PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["r"]) == 1 ? true : false;
        StartCoroutine(InitTurn());
        PConsumableType.BERRY.AddValue(-PVEData.Bets[PVEData.TypeBoard.Value]);
        AnalyticsHelper.SpendVirtualCurrency(PConsumableType.BERRY.ToString().ToLower(), "classic" + -PVEData.Bets[PVEData.TypeBoard.Value]);
        PVEData.IsDeadPlayer.OnDataChanged += PlayerRevival;
    }

    public void Attack()
    {
        new JSONClass()
        {
            {"id", ServerResponse._FIRE_TREASURE.ToJson()}
        }.RequestServer();
    }

    void Attack(JSONNode data)
    {
        bool isWin = int.Parse(data["d"]["w"]) == 1 ? true : false;
        int shipListCount = data["d"]["s"].Count;
        Retreat.interactable = false;

        List<int> shipPointList = data["d"]["s"].ToListInt();
        int selectShipPoint;
        if (isWin)
        {
            selectShipPoint = int.Parse(data["d"]["d"]["p"]) - player.point.Data;
        }
        else
        {
            if (CurrentStep.Data < 5)
            {
                selectShipPoint = shipPointList[2];
            }
            else
            {
                int random = Random.Range(1, shipListCount);
                selectShipPoint = shipPointList[random];
            }
        }
        shipPVEs[selectedEnemy].point.Data = selectShipPoint;
        shipPointList.Remove(selectShipPoint);

        for (int i = 0; i < shipListCount; i++)
        {
            if (i == selectedEnemy)
                continue;

            int random = Random.Range(0, shipPointList.Count);

            shipPVEs[i].point.Data = shipPointList[random];
            shipPointList.RemoveAt(random);
        }

        PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["d"]["r"]) == 1 ? true : false;

        if (int.Parse(data["d"]["w"]) == 1)
        {
            StartCoroutine(Instance.Win(int.Parse(data["d"]["d"]["p"])));
        }
        else
        {
            StartCoroutine(Lose());
        }
    }

    public void EndGameTreasure()
    {
        new JSONClass()
        {
            {"id" , ServerRequest._END_GAME_TREASURE.ToJson() },
            {"t", PVEData.TypeBoard.Value.ToJson()}
        }.RequestServer();
    }
    #endregion 

    public void DestroyEnemyShip()
    {
        for (int i = 0; i < shipPVEs.Count; i++)
        {
            int _i = i;
            Instance.shipPVEs[_i].transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                DestroyImmediate(Instance.shipPVEs[_i].gameObject);
            });
        }
    }

    private void ShowEnemyPoint()
    {
        for (int i = 0; i < 3; i++)
        {
            Instance.shipPVEs[i].ShowPoint(Instance.shipPVEs[i].point.Data <= player.point.Data ? false : true);
        }
    }

    private void HideEnemyPoint()
    {
        for (int i = 0; i < 3; i++)
        {
            Instance.shipPVEs[i].HidePoint();
        }
    }

    PopupConfirm popupComplete;
    private IEnumerator Win(int point)
    {
        ShowEnemyPoint();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
        player.point.Data = point;

        //yield return new WaitForSeconds(1f);

        CurrentStep.Data++;
        if (CurrentStep.Data < 10)
        {
            StartCoroutine(InitTurn());
        }
        else // pha dao
        {
            Retreat.gameObject.SetActive(false);
            EndGameTreasure();

            popupComplete = PopupHelper.CreateConfirm(PrefabFactory.PopupReceiveRewardCompletePVE, null,
                "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][CurrentStep.Data]).ToString(), null, (confirm) =>
            {
                if (confirm)
                {
                    StartCoroutine(OnCompletePopup());
                }
            });
        }
    }

    private IEnumerator OnCompletePopup()
    {
        resource.SetActive(true);
        CoinVFX.CoinVfx(resource.transform.GetChild(0).transform, Position, Position);
        yield return new WaitForSeconds(1);
        PConsumableType.BERRY.AddValue((PVEData.Bets[PVEData.TypeBoard.Value] *
            PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data]));
        yield return new WaitForSeconds(1f);
        PVEData.TypeBoard = -1;
        resource.SetActive(false);
        popupComplete.ForceClose();
        SceneTransitionHelper.Load(ESceneName.PVEBet);
    }

    private IEnumerator Lose()
    {
        ShowEnemyPoint();
        PVEData.IsDeadPlayer.Data = true;
        yield return StartCoroutine(player.BeingDestroyed());
        yield return new WaitForSeconds(1);
        //SceneTransitionHelper.Load(ESceneName.Home);

        PopupHelper.CreateConfirm(PrefabFactory.PopupLossPVE, null, null, null, (confirm) =>
        {
            if (confirm)
            {
                if (IsRevived)
                {
                    PVEData.TypeBoard = -1;
                    SceneTransitionHelper.Load(ESceneName.PVEBet);
                }
                else
                {
                    AdsManager.ShowRewardAds(null, AdsData.AdsUnitIdMap[RewardType.Get_RevivalOnlyPVE]);
                }
            }
            else
            {
                PVEData.TypeBoard = -1;
                SceneTransitionHelper.Load(ESceneName.PVEBet);
            }
        });
    }

    IEnumerator InitTurn()
    {
        DestroyEnemyShip();
        yield return new WaitForSeconds(1f);
        if (CurrentStep.Data > 4)
        {
            Retreat.interactable = true;
        }

        int prefabIndex = GetIndexShipEnemyCurrentTurn();

        float duration = 1f;
        shipPVEs.Clear();
        for (int i = 0; i < 3; i++)
        {
            ShipPVE ship1 = Instantiate(PrefabFactory.ShipsPVE[prefabIndex], enemyRoot).GetComponent<ShipPVE>();
            ship1.transform.localPosition = new Vector3(10, 1.9f - 2 * i, 0);
            ship1.HidePoint();
            ship1.index = i;
            ship1.leanSelectable.enabled = false;
            ship1.transform.DOLocalMove(new Vector3(2, 1.9f - 2 * i, 0), duration).OnComplete(() =>
            {
                if (CurrentStep.Data == 0)
                {
                    ship1.ScaleTargetImage();
                }
            });
            shipPVEs.Add(ship1);
        }
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < 3; i++)
        {
            shipPVEs[i].leanSelectable.enabled = true;
        }
    }

    private void PlayerRevival(bool o, bool n)
    {
        PDebug.Log("PlayerRevival: " + o + "-" + n);
        if (o == true)
        {
            PDebug.Log("Revival");
            if (CurrentStep.Data > 4)
            {
                Retreat.interactable = true;
            }
            HideEnemyPoint();
            SetDisableLeanSelectableShipEnemy(true);
        }
    }

    public int GetIndexShipEnemyCurrentTurn()
    {
        if (CurrentStep.Data < 4)
        {
            return 0;
        }
        if (CurrentStep.Data < 7)
        {
            return 1;
        }
        if (CurrentStep.Data < 9)
        {
            return 2;
        }

        return 3;
    }

    public void SetDisableLeanSelectableShipEnemy(bool disable)
    {
        for (int i = 0; i < 3; i++)
        {
            shipPVEs[i].leanSelectable.enabled = disable;
        }
    }
}
