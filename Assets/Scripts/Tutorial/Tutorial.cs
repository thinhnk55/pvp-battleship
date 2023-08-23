using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : SingletonMono<Tutorial>
{
    [SerializeField] GameObject homeTutorial;
    [SerializeField] GameObject betTutorial;
    [SerializeField] GameObject formationTutorial;
    [SerializeField] GameObject playTutorial;
    void Start()
    {
        switch (SceneTransitionHelper._instance.ESceneValue)
        {
            case ESceneName.PreHome:
                break;
            case ESceneName.Home:
                break;
            case ESceneName.Bet:
                break;
            case ESceneName.MainGame:
                break;
            case ESceneName.TreasureHunt:
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
