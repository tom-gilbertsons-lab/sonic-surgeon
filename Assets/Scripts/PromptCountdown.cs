using UnityEngine;
using System.Collections;
using TMPro;

public class PromptCountdown : MonoBehaviour
{
   
    public IEnumerator PromptOpener()
    {
        string[] countdown = { "3", "2", "1", "GO" };
        foreach (string step in countdown)
        {
            StartCoroutine(AnimatePrompt(step));
            yield return new WaitForSeconds(1f);
        }
    }


    private IEnumerator AnimatePrompt(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
        GetComponent<TextMeshProUGUI>().transform.localScale = Vector3.zero;
        GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1)*5f; // fully opaque

        float t = 0;
        float duration = 0.3f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = (t / duration)*10f;
            GetComponent<TextMeshProUGUI>().transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.5f, progress);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f); // hold the big text

        // fade out
        t = 0;
        duration = 0.5f;
        Color startColor = GetComponent<TextMeshProUGUI>().color;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            GetComponent<TextMeshProUGUI>().color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
    }

}
