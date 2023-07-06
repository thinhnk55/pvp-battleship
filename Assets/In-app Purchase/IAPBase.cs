using System;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Purchasing.Extension;

public class IAPBase : SingletonMono<IAPBase>, IDetailedStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private Action<bool> purchaseAction;
    private string _currentBundleId;

    protected override void Awake()
    {
        base.Awake();
        if (!IsInitialized())
        {
            UnityPurchasing.Initialize(this, InitializePurchasing());
        }
    }

    protected virtual ConfigurationBuilder InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        return builder;
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public string GetProductPriceFromStore(string id, string defaultPrice)
    {
        if (m_StoreController != null && m_StoreController.products != null)
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        else
            return defaultPrice;
    }

    public void BuyProduct(string bundleID, Action<bool> action)
    {
        purchaseAction = action;
        _currentBundleId = bundleID;
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(bundleID);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Product is not found or unavailable");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");
            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result, message) =>
            {
                if (!result)
                {
                    Debug.Log("RestorePurchases continuing: " + message + ". If no further messages, no purchases available to restore.");
                }
                else
                {
                    Debug.Log("RestorePurchases Successfully");
                }
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    // IStoreListener
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, _currentBundleId, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            purchaseAction?.Invoke(true);
        }
        else
        {
            purchaseAction?.Invoke(false);
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log(string.Format("Purchase Failed: Product: '{0}', {1}", error.ToString(), message));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log(string.Format("Purchase Failed: Product: '{0}', {1}", product.definition.storeSpecificId, failureDescription));
        purchaseAction?.Invoke(false);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("Purchase Failed: Product: '{0}', {1}", product.definition.storeSpecificId, failureReason));
        purchaseAction?.Invoke(false);
    }
}
