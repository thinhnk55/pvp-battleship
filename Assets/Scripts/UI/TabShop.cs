using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabShop : Tabs
{
    protected override void ActiveTab()
    {
        base.ActiveTab();
        Color color = buttons[activeIndex].GetComponent<Image>().color;
        color.a = 1;
        buttons[activeIndex].GetComponent<Image>().color = color;
    }

    protected override void InactiveTab()
    {
        base.InactiveTab();
        Color color = buttons[activeIndex].GetComponent<Image>().color;
        color.a = 0;
        buttons[activeIndex].GetComponent<Image>().color = color;
    }
}
