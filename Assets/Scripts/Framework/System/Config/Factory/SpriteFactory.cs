using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SpriteFactory : SingletonScriptableObject<SpriteFactory>
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            if (_instance == null)
            {
                Instance.ToString();
            }
        }
        [SerializeField] private Sprite octile; public static Sprite Octile { get { return Instance.octile; } }

        [SerializeField] private Sprite occupied; public static Sprite Occupied { get { return Instance.occupied; } }
        [SerializeField] private Sprite unoccupiable; public static Sprite Unoccupiable { get { return Instance.unoccupiable; } }
        [SerializeField] private Sprite attacked; public static Sprite Attacked { get { return Instance.attacked; } }
        [SerializeField] private Sprite destroyed; public static Sprite Destroyed { get { return Instance.destroyed; } }
        [SerializeField] private Sprite playerTurn; public static Sprite PlayerTurn { get { return Instance.playerTurn; } }
        [SerializeField] private Sprite opponentTurn; public static Sprite OpponentTurn { get { return Instance.opponentTurn; } }

    }

}