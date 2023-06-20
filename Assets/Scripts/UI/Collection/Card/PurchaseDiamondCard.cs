using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseDiamondCard : CardBase<TransactionInfo>
{
    [SerializeField] TextMeshProUGUI productAmount;
    [SerializeField] Image productIcon;
    [SerializeField] TextMeshProUGUI costAmount;
    [SerializeField] Image costIcon;

    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (productAmount)
        {
            productAmount.text = info.Product[0].Number.ToString();
        }
        if (costAmount)
        {
            costAmount.text = info.Cost[0].Number.ToString();
        }
        if (productIcon)
        {
            productIcon.sprite = SpriteFactory.ResourceIcons[(int)info.Product[0].Type];
        }
        if (costIcon)
        {
            costIcon.sprite = SpriteFactory.ResourceIcons[(int)info.Cost[0].Type];
        }
        if (Button)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() =>
            {

            });
        }
    }

    protected override void OnClicked(TransactionInfo info)
    {
        throw new System.NotImplementedException();
    }

}
