using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGoodRP : PopupGoods
{
    [SerializeField] GameObject RPContent;
    [SerializeField] GoodCollection viewElite;

    public override void Construct(string msg, List<GoodInfo> goodInfos)
    {
        base.Construct(msg, goodInfos);
        if (PNonConsumableType.ELITE.GetValue().Contains(0))
        {
            RPContent.SetActive(false);
        }
        else
        {
            Dictionary<int, GoodInfo> goods = new Dictionary<int, GoodInfo>();
            for (int i = 0; i < GameData.RoyalPass.Level; i++)
            {
                if (!GameData.RoyalPass.EliteObtains.Data.Contains(i))
                {
                    var list = GameData.RoyalPass.RewardElites[i];
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].Type.GetPResourceType() == PResourceType.Nonconsumable)
                        {
                            goods.Add(list[j].Type * 100 + (int)list[j].Value, list[j]);
                        }
                        else
                        {
                            if (goods.ContainsKey(list[j].Type * 100))
                            {
                                goods[list[j].Type * 100] = new GoodInfo()
                                {
                                    Type = list[j].Type,
                                    Value = list[j].Value + goods[list[j].Type * 100].Value,
                                };
                            }
                            else
                            {
                                goods.Add(list[j].Type * 100, list[j]);
                            }
                        }
                    }
                }
            }

            viewElite.BuildUIs(goods.ToList());
            viewElite.SetLayout();
            view.SetLayout(250);
        }
    }
}
