using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(PDataTime.PlayCount);
        if (!GameData.Starter && PDataTime.PlayCount>1)
        {
            PopupHelper.Create(PrefabFactory.PopupStarter);
        } 
        else if (PDataTime.PlayCount == 1)
        {
            PopupHelper.Create(PrefabFactory.PopupTuTorHome);
        }
    }
}
