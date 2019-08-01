using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BoxWidth
{
    /// <summary>
    /// The default width of the box. This is the width of the box at the start of the game
    /// </summary>
    Wide,
    /// <summary>
    /// The width reajusts to the height of the box
    /// </summary>
    Square,
    /// <summary>
    /// Tells the script setting the widths that custom values should be used and <see cref="Wide"/> and <see cref="Square"/> should be ignored
    /// </summary>
    Custom
}

public class BoxMover : MonoBehaviour
{
    [HideInInspector]
    public RectTransform RectTransform;
    private Coroutine movingCoroutine;

    float startWidth = -1;
    float startHeight = -1;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        StartCoroutine(StartAfterSizing());
    }

    public void SetBoxWidth (BoxWidth width, float seconds, float vSeconds, bool sequential, Action onFinishCallback = null)
    {
        switch (width)
        {
            case BoxWidth.Wide:
                SetBoxWidth(startWidth, startHeight, seconds, vSeconds, sequential, onFinishCallback);
                break;

            case BoxWidth.Square:
                SetBoxWidth(startHeight, startHeight, seconds, vSeconds, sequential, onFinishCallback);
                break;

            case BoxWidth.Custom:
                Debug.LogWarning("The wrong SetBoxWidth method was called");
                break;
        }
    }

    public void SetBoxWidth (float width, float height, float hSeconds, float vSeconds = 0.2f, bool sequential = true, Action onFinishCallback = null)
    {
        if (movingCoroutine != null)
            StopCoroutine(movingCoroutine);
        movingCoroutine = StartCoroutine(MoveBox(width, height, hSeconds, vSeconds, sequential, onFinishCallback));
    }

    IEnumerator StartAfterSizing ()
    {
        yield return null;
        startWidth = RectTransform.rect.width;
        startHeight = RectTransform.rect.height;
        yield return null;
    }

    IEnumerator MoveBox (float width, float height, float hSeconds, float vSeconds, bool sequential, Action onFinishCallback)
    {
        // X

        float originalDelta = RectTransform.sizeDelta.x;

        float fullWidth = RectTransform.rect.width + -RectTransform.sizeDelta.x;

        float delta = width - fullWidth;

        Vector2 currentDelta = Vector2.zero;

        Coroutine yRoutine = null;

        if (!sequential)
        {
            yRoutine = StartCoroutine(MoveY(height, vSeconds));
        }

        for (float i = 0; i <= hSeconds; i += Time.deltaTime)
        {
            currentDelta = RectTransform.sizeDelta;
            currentDelta.x = Mathf.Lerp(originalDelta, delta, i / hSeconds);
            RectTransform.sizeDelta = currentDelta;

            yield return null;
        }

        currentDelta = RectTransform.sizeDelta;
        currentDelta.x = delta;
        RectTransform.sizeDelta = currentDelta;

        // Do it for the Y

        if (sequential)
        {
            yRoutine = StartCoroutine(MoveY(height, vSeconds));
        }

        yield return yRoutine;

        if (onFinishCallback != null)
        {
            onFinishCallback.Invoke();
        }

        yield return null;
    }

    IEnumerator MoveY (float h, float s)
    {
        float originalDelta = RectTransform.sizeDelta.y;

        float fullHeight = RectTransform.rect.height + -RectTransform.sizeDelta.y;

        float delta = h - fullHeight;

        Vector2 currentDelta = Vector2.zero;

        Vector2 currentAnchor = Vector2.zero;

        float currentY = RectTransform.anchoredPosition.y;
        float heightDelta = h - RectTransform.rect.height;
        float yMove = heightDelta / 2;  // We need to + Y position by this much

        for (float i = 0; i <= s; i += Time.deltaTime)
        {
            currentDelta = RectTransform.sizeDelta;
            currentDelta.y = Mathf.Lerp(originalDelta, delta, i / s);
            RectTransform.sizeDelta = currentDelta;


            currentAnchor = RectTransform.anchoredPosition;
            currentAnchor.y = Mathf.Lerp(currentY, currentY + yMove, i / s);
            RectTransform.anchoredPosition = currentAnchor;


            yield return null;
        }

        currentDelta = RectTransform.sizeDelta;
        currentDelta.y = delta;
        RectTransform.sizeDelta = currentDelta;

        yield return null;
    }
}
