using Authentication;
using Framework;
using Server;
using Auth = Authentication.AuthenticationBase;

public class ButtonLogout : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        WSClient.Instance.Disconnect(true);
        Auth.Instance.auths[GameData.TypeLogin].SignOut();
        DataAuth.AuthData.token = "";
        SceneTransitionHelper.Load(ESceneName.PreHome);
    }
}


