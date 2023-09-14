using Framework;
using UnityEngine;

public class Home : MonoBehaviour
{
    private void Start()
    {
        if (!GameData.Starter && GameData.Tutorial[0] == 1)
        {
            PopupHelper.Create(PrefabFactory.PopupStarter);
            GameData.Starter = true;
        }
        else if (GameData.Tutorial[0] == 0)
        {
            PopupHelper.Create(PrefabFactory.PopupTuTorHome);
        }
    }

}
