using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfirmPopup : MonoBehaviour
{
    [SerializeField] Button _btnM_Yes; // Khi khong xem duoc ads thi chi hien moi button nay
    [SerializeField] Button _btnR_Yes;
    [SerializeField] Button _btnL_No;
    [SerializeField] PopupConfirm popupConfirm;

    private void Start()
    {
        _btnM_Yes.onClick.AddListener(OnMidButtonYesClick);
        if(PVE.Instance.IsRevived)
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
