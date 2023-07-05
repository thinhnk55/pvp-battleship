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
    private Vector2 handlePos;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        handleRectTransform = handleImage.GetComponent<RectTransform>();
        handlePos = handleRectTransform.anchoredPosition;

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
            handleRectTransform.DOAnchorPos(isOn ? handlePos * -1 : handlePos, .4f).SetEase(Ease.InOutBack);
        }
    }

    private void OnDestroy()
    {
        if (toggle != null) toggle.onValueChanged.RemoveListener(OnSwitch);
    }

}
