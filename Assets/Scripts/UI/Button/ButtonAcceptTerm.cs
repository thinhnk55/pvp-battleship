using Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAcceptTerm : ButtonBase
{
    [SerializeField] TermType buttonType;
    [SerializeField] GameObject image;
    // Start is called before the first frame update
    protected override void Awake()
    {
        ButtonOpenTermPopup.OnUpdateTerm += GetStatusFromData;

        base.Awake();
        GetStatusFromData();
    }

    private void OnDestroy()
    {
        ButtonOpenTermPopup.OnUpdateTerm -= GetStatusFromData;
    }

    public void GetStatusFromData()
    {
        if (buttonType == TermType.PRIVATE_POLICY)
        {
            image.SetActive(GameData.AcceptLoginTerm[0]);
        }
        else
        {
            image.SetActive(GameData.AcceptLoginTerm[1]);
        }
    }

    protected override void Button_OnClicked()
    {
        base.Button_OnClicked();

        if(buttonType == TermType.PRIVATE_POLICY)
        {
            GameData.AcceptLoginTerm[0] = !GameData.AcceptLoginTerm[0];
            image.SetActive(GameData.AcceptLoginTerm[0]);
        }
        else
        {
            GameData.AcceptLoginTerm[1] = !GameData.AcceptLoginTerm[1];
            image.SetActive(GameData.AcceptLoginTerm[1]);
        }
    }

    public enum TermType
    {
        PRIVATE_POLICY,
        USER_AGREEMENT
    }

}
