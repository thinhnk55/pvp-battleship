using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonExtension
{
    public static JSONData ToJson(this int integer)
    {
        return new JSONData(integer);
    }

    public static JSONData ToJson(this short shortType)
    {
        return new JSONData(shortType);
    }
    public static JSONData ToJson(this ServerRequest @event)
    {
        return new JSONData((int)@event);
    }
    public static JSONData ToJson(this ServerResponse @event)
    {
        return new JSONData((int)@event);
    }

    public static List<int> ToListInt(this JSONNode json, bool debug = false)
    {
        List<int> list = new List<int>();
        JSONArray ar = json.AsArray;
        for (int i = 0; i < ar.Count; i++)
        {
            list.Add(json[i].AsInt);
            if (debug)
            {
                Debug.Log(ar[i]);
            }
        }
        return list;
    }

    public static int[] ToArrayInt(this JSONNode json, bool debug = false)
    {
        int[] arr = new int[json.Count];
        JSONArray ar = json.AsArray;
        for (int i = 0; i < ar.Count; i++)
        {
            arr[i] = json[i].AsInt;
            if (debug)
            {
                Debug.Log(ar[i]);
            }
        }
        return arr;
    }

    public static T ToEnum<T>(this JSONNode json) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), json.AsInt);
    }


}
