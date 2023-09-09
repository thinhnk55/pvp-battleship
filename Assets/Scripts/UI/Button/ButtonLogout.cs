using Framework;
using Server;

public class ButtonLogout : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        WSClient.Instance.Disconnect(true);
        SceneTransitionHelper.Load(ESceneName.PreHome);
    }
}


