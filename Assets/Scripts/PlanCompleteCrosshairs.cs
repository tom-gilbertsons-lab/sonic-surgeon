using UnityEngine;
using System.Collections;

public class PlanCompleteCrosshairs : MonoBehaviour
{
    public GameObject c3;
    public float pulseDuration = 3f; // total time for 3 pulses
    public int nPulses;

    private Material c3Mat;

    private void OnEnable()
    {
        if (c3 != null)
        {
            c3.SetActive(true);
            var sr = c3.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                c3Mat = sr.material;
                StartCoroutine(PulseGlow(c3Mat, nPulses, pulseDuration));
            }
        }
    }

    private IEnumerator PulseGlow(Material mat, int pulses, float totalDuration)
    {
        float singlePulseTime = totalDuration / pulses;
        float halfPulse = singlePulseTime / 2f;

        for (int i = 0; i < pulses; i++)
        {
            // Glow up
            float t = 0f;
            while (t < halfPulse)
            {
                t += Time.deltaTime;
                float glow = Mathf.Lerp(0f, 1f, t / halfPulse); // adjust min/max as needed
                mat.SetFloat("_GlowStrength", glow);
                yield return null;
            }

            // Glow down
            t = 0f;
            while (t < halfPulse)
            {
                t += Time.deltaTime;
                float glow = Mathf.Lerp(1f, 0f, t / halfPulse);
                mat.SetFloat("_GlowStrength", glow);
                yield return null;
            }
        }

        mat.SetFloat("_GlowStrength", 0f); // finish clean
    }
}


//using UnityEngine;
//using System.Collections;

//public class PlanCompleteCrosshairs : MonoBehaviour
//{
//    public GameObject c1;
//    public GameObject c2;
//    public GameObject c3;
//    public GameObject c4;

//    private void OnEnable()
//    {
//        // Make sure c1 and c2 are active for fading
//        c1.SetActive(true);
//        c2.SetActive(true);

//        // Set alpha of c1 and c2 to 0 initially
//        SetAlpha(c1, 0f);
//        SetAlpha(c2, 0f);

//        // Disable c3 and c4 for now
//        c3.SetActive(false);
//        c4.SetActive(false);

//        // Start fading in
//        StartCoroutine(FadeAndActivate());
//    }

//    private IEnumerator FadeAndActivate()
//    {
//        float t = 0f;
//        float dur = 0.7f;

//        while (t < dur)
//        {
//            t += Time.deltaTime;
//            float alpha = Mathf.Clamp01(t / dur);
//            SetAlpha(c1, alpha);
//            SetAlpha(c2, alpha);
//            yield return null;
//        }

//        // Make sure they're fully visible
//        SetAlpha(c1, 1f);
//        SetAlpha(c2, 1f);

//        // Now activate c3 and c4
//        c3.SetActive(true);
//        c4.SetActive(true);
//    }

//    private void SetAlpha(GameObject obj, float alpha)
//    {
//        var sr = obj.GetComponent<SpriteRenderer>();
//        if (sr != null)
//        {
//            Color c = sr.color;
//            c.a = alpha;
//            sr.color = c;
//        }
//    }
//}

