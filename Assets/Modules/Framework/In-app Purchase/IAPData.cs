using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Framework
{
    public class IAPData : PDataBlock<IAPData>
    {
        Dictionary<Product, JSONNode> pendingProducts; public static Dictionary<Product, JSONNode> PendingProducts { get => Instance.pendingProducts; set => Instance.pendingProducts = value; }
        protected override void Init()
        {
            base.Init();
            PendingProducts ??= new();
        }
    }
}
