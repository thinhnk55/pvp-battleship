using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEStageCollection : CardCollectionBase<int>
{
    public override void UpdateUIs()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < PVEData.StageCount; i++)
        {
            list.Add(i);
        }

    }
}
