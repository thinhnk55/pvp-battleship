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
        [SerializeField] GameObject _icon;
        [SerializeField] GameObject _iconContainer;
        [SerializeField] GoodCollection view;

        public void Construct(string msg, List<GoodInfo> goodInfos, List<GameObject> gameObjects)
        {
            if (_txtContent)
            {
                _txtContent.text = msg;
            }
            DestroyImmediate(_icon);
            _icon = Instantiate(_icon, _iconContainer.transform);
            view.BuildUIs(goodInfos);
        }

    }

}
