using Authentication;
using Framework;
using Server;
using Auth = Authentication.AuthenticationBase;

public class ButtonLogout : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        //Auth.Instance.auths[Ga].SignIn();

        WSClient.Instance.Disconnect(true);
        SceneTransitionHelper.Load(ESceneName.PreHome);
    }
}