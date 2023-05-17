using TMPro;
using UnityEngine;

namespace Framework
{
    public class PButtonSpendResource : ButtonBase
    {
        [Header("Reference")]
        [SerializeField] protected TextMeshProUGUI _txtMain;

        protected PResourceValue _resourceValue;

        public event Callback<bool> OnSpend;

        #region MonoBehaviour

        void OnDestroy()
        {
            if (_resourceValue != null)
                _resourceValue.Type.GetData().OnDataChanged -= ResourceValue_OnDataChanged;
        }

        #endregion

        #region Public

        public void Construct(PResourceValue value)
        {
            // Check to remove old listener
            if (_resourceValue != null)
                _resourceValue.Type.GetData().OnDataChanged -= ResourceValue_OnDataChanged;

            value.Type.GetData().OnDataChanged += ResourceValue_OnDataChanged;

            _resourceValue = value;

            UpdateUI();
        }

        #endregion

        void ResourceValue_OnDataChanged(int value)
        {
            UpdateUI();
        }

        protected override void Button_OnClicked()
        {
            base.Button_OnClicked();

            if (_resourceValue == null)
            {
                PDebug.LogWarning("{0} is null, can't spend resource", typeof(PResourceValue));
                return;
            }
            else
            {
                if (_resourceValue.Type.GetValue() >= _resourceValue.Count)
                {
                    SpendResource(true);
                }
                else
                {
                    SpendResource(false);
                }
            }
        }

        protected virtual void UpdateUI()
        {
            if (_resourceValue.Count == 0)
                _txtMain.text = "FREE";
            else
                _txtMain.text = _resourceValue.Count.ToString();
        }

        protected virtual void SpendResource(bool success)
        {
            if (success)
                _resourceValue.Type.AddValue(-_resourceValue.Count);

            OnSpend?.Invoke(success);
        }
    }
}