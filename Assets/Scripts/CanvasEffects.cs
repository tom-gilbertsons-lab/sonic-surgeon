using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CanvasEffects : MonoBehaviour
{
    public IEnumerator TypeText(TextMeshProUGUI textElement, string fullText, float letterDelay = 0.3f)
    {
        textElement.text = "";
        foreach (char c in fullText)
        {
            textElement.text += c;
            yield return new WaitForSeconds(letterDelay);
        }
    }

    public IEnumerator TypeTextFixed(TextMeshProUGUI textElement, string fullText, float letterDelay = 0.05f)
    {
        // Set full text using rich text tags to hide it all
        string hiddenText = "";
        foreach (char c in fullText)
            hiddenText += $"<alpha=#00>{c}";

        textElement.text = hiddenText;

        for (int i = 0; i < fullText.Length; i++)
        {
            // Reveal one character at a time
            string visible = fullText.Substring(0, i + 1);
            string hidden = "";
            for (int j = i + 1; j < fullText.Length; j++)
                hidden += $"<alpha=#00>{fullText[j]}";

            textElement.text = visible + hidden;
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

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float startA, float endA, float dur, bool deactivateWhenDone = false)
    {
        float t = 0f;
        cg.alpha = startA;
        cg.blocksRaycasts = false;
        cg.interactable = false;

        while (t < dur)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startA, endA, t / dur);
            yield return null;
        }

        cg.alpha = endA;

        if (endA > 0f)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        if (deactivateWhenDone && endA == 0f)
        {
            cg.gameObject.SetActive(false);
        }
    }


}
