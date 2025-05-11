using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffects : MonoBehaviour
{
    public IEnumerator FadeOutSceneThen(float duration, Action onDone)
    {
        SpriteRenderer[] allSRs = GetComponentsInChildren<SpriteRenderer>(true);
        List<Material> mats = new List<Material>();

        foreach (var sr in allSRs)
        {
            if (sr.material.HasProperty("_Color"))
            {
                sr.material = new Material(sr.material); // prevent shared material overwrite
                mats.Add(sr.material);
            }
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            foreach (var mat in mats)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
            yield return null;
        }

        foreach (var mat in mats)
        {
            Color c = mat.color;
            c.a = 0f;
            mat.color = c;
        }

        onDone?.Invoke();
    }
}
