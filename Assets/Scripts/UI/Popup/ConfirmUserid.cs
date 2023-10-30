using Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUserid : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button deleteButton;
    [SerializeField] TextMeshProUGUI messageNoti;

    // Start is called before the first frame update
    void Start()
    {
        deleteButton.onClick.AddListener(DeleteButtonOnClink);
    }
    private void OnDestroy()
    {
        deleteButton.onClick.RemoveListener(DeleteButtonOnClink);
    }

    private void DeleteButtonOnClink()
    {
        if(string.IsNullOrEmpty(inputField.text))
        {
            messageNoti.SetText("Please fill your userid");
        }
        else if(IsUserIdOfPlayer(inputField.text))
        {
            messageNoti.SetText("");
            HTTPClientAuth.DeleteAccount();
        }
        else
        {
            messageNoti.SetText("User id isn't correct!");
        }
    }

    private bool IsUserIdOfPlayer(string userid)
    {
        return DataAuth.AuthData.userId.Equals(int.Parse(userid));
    }
}
