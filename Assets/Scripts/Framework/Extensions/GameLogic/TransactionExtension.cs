using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class TransactionExtension
    {
        public static PResourceType GetPResourceType(this int goodType)
        {
            if (Enum.IsDefined(typeof(PConsumableType), goodType))
            {
                return PResourceType.Consumable;
            }
            if (Enum.IsDefined(typeof(PNonConsumableType), goodType))
            {
                return PResourceType.Nonconsumable;
            }
            return PResourceType.Nonconsumable;
        }
        public static bool ProcessResource(this int goodType, Predicate<PConsumableType> callback1, Predicate<PNonConsumableType> callback2)
        {
            var _goodType = GetPResourceType(goodType);
            if (_goodType == PResourceType.Consumable)
            {
                return callback1.Invoke((PConsumableType)goodType);
            }
            if (_goodType == PResourceType.Nonconsumable)
            {
                return callback2.Invoke((PNonConsumableType)goodType);
            }
            return false;
        }
        public static void Transact(this int goodType, int value)
        {
            ProcessResource(goodType, 
                (_goodType) => { _goodType.AddValue(value); return true; }, 
                (_goodType) => { _goodType.AddValue(value); return true; }
                );
        }

        public static bool IsAffordable(this int goodType, int value)
        {
            return ProcessResource(goodType, 
                (_goodType) => { return _goodType.GetValue() >= value; }, 
                (_goodType) =>true );
        }

    }
}

