using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class TabLuckyShot : Tabs
    {
        public Sprite tabDefault;
        public Sprite tabHighLight;
        public Vector2 defaultSize;
        public Vector2 highlightSize;

        protected override void Awake()
        {
            base.Awake();
            defaultSize = new Vector2(160, 135);
            highlightSize = new Vector2(200, 180);
            foreach (var tab in tabs)
            {
                RectTransform rectTransform =  tab.GetComponent<RectTransform>();
                rectTransform.sizeDelta = defaultSize;
                rectTransform.SetAnchoredPositionX(-(defaultSize.x / 2));
            }
            tabs[0].GetComponent<RectTransform>().sizeDelta = highlightSize;
            tabs[0].GetComponent<RectTransform>().SetAnchoredPositionX(-(highlightSize.x / 2));
        }

        protected override void ActiveTab(int i)
        {
            base.ActiveTab(i);
            RectTransform rectTransform = tabs[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = highlightSize;
            rectTransform.SetAnchoredPositionX(-(highlightSize.x/2));
            tabs[i].GetComponent<Image>().sprite = tabHighLight;
        }

        protected override void InactiveTab(int i)
        {
            if (i < 0)
                return;
            base.InactiveTab(i);
            RectTransform rectTransform = tabs[i].GetComponent<RectTransform>();
            rectTransform.sizeDelta = defaultSize;
            rectTransform.SetAnchoredPositionX(-(defaultSize.x/2));
            tabs[i].GetComponent<Image>().sprite = tabDefault;

        }
    }
}