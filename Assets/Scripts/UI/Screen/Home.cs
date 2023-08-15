using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    private void Start()
    {
        if (!GameData.Starter)
        {
            PopupHelper.Create(PrefabFactory.PopupStarter);
        }
    }
}
