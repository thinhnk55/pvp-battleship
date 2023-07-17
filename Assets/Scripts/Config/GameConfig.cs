using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : SingletonScriptableObject<GameConfig>
{
    [SerializeField] private string[] betNames; public static string[] BetNames { get { return Instance.betNames; } set { Instance.betNames = value; } }

}