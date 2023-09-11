using Framework;
using UnityEngine;
using UnityEngine.UI;

public class LossPVEPopup : MonoBehaviour
{
    [SerializeField] Button _btnM_Yes; // Khi khong xem duoc ads thi chi hien moi button nay
    [SerializeField] Button _btnR_Yes;
    [SerializeField] Button _btnL_No;
    [SerializeField] PopupConfirm popupConfirm;

    private void Start()
    {
        _btnM_Yes.onClick.AddListener(OnMidButtonYesClick);
        if (PVE.Instance.IsRevived || PVE.Instance.CurrentStep.Data == 9)
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
