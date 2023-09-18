using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class Tabs : CacheMonoBehaviour
    {
        protected List<Button> tabs;
        [SerializeField] protected GameObject rootContent;
        protected List<GameObject> contents;
        protected GameObject activeContent;
        protected PDataUnit<int> activeIndex;
        protected virtual void Awake()
        {
            tabs = GetComponentsInChildren<Button>().ToList();
            for (int i = 0; i < tabs.Count; i++)
            {
                int _i = i;
                tabs[i].onClick.AddListener(() =>
                {
                    activeIndex.Data = _i;
                });
                InactiveTab(_i);
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

        protected virtual void InactiveTab(int i)
        {
            activeContent?.SetActive(false);
        }
        protected virtual void ActiveTab(int i)
        {
            activeContent = contents[i];
            activeContent.SetActive(true);
        }
        public void Activate(int i)
        {
            activeIndex.Data = i;
        }
    }
}
