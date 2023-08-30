using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        Debug.Log("Call");
        HTTPClientBase.Get("https://api.godoo.asia/game/sb/ads_reward", (data) =>
        {
            Debug.Log(data.ToString());
        });

    }
}
