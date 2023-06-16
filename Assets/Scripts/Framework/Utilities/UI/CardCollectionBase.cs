using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    public abstract class CardCollectionBase<T> : CacheMonoBehaviour where T : struct
    {
        protected List<CardBase<T>> cards;
        [SerializeField] protected GameObject cardPrefab;
        [SerializeField] protected Transform contentRoot;
        public virtual void BuildUIs(List<T> infos)
        {
            contentRoot.DestroyChildrenImmediate();
            cards = new List<CardBase<T>>();
            for (int i = 0; i < infos.Count; i++)
            {
                CardBase<T> card = Instantiate(cardPrefab, contentRoot.transform).GetComponent<CardBase<T>>();
                card.collection = this;
                card.BuildUI(infos[i]);
                cards.Add(card);
            }
        }

        public virtual void BuildNewUI(T info)
        {
            CardBase<T> card = Instantiate(cardPrefab, contentRoot.transform).GetComponent<CardBase<T>>();
            card.collection = this;
            card.BuildUI(info);
            cards.Add(card);
        }
        public virtual void RebuildUI(int i,T info)
        {
            CardBase<T> card = cards[i];
            card.BuildUI(info);
        }
    }
}