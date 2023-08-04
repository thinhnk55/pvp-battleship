using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class PopupMessage : PopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _header;
        [SerializeField] TextMeshProUGUI _txtContent;
        [SerializeField] Image _icon;
        public void Construct(string header, string msg, Sprite icon)
        {
            _header.text = header;
            _txtContent.text = msg;
            _icon.sprite = icon;
        }
    }
}