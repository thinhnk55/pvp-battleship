using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Framework
{
    public class PResourceBarUI : CacheMonoBehaviour
    {
        static readonly float EaseValueDuration = 0.75f;

        static readonly int AnimID_Plus = Animator.StringToHash("Plus");
        static readonly int AnimID_Minus = Animator.StringToHash("Minus");
        static readonly int AnimID_Idle = Animator.StringToHash("Idle");

        [Header("Reference")]
        [SerializeField] protected TextMeshProUGUI _txtValue;

        [Header("Config")]
        [SerializeField] protected PResourceType _type;
        [SerializeField] protected bool _manualConstruct;

        Animator _animator;
        int _value = 0;
        Tween _tween;

        #region MonoBehaviour

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();

            if (!_manualConstruct)
                Construct(_type.GetValue());
        }

        protected virtual void OnDestroy()
        {
            _tween?.Kill();

            _type.GetData().OnDataChanged -= ResourceValue_OnDataChanged;
        }

        #endregion

        #region Public

        public void Construct(int value)
        {
            _value = value;
            TextUpdate(value);

            _type.GetData().OnDataChanged += ResourceValue_OnDataChanged;
        }

        public void AddValue(int addValue)
        {
            int startValue = _value;
            int endValue = startValue + addValue;
            _value += addValue;

            EaseValue(startValue, endValue);
        }

        #endregion

        #region Core

        void EaseValue(int startValue, int endValue)
        {
            if (_animator != null)
                _animator.SetTrigger(endValue > startValue ? AnimID_Plus : AnimID_Minus);

            _tween?.Kill();
            _tween = DOTween.To(() => startValue, x => { startValue = x; Tween_ValueUpdate(startValue); }, endValue, EaseValueDuration)
                .SetUpdate(true);
            _tween.OnComplete(Tween_Complete);
        }

        void Tween_ValueUpdate(int value)
        {
            TextUpdate(value);
        }

        void Tween_Complete()
        {
            if (_animator != null)
                _animator.SetTrigger(AnimID_Idle);
        }

        void ResourceValue_OnDataChanged(int oldValue, int newValue)
        {
            if (newValue != _value)
                AddValue(newValue - _value);
        }

        #endregion

        #region Protected

        protected virtual void TextUpdate(int value)
        {
            // Update value text
            _txtValue.text = value.ToString();
        }

        #endregion
    }
}