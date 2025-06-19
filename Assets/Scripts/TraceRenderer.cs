using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TremorTraceRenderer : MonoBehaviour
{
    private TremorSceneManager tremorSceneManager;

    // ── anchor points ─────────────────────────────────────────────────────
    private readonly List<Vector3> basePoints = new();
    private List<Vector3> smoothedPoints;

    // ── stroke management ────────────────────────────────────────────────
    private LineRenderer lineRenderer;   // first stroke (root)
    private LineRenderer activeLR;       // stroke currently receiving points
    private readonly List<Vector3> activePts = new();
    private int strokeIndex;
    private int pointIndex;

    // ── config ────────────────────────────────────────────────────────────
    [SerializeField] private float drawRate = 50f;
    [SerializeField] private int subdivisions = 10;
    [SerializeField] private bool isSpiral = true;

    public bool IsDrawing { get; private set; }
    public Color darkPurple = new(0.2f, 0f, 0.2f);

    // ── spiral jitter state ───────────────────────────────────────────────
    private Vector2 jitterCurrent = Vector2.zero;
    private Vector2 jitterTarget = Vector2.zero;

    // ── straight-bar jitter state ─────────────────────────────────────────
    private float barPerpCurrent = 0f;
    private float barPerpTarget = 0f;

    // ──────────────────────────────────────────────────────────────────────
    #region Unity lifecycle
    void Awake()
    {
        tremorSceneManager = GetComponentInParent<TremorSceneManager>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.03f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.sortingOrder = 8;
        lineRenderer.startColor = darkPurple;
        lineRenderer.endColor = darkPurple;

        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == transform) continue;
            basePoints.Add(child.localPosition);
        }
        smoothedPoints = GenerateCatmullRom(basePoints, subdivisions);

        activeLR = lineRenderer;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region Public API
    public void DrawTrace()
    {
        if (IsDrawing) return;
        ResetLineColor();
        StartCoroutine(DrawRoutine());
    }
    #endregion

    // ──────────────────────────────────────────────────────────────────────
    #region Drawing coroutine
    private IEnumerator DrawRoutine()
    {
        IsDrawing = true;

        for (pointIndex = 0; pointIndex < smoothedPoints.Count; pointIndex++)
        {
            switch (tremorSceneManager.GetCurrentTremorLevel())
            {
                case 0: CRSTLvl0(); break;
                case 1: CRSTLvl1(); break;
                case 2: CRSTLvl2(); break;
                case 3: CRSTLvl3(); break;
                case 4: CRSTLvl4(); break;
            }
            yield return new WaitForSeconds(1f / drawRate);
        }

        CommitStroke();
        IsDrawing = false;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region Level implementations
    private void CRSTLvl0()
    {
        AddPoint(smoothedPoints[pointIndex]);
    }

    /* ───────────── Lvl-1 ───────────── */
    private void CRSTLvl1()
    {
        if (isSpiral)
        {
            if (pointIndex % 8 == 0 || pointIndex == 0)
                jitterTarget = Random.insideUnitCircle * 0.01f;

            jitterCurrent = Vector2.Lerp(jitterCurrent, jitterTarget, 0.35f);
            AddPoint(smoothedPoints[pointIndex] + (Vector3)jitterCurrent);
        }
        else
        {
            const float AMP = 0.022f;   // visible but mild
            const int STEP = 6;        // choose new offset every 6 pts

            if (pointIndex % STEP == 0 || pointIndex == 0)
                barPerpTarget = Random.Range(-AMP, AMP);

            barPerpCurrent = Mathf.Lerp(barPerpCurrent, barPerpTarget, 0.35f);
            Vector3 p = smoothedPoints[pointIndex] +
                        (Vector3)(UnitNormal(pointIndex) * barPerpCurrent);
            AddPoint(p);
        }
    }

    /* ───────────── Lvl-2 ───────────── */
    private void CRSTLvl2()
    {
        if (isSpiral)
        {
            if (pointIndex % 6 == 0 || pointIndex == 0)
                jitterTarget = Random.insideUnitCircle * 0.017f;

            jitterCurrent = Vector2.Lerp(jitterCurrent, jitterTarget, 0.35f);
            AddPoint(smoothedPoints[pointIndex] + (Vector3)jitterCurrent);
        }
        else
        {
            const float AMP = 0.035f;
            const int STEP = 4;

            if (pointIndex % STEP == 0 || pointIndex == 0)
                barPerpTarget = Random.Range(-AMP, AMP);

            barPerpCurrent = Mathf.Lerp(barPerpCurrent, barPerpTarget, 0.45f);
            Vector3 p = smoothedPoints[pointIndex] +
                        (Vector3)(UnitNormal(pointIndex) * barPerpCurrent);
            AddPoint(p);
        }
    }

    /* ───────────── Lvl-3 ───────────── */
    private void CRSTLvl3()
    {
        if (isSpiral)
        {
            if (pointIndex % 4 == 0 || pointIndex == 0)
                jitterTarget = Random.insideUnitCircle * 0.03f;

            jitterCurrent = Vector2.Lerp(jitterCurrent, jitterTarget, 0.40f);
            AddPoint(smoothedPoints[pointIndex] + (Vector3)jitterCurrent);
        }
        else
        {
            // occasional pen-lift
            if (Random.value < 0.07f)
            {
                if (activePts.Count > 0) CommitStroke();
                StartNewStroke();
                barPerpCurrent = barPerpTarget = 0f;
                return;
            }

            const float AMP = 0.055f;
            const int STEP = 3;

            if (pointIndex % STEP == 0 || pointIndex == 0)
                barPerpTarget = Random.Range(-AMP, AMP);

            barPerpCurrent = Mathf.Lerp(barPerpCurrent, barPerpTarget, 0.55f);
            Vector3 p = smoothedPoints[pointIndex] +
                        (Vector3)(UnitNormal(pointIndex) * barPerpCurrent);
            AddPoint(p);
        }
    }

    /* ───────────── Lvl-4  (unchanged) ───────────── */
    /* ───────────── Lvl-4 ───────────── */
    private void CRSTLvl4()
    {
        /* ----------  SPIRAL  (unchanged) ---------- */
        if (isSpiral)
        {
            if (Random.value < 0.07f)
            {
                if (activePts.Count > 0) CommitStroke();
                StartNewStroke();
                jitterCurrent = jitterTarget = Vector2.zero;
                return;
            }

            if (pointIndex % 8 == 0 || pointIndex == 0)
                jitterTarget = Random.insideUnitCircle * 0.04f;

            jitterCurrent = Vector2.Lerp(jitterCurrent, jitterTarget, 0.40f);
            AddPoint(smoothedPoints[pointIndex] + (Vector3)jitterCurrent);
        }

        /* ----------  STRAIGHT BAR  (new) ---------- */
        else
        {
            // keep the same 7 % pen-lift probability
            if (Random.value < 0.07f)
            {
                if (activePts.Count > 0) CommitStroke();
                StartNewStroke();
                barPerpCurrent = barPerpTarget = 0f;   // reset for new stroke
                return;
            }

            const float AMP = 0.08f;   // ← stronger sideways wobble
            const int STEP = 2;       // ← re-target every 2 points (very jittery)

            if (pointIndex % STEP == 0 || pointIndex == 0)
                barPerpTarget = Random.Range(-AMP, AMP);

            barPerpCurrent = Mathf.Lerp(barPerpCurrent, barPerpTarget, 0.65f); // quick chase
            Vector3 p = smoothedPoints[pointIndex] +
                        (Vector3)(UnitNormal(pointIndex) * barPerpCurrent);    // pure perpendicular
            AddPoint(p);
        }
    }

    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region Helpers
    private Vector2 UnitNormal(int idx)
    {
        Vector3 tan;
        if (idx == 0) tan = smoothedPoints[1] - smoothedPoints[0];
        else if (idx == smoothedPoints.Count - 1) tan = smoothedPoints[idx] - smoothedPoints[idx - 1];
        else tan = smoothedPoints[idx + 1] - smoothedPoints[idx - 1];

        tan.Normalize();
        return new Vector2(-tan.y, tan.x);   // 2-D left/right normal
    }

    private void AddPoint(Vector3 p)
    {
        activePts.Add(p);
        activeLR.positionCount = activePts.Count;
        activeLR.SetPositions(activePts.ToArray());
    }

    private void CommitStroke()
    {
        if (activePts.Count == 0) return;
        activeLR.positionCount = activePts.Count;
        activeLR.SetPositions(activePts.ToArray());
        activePts.Clear();
    }

    private void StartNewStroke()
    {
        activeLR = CreateNewStrokeSegment().GetComponent<LineRenderer>();
        activePts.Clear();
    }

    private void ResetLineColor()
    {
        Color purple = new(0.2f, 0f, 0.2f, 1f);
        foreach (LineRenderer lr in GetComponentsInChildren<LineRenderer>())
            lr.startColor = lr.endColor = purple;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region LineRenderer setup
    private GameObject CreateNewStrokeSegment()
    {
        GameObject go = new($"Stroke_{strokeIndex++}");
        go.transform.SetParent(transform, false);

        var lr = go.AddComponent<LineRenderer>();
        CopyLineRendererSettings(lineRenderer, lr);
        return go;
    }

    private static void CopyLineRendererSettings(LineRenderer src, LineRenderer dst)
    {
        dst.material = new Material(src.material);
        dst.widthMultiplier = src.widthMultiplier;
        dst.numCapVertices = src.numCapVertices;
        dst.numCornerVertices = src.numCornerVertices;
        dst.alignment = src.alignment;
        dst.textureMode = src.textureMode;
        dst.startColor = src.startColor;
        dst.endColor = src.endColor;
        dst.colorGradient = src.colorGradient;
        dst.widthCurve = src.widthCurve;
        dst.sortingLayerID = src.sortingLayerID;
        dst.sortingOrder = src.sortingOrder;
        dst.useWorldSpace = src.useWorldSpace;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region Catmull-Rom
    private static List<Vector3> GenerateCatmullRom(List<Vector3> pts, int sub)
    {
        List<Vector3> res = new();
        for (int i = 0; i < pts.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? pts[i] : pts[i - 1];
            Vector3 p1 = pts[i];
            Vector3 p2 = pts[i + 1];
            Vector3 p3 = (i + 2 < pts.Count) ? pts[i + 2] : p2;

            for (int j = 0; j < sub; j++)
            {
                float t = j / (float)sub;
                Vector3 pos = 0.5f *
                    (2f * p1 +
                     (-p0 + p2) * t +
                     (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                     (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
                res.Add(pos);
            }
        }
        res.Add(pts[^1]);
        return res;
    }
    #endregion
    // ──────────────────────────────────────────────────────────────────────

    #region Fade-out
    public IEnumerator FadeOutAndClear(float fadeTime = 1.5f)
    {
        foreach (var lr in GetComponentsInChildren<LineRenderer>())
            StartCoroutine(FadeSingle(lr, fadeTime));
        yield return new WaitForSeconds(fadeTime);
    }

    private static IEnumerator FadeSingle(LineRenderer lr, float fadeTime)
    {
        float a0 = lr.startColor.a, t = 0f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(a0, 0f, t / fadeTime);
            Color c = new(0.2f, 0f, 0.2f, a);
            lr.startColor = lr.endColor = c;
            yield return null;
        }
        lr.positionCount = 0;
    }
    #endregion
}
