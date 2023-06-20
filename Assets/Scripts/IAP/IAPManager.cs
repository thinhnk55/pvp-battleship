using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : IStoreListener
{

    private IStoreController controller;
    private IExtensionProvider extensions;

    public IAPManager()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("100_gold_coins", ProductType.Consumable, new IDs
        {
            {"100_gold_coins_google", GooglePlay.Name},
            {"100_gold_coins_mac", MacAppStore.Name}
        });

        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        var additional = new HashSet<ProductDefinition>() {
        new ProductDefinition("coins.500", ProductType.Consumable),
        new ProductDefinition("armour", ProductType.NonConsumable)
    };

        Action onSuccess = () => {
            Debug.Log("Fetched successfully!");
            // The additional products are added to the set of
            // previously retrieved products and are browseable
            // and purchasable.
            foreach (var product in controller.products.all)
            {
                Debug.Log(product.definition.id);
            }
        };

        Action<InitializationFailureReason> onFailure = (InitializationFailureReason i) => {
            Debug.Log("Fetching failed for the specified reason: " + i);
        };

        controller.FetchAdditionalProducts(additional, onSuccess, onFailure);
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log(message);
        Debug.Log(error);
    }
}