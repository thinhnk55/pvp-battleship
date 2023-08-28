using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVE : MonoBehaviour
{
     int stagesCount;
    PDataUnit<int> currentStages;

    [SerializeField]PVEStageCollection stagesView;

    [SerializeField] ShipPVE player;
    List<ShipPVE> shipPVEs;

    private void Start()
    {
        currentStages = new PDataUnit<int>(-1);
        currentStages.OnDataChanged += stagesView.OnStageChange;
        currentStages.Data = 0;

        player.point.Data = 10;
        int prefabIndex = 0 ;
        if (currentStages.Data< 4)
        {
            prefabIndex = 0 ;
        }
        else if (currentStages.Data < 7)
        {
            prefabIndex = 1;
        }
        else if (currentStages.Data < 9)
        {
            prefabIndex = 2;
        }
        else if (currentStages.Data < 100)
        {
            prefabIndex = 3;
        }
        ShipPVE ship1 = Instantiate(PrefabFactory.Ships[prefabIndex], new Vector3(2, 2, 0), Quaternion.identity,  transform).GetComponent<ShipPVE>();
        ShipPVE ship2 = Instantiate(PrefabFactory.Ships[prefabIndex], new Vector3(2, 0, 0), Quaternion.identity, transform).GetComponent<ShipPVE>();
        ShipPVE ship3 = Instantiate(PrefabFactory.Ships[prefabIndex], new Vector3(2, -2, 0), Quaternion.identity, transform).GetComponent<ShipPVE>();
        shipPVEs.Add(ship1);
        shipPVEs.Add(ship2);
        shipPVEs.Add(ship3);
    }
}
