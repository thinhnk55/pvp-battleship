using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUpgradeRP : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        IAP.PurchaseProduct($"{ApplicationConfig.BundleId}.elite", (success, product) =>
        {
            if (success)
            {
                JSONNode jsonNode = new JSONClass
            {
                { "id", ServerResponse._RP_UPGRADE.ToJson() },
                { "r", JSON.Parse(product.receipt.Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}")) }
            };
                WSClient.Instance.Send(jsonNode);
            }
        });

    }
}
