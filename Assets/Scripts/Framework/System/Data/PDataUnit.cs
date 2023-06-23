using UnityEngine;

namespace Framework
{
    [System.Serializable]
    public class PDataUnit<T>
    {
        [SerializeField] T _data;

        public T Data
        {
            get { return _data; }
            set
            {
                if (!_data.Equals(value))
                {
                    OnDataChanged?.Invoke(_data , value);
                    _data = value;
                }
            }
        }

        public event Callback<T, T> OnDataChanged;

        public PDataUnit(T defaultValue)
        {
            _data = defaultValue;
        }
    }
}