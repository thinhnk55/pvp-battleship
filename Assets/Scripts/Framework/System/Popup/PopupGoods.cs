using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class PopupGoods : PopupBehaviour
    {
        [Header("Reference")]
        [SerializeField] TextMeshProUGUI _txtContent;
        [SerializeField] Image _imgContent;
        [SerializeField] CardCollectionBase<GoodInfo> view;

        public void Construct(string msg, List<GoodInfo> goodInfos)
        {
            _txtContent.text = msg;
            view.BuildUIs(goodInfos);
        }

    }

}
