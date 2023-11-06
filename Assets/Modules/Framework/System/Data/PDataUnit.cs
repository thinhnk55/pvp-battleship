using Newtonsoft.Json;
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
                if (_data != null && !_data.Equals(value))
                {
                    T oldValue = _data;
                    _data = value;
                    OnDataChanged?.Invoke(oldValue, value);
                }
            }
        }
        [JsonIgnore]
        public Callback<T, T> OnDataChanged;

        public PDataUnit(T defaultValue)
        {
            _data = defaultValue;
        }
    }
}
