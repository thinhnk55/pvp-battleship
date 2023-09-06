using DG.Tweening;
using Framework;
using Monetization;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

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
        PVEData.IsDeadPlayer.OnDataChanged += PlayerRevival;
    }

    private void Start()
    {
        NewGameTreasure();
        player.leanSelectable.enabled = false;
        shipPVEs = new List<ShipPVE>();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._FIRE_TREASURE, Attack);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._DATA_TREASURE, GetData);
        ServerMessenger.AddListener<JSONNode>(ServerResponse._NEWGAME_TREASURE, NewGameTreasure);
        StartCoroutine(InitTurn());
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
    private void GetData()
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
        if (int.Parse(data["e"]) != 0)
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
        Retreat.SetActive(false);
        for (int i = 0; i < data["d"]["s"].Count; i++)
        {
            shipPVEs[i].point.Data = int.Parse(data["d"]["s"][i]);
        }

        PVEData.IsDeadPlayer.Data = int.Parse(data["d"]["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["d"]["r"]) == 1 ? true : false;

        if (int.Parse(data["d"]["w"]) == 1) // Win
        {
            Debug.Log("Win");
            StartCoroutine(Instance.Win(int.Parse(data["d"]["d"]["p"])));
        }
        else // Lose
        {
            Debug.Log("Lose");
            StartCoroutine(Lose());
        }
    }
    #endregion 

    private void DestroyEnemyShip()
    {
        for (int i = 0; i < 3; i++)
        {
            int _i = i;
            Instance.shipPVEs[_i].transform.DOScale(0, 1f).OnComplete(() =>
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

    private IEnumerator Win(int point)
    {
        ShowEnemyPoint();
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
        player.point.Data = point;
        yield return new WaitForSeconds(1);

        DestroyEnemyShip();

        yield return new WaitForSeconds(1.5f);

        if (CurrentStep.Data < 9)
        {
            CurrentStep.Data++;
            StartCoroutine(InitTurn());
        }
        else // pha dao
        {
            PopupHelper.CreateConfirm(PrefabFactory.PopupLossPVE, null,
                "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][CurrentStep.Data]).ToString(), null, (confirm) =>
            {
                if (confirm)
                {

                }
            });
        }
    }

    private IEnumerator Lose()
    {
        yield return StartCoroutine(player.BeingDestroyed());
        yield return new WaitForSeconds(1);
        //SceneTransitionHelper.Load(ESceneName.Home);

        PopupHelper.CreateConfirm(PrefabFactory.PopupLossPVE, null, null, null, (confirm) =>
        {
            if(confirm)
            {
                AdsManager.ShowRewardAds(null, AdsData.adsUnitIdMap[RewardType.Get_X2DailyGift]);
            }
            else
            {
                SceneTransitionHelper.Load(ESceneName.Home);
            }
        });
    }


    IEnumerator InitTurn()
    {
        Retreat.SetActive(true);
        int prefabIndex = 0;
        if (CurrentStep.Data < 4)
        {
            prefabIndex = 0;
        }
        else if (CurrentStep.Data < 7)
        {
            prefabIndex = 1;
        }
        else if (CurrentStep.Data < 9)
        {
            prefabIndex = 2;
        }
        else if (CurrentStep.Data < 100)
        {
            prefabIndex = 3;
        }
        float duration = 2;
        shipPVEs.Clear();
        for (int i = 0; i < 3; i++)
        {
            ShipPVE ship1 = Instantiate(PrefabFactory.ShipsPVE[prefabIndex], enemyRoot).GetComponent<ShipPVE>();
            ship1.transform.localPosition = new Vector3(10, 2 - 2 * i, 0);
            ship1.HidePoint();
            ship1.index = i;
            ship1.leanSelectable.enabled = false;
            ship1.transform.DOLocalMove(new Vector3(2, 2 - 2 * i, 0), duration);
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
        if(o == true)
        {
            Retreat.SetActive(true);
            HideEnemyPoint();
            SetDisableLeanSelectableShipEnemy(true);
        }
    }

    public void SetDisableLeanSelectableShipEnemy(bool disable)
    {
        for (int i = 0; i < 3; i++)
        {
            shipPVEs[i].leanSelectable.enabled = disable;
        }
    }
}
