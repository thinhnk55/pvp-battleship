using Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LossPVEPopup : MonoBehaviour
{
    [SerializeField] Button _btnM_Yes; // Khi khong xem duoc ads thi chi hien moi button nay
    [SerializeField] Button _btnR_Yes;
    [SerializeField] Button _btnL_No;
    [SerializeField] Button _btnRewind;
    [SerializeField] PopupConfirm popupConfirm;

    private void Start()
    {
        _btnM_Yes.onClick.AddListener(OnMidButtonYesClick);
        _btnRewind.onClick.AddListener(NewGame);

        SetUpButton();
    }


    private void OnDestroy()
    {
        _btnM_Yes.onClick.RemoveListener(OnMidButtonYesClick);
        _btnRewind.onClick.RemoveListener(NewGame);
    }

    private void NewGame()
    {
        PVE.Instance.RemoveListener();
        PVE.Instance.NewGameTreasure();
        popupConfirm.ForceClose();
    }

    private void SetUpButton()
    {
        if (PVE.Instance.IsRevived)
        {
            _btnM_Yes.gameObject.SetActive(true);
            _btnR_Yes.gameObject.SetActive(false);
            _btnL_No.gameObject.SetActive(false);
        }
    }

    public void OnMidButtonYesClick()
    {
        popupConfirm.OnConfirm.Invoke(true);
    }

}
