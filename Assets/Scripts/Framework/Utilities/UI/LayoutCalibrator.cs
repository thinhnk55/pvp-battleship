using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LayoutCalibrator : MonoBehaviour
{
    [SerializeField] float padding;
    [SerializeField] List<RectTransform> rects;
    [SerializeField] List<TextMeshProUGUI> texts;
    void Start()
    {
        float width = 0;
        for (int i = 0; i < rects.Count; i++)
        {
            width += rects[0].sizeDelta.x;
        }
        GetComponent<RectTransform>().anchoredPosition = new Vector2(-width, 0);
        for (int i = 0; i < texts.Count; i++)
        {
            width += texts[i].preferredWidth;
        }
        GetComponent<RectTransform>().SetWidth(width + padding);

    }

    void Update()
    {
        
    }
}
