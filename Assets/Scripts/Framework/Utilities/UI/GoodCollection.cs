using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

namespace Framework
{
    public class GoodCollection : CardCollectionBase<GoodInfo>
    {
        public override void UpdateUIs()
        {
            
        }

        public override void BuildUIs(List<GoodInfo> infos)
        {
            base.BuildUIs(infos);

        }

        public void SetLayout()
        {
            var grid = contentRoot.GetComponent<GridLayoutGroup>();
            float size = Mathf.Clamp(500 - cards.Count * 160, 180, 500);
            grid.cellSize = new Vector2(size, size);
        }
    }
}

