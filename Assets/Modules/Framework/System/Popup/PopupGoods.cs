using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

namespace Framework
{
    public class PopupGoods : PopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _txtContent;
        public GoodCollection view;
        public virtual void Construct(string msg, List<GoodInfo> goodInfos)
        {
            if (_txtContent)
            {
                _txtContent.text = msg;
            }
            view.BuildUIs(goodInfos);
            view.SetLayout(Mathf.Clamp(500 - goodInfos.Count * 100, 180, 500));
        }

    }

}
