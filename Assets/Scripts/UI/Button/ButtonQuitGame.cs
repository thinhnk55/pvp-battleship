using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonQuitGame : ButtonBase
{
    [SerializeField] PopupBehaviour popupBehaviour;
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        if (GameData.Tutorial[4] == 1)
        {
            CoreGame.Instance.QuitGame();
        }
        else
        {
            GameData.Tutorial[4] = 1;
            GameData.Tutorial[4] = 1;
            GameData.Tutorial[3] = 1;
            GameData.Tutorial[2] = 1;
            GameData.Tutorial[1] = 1;
            Bot.ReplayTutorial = false;
            SceneManager.LoadScene("Home");
        }
        popupBehaviour.Close();
        AudioHelper.StopMusic();
    }
}
