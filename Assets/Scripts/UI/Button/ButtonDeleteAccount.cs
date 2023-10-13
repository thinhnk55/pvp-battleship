using Authentication;
using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteAccount : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        HTTPClientAuth.DeleteAccount();
    }
}
