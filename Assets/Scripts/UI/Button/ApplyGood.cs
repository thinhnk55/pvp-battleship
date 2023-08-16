using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGood : ButtonBase
{
    GoodCard good;
    [SerializeField]GameObject content;
    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        good = content.transform.GetChild(0).GetComponent<GoodCard>();
        if(good && good.Info.Type.GetPResourceType() == PResourceType.Nonconsumable)
        {
            switch ( (PNonConsumableType)good.Info.Type)
            {
                case PNonConsumableType.AVATAR:
                    WSClient.ChangeAvatar((int)good.Info.Value);
                    break;
                case PNonConsumableType.AVATAR_FRAME:
                    WSClient.ChangeFrame((int)good.Info.Value);
                    break;
                case PNonConsumableType.BATTLE_FIELD:
                    WSClient.ChangeBattleField((int)good.Info.Value);
                    break;
                default:
                    break;

            }
        }
    }
}
