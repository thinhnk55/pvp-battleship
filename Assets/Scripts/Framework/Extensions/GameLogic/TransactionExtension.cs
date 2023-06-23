using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class TransactionExtension
    {
        public static void Transact(this PGoodType goodType, int value)
        {
            switch (goodType)
            {
                case PGoodType.USD:
                    break;
                case PGoodType.GEM:
                    ((PResourceType)goodType).AddValue(value);
                    break;
                case PGoodType.BERI:
                    ((PResourceType)goodType).AddValue(value);
                    break;
                case PGoodType.AVATAR:
                    break;
                case PGoodType.AVATAR_FRAME:
                    break;
                case PGoodType.SHIP_SKIN:
                    break;
                default:
                    break;
            }
        }
        public static bool IsAffordable(this PGoodType goodType, int value)
        {
            switch (goodType)
            {
                case PGoodType.USD:
                    break;
                case PGoodType.GEM:
                    return ((PResourceType)goodType).GetValue() >= value;
                case PGoodType.BERI:
                    return ((PResourceType)goodType).GetValue() >= value;
                case PGoodType.AVATAR:
                    break;
                case PGoodType.AVATAR_FRAME:
                    break;
                case PGoodType.SHIP_SKIN:
                    break;
                default:
                    return true;
            }
            return true;
        }
    }
}

