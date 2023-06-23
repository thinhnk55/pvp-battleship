using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JsonExtension
{
    public static JSONData ToJson(this int integer)
    {
        return new JSONData(integer);
    }
    public static JSONData ToJson(this GameServerEvent @event)
    {
        return new JSONData((int)@event);
    }

    public static List<int> ToList(this JSONNode json, bool debug = false)
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

    public static T ToEnum<T>(this JSONNode json) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), json.AsInt);
    }
}
