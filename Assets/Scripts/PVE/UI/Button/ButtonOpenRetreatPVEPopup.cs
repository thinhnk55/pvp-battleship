using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOpenRetreatPVEPopup : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        PopupConfirm popupRetreatPVE = PopupHelper.CreateConfirm(PrefabFactory.PopupRetreatPVE, null,
            "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data]).ToString(), null, (confirm) => {
            if(confirm)
            {
                    Debug.Log("You have recevied " + 500);
                    //PConsumableType.BERI.AddValue();
                    //CoinVFX.CoinVfx(resource, Position, Position);
            }
        });
    }
}
