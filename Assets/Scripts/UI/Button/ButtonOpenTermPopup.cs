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
    public static Action<TermType> OnOpenTermsPopup;
    private String header;
    private String body;
    [SerializeField] TermType termType;

    public void Start()
    {
        header = termType == TermType.PRIVATE_POLICY ? "Private Policy" : "User Agreement";
        body = termType == TermType.PRIVATE_POLICY ? GameConfig.PrivatePolicy : GameConfig.UserAgreement;

        OnOpenTermsPopup += OpenTermsPopup;
    }


    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        NavigateToWebsite(this.termType);
    }

    private void OpenTermsPopup(TermType termType)
    {
        if (this.termType != termType) { return; }

        if (termType == TermType.PRIVATE_POLICY)
        {
            PopupHelper.CreateConfirm(PrefabFactory.PopupPrivacyPolicy, null, null, null, (confirm) =>
            {
                int index = termType == TermType.PRIVATE_POLICY ? 0 : 1;

                GameData.AcceptLoginTerm[index] = confirm ? true : false;
                OnUpdateTerm();
            });
        }
        else
        {
            PopupHelper.CreateConfirm(PrefabFactory.PopupUserTerms, null, null, null, (confirm) =>
            {
                int index = termType == TermType.PRIVATE_POLICY ? 0 : 1;

                GameData.AcceptLoginTerm[index] = confirm ? true : false;
                OnUpdateTerm();
            });
        }
    }

    private void NavigateToWebsite(TermType termType)
    {
        if (this.termType != termType) { return; }

        if (termType == TermType.PRIVATE_POLICY)
        {
            Application.OpenURL("https://meepogames.com/privacy-policy");
        }
        else
        {
            Application.OpenURL("https://meepogames.com/terms-and-conditions");
        }
    }


    public enum TermType
    {
        PRIVATE_POLICY,
        USER_AGREEMENT
    }
}
