using Framework;
using Server;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HDROutputUtils;

public class SystemMaintenance : MonoBehaviour
{
    private void HTTPGetCheckOperation()
    {
        PCoroutine.PStartCoroutine(HTTPClientBase.Get(ServerConfig.HttpURL + "/config/operation"
            , (res) =>
            {
                HandleHTTPGetCheckOperation(res);
            })

        );
    }
    // Start is called before the first frame update
    void Start()
    {
        HTTPGetCheckOperation();
    }

    private void HandleHTTPGetCheckOperation(JSONNode data)
    {
        if (data["data"]["maintain"]["is_maintain"] == false)
        {
            SceneManager.LoadScene("Loading");
        }
        else
        {
            Debug.Log("Maintain!!!");
        }


    }
}
