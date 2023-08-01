using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public struct GoodInfo
    {
        public float Value;
        public int Type;
    }
    public class GoodCard : CardBase<GoodInfo>
    {
        [SerializeField] TextMeshProUGUI value;
        [SerializeField] Image icon;
        [SerializeField] Image BG;

        public override void BuildUI(GoodInfo info)
        {
            base.BuildUI(info);
            if (info.Type.GetPResourceType() == PResourceType.Nonconsumable)
            {
                icon.sprite = SpriteFactory.ResourceIcons[info.Type].sprites.GetLoop((int)info.Value);
                if (info.Type == (int)PNonConsumableType.AVATAR || info.Type == (int)PNonConsumableType.AVATAR_FRAME)
                {
                    var rect = icon.GetComponent<RectTransform>();
                    rect.SetWidth(rect.sizeDelta.y * 0.8f);
                }
                BG?.SetSprite(SpriteFactory.RoyalPassOther);
            }
            else
            {
                value.text = info.Value.ToString();
                icon.sprite = SpriteFactory.ResourceIcons[info.Type].sprites[3];
                if (info.Type == (int)PConsumableType.GEM)
                {
                    BG?.SetSprite(SpriteFactory.RoyalPassGem);
                }
                else if (info.Type == (int)PConsumableType.BERI)
                {
                    BG?.SetSprite(SpriteFactory.RoyalPassBeri);
                }
            }
        }

        protected override void OnClicked(GoodInfo info)
        {

        }
    }
}
