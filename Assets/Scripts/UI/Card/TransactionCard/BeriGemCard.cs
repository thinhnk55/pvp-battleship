using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Framework;

public class BeriGemCard : TransactionCard
{
    [SerializeField] TextMeshProUGUI bonusText;
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);

        for (int i = 0; i < productIcon.Length; i++)
        {
            if (info.Product[i].Type == (int)PConsumableType.GEM)
            {
                productIcon[i].sprite = SpriteFactory.ResourceIcons[info.Product[0].Type].sprites.GetClamp(((int)info.Product[0].Value) / 20);
            }
            else if (info.Product[i].Type == (int)PConsumableType.BERI)
            {
                productIcon[i].sprite = SpriteFactory.ResourceIcons[info.Product[0].Type].sprites.GetClamp(((int)info.Product[0].Value) / 200);
            }
        }
    }
}
