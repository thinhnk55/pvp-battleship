using Framework;
using Server;
using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemMaintenance : SingletonMono<SystemMaintenance>
{
    public bool forceUpdate = false;
    public string linkGame;
    private void HTTPGetCheckOperation()
    {
        PopupBehaviour loadingUI = PopupHelper.Create(PrefabFactory.LoadingUI);
        PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/config/operation"
            , (res) =>
            {
                loadingUI.ForceClose();
                HandleHTTPGetCheckOperation(res);
            })

        );
    }
    // Start is called before the first frame update
    void Start()
    {
        HTTPGetCheckOperation();
    }

    private void HandleHTTPGetCheckOperation(string data)
    {
        string platform;
#if UNITY_ANDROID
        platform = "android";
#elif UNITY_IOS
        platform = "ios";
#endif
        JSONNode jsonRes = JSONNode.Parse(data);
        if (jsonRes["data"]["maintain"]["is_maintain"].AsBool == false) // 
        {
            if (IsOldVersion(Application.version, jsonRes["data"]["update"][platform]["version"]))
            {
                HandleOldVersion(jsonRes, platform);
                return;
            }

            SceneManager.LoadScene("Loading");
        }
        else
        {
            DateTime startMaintain = TimeStampToDateTime(jsonRes["data"]["maintain"]["from"].AsLong);
            DateTime endMaintain = TimeStampToDateTime(jsonRes["data"]["maintain"]["to"].AsLong);
            string from = startMaintain.ToString();
            string to = endMaintain.ToString();
            string message = "Our Game is maintaining for an update from [" + from + "] to [" + to + "]. We apologize for any inconvenience this may cause. Thank you for your understanding and patience!";
            PopupHelper.CreateConfirm(PrefabFactory.PopupMaintain, null, message, null, (success) =>
            {
                if (success)
                {
                    Debug.Log("Quit");
                    Application.Quit();
                }
            });
        }
    }

    private void HandleOldVersion(JSONNode jsonRes, string platform)
    {
        forceUpdate = jsonRes["data"]["update"][platform]["force"].AsBool;
        linkGame = jsonRes["data"]["update"][platform]["url"].ToString();
        string message = "Version {" + jsonRes["data"]["update"][platform]["version"] + "} is ready!\nUpgrade now to experience the latest features.";
        PopupHelper.CreateMessage(PrefabFactory.PopupUpgradeVersion, null, message, null);
    }

    public static DateTime TimeStampToDateTime(long timeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(timeStamp).ToLocalTime();
        return dateTime;
    }

    private bool IsOldVersion(string versionPlayer, string newVersion)
    {
        string[] parts1 = versionPlayer.Split('.');
        string[] parts2 = newVersion.Split('.');

        for (int i = 0; i < Mathf.Max(parts1.Length, parts2.Length); i++)
        {
            int part1 = i < parts1.Length ? int.Parse(parts1[i]) : 0;
            int part2 = i < parts2.Length ? int.Parse(parts2[i]) : 0;

            if (part2 > part1)
                return true;
        }

        return false;
    }
}
