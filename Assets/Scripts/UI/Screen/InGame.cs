using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI opponentUsername;
    private void Awake()
    {
        username.text = GameData.Player.Username;
    }
}
