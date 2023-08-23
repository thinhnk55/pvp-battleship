using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class PDataAuth : PDataBlock<PDataAuth>
    {
        [SerializeField] private AuthData authData; public static AuthData AuthData { get { return Instance.authData; } set { Instance.authData = value; } }

        protected override void Init()
        {
            base.Init();
            Instance.authData = Instance.authData ?? new AuthData();
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