using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonQuitGame : ButtonBase
{
    [SerializeField] PopupBehaviour popupBehaviour;
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        CoreGame.Instance.QuitGame();
        popupBehaviour.Close();
        AudioHelper.StopMusic();
    }
}
