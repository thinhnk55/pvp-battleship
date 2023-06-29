using Framework;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class ChangeName : SingletonMono<ChangeName>
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI messageTxt;
    [SerializeField] PopupBehaviour popup;
    void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (IsValid(inputField.text, out string message))
            {
                WSClient.RequestChangeName(inputField.text);
                messageTxt.text = message;
            }
            else
            {
                messageTxt.text = message;
            }
        });
        inputField.onSelect.AddListener((text) => { inputField.placeholder.GetComponent<TextMeshProUGUI>().text = ""; });
        ServerMessenger.AddListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_NAME, ReceiveChangeName);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServerMessenger.RemoveListener<JSONNode>(GameServerEvent.RECIEVE_CHANGE_NAME, ReceiveChangeName);
    }
    public static void ReceiveChangeName(JSONNode json)
    {
        if (json["n"] == GameData.Player.Username.Data)
        {
            Instance.messageTxt.text = "This Name already in used";
        }
        GameData.Player.Username.Data = json["n"];

    }

    bool IsValid(string text, out string message)
    {
        if (string.IsNullOrEmpty(text))
        {
            message = "Please fill your name";
            return false;
        }
        else if (text.Length > 16 || text.Length <3)
        {
            message = "Name too short or too long";
            return false;
        }
        else if (text == GameData.Player.Username.Data)
        {
            message = "This is your name";
            return false;
        }
        message = "";
        return true;
    }
}
