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

        public override void BuildUI(GoodInfo info)
        {
            base.BuildUI(info);
            value.text = info.Value.ToString();
            if (info.Type.GetPResourceType() == PResourceType.Nonconsumable)
            {
                icon.sprite = SpriteFactory.ResourceIcons[info.Type].sprites.GetLoop((int)info.Value);
            }
            else
            {
                icon.sprite = SpriteFactory.ResourceIcons[info.Type].sprites[3];
            }
        }

        protected override void OnClicked(GoodInfo info)
        {

        }
    }
}
