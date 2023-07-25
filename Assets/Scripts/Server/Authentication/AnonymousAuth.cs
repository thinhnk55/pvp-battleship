using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnonymousAuth : ISocialAuth
{
    public void Initialize()
    {
    }

    public void SignIn()
    {
        HTTPClient.Instance.LoginDeviceId();
    }

    public void SignOut()
    {
        throw new System.NotImplementedException();
    }

    public void SignUp()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
    }
}
