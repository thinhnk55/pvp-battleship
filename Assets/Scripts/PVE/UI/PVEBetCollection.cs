using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
public struct PVEBetInfo
{
    public int cost;
    //public string name;
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
            list.Add(new PVEBetInfo()
            {
                cost = PVEData.Bets[_i],
                //name = GameConfig.BetPVENames[_i],
                onclick = () =>
                {
                    if (PConsumableType.BERI.GetValue()>= PVEData.Bets[_i])
                    {
                        PVEData.PlayerData.TypeBoard = _i;
                        SceneTransitionHelper.Load(ESceneName.PVE);
                    }  
                }
            });
        }

        BuildUIs(list);
    }
    
}
