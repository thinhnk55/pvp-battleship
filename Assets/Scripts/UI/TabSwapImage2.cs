using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwapImage2 : TabSwapImage
{
    protected override void ActiveTab(int i)
    {
        base.ActiveTab(i);
        if (i == 1)
        {
            tabs[i].gameObject.SetChildrenRecursively<RectTransform>((rect) =>
            {
                rect.ScaleByX(-1);
            });
        }
    }
    protected override void InactiveTab(int i)
    {
        base.InactiveTab(i);
        if (i == 0)
        {
            tabs[i].gameObject.SetChildrenRecursively<RectTransform>((rect) =>
            {
                rect.ScaleByX(-1);
            });
        }
    }
}
