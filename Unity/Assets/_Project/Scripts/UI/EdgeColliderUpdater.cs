using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EdgeColliderUpdater : MonoBehaviour
{
    private RectTransform RectTransform;
    private EdgeCollider2D Collider;

    public bool update = false;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Collider = GetComponent<EdgeCollider2D>();
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
        Vector3[] vectors = new Vector3[4];
        RectTransform.GetLocalCorners(vectors);

        Vector2[] newPoints = new Vector2[5];
        Vector2 dir = TopLeft();

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    dir = TopLeft();
                    break;

                case 1:
                    dir = TopRight();
                    break;

                case 2:
                    dir = BottomRight();
                    break;

                case 3:
                    dir = BottomLeft();
                    break;
            }
            newPoints[i] = (Vector2)vectors[i] + dir * 80;

        }
        newPoints[4] = (Vector2)vectors[0] + BottomLeft() * 80;

        Collider.points = newPoints;
    }

    static Vector2 TopLeft ()
    {
        return new Vector2(-1, 1);
    }

    static Vector2 TopRight()
    {
        return new Vector2(1, 1);
    }

    static Vector2 BottomLeft()
    {
        return new Vector2(-1, -1);
    }

    static Vector2 BottomRight()
    {
        return new Vector2(1, -1);
    }

}
