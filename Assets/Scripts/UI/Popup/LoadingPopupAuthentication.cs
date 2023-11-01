using Authentication;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPopupAuthentication : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HTTPClientAuth.HandleLoginAccountResponse += HandleLoginAccountResponse;
        HTTPClientAuth.HandleLogoutResponse += HandleLogoutResponse;
        HTTPClientAuth.HandleDisableAccountResponse += HandleDisableAccountResponse;
        HTTPClientAuth.HandleDeleteAccountResponse += HandleDeleteAccountResponse;
    }
    private void OnDestroy()
    {
        HTTPClientAuth.HandleLoginAccountResponse -= HandleLoginAccountResponse;
        HTTPClientAuth.HandleLogoutResponse -= HandleLogoutResponse;
        HTTPClientAuth.HandleDisableAccountResponse -= HandleDisableAccountResponse;
        HTTPClientAuth.HandleDeleteAccountResponse -= HandleDeleteAccountResponse;
    }

    private void HandleDeleteAccountResponse(string res)
    {
        JSONNode jsonParse = JSONNode.Parse(res);
        if (int.Parse(jsonParse["error"]) == 0)
        {
            return;
        }

        Destroy(this.gameObject);
    }

    private void HandleDisableAccountResponse(string res)
    {
        JSONNode jsonParse = JSONNode.Parse(res);
        if (int.Parse(jsonParse["error"]) == 0)
        {
            return;
        }

        Destroy(this.gameObject);
    }

    private void HandleLoginAccountResponse(string res)
    {
        JSONNode jsonParse = JSONNode.Parse(res);
        if (int.Parse(jsonParse["error"]) == 0)
        {
            return;
        }

        Destroy(this.gameObject);
    }

    private void HandleLogoutResponse(string res)
    {
        JSONNode jsonParse = JSONNode.Parse(res);
        if (int.Parse(jsonParse["error"]) == 0)
        {
            return;
        }

        Destroy(this.gameObject);
    }
}
