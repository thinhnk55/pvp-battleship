using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAutoFire : ButtonBase
{
    [SerializeField] GameObject TurnOnObj;
    [SerializeField] GameObject TurnOffObj;
    public bool TurnOn;
    // Start is called before the first frame update
    void Start()
    {
        SetUpButton(false);
    }

    private void SetUpButton(bool turnOn)
    {
        TurnOffObj.SetActive(!turnOn);
        TurnOnObj.SetActive(turnOn);
    }

    public void ButtonOnclick()
    {
        TurnOn = !TurnOn;
        SetUpButton(TurnOn);
        CoreGame.Instance.IsAutoFireMode = TurnOn;
    }

}
