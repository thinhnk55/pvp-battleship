using Framework;
using Server;
using SimpleJSON;
using System.Collections;
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
            "+" + (PVEData.Bets[PVEData.TypeBoard.Value] * PVEData.StageMulReward[PVEData.TypeBoard.Value][PVE.Instance.CurrentStep.Data]).ToString(), null, (confirm) =>
            {
                if (confirm)
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
        int currentBeri = int.Parse(data["d"]["g"]);
        int rewardBeri = int.Parse(data["d"]["e"]);

        StartCoroutine(GetBeri(currentBeri + rewardBeri));
        PVEData.TypeBoard = int.Parse(data["d"]["d"]["t"]);
    }

    IEnumerator GetBeri(int beri)
    {
        PopupBehaviour popupResource = PopupHelper.Create(PrefabFactory.PopupResourcePVE);
        CoinVFX.CoinVfx(popupResource.transform.GetChild(0).transform, Position, Position);
        yield return new WaitForSeconds(1);
        PConsumableType.BERRY.SetValue(beri);
        yield return new WaitForSeconds(1f);
        popupResource.ForceClose();
        popupRetreatPVE?.ForceClose();
        SceneTransitionHelper.Load(ESceneName.Home);
    }
}
