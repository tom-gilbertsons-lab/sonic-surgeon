using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawCRST : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float drawDuration = 15f;
    public int subdivisions = 10;
    [Range(0, 4)] public int tremorLevel = 0;

    private List<Vector3> basePoints = new List<Vector3>();
    private List<Vector3> smoothedPoints = new List<Vector3>();

    void Start()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == transform) continue;
            basePoints.Add(child.localPosition);
        }

        smoothedPoints = GenerateCatmullRom(basePoints, subdivisions);

        lineRenderer.positionCount = 0;
        StartCoroutine(DrawTrace());
    }

    IEnumerator DrawTrace()
    {
        List<Vector3> segment = new List<Vector3>();
        float delay = drawDuration / smoothedPoints.Count;

        for (int i = 0; i < smoothedPoints.Count; i++)
        {
            if (ShouldSkipPoint(tremorLevel)) continue;

            Vector3 point = smoothedPoints[i];

            // For Level 4: very rare disconnected scratch marks
            if (tremorLevel == 4)
            {
                if (Random.value < 0.1f)
                {
                    int scratchLength = Random.Range(2, 4);
                    List<Vector3> scratch = new List<Vector3>();
                    for (int j = 0; j < scratchLength && i + j < smoothedPoints.Count; j++)
                    {
                        scratch.Add(smoothedPoints[i + j] + Random.insideUnitSphere * 0.1f);
                    }

                    foreach (var p in scratch)
                    {
                        segment.Add(p);
                        lineRenderer.positionCount = segment.Count;
                        lineRenderer.SetPositions(segment.ToArray());
                        yield return new WaitForSeconds(delay);
                    }

                    segment.Clear();
                    lineRenderer.positionCount = 0;
                    i += scratchLength;
                    continue;
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                    continue;
                }
            }

            // For Level 3: occasional stroke break
            if (tremorLevel == 3 && Random.value < 0.2f)
            {
                lineRenderer.positionCount = segment.Count;
                lineRenderer.SetPositions(segment.ToArray());
                segment.Clear();
                lineRenderer.positionCount = 0;
                yield return new WaitForSeconds(delay);
                continue;
            }

            // All other levels
            Vector3 trembled = ApplyTremorToPoint(point, tremorLevel);
            segment.Add(trembled);
            lineRenderer.positionCount = segment.Count;
            lineRenderer.SetPositions(segment.ToArray());
            yield return new WaitForSeconds(delay);
        }
    }


    Vector3 ApplyTremorToPoint(Vector3 point, int level)
    {
        float r = 0f;

        if (level == 1) r = 0.005f;
        else if (level == 2) r = 0.02f;
        else if (level == 3) r = 0.05f;
        else if (level == 4) r = 0.08f;

        if (r > 0f)
            return point + Random.insideUnitSphere * r;

        return point;
    }

    bool ShouldSkipPoint(int level)
    {
        if (level == 0) return false;

        float skipChance = 0f;
        if (level == 1) skipChance = 0.15f;
        else if (level == 2) skipChance = 0.3f;
        else if (level == 3) skipChance = 0.5f;
        else if (level == 4) skipChance = 0.75f;

        return Random.value < skipChance;
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

        result.Add(points[points.Count - 1]);
        return result;
    }
}

