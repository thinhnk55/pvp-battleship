using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StarterCard : DealCard
{
    private void Awake()
    {
        BuildUI(GameData.TransactionConfigs[TransactionType.starter2][0]);
    }
}
