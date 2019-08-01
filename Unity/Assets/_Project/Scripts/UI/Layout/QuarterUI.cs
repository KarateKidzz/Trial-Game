using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the content's width and height to half the parent's size. UI must stretch on the X and Y to work. Use pivot to control corner
/// </summary>
[ExecuteInEditMode]
public class QuarterUI : MonoBehaviour
{
    RectTransform RectTransform;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float fullWidth = RectTransform.rect.width + -RectTransform.sizeDelta.x;
        float fullHeight = RectTransform.rect.height + -RectTransform.sizeDelta.y;

        float hDelta = (fullWidth / 2) - fullWidth;
        float vDelta = (fullHeight / 2) - fullHeight;

        RectTransform.sizeDelta = new Vector2(hDelta, vDelta);
        RectTransform.anchoredPosition = new Vector2(0, 0);

    }
}
