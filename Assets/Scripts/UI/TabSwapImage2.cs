using Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabSwapImage2 : TabSwapImage
{
    protected override void Awake()
    {
        tabs = GetComponentsInChildren<Button>().ToList();
        for (int i = 0; i < tabs.Count; i++)
        {
            int _i = i;
            tabs[i].onClick.AddListener(() =>
            {
                activeIndex.Data = _i;
            });
        }
        contents = new List<GameObject>();
        for (int i = 0; i < rootContent.transform.childCount; i++)
        {
            contents.Add(rootContent.transform.GetChild(i).gameObject);
            contents[i].SetActive(false);
        }
        activeIndex = new PDataUnit<int>(-1);
        activeIndex.OnDataChanged += (oldIndex, newIndex) =>
        {
            InactiveTab(oldIndex);
            ActiveTab(newIndex);
        };
        Activate(0);
    }
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
