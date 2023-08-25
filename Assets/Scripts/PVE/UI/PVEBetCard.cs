using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PVEBetCard : CardBase<PVEBetInfo>
{
    [SerializeField] TextMeshProUGUI nameCard;
    [SerializeField] TextMeshProUGUI cost;
    protected override void OnClicked(PVEBetInfo info)
    {
        nameCard.SetText(info.name);
        cost?.SetText(info.cost.ToString());
        Button?.onClick.RemoveAllListeners();
        Button?.onClick.AddListener(() =>
        {
            info.onclick?.Invoke();
        });
    }

}
