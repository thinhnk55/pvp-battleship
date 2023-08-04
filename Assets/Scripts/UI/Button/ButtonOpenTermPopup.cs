using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOpenTermPopup : ButtonBase
{
/*    [SerializeField] Text Header;
    [SerializeField] Text Body;*/
    [SerializeField] TermType termType;
    public static Action OnUpdateTerm;


    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();
        
        PopupHelper.CreateConfirm(PrefabFactory.PopupUserTerms, "CONFIRM", "Do you want to buy this item?", null, (confirm) => {
            int index = termType == TermType.PRIVATE_POLICY ? 0 : 1;

            GameData.AcceptLoginTerm[index] = confirm ? true : false; 
            OnUpdateTerm();
        });
    }


    public enum TermType
    {
        PRIVATE_POLICY,
        USER_AGREEMENT
    }
}
