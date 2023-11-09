using Framework;
using Server;
using SimpleJSON;
using UnityEngine.Purchasing;

public class ButtonUpgradeRP : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        IAP.PurchaseProduct($"{ApplicationConfig.BundleId}.elite2", (success, product) =>
        {
            if (success)
            {
#if UNITY_ANDROID
                RequestUpgradeRP(product.receipt.Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}"), product);
#elif UNITY_IOS
                RequestUpgradeRP(new JSONClass() { { "TransactionID", product.transactionID }, { "Store", "AppleAppStore" } }.ToString());
#endif
            }
        });

    }

    public static void RequestUpgradeRP(string data = null, Product product = null)
    {
        JSONNode jsonNode = new JSONClass
        {
            { "id", ServerResponse._RP_UPGRADE.ToJson() },
        };
        if (data != null && data != "")
        {
            jsonNode.Add("r", JSON.Parse(data));
        }

        if (product != null)
            IAPData.PendingProducts.TryAdd(product, jsonNode);
        WSClient.Instance.Send(jsonNode);
    }
}
