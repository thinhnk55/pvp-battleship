using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum SocialAuthType
{
    Google,
    GooglePlay,
    Facebook,
    Apple,
    Anonymous,
}

public interface ISocialAuth
{
    void Initialize();
    void SignUp();
    void SignIn();
    void SignOut();
}
