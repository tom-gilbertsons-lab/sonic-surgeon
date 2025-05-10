using UnityEngine;
using System.Collections;
using TMPro;

public class PromptCountdown : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    public Material promptMat;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator PromptOpener()
    {
        string[] countdown = { "3", "2", "1", "Go!" };
        foreach (string step in countdown)
        {
            yield return StartCoroutine(AnimatePrompt(step));
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator AnimatePrompt(string text)
    {
        // Use colour from the material
        Color baseColor = promptMat.GetColor("_FaceColor");
        baseColor.a = 1f;

        tmp.text = text;
        tmp.transform.localScale = Vector3.zero;
        tmp.color = baseColor;

        float duration = 0.3f;
        float t = 0f;

        // Smooth scale-up
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            tmp.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.5f, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Fade out using colour alpha (no material edits)
        t = 0f;
        duration = 0.5f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            tmp.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }

        tmp.text = "";
        tmp.transform.localScale = Vector3.one;
    }


}
