using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawCRST : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float drawDuration = 5f;
    public int subdivisions = 10; // more = smoother

    private List<Vector3> basePoints = new List<Vector3>();
    private List<Vector3> smoothedPoints = new List<Vector3>();

    void Start()
    {
        // Get original points
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == transform) continue;
            basePoints.Add(child.localPosition);
        }

        // Generate smooth points
        smoothedPoints = GenerateCatmullRom(basePoints, subdivisions);

        // Start animated draw
        lineRenderer.positionCount = 0;
        StartCoroutine(DrawLine());
    }

    IEnumerator DrawLine()
    {
        int total = smoothedPoints.Count;
        float delay = drawDuration / total;

        for (int i = 0; i < total; i++)
        {
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, smoothedPoints[i]);
            yield return new WaitForSeconds(delay);
        }
    }

    List<Vector3> GenerateCatmullRom(List<Vector3> points, int subdivisions)
    {
        List<Vector3> result = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i + 2 < points.Count ? points[i + 2] : p2;

            for (int j = 0; j < subdivisions; j++)
            {
                float t = j / (float)subdivisions;
                Vector3 pos = 0.5f * (
                    2f * p1 +
                    (-p0 + p2) * t +
                    (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                    (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
                );
                result.Add(pos);
            }
        }

        result.Add(points[points.Count - 1]); // last point
        return result;
    }
}
