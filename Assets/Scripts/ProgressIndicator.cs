using UnityEngine;

public class ProgressIndicator : MonoBehaviour
{
    [Range(0f, 1f)]
    public float progress = 0f;

    public Color colorA = new Color(1f, 1f, 1f, 0f);
    public Color colorB = new Color(1f, 1f, 1f, 1f);

    private Transform[] innerObjects;

    void Start()
    {
        innerObjects = GetComponentsInChildren<Transform>(includeInactive: true);
    }

    void Update()
    {
        if (innerObjects == null || innerObjects.Length == 0)
            return;

        int binSize = 8;
        for (int i = 0; i < innerObjects.Length; i++)
        {
            if (innerObjects[i] == transform) continue; // skip root

            float localProgress = Mathf.Clamp01((progress * innerObjects.Length - i) / binSize);
            Renderer rend = innerObjects[i].GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = Color.Lerp(colorA, colorB, localProgress);
            }
        }
    }
}
