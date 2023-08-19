using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogout : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        WSClientHandler.Instance.Disconnect();
        SceneTransitionHelper.Load(ESceneName.PreHome);
    }
}


