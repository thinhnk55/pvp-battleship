using SimpleJSON;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace Framework
{
    public static class PResourceExtensions
    {
        // Consumable
        public static PDataUnit<int> GetData(this PConsumableType type)
        {
            return PDataResource.GetResourceData(type);
        }

        public static int GetValue(this PConsumableType type)
        {
            PDataUnit<int> data = type.GetData();

            return data.Data;
        }

        public static void AddValue(this PConsumableType type, int value)
        {
            type.GetData().Data += value;
        }

        public static void SetValue(this PConsumableType type, int value)
        {
            type.GetData().Data = value;
        }


        //NonConsumable
        public static PDataUnit<HashSet<int>> GetData(this PNonConsumableType type)
        {
            return PDataResource.GetResourceData(type);
        }

        public static HashSet<int> GetValue(this PNonConsumableType type)
        {
            PDataUnit<HashSet<int>> data = type.GetData();

            return data.Data;
        }

        public static void AddValue(this PNonConsumableType type, int value)
        {
            type.GetData().Data.Add(value);
        }

        public static void SetValue(this PNonConsumableType type, List<int> value)
        {
            type.GetData().Data.AddRange(value);
        }

        public static void FromJson(this PNonConsumableType type, JSONNode data)
        {
            type.GetData().Data.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                type.AddValue(int.Parse(data[i]));
            }
        }
    }
}