using Framework;

public class ButtonFunc : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        PDebug.Log("Call");
        HTTPClientBase.Get("https://api.godoo.asia/game/sb/ads_reward", (data) =>
        {
            PDebug.Log(data.ToString());
        });

    }
}
