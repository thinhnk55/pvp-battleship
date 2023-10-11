using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Framework
{
    public class TabAlpha : Tabs
    {
        public float alpha;
        protected override void ActiveTab(int i)
        {
            base.ActiveTab(i);
            Color color = tabs[i].GetComponent<Image>().color;
            color.a = 1;
            tabs[i].GetComponent<Image>().color = color;
        }

        protected override void InactiveTab(int i)
        {
            if (i < 0)
                return;
            base.InactiveTab(i);
            Color color = tabs[i].GetComponent<Image>().color;
            color.a = alpha;
            tabs[i].GetComponent<Image>().color = color;
        }
    }
}