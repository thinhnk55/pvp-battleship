using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISocialGoogle
{
    void ProcessAuthentication(SignInStatus status);
}
