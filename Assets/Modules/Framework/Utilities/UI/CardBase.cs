using Framework;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    public abstract class CardBase<T> : CacheMonoBehaviour where T : struct
    {
        public T Info;
        public Button Button;
        public Callback OnClick;
        [HideInInspector] public CardCollectionBase<T> Collection;
        public virtual void BuildUI(T info)
        {
            this.Info = info;
        }
        protected abstract void OnClicked(T info);
    }
}
