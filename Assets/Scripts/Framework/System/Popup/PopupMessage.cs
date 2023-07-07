using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class PopupMessage : PopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _txtContent;
        [SerializeField] Image _imgContent;

        public void Construct(string msg, Sprite sprite)
        {
            _txtContent.text = msg;
            _imgContent.sprite = sprite;
        }
    }
}