using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.PlayerSettings;

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

        public void SetLayout(float size = 200)
        {
            var grid = contentRoot.GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(size, size);
        }
    }
}

