using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP : IAPBase
{
    protected override ConfigurationBuilder InitializePurchasing()
    {
        var builder = base.InitializePurchasing();
        return builder;
    }
}
