using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject pointedObject = eventData.pointerCurrentRaycast.gameObject;

        // Check if it has a Button component
        Button button = pointedObject.GetComponent<Button>();
        if (button != null)
        {
            // The currently pointed object is a button
            SoundType.CLICK.PlaySound();
            Debug.Log("Clicked on a button!");
        }
        else
        {
            // The currently pointed object is not a button
            Debug.Log("Clicked on something else.");
        }
    }

}
