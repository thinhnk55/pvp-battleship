using DG.Tweening;
using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PVE : SingletonMono<PVE>
{
    public static int TypeBoard;
    public PDataUnit<int> CurrentStep;
    public bool IsDead;
    public bool IsRevived;
    [SerializeField] PVEStageCollection stagesView;
    [SerializeField] ShipPVE player;
    List<ShipPVE> shipPVEs;
    [SerializeField] Transform enemyRoot;
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
        GetData();
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
        TypeBoard = int.Parse(data["d"]["t"]);
        CurrentStep.Data = int.Parse(data["d"]["s"]);
        player.point.Data = int.Parse(data["d"]["p"]);
        IsDead = int.Parse(data["d"]["d"]) == 1 ? true : false;
        IsRevived = int.Parse(data["d"]["r"]) == 1 ? true : false;
        if (TypeBoard == -1 || IsDead)
        {
            NewGameTreasure();
            return;
        }
    }


    private void NewGameTreasure()
    {
        new JSONClass()
        {
            {"id", ServerRequest._NEWGAME_TREASURE.ToJson()},
            {"t", TypeBoard.ToJson()},
        }.RequestServer();
    }

    private void NewGameTreasure(JSONNode data)
    {
        // new game
        TypeBoard = int.Parse(data["d"]["t"]);
        CurrentStep.Data = int.Parse(data["d"]["s"]);
        player.point.Data = int.Parse(data["d"]["p"]);
        IsDead = int.Parse(data["d"]["d"]) == 1 ? true : false;
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
        for (int i = 0; i < data["d"]["s"].Count; i++)
        {
            shipPVEs[i].point.Data = int.Parse(data["d"]["s"][i]);
        }

        if (int.Parse(data["d"]["w"]) == 1) // Win
        {
            Debug.Log("Win");
            StartCoroutine(Instance.Win(int.Parse(data["d"]["d"]["p"])));
        }
        else // Lose
        {
            Debug.Log("Lose");
            //StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
            StartCoroutine(Lose());
        }
    }
    #endregion 

    private IEnumerator Win(int point)
    {
        for (int i = 0; i < 3; i++)
        {
            Instance.shipPVEs[i].ShowPoint();
        }
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
        player.point.Data = point;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            int _i = i;
            Instance.shipPVEs[_i].transform.DOScale(0, 1f).OnComplete(() =>
            {
                DestroyImmediate(Instance.shipPVEs[_i].gameObject);
            });
        }
        yield return new WaitForSeconds(1.5f);
        if (CurrentStep.Data < 9)
        {
            CurrentStep.Data++;
            StartCoroutine(InitTurn());
        }
    }

    private IEnumerator Lose()
    {
        yield return StartCoroutine(player.BeingDestroyed());
        yield return new WaitForSeconds(1);
        SceneTransitionHelper.Load(ESceneName.Home);

        //if(!IsRevived)
        //{
        //    PopupHelper.CreateConfirm(PrefabFactory.PopupRevivalOnlyPVE, null, "Unfortunately! You have not received any reward yet", null, (confirm) =>
        //    {
        //        // Player dong y xem quang cao de hoi sinh
        //        if (confirm)
        //        {
        //            Debug.Log("You are revived");
        //            SceneTransitionHelper.Load(ESceneName.Home);
        //        }
        //        else
        //        {
        //            SceneTransitionHelper.Load(ESceneName.Home);
        //        }
        //    });
        //    yield break;
        //}

        //PopupHelper.CreateConfirm(PrefabFactory.PopupRevivalOnlyPVE, null, "Unfortunately! You have not received any reward yet", null, (confirm) =>
        //{
        //    // Player dong y xem quang cao de hoi sinh
        //    if (confirm)
        //    {
        //        Debug.Log("You are revived");
        //    }
        //    else
        //    {
        //        SceneTransitionHelper.Load(ESceneName.Home);
        //    }
        //});

    }

    IEnumerator InitTurn()
    {
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

    public void DisableLeanSelectableShipEnemy()
    {
        for (int i = 0; i < 3; i++)
        {
            shipPVEs[i].leanSelectable.enabled = false;
        }
    }
}
