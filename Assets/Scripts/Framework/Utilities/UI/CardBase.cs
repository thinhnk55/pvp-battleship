using Framework;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public abstract class CardBase<T> : CacheMonoBehaviour where T : struct
    {
        public Button Button;
        public Callback onClick;
        public T info;
        public CardCollectionBase<T> collection;
        public virtual void BuildUI(T info)
        {
            this.info = info;        
        }
        protected abstract void OnClicked(T info);
    }
}
