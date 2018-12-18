using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MoveLineRenderer : MonoBehaviour
{
    LineRenderer lineRenderer;

    List<Transform> vertexTransforms = new List<Transform>();

    //=====================================================
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        DrawLineUpdate();
    }
    //-----------------------------------------------------
    //  Public
    //-----------------------------------------------------
    public void AddVertex(Transform obj)
    {
        vertexTransforms.Add(obj.transform);
        lineRenderer.positionCount++;

        DrawLineUpdate();
    }
    public void RemoveVertex(Transform obj)
    {
        if (lineRenderer.positionCount <= 0) return;

        vertexTransforms.Remove(obj.transform);
        lineRenderer.positionCount--;

        DrawLineUpdate();
    }
    //-----------------------------------------------------
    //  Private
    //-----------------------------------------------------
    void DrawLineUpdate()
    {
        for (int i = 0; i < vertexTransforms.Count; i++)
        {
            lineRenderer.SetPosition(i, vertexTransforms[i].position);
        }
    }
}