using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BrainPulseFX : MonoBehaviour
{
    public float fadeDuration = 4f;
    public float holdTime = 1f;

    private Image[] images;
    private List<int> sequence = new List<int> { 0, 1, 2, 3, 2, 1, 0, 1, 2, 3, 2, 1 };

    void Start()
    {
        images = GetComponentsInChildren<Image>(true);

        // Set all images to fully transparent
        foreach (var img in images)
            SetAlpha(img, 0f);

        StartCoroutine(PulseLoop());
    }

    IEnumerator PulseLoop()
    {
        for (int i = 0; i < sequence.Count - 1; i++)
        {
            int current = sequence[i];
            int next = sequence[i + 1];

            // Reset all unrelated images
            for (int j = 0; j < images.Length; j++)
                if (j != current && j != next)
                    SetAlpha(images[j], 0f);

            float t = 0f;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / fadeDuration);
                float eased = Mathf.SmoothStep(0f, 1f, progress);

                float currentAlpha = Mathf.Lerp(1f, 0f, eased);
                float nextAlpha = Mathf.Lerp(0f, 1f, eased);

                SetAlpha(images[current], currentAlpha);
                SetAlpha(images[next], nextAlpha);

                yield return null;
            }

            // Ensure final state is clean
            SetAlpha(images[current], 0f);
            SetAlpha(images[next], 1f);

            yield return new WaitForSeconds(holdTime);
        }

        // Loop again
        StartCoroutine(PulseLoop());
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}

