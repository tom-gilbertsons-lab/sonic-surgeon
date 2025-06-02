//using UnityEngine;
//using System.Collections;

//public class PlanCompleteCrosshairs : MonoBehaviour
//{
//    public GameObject c3;
//    public float pulseDuration = 3f; // total time for 3 pulses
//    public int nPulses;

//    private Material c3Mat;

//    private void OnEnable()
//    {
//        if (c3 != null)
//        {
//            c3.SetActive(true);
//            var sr = c3.GetComponent<SpriteRenderer>();
//            if (sr != null)
//            {
//                c3Mat = sr.material;
//                StartCoroutine(PulseGlow(c3Mat, nPulses, pulseDuration));
//            }
//        }
//    }

//    private IEnumerator PulseGlow(Material mat, int pulses, float totalDuration)
//    {
//        float singlePulseTime = totalDuration / pulses;
//        float halfPulse = singlePulseTime / 2f;

//        for (int i = 0; i < pulses; i++)
//        {
//            // Glow up
//            float t = 0f;
//            while (t < halfPulse)
//            {
//                t += Time.deltaTime;
//                float glow = Mathf.Lerp(0f, 1f, t / halfPulse); // adjust min/max as needed
//                mat.SetFloat("_GlowStrength", glow);
//                yield return null;
//            }

//            // Glow down
//            t = 0f;
//            while (t < halfPulse)
//            {
//                t += Time.deltaTime;
//                float glow = Mathf.Lerp(1f, 0f, t / halfPulse);
//                mat.SetFloat("_GlowStrength", glow);
//                yield return null;
//            }
//        }

//        mat.SetFloat("_GlowStrength", 0f); // finish clean
//    }
//}
