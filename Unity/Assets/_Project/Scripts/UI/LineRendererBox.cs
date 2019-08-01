using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LineRendererBox : MonoBehaviour
{
    private RectTransform RectTransform;
    private LineRenderer LineRenderer;

    public bool update = false;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        LineRenderer = GetComponent<LineRenderer>();

        LineRenderer.positionCount = 4;
    }

    void Start()
    {
        DrawLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            DrawLine();
        }
    }

    public void DrawLine ()
    {
        Vector3[] vectors = new Vector3[4];
        RectTransform.GetLocalCorners(vectors);

        for (int i = 0; i < vectors.Length; i++)
        {
            LineRenderer.SetPosition(i, vectors[i]);
        }
    }
}
