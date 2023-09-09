using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirAsMovement : CacheMonoBehaviour
{
    Vector3 previousPos;
    Vector3 currentPos;
    void Start()
    {
        previousPos = Position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = Position;
        EulerAngles = new Vector3(0, 0, Mathf.Atan2((currentPos - previousPos).y, (currentPos - previousPos).x)) * Mathf.Rad2Deg;
        previousPos = Position;
    }
}
