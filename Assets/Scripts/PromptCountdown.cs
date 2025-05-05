using UnityEngine;
using System.Collections;
using TMPro;

public class PromptCountdown : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator PromptOpener()
    {
        string[] countdown = { "3", "2", "1", "GO!" };
        foreach (string step in countdown)
        {
            yield return StartCoroutine(AnimatePrompt(step));
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator AnimatePrompt(string text)
    {
        tmp.text = text;
        tmp.transform.localScale = Vector3.zero;
        tmp.color = new Color(1f, 1f, 1f, 1f); // fully opaque, no bloom weirdness

        float duration = 0.3f;
        float t = 0f;

        // Scale up
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            tmp.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.5f, progress);
            yield return null;
        }

        // Hold big text
        yield return new WaitForSeconds(0.3f);

        // Fade out
        t = 0f;
        duration = 0.5f;
        Color startColor = tmp.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        tmp.text = "";
        tmp.transform.localScale = Vector3.one; // reset scale
    }
}
