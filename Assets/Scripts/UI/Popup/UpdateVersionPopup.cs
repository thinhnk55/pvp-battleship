using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateVersionPopup : MonoBehaviour
{
    [SerializeField] Button _btnM_Yes; // Force Update
    [SerializeField] Button _btnR_Yes;
    [SerializeField] Button _btnL_No;

    [SerializeField] PopupBehaviour popupConfirm;
    // Start is called before the first frame update
    void Start()
    {
        SetUpButton();
        _btnM_Yes.onClick.AddListener(OnButtonYesClick);
        _btnR_Yes.onClick.AddListener(OnButtonYesClick);
        _btnL_No.onClick.AddListener(OnButtonCloseClick);
    }

    private void OnDestroy()
    {
        _btnM_Yes.onClick.RemoveListener(OnButtonYesClick);
        _btnR_Yes.onClick.RemoveListener(OnButtonYesClick);
        _btnL_No.onClick.RemoveListener(OnButtonCloseClick);
    }

    private void SetUpButton()
    {
        if (SystemMaintenance.Instance.forceUpdate)
        {
            PGameMaster.ClearData();
            _btnM_Yes.gameObject.SetActive(true);
            _btnR_Yes.gameObject.SetActive(false);
            _btnL_No.gameObject.SetActive(false);
        }
    }

    public void OnButtonYesClick()
    {
        Application.OpenURL(SystemMaintenance.Instance.linkGame);
    }

    public void OnButtonCloseClick()
    {
        SceneManager.LoadScene("Loading");
    }
}
