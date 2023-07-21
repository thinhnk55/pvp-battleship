using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ButtonLogout : ButtonBase
    {
        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();
            WSClientBase.Instance.ws.Close();
            SceneTransitionHelper.Load(ESceneName.PreHome);
            //WSClientBase.Instance;
        }
    }

}
