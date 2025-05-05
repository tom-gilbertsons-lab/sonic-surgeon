using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TreatProgress : MonoBehaviour
{
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;

    // From yellow to red
    public Color lowColor = new Color(1f, 1f, 0.3f, 0.1f);
    public Color highColor = new Color(2f, 0f, 0f, 1.0f);

    private void Awake()
    {
        ResetAll();
    }

    public void ShowProgress(int count)
    {

        stage3.SetActive(count == 24);
        stage4.SetActive(count == 24);

        // Stage 1 (1–11)
        if (count >= 1)
        {
            float t = Mathf.Clamp01((count - 1) / 10f);
            Color target = Color.Lerp(lowColor, highColor, t);
            ApplyColor(stage1, target);
        }

        // Stage 2 (12–23)
        if (count >= 12)
        {
            float t = Mathf.Clamp01((count - 12) / 11f);
            Color target = Color.Lerp(lowColor, highColor, t);
            ApplyColor(stage2, target);
        }
    }

    private void ApplyColor(GameObject obj, Color targetColor)
    {
        if (!obj.activeSelf)
        {
            obj.SetActive(true);
            // Start from transparent version of the target colour
            Image img = obj.GetComponent<Image>();
            if (img != null)
            {
                Color transparent = targetColor;
                transparent.a = 0f;
                img.color = transparent;
            }
        }

        StartCoroutine(FadeToColor(obj, targetColor, 0.2f));
    }

    private IEnumerator FadeToColor(GameObject obj, Color targetColor, float duration)
    {
        Image img = obj.GetComponent<Image>();
        if (img == null) yield break;

        Color startColor = img.color;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            img.color = Color.Lerp(startColor, targetColor, t / duration);
            yield return null;
        }

        img.color = targetColor;
    }

    public void ResetAll()
    {
        stage1.SetActive(false);
        stage2.SetActive(false);
        stage3.SetActive(false);
        stage4.SetActive(false);
    }
}
