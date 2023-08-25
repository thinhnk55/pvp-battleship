using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVE : MonoBehaviour
{
    int stagesCount;
    PDataUnit<int> currentStages;

    [SerializeField]PVEStageCollection stagesView;

    private void Awake()
    {
        currentStages = new PDataUnit<int>(-1);
        currentStages.OnDataChanged += stagesView.OnStageChange;
        currentStages.Data = 0;
    }
}
