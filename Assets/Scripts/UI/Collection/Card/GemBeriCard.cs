using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Framework;

public class GemBeriCard : TransactionCard
{
    [SerializeField] TextMeshProUGUI bonusText;
    public override void BuildUI(TransactionInfo info)
    {
        base.BuildUI(info);

    }
}
