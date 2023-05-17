using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PDataProduct : PDataBlock<PDataProduct>
    {
        [SerializeField] Dictionary<string, PData<int>> _products;

        public static PData<int> GetProduct(PProduct product, int defaultValue = 0)
        {
            return GetProduct(product.ToString(), defaultValue);
        }

        public static int GetProductValue(PProduct product, int defaultValue = 0)
        {
            return GetProduct(product.ToString(), defaultValue).Data;
        }

        public static void SetProductValue(PProduct product, int value)
        {
            GetProduct(product.ToString()).Data = value;
        }

        public static PData<int> GetProduct(string id, int defaultValue = 0)
        {
            if (!Instance._products.ContainsKey(id))
                Instance._products.Add(id, new PData<int>(defaultValue));

            return Instance._products[id];
        }

        public static int GetProductValue(string id, int defaultValue = 0)
        {
            return GetProduct(id, defaultValue).Data;
        }

        public static void SetProductValue(string id, int value)
        {
            GetProduct(id).Data = value;
        }

        protected override void Init()
        {
            base.Init();

            if (_products == null)
                _products = new Dictionary<string, PData<int>>();
        }
    }
}