using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Framework
{
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
                icon.SetNativeRatioFixedHeight();
                value?.SetText("+1");
                BG?.SetSprite(SpriteFactory.RoyalPassOther);
            }
            else
            {
                value.text = ((long)info.Value).ToString();
                icon.sprite = SpriteFactory.ResourceIcons[info.Type].sprites[3];
                if (info.Type == (int)PConsumableType.GEM)
                {
                    BG?.SetSprite(SpriteFactory.RoyalPassGem);
                }
                else if (info.Type == (int)PConsumableType.BERRY)
                {
                    BG?.SetSprite(SpriteFactory.RoyalPassBeri);
                }
            }
        }

        protected override void OnClicked(GoodInfo info)
        {

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

}
