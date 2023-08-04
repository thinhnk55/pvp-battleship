using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOpenTermPopup : ButtonBase
{
/*    [SerializeField] Text Header;
    [SerializeField] Text Body;*/
    public static Action OnUpdateTerm;
    public static Action<TermType> OnOpenTermPopup;
    private String header;
    private String body;
    [SerializeField] TermType termType;

    public void Start()
    {
        header = termType == TermType.PRIVATE_POLICY ? "Private Policy" : "User Agreement";
        body = termType == TermType.PRIVATE_POLICY ? GameConfig.PrivatePolicy : GameConfig.UserAgreement;

        OnOpenTermPopup += OpenTermsPopup;
    }


    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        OpenTermsPopup(this.termType);
    }

    private void OpenTermsPopup(TermType termType)
    {
        if(this.termType != termType) { return; }

        PopupHelper.CreateConfirm(PrefabFactory.PopupUserTerms, header, body, null, (confirm) => {
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
