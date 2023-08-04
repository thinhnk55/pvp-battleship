using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZ : CacheMonoBehaviour
{
    [SerializeField] float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        EulerAngles  -= rotSpeed * Vector3.forward * Time.deltaTime;
    }
}
