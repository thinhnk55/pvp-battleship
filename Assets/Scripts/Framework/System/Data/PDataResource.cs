using UnityEngine;

namespace Framework
{
    public class PDataResource : PDataBlock<PDataResource>
    {
        [SerializeField] PData<int> _resourceCoin;
        [SerializeField] PData<int> _resourceGem;

        protected override void Init()
        {
            base.Init();

            _resourceCoin = _resourceCoin ?? new PData<int>(InitializationConfig.InitCoin);
            _resourceGem = _resourceGem ?? new PData<int>(InitializationConfig.InitGem);
        }

        public static PData<int> GetResourceData(PResourceType type)
        {
            switch (type)
            {
                case PResourceType.Coin:
                    return Instance._resourceCoin;
                case PResourceType.Gem:
                    return Instance._resourceGem;
            }

            return null;
        }
    }
}