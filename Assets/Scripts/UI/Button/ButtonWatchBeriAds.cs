using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWatchBeriAds : MonoBehaviour
{
    [SerializeField] Text BeriBonusAmount;
    // Start is called before the first frame update
    void Start()
    {
        BeriBonusAmount.text += GameData.BeriBonusAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
