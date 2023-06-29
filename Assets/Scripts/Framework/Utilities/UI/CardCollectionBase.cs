using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    public abstract class CardCollectionBase<T> : CacheMonoBehaviour where T : struct
    {
        protected List<CardBase<T>> cards;
        protected CardBase<T> selectedCard; public CardBase<T> SelectedCard { get { return selectedCard; } set { OnSelectedCard?.Invoke(selectedCard, value); selectedCard = value;} }
        protected Callback<CardBase<T>, CardBase<T>> OnSelectedCard;

        [SerializeField] protected GameObject cardPrefab;
        [SerializeField] protected Transform contentRoot;
        public abstract void UpdateUIs();
        public virtual void BuildUIs(List<T> infos)
        {
            contentRoot.DestroyChildrenImmediate();
            cards = new List<CardBase<T>>();
            for (int i = 0; i < infos.Count; i++)
            {
                CardBase<T> card = Instantiate(cardPrefab, contentRoot.transform).GetComponent<CardBase<T>>();
                card.Collection = this;
                card.BuildUI(infos[i]);
                cards.Add(card);
            }
        }

        public virtual void AddUI(T info)
        {
            CardBase<T> card = Instantiate(cardPrefab, contentRoot.transform).GetComponent<CardBase<T>>();
            card.Collection = this;
            card.BuildUI(info);
            cards.Add(card);
        }
        public virtual void ModifyUIAt(int i,T info)
        {
            CardBase<T> card = cards[i];
            card.BuildUI(info);
        }
    }
}