using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class Tabs : CacheMonoBehaviour
    {
        protected List<Button> buttons;
        [SerializeField] protected GameObject rootContent;
        protected List<GameObject> contents;
        protected GameObject activeContent;
        protected int activeIndex;
        private void Awake()
        {
            contents = new List<GameObject>();
            buttons = GetComponentsInChildren<Button>().ToList();
            for (int i = 0; i < rootContent.transform.childCount; i++)
            {
                contents.Add(rootContent.transform.GetChild(i).gameObject);
                contents[i].SetActive(false);
            }
            activeContent = contents[0];
            activeContent.SetActive(true);
            for (int i = 0; i < buttons.Count; i++)
            {
                int _i = i;
                buttons[i].onClick.AddListener(() =>
                {
                    InactiveTab();
                    activeIndex = _i;
                    ActiveTab();
                });
            }
        }

        protected virtual void InactiveTab()
        {
            activeContent.SetActive(false);
        }
        protected virtual void ActiveTab()
        {
            activeContent = contents[activeIndex];
            activeContent.SetActive(true);
        }
    }
}
