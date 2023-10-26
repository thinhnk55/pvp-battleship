using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PDataResource : PDataBlock<PDataResource>
    {
        [SerializeField] Dictionary<PConsumableType, PDataUnit<int>> _consumableResource;
        [SerializeField] Dictionary<PNonConsumableType, PDataUnit<HashSet<int>>> _nonConsumableResource;
        [SerializeField] PDataUnit<int> _resourceDiamond;
        [SerializeField] PDataUnit<int> _resourceBeri;  
        protected override void Init()
        {
            base.Init();
            _consumableResource = _consumableResource ?? new Dictionary<PConsumableType, PDataUnit<int>>(); 
            foreach (PConsumableType item in Enum.GetValues(typeof(PConsumableType)))
            {
                _consumableResource.TryAdd(item, new PDataUnit<int>(0));
            }
            _nonConsumableResource = _nonConsumableResource ?? new Dictionary<PNonConsumableType, PDataUnit<HashSet<int>>>();
            foreach (PNonConsumableType item in Enum.GetValues(typeof(PNonConsumableType)))
            {
                _nonConsumableResource.TryAdd(item, new PDataUnit<HashSet<int>>(new HashSet<int>()));
            }
            _resourceDiamond = _resourceDiamond ?? new PDataUnit<int>(InitializationConfig.InitCoin);
            _resourceBeri = _resourceBeri ?? new PDataUnit<int>(InitializationConfig.InitGem);
        }

        public static PDataUnit<int> GetResourceData(PConsumableType type)
        {
            return Instance._consumableResource[type];
        }

        public static PDataUnit<HashSet<int>> GetResourceData(PNonConsumableType type)
        {
            return Instance._nonConsumableResource[type];

        }
    }
}