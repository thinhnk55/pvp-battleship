using Authentication;
using System;
using System.Collections;
using System.Collections.Generic;
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

    private void DeleteButtonOnClink()
    {
        if(IsUserIdOfPlayer(inputField.text))
        {
            messageNoti.SetText("");
            HTTPClientAuth.DeleteAccount();
        }
        else
        {
            messageNoti.SetText("User id isn't correct!!!");
        }
    }

    private bool IsUserIdOfPlayer(string userid)
    {
        return DataAuth.AuthData.userId.Equals(int.Parse(userid));
    }
}
