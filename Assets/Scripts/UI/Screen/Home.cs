using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Home : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI username;
    private void Start()
    {
        username.text = GameData.Player.Username.Data;
    }
}
