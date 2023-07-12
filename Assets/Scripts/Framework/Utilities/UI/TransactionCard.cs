using Framework;
using SimpleJSON;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
namespace Framework {
    public struct GoodInfo
    {
        public float Value;
        public int Type;
    }

    public struct TransactionInfo
    {
        public int Index;
        public TransactionType TransactionType;
        public string Title;
        public GoodInfo[] Cost;
        public GoodInfo[] Product;
        public int Bonus;
        public static TransactionInfo FromJson(JSONNode data, int id, int index)
        {
            TransactionInfo transactionInfo = new TransactionInfo();
            transactionInfo.TransactionType = (TransactionType)id;
            transactionInfo.Index = index;
            transactionInfo.Cost = new GoodInfo[data["cost"].Count];
            transactionInfo.Product = new GoodInfo[data["product"].Count];
            transactionInfo.Bonus = (int)(float.Parse(data["bonus"]) * 100);
            for (int i = 0; i < data["cost"].Count; i++)
            {
                transactionInfo.Cost[i] = new GoodInfo()
                {
                    Value = float.Parse(data["cost"][i]),
                    Type = int.Parse(data["cost_type"][i]),
                };
            }
            for (int i = 0; i < data["product"].Count; i++)
            {
                transactionInfo.Product[i] = new GoodInfo()
                {
                    Value = float.Parse(data["product"][i]),
                    Type = int.Parse(data["product_type"][i]),
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
            if (Cost[0].Type != 0)
            {
                for (int i = 0; i < Cost.Length; i++)
                {
                    var cost = Cost[i];
                    cost.Type.Transact(-(int)cost.Value);
                }
            }
            else
            {
                IAP.Instance.ConfirmPendingPurchase();
            }
            for (int i = 0; i < Product.Length; i++)
            {
                var product = Product[i];
                product.Type.Transact((int)product.Value);
            }
            PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "You have received", null);
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
        [SerializeField] protected TextMeshProUGUI title;

        [SerializeField] protected Image[] productIcon;
        [SerializeField] protected TextMeshProUGUI[] productAmount;

        [SerializeField] protected Image[] costIcon;
        [SerializeField] protected TextMeshProUGUI[] costAmount;

        [SerializeField] protected TextMeshProUGUI bonus;
        [SerializeField] protected TextMeshProUGUI status;

        public override void BuildUI(TransactionInfo info)
        {
            base.BuildUI(info);
            if (title)
                title.text = info.Title;
            for (int i = 0; i < productIcon.Length; i++)
            {
                productIcon[i].sprite = SpriteFactory.ResourceIcons[(int)info.Product[i].Type].sprites.GetLoop((int)info.Product[i].Value);
            }
            for (int i = 0; i < productAmount.Length; i++)
            {
                productAmount[i].text = info.Product[i].Value.ToString();
            }
            if (status)
            {
                status.text = GetStatus(info);
            }
            if (info.Cost[0].Type >= 0)
            {
                for (int i = 0; i < costIcon.Length; i++)
                {
                    costIcon[i].sprite = SpriteFactory.ResourceIcons[(int)info.Cost[i].Type].sprites.GetLoop((int)info.Cost[i].Value);
                }
                for (int i = 0; i < costAmount.Length; i++)
                {
                    if (info.Cost[i].Value == (int)info.Cost[i].Value)
                    {
                        costAmount[i].text = info.Cost[i].Value.ToString("F0");
                    }
                    else
                    {
                        costAmount[i].text = info.Cost[i].Value.ToString("F2");
                    }
                }

                if (bonus)
                {
                    bonus.text = "Bonus " + info.Bonus.ToString() + "%";
                }

                if (Button)
                {
                    if (info.Cost[0].Type != 0)
                    {
                        Button.onClick.AddListener(() =>
                        {
                            PopupHelper.CreateConfirm(PrefabFactory.PopupConfirm, "Buy?", "Buy?", (confirm) => {
                                if (confirm)
                                {
                                    TransactionAction(info.TransactionType, info)?.Invoke();
                                    Collection.UpdateUIs();
                                }
                            });
                        }
                        );
                    }
                    else
                    {
                        Button.onClick.AddListener(() =>
                        {
                            IAP.Instance.PurchaseProduct($"{ApplicationConfig.BundleId}.{((PConsumableType)info.Product[0].Type).ToString().ToLower()}.{info.Product[0].Value}", (success, product) =>
                            {
                                if (success)
                                {
                                    TransactionAction(info.TransactionType, info, product)?.Invoke();
                                }
                            });
                        });
                    }

                }

            }
            else
            {
                for (int i = 0; i < costAmount.Length; i++)
                {
                    costAmount[i].text = "Royal Pass";
                }
            }



        }

        protected override void OnClicked(TransactionInfo info)
        {
            throw new System.NotImplementedException();
        }
        protected virtual string GetStatus(TransactionInfo info)
        {
            return "";
        }
        public static UnityAction TransactionAction(TransactionType transactionType, TransactionInfo Info, Product product = null)
        {
            UnityAction action = null;
            action += () => { };
            if (transactionType == TransactionType.USD_GEM)
            {
                action = () =>
                {
                    RequestTransactionMoney(JSON.Parse(product.receipt));
                };
            }
            else
            {
                action = () => {
                    if (Info.IsAffordble())
                    {
                        RequestTransaction((int)Info.TransactionType, Info.Index);
                    }
                    else
                    {
                        Debug.Log("unaffordable");
                    }

                };
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

        public static void RequestTransactionMoney(JSONNode data)
        {
            JSONNode jsonNode = new JSONClass()
            {
                { "id", GameServerEvent.REQUEST_TRANSACTION_MONEY.ToJson() },
                { "data", data},
            };
            WSClientBase.Instance.Send(jsonNode);
        }
    }
    
}
