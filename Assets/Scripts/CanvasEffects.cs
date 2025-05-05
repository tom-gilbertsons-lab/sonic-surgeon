using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CanvasEffects : MonoBehaviour
{
    public IEnumerator TypeText(TextMeshProUGUI textElement, string fullText, float letterDelay = 0.05f)
    {
        textElement.text = "";
        foreach (char c in fullText)
        {
            textElement.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    public IEnumerator FadeText(TextMeshProUGUI tmp, float startA, float endA, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, endA, t / dur);
            var c = tmp.color;
            c.a = a;
            tmp.color = c;
            yield return null;
        }
        var final = tmp.color;
        final.a = endA;
        tmp.color = final;
    }

    public IEnumerator FadeImage(Image img, float startA, float endA, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, endA, t / dur);
            var c = img.color;
            c.a = a;
            img.color = c;
            yield return null;
        }
        var final = img.color;
        final.a = endA;
        img.color = final;
    }
}
