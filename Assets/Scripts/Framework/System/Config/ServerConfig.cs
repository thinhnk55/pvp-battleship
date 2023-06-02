using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Framework
{

    [Serializable]
    public class ServerConfig : SingletonScriptableObject<ServerConfig>
    {
        [SerializeField] private string webSocketURL; public static string WebSocketURL { get { return Instance.webSocketURL; } }
        [SerializeField] private string id; public static string Id { get { return Instance.id; } }
        [SerializeField] private string dataBaseURL; public static string DataBaseURL { get { return Instance.dataBaseURL; } }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            if (_instance == null)
            {
                Instance.ToString();
            }
        }
    }
}
