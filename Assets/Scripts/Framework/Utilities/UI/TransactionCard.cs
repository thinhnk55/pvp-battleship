using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Framework{
    public enum PGoodType
    {
        USD,
        GEM,
        BERI,
        AVATAR,
        AVATAR_FRAME,
        SHIP_SKIN
    }
    public struct GoodInfo
    {
        public float Value;
        public PGoodType Type;
    }

    public struct TransactionInfo
    {
        public int Index;
        public TransactionType TransactionType;
        public string Title;
        public GoodInfo[] Cost;
        public GoodInfo[] Product;
        public static TransactionInfo FromJson(JSONNode data, int id, int index)
        {
            TransactionInfo transactionInfo = new TransactionInfo();
            transactionInfo.TransactionType = (TransactionType)id;
            transactionInfo.Index = index;
            transactionInfo.Cost = new GoodInfo[data["cost"].Count];
            transactionInfo.Product = new GoodInfo[data["product"].Count];
            for (int i = 0; i < data["cost"].Count; i++)
            {
                transactionInfo.Cost[i] = new GoodInfo()
                {
                    Value = float.Parse(data["cost"][i]),
                    Type = data["cost_type"][i].ToEnum<PGoodType>(),
                };
            }
            for (int i = 0; i < data["product"].Count; i++)
            {
                transactionInfo.Product[i] = new GoodInfo()
                {
                    Value = float.Parse(data["product"][i]),
                    Type = data["product_type"][i].ToEnum<PGoodType>(),
                };
            }
            return transactionInfo;
        }
        public static List<TransactionInfo> ListFromJson(JSONNode data, int id)
        {
            List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
            for (int i = 0; i < data.Count; i++)
            {
                transactionInfos.Add(TransactionInfo.FromJson(data[i], id, i));
            }
            return transactionInfos;
        }

        public void Transact()
        {
            for (int i = 0; i < Cost.Length; i++)
            {
                var cost = Cost[i];
                cost.Type.Transact(-(int)cost.Value);
            }
            for (int i = 0; i < Product.Length; i++)
            {
                var product = Product[i];
                product.Type.Transact((int)product.Value);
            }
        }
        public bool IsAffordble()
        {
            for (int i = 0; i < Cost.Length; i++)
            {
                var cost = Cost[i];
                if (!cost.Type.IsAffordable((int)cost.Value))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class TransactionCard : CardBase<TransactionInfo>
    {
        [SerializeField] protected TextMeshProUGUI Title;

        [SerializeField] protected Image[] ProductIcon;
        [SerializeField] protected TextMeshProUGUI[] ProductAmount;

        [SerializeField] protected Image[] CostIcon;
        [SerializeField] protected TextMeshProUGUI[] CostAmount;

        public override void BuildUI(TransactionInfo info)
        {
            base.BuildUI(info);
            if (Title)
                Title.text = info.Title;
            if (ProductIcon[0])
                ProductIcon[0].sprite = SpriteFactory.ResourceIcons[(int)info.Product[0].Type];
            if (ProductAmount[0])
                ProductAmount[0].text = info.Product[0].Value.ToString();
            if (CostIcon[0])
                CostIcon[0].sprite = SpriteFactory.ResourceIcons[(int)info.Cost[0].Type];
            if (CostAmount[0])
            {
                if (info.Cost[0].Value == (int)info.Cost[0].Value)
                {
                    CostAmount[0].text = info.Cost[0].Value.ToString("F0");
                }
                else
                {
                    CostAmount[0].text = info.Cost[0].Value.ToString("F2");
                }

            }
            if (Button)
                Button.onClick.AddListener(
                    TransactionAction(info.TransactionType, this)
                );
        }

        protected override void OnClicked(TransactionInfo info)
        {
            throw new System.NotImplementedException();
        }
        public static UnityAction TransactionAction(TransactionType transactionType, TransactionCard card)
        {
            UnityAction action = null;
            action += () => { };
            bool affordable = true;
            switch (transactionType)
            {
                case TransactionType.USD_GEM:
                    break;
                case TransactionType.GEM_BERI:
                    action = () => {
                        for (int i = 0; i < card.Info.Cost.Length; i++)
                        {
                            var cost = card.Info.Cost[i];
                            affordable = cost.Type.IsAffordable((int)cost.Value);
                            if (!affordable)
                            {
                                break;
                            }
                        }
                        if (affordable)
                        {
                            RequestTransaction((int)card.Info.TransactionType, card.Info.Index);
                        }
                        else
                        {
                            Debug.Log("unaffordable");
                        }
                            
                    };
                    break;
                case TransactionType.BERI_AVATAR:
                    break;
                case TransactionType.BERI_AVATAR_FRAME:
                    break;
                case TransactionType.BERI_SKIN_SHIP:
                    break;
                default:
                    break;
            }
            return action;
        }
        public static void RequestTransaction(int id, int index)
        {
            JSONNode jsonNode = new JSONClass()
            {
                { "id", GameServerEvent.REQUEST_TRANSACTION.ToJson() },
                { "itemId", id.ToJson() },
                { "itemIndex", index.ToJson() },
            };
            WSClientBase.Instance.Send(jsonNode);
        }


    }
    
}
