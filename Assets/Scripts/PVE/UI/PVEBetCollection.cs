using Framework;
using System.Collections.Generic;
using UnityEngine;

public struct PVEBetInfo
{
    public bool IsQualified;
    public int id;
    public int cost;
    public Callback onclick;
}
public class PVEBetCollection : CardCollectionBase<PVEBetInfo>
{
    [SerializeField] SnapScrollView SnapScrollView;
    [SerializeField] int highestAvalableBet;

    public override void UpdateUIs()
    {
        List<PVEBetInfo> list = new List<PVEBetInfo>();
        for (int i = 0; i < PVEData.Bets.Count; i++)
        {
            int _i = i;
            bool isQualified = PConsumableType.BERRY.GetValue() < PVEData.Bets[i] ? false : true;
            list.Add(new PVEBetInfo()
            {
                id = _i,
                IsQualified = isQualified,
                cost = PVEData.Bets[i],
                onclick = () =>
                {
                    isQualified = PConsumableType.BERRY.GetValue() < PVEData.Bets[_i] ? false : true;
                    if (isQualified)
                    {
                        PVEData.TypeBoard = _i;
                        SceneTransitionHelper.Load(ESceneName.PVE);
                    }
                    else
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupOutOfResource, "Message", "Not enough money", null, (ok) =>
                        {
                            if (ok)
                            {
                                PopupBehaviour.CloseAll();
                                PopupHelper.Create(PrefabFactory.PopupShop).GetComponentInChildren<Tabs>().Activate(0);
                            }
                        });
                    }
                }
            }); ;
            //if (isQualified && _i > highestAvalableBet)
            //{
            //    highestAvalableBet = _i;
            //}
        }

        BuildUIs(list);
    }

    private void Start()
    {
        UpdateUIs();
        SnapScrollView.Init();
        if (PVEData.TypeBoard != -1 && PVEData.IsDeadPlayer.Data == false) // Old game
        {
            highestAvalableBet = PVEData.TypeBoard.Value;
        }
        SnapScrollView.SetToChildPosition(highestAvalableBet);
    }

}
