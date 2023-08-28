using Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class ShipPVE : MonoBehaviour
{
    [SerializeField] TextMeshPro pointTxt;
    public PDataUnit<int> point;
    private void Awake()
    {
        point = new PDataUnit<int>(0);
        point.OnDataChanged += (o,n) => pointTxt.text = n.ToString();
    }
    public void BeingAttacked()
    {

    }
}
