using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabColor : Tabs
{
    protected override void ActiveTab(int i)
    {
        base.ActiveTab(i);
        Color color = buttons[i].GetComponent<Image>().color;
        color.a = 1;
        buttons[i].GetComponent<Image>().color = color;
    }

    protected override void InactiveTab(int i)
    {
        if (i<0)
            return;
        base.InactiveTab(i);
        Color color = buttons[i].GetComponent<Image>().color;
        color.a = 0;
        buttons[i].GetComponent<Image>().color = color;
    }
}
