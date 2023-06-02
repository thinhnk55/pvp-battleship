using Framework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
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
}
