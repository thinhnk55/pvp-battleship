using Framework;
using System.Collections;
using System.Collections.Generic;
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
        if (!info.Unlocked || info.Obtained)
        {
            gameObject.SetChildrenRecursively<Image>((img) => { img.color = Color.gray; });
            if (!info.Unlocked)
            {
                unlocked.color = Color.white;
            }
            if (info.Obtained)
            {
                obtained.color = Color.white;
            }
        }
        unlocked?.SetAlpha(info.Unlocked ? 0 : 1);
        obtained?.SetAlpha(info.Obtained ? 1 : 0);

        Id?.SetText(info.Id.ToString());
        if (Container)
        {
            Container.sprite = info.Elite ? SpriteFactory.RoyalPassEliteBG : SpriteFactory.RoyalPassFreeBG;
        }
        if (info.Reward.Length>1)
        {
            Icon.sprite = SpriteFactory.RoyalPassTreasure;
        }
        else if (info.Reward.Length == 1)
        {
            if (info.Reward[0].Type.GetPResourceType()== PResourceType.Consumable)
            {
                Number?.SetText(info.Reward[0].Value.ToString());
            }
            else
            {
                Number?.SetText("");
            }
            if (Icon)
            {
                if (info.Reward[0].Type.GetPResourceType() == PResourceType.Consumable )
                {
                    if (info.Reward[0].Type == (int)PConsumableType.GEM)
                    {
                        Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.GetClamp(((int)info.Reward[0].Value) / 2);
                    }
                    else if (info.Reward[0].Type == (int)PConsumableType.BERI)
                    {
                        Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.GetClamp(((int)info.Reward[0].Value) / 200);
                    }
                }
                else
                {
                    Icon.sprite = SpriteFactory.ResourceIcons[info.Reward[0].Type].sprites.GetLoop((int)info.Reward[0].Value);
                }
            }
        }
        else if (info.Reward.Length == 0)
        {
            Icon.gameObject.SetActive(false);
        }
        if (Button && info.Unlocked && !info.Obtained)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(()=> info.Obtain?.Invoke(info));
        }
    }
}
