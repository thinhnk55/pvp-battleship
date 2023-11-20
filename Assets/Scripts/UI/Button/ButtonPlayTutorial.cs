using Framework;
using System;
using UnityEngine.SceneManagement;


public class ButtonPlayTutorial : ButtonBase
{
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        GameData.Tutorial[4] = 0;
        SceneManager.LoadScene("MainGame");
    }
}
