using DG.Tweening;
using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PVE : SingletonMono<PVE>
{
    int stagesCount;
    PDataUnit<int> currentStages;

    [SerializeField]PVEStageCollection stagesView;

    [SerializeField] ShipPVE player;
    List<ShipPVE> shipPVEs;
    [SerializeField] Transform enemyRoot;
    public int selectedEnemy;


    [SerializeField] TMP_InputField input;
    private void Start()
    {
        shipPVEs = new List<ShipPVE>();
        ServerMessenger.AddListener<int>(ServerResponse._PVE_ATTACK, Attack);
        currentStages = new PDataUnit<int>(-1);
        currentStages.OnDataChanged += stagesView.OnStageChange;
        currentStages.Data = 0;
        player.point.Data = 10;
        StartCoroutine(InitTurn());
    }
    protected override void OnDestroy()
    {
        ServerMessenger.RemoveListener<int>(ServerResponse._PVE_ATTACK, Attack);
        base.OnDestroy();   
    }
    void Attack(int data)
    {
        Debug.Log(player.point.Data);
        shipPVEs[selectedEnemy].point.Data = data;
        if (player.point.Data > data) // win
        {
            StartCoroutine(Instance.Win(player.point.Data + data));
        }
        else //lose
        {
            StartCoroutine(Instance.shipPVEs[selectedEnemy].BeingDestroyed());
            StartCoroutine(Lose());
        }
    }

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
        if (currentStages.Data < 9)
        {
            currentStages.Data++;
            StartCoroutine(InitTurn());
        }
    }

    private IEnumerator Lose()
    {
        yield return StartCoroutine(player.BeingDestroyed());
        yield return new WaitForSeconds(1);
        SceneTransitionHelper.Load(ESceneName.PVE);
    }

    public void Attack()
    {
        //new JSONClass()
        //{
        //    { "id", ServerRequest._PVE_ATTACK.ToJson() } 
        //}.RequestServer();
        if (input.text == "")
        {
            ServerMessenger.Broadcast<int>(ServerResponse._PVE_ATTACK, player.point.Data + Random.Range(-3, 0));
        }
        else
        {
            ServerMessenger.Broadcast<int>(ServerResponse._PVE_ATTACK, int.Parse(input.text));
        }
    }

    IEnumerator InitTurn()
    {
        int prefabIndex = 0;
        if (currentStages.Data < 4)
        {
            prefabIndex = 0;
        }
        else if (currentStages.Data < 7)
        {
            prefabIndex = 1;
        }
        else if (currentStages.Data < 9)
        {
            prefabIndex = 2;
        }
        else if (currentStages.Data < 100)
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
}
