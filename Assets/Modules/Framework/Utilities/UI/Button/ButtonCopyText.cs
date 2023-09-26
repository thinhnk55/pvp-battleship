using TMPro;
using UnityEngine;
namespace Framework
{
    public class ButtonCopyText : ButtonBase
    {
        [SerializeField] TextMeshProUGUI text;
        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();
            GUIUtility.systemCopyBuffer = text.text;
        }
    }
}
