using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct RoyalPassInfo
{
    public int Id;
    public bool Unlocked;
    public GoodInfo[] Reward;
    public bool Obtained;
    public bool Elite;
    public Callback<RoyalPassInfo> Obtain;
}
public class RoyalPassCard : CardBase<RoyalPassInfo>
{
    [SerializeField] Image BG;
    [SerializeField] Image OrderFill;
    [SerializeField] Image OrderBG;
    [SerializeField] Image unlocked;
    [SerializeField] Image obtained;
    [SerializeField] TextMeshProUGUI Id;
    [SerializeField] TextMeshProUGUI Number;
    [SerializeField] Image Icon;
    [SerializeField] Image Container;
    protected override void OnClicked(RoyalPassInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void BuildUI(RoyalPassInfo info)
    {
        base.BuildUI(info);
        BG?.SetSprite(info.Id == GameData.RoyalPass.Level ? SpriteFactory.RoyalPassMilestone : BG.sprite);
        if (true)
        {
            //Container.gameObject.SetChildrenRecursively<Image>((img) => { img.color = Color.gray; });
            if (!info.Unlocked)
            {
                unlocked.color = Color.white;
            }
            if (info.Obtained)
            {
                obtained.color = Color.white;
            }
        }
        OrderFill?.SetAlpha(info.Id > GameData.RoyalPass.Level ? 0 : 1);
        unlocked?.SetAlpha(info.Unlocked ? 0 : 1);
        obtained?.SetAlpha(info.Obtained ? 1 : 0);

        Id?.SetText(info.Id.ToString());

        if (info.Reward.Length > 1)
        {
            Icon.sprite = SpriteFactory.RoyalPassTreasure;
        }
        else if (info.Reward.Length == 1)
        {
            if (info.Reward[0].Type.GetPResourceType() == PResourceType.Consumable)
            {
                Number?.SetText(info.Reward[0].Value.ToString());
            }
            else
            {
                Number?.SetText("");
            }
            if (Icon)
            {
                if (info.Reward[0].Type.GetPResourceType() == PResourceType.Consumable)
                {
                    if (info.Reward[0].Type == (int)PConsumableType.GEM)
                    {
                        Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites[Mathf.Clamp(((int)info.Reward[0].Value) / 200, 1, SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.Length - 2)];
                        Container?.SetSprite(SpriteFactory.RoyalPassGem);
                    }
                    else if (info.Reward[0].Type == (int)PConsumableType.BERRY)
                    {
                        Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites[Mathf.Clamp(((int)info.Reward[0].Value) / 200, 1, SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.Length - 2)];
                        Container?.SetSprite(SpriteFactory.RoyalPassBeri);
                    }
                }
                else
                {
                    if (info.Reward[0].Type == (int)PNonConsumableType.AVATAR || info.Reward[0].Type == (int)PNonConsumableType.AVATAR_FRAME)
                    {
                        var rect = Icon.GetComponent<RectTransform>();
                        rect.SetWidth(rect.sizeDelta.y * 0.8f);
                    }
                    Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.GetLoop((int)info.Reward[0].Value);
                    Container?.SetSprite(SpriteFactory.RoyalPassOther);
                }
            }
        }
        else if (info.Reward.Length == 0)
        {
            Number?.SetText("");
            if (Button)
            {
                Button.gameObject.SetChildrenRecursively<Image>((image) => { image.SetAlpha(0); });
            }
        }
        if (Button && info.Unlocked && !info.Obtained)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => info.Obtain?.Invoke(info));
        }
    }
}
