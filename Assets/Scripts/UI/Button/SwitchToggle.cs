using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] Image handleImage;
    [SerializeField] Sprite offHandleSprite;
    [SerializeField] Sprite onHandleSprite;

    private Toggle toggle;
    private RectTransform handleRectTransform;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handleRectTransform = handleImage.GetComponent<RectTransform>();

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnSwitch);
            if (toggle.isOn)
                OnSwitch(true);
        }
    }


    void OnSwitch(bool isOn)
    {
        if (handleImage != null && onHandleSprite != null && offHandleSprite != null)
            handleImage.sprite = isOn ? onHandleSprite : offHandleSprite;
        if (handleRectTransform != null)
        {
            handleRectTransform.DOAnchorPos(isOn ? handleRectTransform.anchoredPosition * -1 : handleRectTransform.anchoredPosition, .4f).SetEase(Ease.InOutBack);
        }
    }

    private void OnDestroy()
    {
        if (toggle != null) toggle.onValueChanged.RemoveListener(OnSwitch);
    }

}
