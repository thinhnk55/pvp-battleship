using Framework;
using Server;
using SimpleJSON;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
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
        if (!data["promotion"].IsNull)
        {
            transactionInfo.Bonus = (int)(data["promotion"].AsFloat * 100);
        }

        List<float> cost_value = new List<float>();
        List<int> cost_type = new List<int>();
        List<int> product_value = new List<int>();
        List<int> product_type = new List<int>();
        switch ((TransactionType)id)
        {
            case TransactionType.starter:
                cost_type.Add(0);
                cost_value.Add(data["price"].AsFloat);
                for (int i = 0; i < data["code"].Count; i++)
                {
                    product_value.Add(data["code"][i].AsInt);
                }
                for (int i = 0; i < data["product"].Count; i++)
                {
                    product_type.Add(data["product"][i].AsInt);
                }
                break;
            case TransactionType.usd:
#if UNITY_ANDROID || UNITY_STANDALONE
                if (data["platform"].AsInt == 0 || data["platform"].AsInt == 2)
                {
                    cost_type.Add(0);
                    cost_value.Add(data["price"].AsFloat);
                    product_type.Add((int)PConsumableType.GEM);
                    product_value.Add(data["quantity"].AsInt);
                }
#elif UNITY_IOS
                    if (data["platform"].AsInt == 0 || data["platform"].AsInt == 1)
                    {
                        cost_type.Add(0);
                        cost_value.Add(data["price"].AsFloat);
                        product_type.Add((int)PConsumableType.GEM);
                        product_value.Add(data["quantity"].AsInt);
                    }
#endif
                break;

            case TransactionType.diamond:
                cost_type.Add((int)PConsumableType.GEM);
                cost_value.Add(data["price"].AsInt);
                product_type.Add((int)PConsumableType.BERRY);
                product_value.Add(data["quantity"].AsInt);
                break;
            case TransactionType.gold_avatar:
                cost_type.Add((int)PConsumableType.BERRY);
                cost_value.Add(data["price"].AsInt);
                product_type.Add((int)PNonConsumableType.AVATAR);
                product_value.Add(data["avatar"].AsInt);
                break;
            case TransactionType.gold_frame:
                cost_type.Add((int)PConsumableType.BERRY);
                cost_value.Add(data["price"].AsInt);
                product_type.Add((int)PNonConsumableType.AVATAR_FRAME);
                product_value.Add(data["avatar_frame"].AsInt);
                break;
            case TransactionType.gold_battlefield:
                cost_type.Add((int)PConsumableType.BERRY);
                cost_value.Add(data["price"].AsInt);
                product_type.Add((int)PNonConsumableType.BATTLE_FIELD);
                product_value.Add(data["battlefield"].AsInt);
                break;
            case TransactionType.gold_skinship:
                break;
            case TransactionType.elite:
                cost_type.Add(0);
                cost_value.Add(data["price"].AsFloat);
                product_value.Add(1);
                product_type.Add((int)PNonConsumableType.ELITE);
                break;
            default:
                break;
        }
        transactionInfo.Cost = new GoodInfo[cost_value.Count];
        transactionInfo.Product = new GoodInfo[product_value.Count];
        for (int i = 0; i < cost_value.Count; i++)
        {
            transactionInfo.Cost[i] = new GoodInfo()
            {
                Value = cost_value[i],
                Type = cost_type[i],
            };
        }
        for (int i = 0; i < product_type.Count; i++)
        {
            transactionInfo.Product[i] = new GoodInfo()
            {
                Value = product_value[i],
                Type = product_type[i],
            };
        }
        return transactionInfo;
    }
    public static List<TransactionInfo> ListFromJson(JSONNode data, int id)
    {
        List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        for (int i = 0; i < data.Count; i++)
        {
            var info = FromJson(data[i], id, transactionInfos.Count);
            if (info.Cost.Length > 0)
            {
                transactionInfos.Add(info);
            }
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
            IAP.ConfirmPendingPurchase();
        }
        for (int i = 0; i < Product.Length; i++)
        {
            var product = Product[i];
            product.Type.Transact((int)product.Value);
        }
        if (Product.Length == 1 && Product[0].Type.GetPResourceType() == PResourceType.Nonconsumable)
        {
            PopupHelper.CreateGoods(PrefabFactory.PopupGoodApply, "You have received", Product.ToList());
        }
        else
        {
            PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", Product.ToList());
        }
        if (Product.Length > 1)
        {
            //PopupHelper.CreateGoods(PrefabFactory.PopupGood, "You have received", Product.ToList());

        }
        else
        {
            //PopupHelper.CreateMessage(PrefabFactory.PopupMessage, "Message" , "You have received", SpriteFactory.ResourceIcons[Product[0].Type].sprites.GetLoop(Index));
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
    [SerializeField] protected TextMeshProUGUI title;

    [SerializeField] protected Image[] productIcon;
    [SerializeField] protected TextMeshProUGUI[] productAmount;

    [SerializeField] protected Image[] costIcon;
    [SerializeField] protected TextMeshProUGUI[] costAmount;

    [SerializeField] protected TextMeshProUGUI bonus;
    [SerializeField] protected TextMeshProUGUI status;
    [SerializeField] protected Image otherIcon;
    [SerializeField] protected TextMeshProUGUI otherText;
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
            productAmount[i].text = ((long)info.Product[i].Value).ToString();
        }
        if (status)
        {
            status.text = GetStatus(info);
        }
        if (info.Cost[0].Value >= 0)
        {
            if (info.Cost[0].Type > 0)
            {
                for (int i = 0; i < costIcon.Length; i++)
                {
                    if (info.Cost[i].Type.GetPResourceType() == PResourceType.Consumable)
                    {
                        costIcon[i].sprite = SpriteFactory.ResourceIcons[info.Cost[i].Type].sprites.GetClamp(0);
                    }
                    else
                    {
                        costIcon[i].sprite = SpriteFactory.ResourceIcons[info.Cost[i].Type].sprites.GetClamp((int)info.Cost[i].Value);
                    }
                }
                for (int i = 0; i < costAmount.Length; i++)
                {
                    if (info.Cost[i].Value == (int)info.Cost[i].Value)
                    {
                        costAmount[i].text = GetStringNumber((long)info.Cost[i].Value);
                    }
                    else
                    {
                        costAmount[i].text = info.Cost[i].Value.ToString("F2");
                    }
                }
            }
            else
            {
                for (int i = 0; i < costAmount.Length; i++)
                {
                    if (info.Product.Length == 1)
                    {
                        costAmount[i].text = IAP.GetProductPriceFromStore(ApplicationConfig.BundleId + "." + ((PConsumableType)info.Product[0].Type).ToString().ToLower() + "." + info.Index);
                    }
                    else
                    {
                        costAmount[i].text = IAP.GetProductPriceFromStore(ApplicationConfig.BundleId + "." + ((TransactionType)info.TransactionType).ToString().ToLower());
                    }
                }
                for (int i = 0; i < costIcon.Length; i++)
                {
                    DestroyImmediate(costIcon[i].gameObject);
                }
            }


            if (bonus)
            {
                if (info.Bonus > 0)
                {
                    bonus.text = "Bonus " + info.Bonus.ToString() + "%";
                }
                else
                {
                    bonus.transform.parent.gameObject.SetActive(false);
                }
            }

            if (Button)
            {
                if (info.Cost[0].Type != 0)
                {
                    Button.onClick.AddListener(() =>
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupConfirm, "CONFIRM", "Do you want to buy this item?", productIcon[0].sprite, (confirm) =>
                        {
                            if (confirm)
                            {
                                TransactionAction(info.TransactionType, info)?.Invoke();
                                Collection?.UpdateUIs();
                            }
                        });
                    }
                    );
                }
                else
                {
                    if (info.Product.Length == 1 && info.TransactionType != TransactionType.elite)
                    {
                        Button.onClick.AddListener(() =>
                        {
                            IAP.PurchaseProduct($"{ApplicationConfig.BundleId}.{((PConsumableType)info.Product[0].Type).ToString().ToLower()}.{info.Index}", (success, product) =>
                            {
                                if (success)
                                {
                                    TransactionAction(info.TransactionType, info, product)?.Invoke();
                                }
                            });
                        });
                    }
                    else
                    {
                        Button.onClick.AddListener(() =>
                        {
                            IAP.PurchaseProduct($"{ApplicationConfig.BundleId}.{info.TransactionType.ToString().ToLower()}", (success, product) =>
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

        }
        else
        {
            for (int i = 0; i < costAmount.Length; i++)
            {
                costAmount[i].text = "Royal Pass";
                DestroyImmediate(costIcon[i].gameObject);
                costAmount[i].GetComponentInParent<LayoutCalibrator>().Calibrate();
            }
            otherIcon?.SetAlpha(1);
            if (info.Cost[0].Value == -1)
            {
                otherIcon?.SetSprite(SpriteFactory.ResourceIcons[(int)PNonConsumableType.ELITE].sprites[0]);
                otherText?.SetText("RP");
            }
            else if (true)
            {
                otherIcon?.SetSprite(SpriteFactory.ResourceIcons[(int)PNonConsumableType.ELITE].sprites[0]);
                otherText?.SetText("Starter");
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
        if (Info.Cost[0].Type == 0)
        {
            action = () =>
            {
#if UNITY_ANDROID
                RequestTransaction((int)Info.TransactionType, Info.Index, product.receipt.Replace("\\", "").Replace("\"{", "{").Replace("}\"", "}"), product);
#elif UNITY_IOS
                RequestTransaction((int)Info.TransactionType, Info.Index, new JSONClass() { { "TransactionID", product.transactionID }, { "Store", "AppleAppStore" } }.ToString(), product);
#endif
            };
        }
        else
        {
            action = () =>
            {
                if (Info.IsAffordble())
                {
                    RequestTransaction((int)Info.TransactionType, Info.Index);
                }
                else
                {
                    if (Info.Cost[0].Type == (int)PConsumableType.GEM)
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupOutOfResource, "Message", "Not enough money", null, (ok) =>
                        {
                            if (ok)
                            {
                                PopupBehaviour.CloseAll();
                                PopupHelper.Create(PrefabFactory.PopupShop).GetComponentInChildren<Tabs>().Activate(2);
                            }
                        });
                    }
                    else
                    {
                        PopupHelper.CreateConfirm(PrefabFactory.PopupOutOfResource, "Message", "Not enough money", null, (ok) =>
                        {
                            if (ok)
                            {
                                PopupBehaviour.CloseAll();
                                PopupHelper.Create(PrefabFactory.PopupShop).GetComponentInChildren<Tabs>().Activate(1);
                            }
                        });
                    }

                }

            };
        }
        return action;
    }
    public static void RequestTransaction(int id, int index, string data = null, Product product = null)
    {
        JSONNode jsonNode = new JSONClass()
            {
                { "id", ServerResponse._TRANSACTION.ToJson() },
                { "s", id.ToJson() },
                { "p", index.ToJson() },
            };
        if (data != null && data!= "")
        {
            jsonNode.Add("r", JSON.Parse(data));
        }

        if (product != null)
            IAP.Instance.pendingProducts.TryAdd(product, jsonNode);
        WSClient.Instance.Send(jsonNode);
    }

    public static string GetStringNumber(long number)
    {
        string s = "";
        if (number < 1000)
        {
            s = (number).ToString();
        }
        else if (number / 1000 < 1000)
        {
            s = (number / 1000).ToString() + "K";
        }
        else if (number / 1000000 < 1000)
        {
            s = (number / 1000000).ToString() + "M";
        }
        else if (number / 1000000000 < 1000)
        {
            s = (number / 1000000000).ToString() + "B";
        }
        return s;
    }
}


