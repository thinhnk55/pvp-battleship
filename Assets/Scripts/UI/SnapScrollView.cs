using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrollView : MonoBehaviour
{
    public float snapSpeed = 10f;  // The speed at which the scrolling snaps to a position
    public float snapThresholdMax = 100f;  // The distance threshold for snapping to a position
    public float snapThresholdMin = 30;  // The distance threshold for snapping to a position

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

    public void Init()
    {
        if (scrollViewMiddle != null)
        {
            scrollViewMiddle.position = scrollViewMiddle.parent.position;
            SnapPointXPos = scrollViewMiddle.anchoredPosition.x;
        }
        else
            SnapPointXPos = Screen.width / 2;

        //snapThreshold = Screen.width * 3 / 5;
        snapThresholdMax = SnapPointXPos * 6 / 5;
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
        childRects = new RectTransform[contentRect.childCount];
        for (int i = 0; i < contentRect.childCount; i++)
        {
            childRects[i] = contentRect.GetChild(i).GetComponent<RectTransform>();
        }
        contentRect.anchoredPosition = childRects[0].anchoredPosition * Vector2.right;
        layoutGroup.padding.left = (int)SnapPointXPos;
        layoutGroup.padding.right = (int)SnapPointXPos;
        LeanTouch.OnFingerDown += OnSelected;
        LeanTouch.OnFingerUp += OnUnselected;
        closestPosition = float.MaxValue;
        isSnapping = false;
    }
    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= OnSelected;
        LeanTouch.OnFingerUp -= OnUnselected;
    }
    private void Update()
    {
        // Check if scrolling is in progress
        isScrolling = Mathf.Abs(scrollRect.velocity.x) > Mathf.Abs(snapSpeed * closestPosition / 8);
        // Check if scrolling has ended or user interaction stopped
        if (!isDragging)
        {
            closestPosition = float.MaxValue;
            float currentScrollPos = contentRect.anchoredPosition.x;
            foreach (RectTransform childRect in childRects)
            {
                float childPos = childRect.anchoredPosition.x;
                float distance = Mathf.Abs(childPos + currentScrollPos - SnapPointXPos);
                if (distance < Mathf.Abs(closestPosition))
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
            if (!isScrolling)
            {
                // Find the closest snap position when not scrolling and dragging
                // Snap to the closest position if it's within the snap threshold
                Debug.Log(closestPosition);
                if (Mathf.Abs(closestPosition) < snapThresholdMax && Mathf.Abs(closestPosition) > snapThresholdMin)
                {
                    isSnapping = true;
                    scrollRect.velocity = new Vector2(-closestPosition * snapSpeed, 0f);
                }
            }
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

    public void SetToChildPosition(int childIndex)
    {
        if (childIndex < 0) childIndex = 0;
        if (childIndex >= childRects.Length) childIndex = childRects.Length - 1;
        // Calculate the target normalized position based on the child's position
        float targetNormalizedPos = (childIndex + 1f) / (childRects.Length + 1);
        Debug.Log(targetNormalizedPos);
        // Set the scroll position to the target normalized position
        isDragging = true;
        scrollRect.horizontalNormalizedPosition = targetNormalizedPos - 0.05f;
    }
}