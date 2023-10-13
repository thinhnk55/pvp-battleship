using DG.Tweening;
using FirebaseIntegration;
using Framework;
using Monetization;
using Server;
using SimpleJSON;
//using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PVE : SingletonMono<PVE>
{
    public PDataUnit<int> CurrentStep;
    public bool IsRevived;
    [SerializeField] PVEStageCollection stagesView;
    [SerializeField] ShipPVE player;
    List<ShipPVE> shipPVEs;
    [SerializeField] Transform enemyRoot;
    [SerializeField] GameObject Retreat;
    public int selectedEnemy;
    [SerializeField] TMP_InputField input;

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
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._FIRE_TREASURE, Attack);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._DATA_TREASURE, GetData);
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._NEWGAME_TREASURE, NewGameTreasure);
        CurrentStep.OnDataChanged -= stagesView.OnStageChange;
        PVEData.IsDeadPlayer.OnDataChanged -= PlayerRevival;
        base.OnDestroy();
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


    private void NewGameTreasure()
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
        int shipListCount = data["d"]["s"].Count;
        Retreat.SetActive(false);


        for (int i = 0; i < shipListCount; i++)
        {
            shipPVEs[i].point.Data = int.Parse(data["d"]["s"][i]);
        }

        if (Instance.shipPVEs[selectedEnemy].point.Data != int.Parse(data["d"]["d"]["p"]) - player.point.Data)
        {
            int tmp = Instance.shipPVEs[selectedEnemy].point.Data;
            if (int.Parse(data["d"]["w"]) == 1) // Win
            {
                Instance.shipPVEs[selectedEnemy].point.Data = shipPVEs[0].point.Data;
                shipPVEs[0].point.Data = tmp;
            }
            else
            {
                Instance.shipPVEs[selectedEnemy].point.Data = shipPVEs[shipListCount - 1].point.Data;
                shipPVEs[shipListCount - 1].point.Data = tmp;
            }

        }

        PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["d"]["r"]) == 1 ? true : false;

        if (int.Parse(data["d"]["w"]) == 1) // Win
        {
            StartCoroutine(Instance.Win(int.Parse(data["d"]["d"]["p"])));
        }
        else // Lose
        {
            StartCoroutine(Lose());
        }
    }
    #endregion 

    private void DestroyEnemyShip()
    {
        for (int i = 0; i < 3; i++)
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
            Instance.shipPVEs[i].ShowPoint();
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
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
        player.point.Data = point;

        DestroyEnemyShip();

        yield return new WaitForSeconds(1f);

        if (CurrentStep.Data < 9)
        {
            CurrentStep.Data++;
            StartCoroutine(InitTurn());
        }
        else // pha dao
        {
            popupComplete = PopupHelper.CreateConfirm(PrefabFactory.PopupReceiveRewardCompletePVE, null,
                "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][CurrentStep.Data]).ToString(), null, (confirm) =>
            {
                if (confirm)
                {
                    StartCoroutine(OnPressConfirmInCompletePopup());
                }
            });
        }
    }

    private IEnumerator OnPressConfirmInCompletePopup()
    {
        PopupBehaviour popupResource = PopupHelper.Create(PrefabFactory.PopupResourcePVE);
        CoinVFX.CoinVfx(popupResource.transform.GetChild(0).transform, Position, Position);
        yield return new WaitForSeconds(1);
        PConsumableType.BERRY.AddValue((PVEData.Bets[PVEData.TypeBoard.Value] *
            PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data]));
        yield return new WaitForSeconds(1f);
        PVEData.TypeBoard = -1;
        popupResource.ForceClose();
        popupComplete.ForceClose();
        SceneTransitionHelper.Load(ESceneName.Home);
    }

    private IEnumerator Lose()
    {
        PVEData.TypeBoard = -1;
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
                    SceneTransitionHelper.Load(ESceneName.Home);
                }
                else
                {
                    AdsManager.ShowRewardAds(null, AdsData.AdsUnitIdMap[RewardType.Get_RevivalOnlyPVE]);
                }
            }
            else
            {
                SceneTransitionHelper.Load(ESceneName.Home);
            }
        });
    }

    IEnumerator InitTurn()
    {
        if (CurrentStep.Data > 4)
        {
            Retreat.SetActive(true);
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
        if (o == true)
        {
            Debug.Log("Revival");
            if (CurrentStep.Data > 4)
            {
                Retreat.SetActive(true);
            }
            HideEnemyPoint();
            SetDisableLeanSelectableShipEnemy(true);
            Debug.Log("Revival Done");
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
