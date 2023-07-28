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
        [SerializeField] GameObject _icon;
        [SerializeField] GameObject _iconContainer;
        public void Construct(string header, string msg, GameObject icon)
        {
            _header.text = header;
            _txtContent.text = msg;
            DestroyImmediate(_icon);
            _icon = Instantiate(icon, _iconContainer.transform);
        }
    }
}