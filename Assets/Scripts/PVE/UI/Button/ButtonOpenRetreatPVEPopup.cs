using Framework;
using Lean.Common.Editor;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonOpenRetreatPVEPopup : ButtonBase
{
    private PopupConfirm popupRetreatPVE;

    private void Start()
    {
        ServerMessenger.AddListener<JSONNode>(ServerResponse._END_GAME_TREASURE, EndGameTreasure);
    }

    private void OnDestroy()
    {
        ServerMessenger.RemoveListener<JSONNode>(ServerResponse._END_GAME_TREASURE, EndGameTreasure);
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        popupRetreatPVE = PopupHelper.CreateConfirm(PrefabFactory.PopupRetreatPVE, null,
            "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data]).ToString(), null, (confirm) => {
            if(confirm)
            {
                EndGameTreasure();
            }
            else
            {
                popupRetreatPVE.ForceClose();
            }
        });
    }


    public void EndGameTreasure()
    {
        new JSONClass()
        {
            {"id" , ServerRequest._END_GAME_TREASURE.ToJson() },
            {"t", PVEData.TypeBoard.Value.ToJson()}
        }.RequestServer();
    }

    public void EndGameTreasure(JSONNode data)
    {
        StartCoroutine(GetBeri(int.Parse(data["d"]["g"] + int.Parse(data["d"]["e"]))));
        PVEData.TypeBoard = int.Parse(data["d"]["d"]["t"]);
    }

    IEnumerator GetBeri(int beri)
    {
        PopupBehaviour popupResource = PopupHelper.Create(PrefabFactory.PopupResourcePVE);
        CoinVFX.CoinVfx(popupResource.transform.GetChild(0).transform, Position, Position);
        yield return new WaitForSeconds(1);
        PConsumableType.BERI.SetValue(beri);
        yield return new WaitForSeconds(0.5f);
        popupResource.ForceClose();
        popupRetreatPVE?.ForceClose();
        yield return new WaitForSeconds(0.5f);
        SceneTransitionHelper.Load(ESceneName.Home);
    }
}
