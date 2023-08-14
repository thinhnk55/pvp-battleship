using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAuthentication
{
    public void Authenticate();
    public void Signup(int type);
    public void Signin(LoginType type);
    public void Signout(int type);
    public void Delete();
    public void UpdatePlayerName();

}
