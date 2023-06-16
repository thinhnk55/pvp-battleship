using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : CacheMonoBehaviour
{
    public float rotSpeed;
    private void OnEnable()
    {
        MusicType.RADAR.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        EulerAngles = EulerAngles + rotSpeed * Vector3.forward * Time.deltaTime;
    }
    private void OnDisable()
    {
        AudioHelper.StopMusic();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Ship")
        {
            Color coloe;
            ColorUtility.TryParseHtmlString("#ff3e3e", out coloe);
            collision.GetComponent<Image>().color = coloe;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Ship")
        {
            Color coloe;
            ColorUtility.TryParseHtmlString("#e6e6e6", out coloe);
            collision.GetComponent<Image>().color = coloe;
        }
    }
}
