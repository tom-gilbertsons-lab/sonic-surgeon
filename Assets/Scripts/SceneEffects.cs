using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffects : MonoBehaviour
{
    public IEnumerator FadeOutSceneThen(float duration, Action onDone)
    {
        SpriteRenderer[] allSRs = GetComponentsInChildren<SpriteRenderer>(true);
        List<(Material mat, Color mainStart, Color glowStart, float glowStrengthStart)> fades = new();

        foreach (var sr in allSRs)
        {
            if (sr.material.HasProperty("_MainColour"))
            {
                sr.material = new Material(sr.material); // avoid shared overwrite
                Material mat = sr.material;

                Color main = mat.GetColor("_MainColour");
                Color glow = mat.GetColor("_GlowColour");
                float strength = mat.HasProperty("_GlowStrength") ? mat.GetFloat("_GlowStrength") : 1f;

                fades.Add((mat, main, glow, strength));
            }
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            foreach (var (mat, mainStart, glowStart, glowStrengthStart) in fades)
            {
                Color newMain = mainStart;
                newMain.a = alpha;
                mat.SetColor("_MainColour", newMain);

                Color newGlow = glowStart;
                newGlow.a = alpha;
                mat.SetColor("_GlowColour", newGlow);

                mat.SetFloat("_GlowStrength", glowStrengthStart * alpha);
            }

            yield return null;
        }

        foreach (var (mat, _, _, _) in fades)
        {
            mat.SetFloat("_GlowStrength", 0f);
        }

        onDone?.Invoke();
    }



    public IEnumerator FadeOutObject(GameObject obj, float duration)
    {
        SpriteRenderer[] allSRs = obj.GetComponentsInChildren<SpriteRenderer>(true);
        List<(Material mat, Color mainStart, Color glowStart, float glowStrengthStart)> fades = new();

        foreach (var sr in allSRs)
        {
            if (sr.material.HasProperty("_MainColour"))
            {
                sr.material = new Material(sr.material); // avoid shared overwrite
                Material mat = sr.material;

                Color main = mat.GetColor("_MainColour");
                Color glow = mat.GetColor("_GlowColour");
                float strength = mat.HasProperty("_GlowStrength") ? mat.GetFloat("_GlowStrength") : 1f;

                fades.Add((mat, main, glow, strength));
            }
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            foreach (var (mat, mainStart, glowStart, glowStrengthStart) in fades)
            {
                Color newMain = mainStart;
                newMain.a = alpha;
                mat.SetColor("_MainColour", newMain);

                Color newGlow = glowStart;
                newGlow.a = alpha;
                mat.SetColor("_GlowColour", newGlow);

                mat.SetFloat("_GlowStrength", glowStrengthStart * alpha);
            }

            yield return null;
        }

        // Final cleanup
        foreach (var (mat, _, _, _) in fades)
        {
            mat.SetFloat("_GlowStrength", 0f);
        }
    }


}
