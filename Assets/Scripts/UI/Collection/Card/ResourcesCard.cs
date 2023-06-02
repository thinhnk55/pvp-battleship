using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI beri;
    [SerializeField] TextMeshProUGUI diamond;
    // Start is called before the first frame update
    void Start()
    {
        beri.text = PResourceType.Beri.GetValue().ToString();
        diamond.text = PResourceType.Diamond.GetValue().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
