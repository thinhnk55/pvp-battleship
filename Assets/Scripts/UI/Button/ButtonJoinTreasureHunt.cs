using Framework;
using TMPro;
using UnityEngine;

public class ButtonJoinTreasureHunt : ButtonBase
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] ESceneName eSceneValue;

    private void OnEnable()
    {
        ChangeStatusButton();
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        SceneTransitionHelper.Load(eSceneValue);
    }

    private void ChangeStatusButton()
    {
        Debug.Log(PVEData.TypeBoard + "---" + PVEData.IsDeadPlayer.Data);
        if (PVEData.TypeBoard == -1 || PVEData.IsDeadPlayer.Data) // New Game
        {
            return;
        }
        eSceneValue = ESceneName.PVE;
        buttonText.SetText("Continue");
    }
}
