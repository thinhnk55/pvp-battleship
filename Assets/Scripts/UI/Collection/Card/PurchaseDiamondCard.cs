using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseDiamondCard : CardBase<TransactionInfo>
{
    [SerializeField] TextMeshProUGUI goodAmount;
    [SerializeField] Image goodIcon;
    [SerializeField] TextMeshProUGUI costAmount;
    [SerializeField] Image costIcon;

    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);
        if (goodAmount)
        {
            goodAmount.text = info.Product[0].Number.ToString();
        }
        if (costAmount)
        {
            costAmount.text = info.Cost[0].Number.ToString();
        }
        if (goodIcon)
        {
            goodIcon.sprite = info.Product[0].Icon;
        }
        if (costIcon)
        {
            costIcon.sprite = info.Cost[0].Icon;
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
