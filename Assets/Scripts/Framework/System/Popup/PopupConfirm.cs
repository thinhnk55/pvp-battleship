using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class PopupConfirm : PopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _txtHeader;
        [SerializeField] TextMeshProUGUI _txtContent;
        [SerializeField] Image _icon;
        [SerializeField] Button _btnYes;
        [SerializeField] Button _btnNo;

        public event Callback<bool> OnConfirm;

        protected override void Awake()
        {
            base.Awake();

            _btnNo.onClick.AddListener(ButtonClickNoCallback);
            _btnYes.onClick.AddListener(ButtonClickYesCallback);
        }

        public void Construct(string header, string content, Sprite icon, Callback<bool> onComfirm)
        {
            _txtHeader?.SetText(header);
            _txtContent?.SetText(content);
            _icon?.SetSprite(icon);
            if(_icon != null) 
            {
                RectTransform rect = _icon?.GetComponent<RectTransform>();
                rect.SetScaleX(icon.GetSize().x / icon.GetSize().y);
            }
            OnConfirm += onComfirm;
        }

        void ButtonClickNoCallback()
        {
            OnConfirm?.Invoke(false);

            InternalClose();
        }

        void ButtonClickYesCallback()
        {
            OnConfirm?.Invoke(true);

            InternalClose();
        }

        protected override void HandleClose()
        {
            OnConfirm?.Invoke(false);
        }
    }
}
