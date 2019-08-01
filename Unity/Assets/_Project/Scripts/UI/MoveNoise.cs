using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNoise : MonoBehaviour
{
    Vector2 startPos;
    RectTransform RectTransform;

    public float moveSpeed = 2;
    public float moveHeight = 50;

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
        startPos = RectTransform.position;
    }

    void Update()
    {
        Vector3 pos = RectTransform.position;

        float newY = Mathf.Sin(Time.time * moveSpeed);

        RectTransform.anchoredPosition = new Vector3(0, newY) * moveHeight;
    }
}
