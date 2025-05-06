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

    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float startA, float endA, float dur, bool makeInteractiveAfter = true)
    {
        float t = 0f;
        cg.alpha = startA;
        cg.blocksRaycasts = true;
        cg.interactable = false;

        while (t < dur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, endA, t / dur);
            cg.alpha = a;
            yield return null;
        }

        cg.alpha = endA;

        if (makeInteractiveAfter && endA > 0f)
        {
            cg.interactable = true;
        }
        else
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public IEnumerator FadeOutCanvasGroup(CanvasGroup cg, float dur, bool deactivateAfter = true)
    {
        float startA = cg.alpha;
        float t = 0f;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        while (t < dur)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startA, 0f, t / dur);
            cg.alpha = a;
            yield return null;
        }

        cg.alpha = 0f;

        if (deactivateAfter)
        {
            cg.gameObject.SetActive(false);
        }
    }

}
