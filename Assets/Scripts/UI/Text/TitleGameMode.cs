using TMPro;
using UnityEngine;

public class TitleGameMode : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = GameConfig.BetNames[CoreGame.bet];
    }
}
