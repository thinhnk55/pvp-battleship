using Framework;
using Server;
//using Lean.Common.Editor;
using SimpleJSON;
using System.Collections;
using UnityEngine;

public class ButtonOpenRetreatPVEPopup : ButtonBase
{
    private PopupConfirm popupRetreatPVE;
    [SerializeField] GameObject Resource;

    private void OnEnable()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._END_GAME_TREASURE, EndGameTreasure);
    }

    private void OnDisable()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._END_GAME_TREASURE, EndGameTreasure);
        
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        int currentReward = PVEData.Bets[PVEData.TypeBoard.Value] *
            PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data];
        popupRetreatPVE = PopupHelper.CreateConfirm(PrefabFactory.PopupRetreatPVE, null,
            "+" + currentReward.ToString(), null, (confirm) =>
            {
                if (confirm)
                {
                    PVE.Instance.EndGameTreasure();
                }
                else
                {
                    popupRetreatPVE.ForceClose();
                }
            });
    }


    public void EndGameTreasure(JSONNode data)
    {
        StartCoroutine(GetBeri(int.Parse(data["d"]["g"])));
        PVEData.TypeBoard = int.Parse(data["d"]["d"]["t"]);
    }

    //IEnumerator GetBeri(int beri)
    //{
    //    PopupBehaviour popupResource = PopupHelper.Create(PrefabFactory.PopupResourcePVE);
    //    CoinVFX.CoinVfx(popupResource.transform.GetChild(0).transform, Position, Position);
    //    popupRetreatPVE?.ForceClose();
    //    yield return new WaitForSeconds(1);
    //    PConsumableType.BERRY.SetValue(beri);
    //    yield return new WaitForSeconds(1.5f);
    //    popupResource.ForceClose();
    //    SceneTransitionHelper.Load(ESceneName.Home);
    //}

    IEnumerator GetBeri(int beri)
    {
        Resource.SetActive(true);
        CoinVFX.CoinVfx(Resource.transform.GetChild(0).transform, popupRetreatPVE.transform.position, popupRetreatPVE.transform.position);
        yield return new WaitForSeconds(1);
        PConsumableType.BERRY.SetValue(beri);
        yield return new WaitForSeconds(1.5f);
        Resource.SetActive(false);
        popupRetreatPVE?.ForceClose();
        SceneTransitionHelper.Load(ESceneName.Home);
    }


}
