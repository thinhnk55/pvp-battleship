using Framework;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    private void Start()
    {
        if (!GameData.Starter && !GameData.StarterShow && ServerData.IsTutorialComplete)
        {
            GameData.Tutorial = new List<int> { 1, 1, 1, 1 };
            PopupHelper.Create(PrefabFactory.PopupStarter);
            GameData.StarterShow = true;
        }
        else if (!ServerData.IsTutorialComplete)
        {
            GameData.Tutorial = new List<int>() { 1, 0, 0, 0 };
            PopupHelper.Create(PrefabFactory.PopupTuTorHome);
        }
        ServerData.isTutorialCompleteOnChange += IsTutorialCompleteOnDataChanged;
    }
    private void OnDestroy()
    {
        ServerData.isTutorialCompleteOnChange -= IsTutorialCompleteOnDataChanged;
    }
    private void IsTutorialCompleteOnDataChanged(bool isTutorialComplete)
    {
        if (!GameData.Starter && !GameData.StarterShow && isTutorialComplete)
        {
            GameData.Tutorial = new List<int> { 1, 1, 1, 1 };
            PopupHelper.Create(PrefabFactory.PopupStarter);
            GameData.StarterShow = true;
        }
        else if (!isTutorialComplete)
        {
            GameData.Tutorial = new List<int>() { 1, 0, 0, 0 };
            PopupHelper.Create(PrefabFactory.PopupTuTorHome);
        }
    }
}
