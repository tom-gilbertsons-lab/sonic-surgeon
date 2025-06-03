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

    public IEnumerator FadeCanvasGroupOld(CanvasGroup cg, float startA, float endA, float dur, bool deactivateWhenDone = false)
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


    public void FadeIn(GameObject obj, float duration,
                     GameObject[] manualDeactivate = null,
                     bool fadeChildrenGraphics = false)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("No CanvasGroup found on object: " + obj.name);
            return;
        }

        obj.SetActive(true);
        StartCoroutine(FadeCanvasGroup(cg, 0f, 1f, duration, false, manualDeactivate, fadeChildrenGraphics));
    }

    public void FadeOut(GameObject obj, float duration,
                        GameObject[] manualDeactivate = null,
                        bool fadeChildrenGraphics = false)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("No CanvasGroup found on object: " + obj.name);
            return;
        }

        StartCoroutine(FadeCanvasGroup(cg, 1f, 0f, duration, true, manualDeactivate, fadeChildrenGraphics));
    }

    public IEnumerator FadeOutRoutine(GameObject obj, float duration,
                                   GameObject[] manualDeactivate = null,
                                   bool fadeChildrenGraphics = false)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("No CanvasGroup found on object: " + obj.name);
            yield break;
        }

        yield return StartCoroutine(FadeCanvasGroup(cg, 1f, 0f, duration, true, manualDeactivate, fadeChildrenGraphics));
    }

    public IEnumerator FadeInRoutine(GameObject obj, float duration,
                                      GameObject[] manualDeactivate = null,
                                      bool fadeChildrenGraphics = false)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            Debug.LogWarning("No CanvasGroup found on object: " + obj.name);
            yield break;
        }

        obj.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(cg, 0f, 1f, duration, false, manualDeactivate, fadeChildrenGraphics));
    }


    public IEnumerator InfoBox(GameObject obj, bool end)
    {

        if (!end)
        {
            yield return FadeInRoutine(obj, 0.4f, fadeChildrenGraphics: true);
            yield return new WaitForSeconds(0.7f);
            yield return FadeOutRoutine(obj, 0.4f, fadeChildrenGraphics: true);
        }
        else
        {
            yield return FadeInRoutine(obj, 0.2f, fadeChildrenGraphics: true);
            yield return new WaitForSeconds(1.5f);
            yield return FadeOutRoutine(obj, 0.4f, fadeChildrenGraphics: true);
        }
    }



    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration,
                                   bool deactivateAfter = false,
                                   GameObject[] manualDeactivate = null,
                                   bool fadeChildrenGraphics = false)
    {
        if (manualDeactivate != null)
        {
            foreach (GameObject go in manualDeactivate)
            {
                if (go == null) continue;
                Outline outline = go.GetComponent<Outline>();
                if (outline != null) outline.enabled = false;
                go.SetActive(false);
            }
        }

        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = from;

        Graphic[] graphics = null;
        Outline[] outlines = null;
        Color baseOutlineColor = Color.white;

        // Background (if present) is faded separately
        Transform bgTransform = cg.transform.Find("Background");
        Graphic backgroundGraphic = bgTransform ? bgTransform.GetComponent<Graphic>() : null;
        Color bgColor = backgroundGraphic ? backgroundGraphic.color : Color.clear;

        if (fadeChildrenGraphics)
        {
            graphics = cg.GetComponentsInChildren<Graphic>(includeInactive: true);
            outlines = cg.GetComponentsInChildren<Outline>(includeInactive: true);

            if (outlines.Length > 0)
                baseOutlineColor = outlines[0].effectColor;

            float initialVisual = SnowballFade(from);
            foreach (var g in graphics)
            {
                if (g != null && g != backgroundGraphic) // skip background here
                {
                    Color baseColor = g.color;
                    baseColor.a = initialVisual;
                    g.color = baseColor;
                }
            }

            foreach (var o in outlines)
            {
                if (o != null)
                {
                    float oa = (from > to) ? 1f : 0f;
                    o.effectColor = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, oa);
                }
            }
        }

        float elapsed = 0f;
        float outlineFadeTime = duration * 0.1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = t * t * (3f - 2f * t);
            float currentAlpha = Mathf.Lerp(from, to, easedT);
            cg.alpha = currentAlpha;

            // === Background fades in first or out last ===
            if (backgroundGraphic)
            {
                float bgT = (from < to)
                    ? Mathf.Clamp01(t * 1.2f)               // fade in faster
                    : Mathf.Clamp01((t - 0.2f) / 0.8f);     // fade out later

                float easedBgT = bgT * bgT * (3f - 2f * bgT);
                float bgAlpha = Mathf.Lerp(from, to, easedBgT);

                Color bgC = bgColor;
                bgC.a = bgAlpha;
                backgroundGraphic.color = bgC;
            }

            if (fadeChildrenGraphics && graphics != null)
            {
                float visual = SnowballFade(currentAlpha);
                foreach (var g in graphics)
                {
                    if (g != null && g != backgroundGraphic)
                    {
                        Color original = g.color;
                        g.color = new Color(original.r, original.g, original.b, visual);
                    }
                }
            }

            if (fadeChildrenGraphics && outlines != null)
            {
                float oAlpha = 0f;
                if (from > to)
                {
                    oAlpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsed / outlineFadeTime));
                }
                else
                {
                    float fadeInStart = duration - outlineFadeTime;
                    if (elapsed >= fadeInStart)
                    {
                        float p = (elapsed - fadeInStart) / outlineFadeTime;
                        oAlpha = Mathf.Lerp(0f, 1f, Mathf.Clamp01(p));
                    }
                }

                foreach (var o in outlines)
                {
                    if (o != null)
                        o.effectColor = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, oAlpha);
                }
            }

            yield return null;
        }

        cg.alpha = to;

        if (backgroundGraphic)
        {
            Color finalBg = bgColor;
            finalBg.a = to;
            backgroundGraphic.color = finalBg;
        }

        if (fadeChildrenGraphics && graphics != null)
        {
            float visualFinal = SnowballFade(to);
            foreach (var g in graphics)
            {
                if (g != null && g != backgroundGraphic)
                {
                    Color c = g.color;
                    c.a = visualFinal;
                    g.color = c;
                }
            }
        }

        if (fadeChildrenGraphics && outlines != null)
        {
            float finalAlpha = (to > from) ? 1f : 0f;
            foreach (var o in outlines)
            {
                if (o != null)
                    o.effectColor = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, finalAlpha);
            }
        }

        if (to > 0.01f)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        if (deactivateAfter && to == 0f)
            cg.gameObject.SetActive(false);
    }


    private float SnowballFade(float a)
    {
        float rampStart = 0.15f;
        if (a <= rampStart) return 0f;

        float t = (a - rampStart) / (1f - rampStart);
        return Mathf.Pow(t, 1.5f);
    }



}



