using Framework;
using Server;
using SimpleJSON;
using UnityEngine.Purchasing;

public class IAP : IAPBase
{
    protected override void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.0", ProductType.Consumable); //,new IDs{{"100_gold_coins_google", GooglePlay.Name},{"100_gold_coins_mac", MacAppStore.Name},}
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.1", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.2", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.3", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.4", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.5", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.6", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.gem.7", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.starter", ProductType.Consumable);
        builder.AddProduct($"{ApplicationConfig.BundleId}.elite", ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    protected override void Awake()
    {
        base.Awake();
        ServerMessenger.AddListener<JSONNode>(ServerResponse._PROFILE, OnGetProfile);
    }

    public void OnGetProfile(JSONNode json)
    {
        if (IAPData.PendingProducts.Count > 0)
        {
            foreach (var pendingProduct in IAPData.PendingProducts)
            {
                pendingProduct.Value.RequestServer();
            }
        }
    }
}
