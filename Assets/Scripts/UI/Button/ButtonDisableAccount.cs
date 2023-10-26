using Authentication;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDisableAccount : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        HTTPClientAuth.DisableAccount();
    }
}
