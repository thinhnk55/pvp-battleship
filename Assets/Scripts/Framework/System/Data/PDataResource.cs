using UnityEngine;

namespace Framework
{
    public class PDataResource : PDataBlock<PDataResource>
    {
        [SerializeField] PDataUnit<int> _resourceDiamond;
        [SerializeField] PDataUnit<int> _resourceBeri;

        protected override void Init()
        {
            base.Init();

            _resourceDiamond = _resourceDiamond ?? new PDataUnit<int>(InitializationConfig.InitCoin);
            _resourceBeri = _resourceBeri ?? new PDataUnit<int>(InitializationConfig.InitGem);
        }

        public static PDataUnit<int> GetResourceData(PResourceType type)
        {
            switch (type)
            {
                case PResourceType.Diamond:
                    return Instance._resourceDiamond;
                case PResourceType.Beri:
                    return Instance._resourceBeri;
            }

            return null;
        }
    }
}