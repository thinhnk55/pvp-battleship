using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Authentication
{
    public class DataAuth : PDataBlock<DataAuth>
    {
        [SerializeField] private AuthData authData; public static AuthData AuthData { get { return Instance.authData; } set { Instance.authData = value; } }
        [SerializeField] private PDataUnit<bool> isLinkedGoogleAccount; public static PDataUnit<bool> IsLinkedGoogleAccount { get { return Instance.isLinkedGoogleAccount; } set { Instance.isLinkedGoogleAccount = value; } }
        [SerializeField] private PDataUnit<bool> isLinkedAppleAccount; public static PDataUnit<bool> IsLinkedAppleAccount { get { return Instance.isLinkedAppleAccount; } set { Instance.isLinkedAppleAccount = value; } }

        protected override void Init()
        {
            base.Init();
            Instance.authData ??= new AuthData();
            Instance.isLinkedAppleAccount ??= new PDataUnit<bool>(false);
            Instance.isLinkedGoogleAccount ??= new PDataUnit<bool>(false);
        }
    }
}

[SerializeField]
public class AuthData
{
    public int userId;
    public string username;
    public string token;
    public string refresh_token;
}