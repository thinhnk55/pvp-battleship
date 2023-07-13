using Lean.Common;
using Lean.Touch;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrollView : MonoBehaviour
{
    public float snapSpeed = 10f;  // The speed at which the scrolling snaps to a position
    public float snapThreshold = 100f;  // The distance threshold for snapping to a position


    private ScrollRect scrollRect;
    private RectTransform contentRect;
    private RectTransform[] childRects;
    [SerializeField] LayoutGroup layoutGroup;
    private bool isScrolling;
    private bool isDragging;
    private bool isSnapping;
    [SerializeField] private float closestPosition;
    [SerializeField] private RectTransform scrollViewMiddle;
    private static float snapPointXPos;

    public static float SnapPointXPos { get => snapPointXPos; set => snapPointXPos = value; }

    private void Start()
    {
        if (scrollViewMiddle != null)
        {
            scrollViewMiddle.position = scrollViewMiddle.parent.position;
            SnapPointXPos = scrollViewMiddle.anchoredPosition.x;
        }
        else
            SnapPointXPos = Screen.width / 2;

        //snapThreshold = Screen.width * 3 / 5;
        snapThreshold = SnapPointXPos * 6 / 5;
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
        childRects = new RectTransform[contentRect.childCount];
        closestPosition = float.MaxValue;
        for (int i = 0; i < contentRect.childCount; i++)
        {
            childRects[i] = contentRect.GetChild(i).GetComponent<RectTransform>();
        }
        contentRect.anchoredPosition = childRects[0].anchoredPosition * Vector2.right;
        //layoutGroup.padding.left = Screen.width / 2;
        //layoutGroup.padding.right = Screen.width / 2;
        layoutGroup.padding.left = (int) SnapPointXPos;
        layoutGroup.padding.right = (int) SnapPointXPos;
        LeanTouch.OnFingerDown += OnSelected;
        LeanTouch.OnFingerUp += OnUnselected;
    }
    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= OnSelected;
        LeanTouch.OnFingerUp -= OnUnselected;
    }
    private void Update()
    {
        // Check if scrolling is in progress
        isScrolling = Mathf.Abs(scrollRect.velocity.x) > Mathf.Abs(snapSpeed * closestPosition/ 8);

        // Check if scrolling has ended or user interaction stopped
        if (!isDragging)
        {
            closestPosition = float.MaxValue;
            float currentScrollPos = contentRect.anchoredPosition.x;
            foreach (RectTransform childRect in childRects)
            {
                float childPos = childRect.anchoredPosition.x;
                //float distance = Mathf.Abs(childPos + currentScrollPos - Screen.width / 2 );
                float distance = Mathf.Abs(childPos + currentScrollPos - SnapPointXPos);

                if (distance < Mathf.Abs(closestPosition))
                    //closestPosition = childPos + currentScrollPos - Screen.width / 2;
                    closestPosition = childPos + currentScrollPos - SnapPointXPos;
            }

            if (isSnapping)
            {
                if (Mathf.Abs(closestPosition) < 25f)
                {
                    closestPosition = float.MaxValue;
                    isSnapping = false;
                    scrollRect.StopMovement();
                }
            }

        }
        if (!isScrolling)
        {
            if (!isDragging)
            {
                // Find the closest snap position when not scrolling and dragging
                // Snap to the closest position if it's within the snap threshold
                if (Mathf.Abs(closestPosition) < snapThreshold && Mathf.Abs(closestPosition) > 10f)
                {
                    isSnapping = true;
                    scrollRect.velocity = new Vector2(-closestPosition * snapSpeed, 0f);
                }

            }
            /**/
        }


    }

    public void OnSelected(LeanFinger leanFinger)
    {
        isDragging = true;
    }

    public void OnUnselected(LeanFinger leanFinger)
    {
        isDragging = false;
    }
}