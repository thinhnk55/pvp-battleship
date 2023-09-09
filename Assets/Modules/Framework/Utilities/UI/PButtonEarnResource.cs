using TMPro;
using UnityEngine;

namespace Framework
{
    public class PButtonEarnResource : ButtonBase
    {
        [Header("Reference")]
        [SerializeField] protected TextMeshProUGUI _txtMain;

        protected PResourceValue _resourceValue;

        public event Callback OnEarn;

        #region Public

        public void Construct(PResourceValue value)
        {
            _resourceValue = value;

            UpdateUI();
        }

        public void SetEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

        #endregion

        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();

            if (!this.enabled)
                return;

            if (_resourceValue == null)
            {
                PDebug.LogWarning("{0} is null, can't earn resource", typeof(PResourceValue));
            }
            else
            {
                EarnResource();
            }
        }

        protected virtual void UpdateUI()
        {
            _txtMain.text = _resourceValue.Count.ToString();
        }

        protected virtual void EarnResource()
        {
            _resourceValue.Type.AddValue(_resourceValue.Count);

            OnEarn?.Invoke();
        }
    }
}