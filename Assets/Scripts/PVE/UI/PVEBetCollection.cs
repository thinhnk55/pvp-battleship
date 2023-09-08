using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
public struct PVEBetInfo
{
    public bool IsQualified;
    public int id;
    public int cost;
    public Callback onclick;
}
public class PVEBetCollection : CardCollectionBase<PVEBetInfo> 
{
    private void Awake()
    {
        UpdateUIs();
    }
    public override void UpdateUIs()
    {
        List<PVEBetInfo> list = new List<PVEBetInfo>();
        for (int i = 0; i < PVEData.Bets.Count; i++)
        {
            int _i = i;
            bool isQualified = PConsumableType.BERI.GetValue() < PVEData.Bets[i] ? false : true;
            list.Add(new PVEBetInfo()
            {
                id = _i,
                IsQualified = isQualified,
                cost = PVEData.Bets[i],
                onclick = () =>
                {
                    if (isQualified)
                    {
                        PVEData.TypeBoard = _i;
                        SceneTransitionHelper.Load(ESceneName.PVE);
                        PConsumableType.BERI.AddValue(-PVEData.Bets[_i]);
                    }
                    else
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupOutOfResource, "Message", "Not enough money", null, (ok) =>
                        {
                            if (ok)
                            {
                                PopupBehaviour.CloseAll();
                                PopupHelper.Create(PrefabFactory.PopupShop).GetComponentInChildren<Tabs>().Activate(1);
                            }
                        });
                    }
                }
            });; 
        }

        BuildUIs(list);
    }
    
}
