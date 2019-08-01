using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BoxColliderUpdater : MonoBehaviour
{
    private RectTransform RectTransform;
    private BoxCollider2D Collider;

    public bool update = false;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        UpdateBounds();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Application.isEditor && !Application.isPlaying)
        {
            UpdateBounds();
        }
#endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (update)
        {
            UpdateBounds();
        }
    }

    public void UpdateBounds()
    {
        Collider.size = new Vector2(RectTransform.rect.width, RectTransform.rect.height);

        Collider.offset = RectTransform.rect.center;
    }
}
