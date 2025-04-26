using UnityEngine;

public class VimSubColourCycle : MonoBehaviour
{
    // cycles tougt colour change 

    public SpriteRenderer sr;
    private TreatModeManager treatModeManager;

    private int tapCount = 0;
    private Color targetColour;

    private float fadeTimer = 0f;
    private float delayTimer = 0f;
    private bool isFading = false;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, 0f); // fully transparent
        // Find manager automatically in parent hierarchy:
        treatModeManager = GetComponentInParent<TreatModeManager>();
        targetColour = sr.color;
    }


    void Update()
    {
        if (isFading)
        {
            if (delayTimer > 0f)
            {
                delayTimer -= Time.deltaTime;
                return;
            }

            fadeTimer += Time.deltaTime;
            float t = fadeTimer / treatModeManager.fadeDuration;
            sr.color = Color.Lerp(sr.color, targetColour, t);

            if (t >= 1f)
            {
                sr.color = targetColour;
                isFading = false;
            }
        }
    }


    public void AccumulateDose()
    {
        if (tapCount >= treatModeManager.doseColours.Length) return;

        tapCount++;
        Color baseColour = treatModeManager.doseColours[tapCount - 1];

        // Add subtle random variation to RGB, not affecting alpha
        float variationAmount = 0.2f; // max variation range ±0.1 (adjust as desired)

        float newR = Mathf.Clamp01(baseColour.r + Random.Range(-variationAmount, variationAmount));
        float newG = Mathf.Clamp01(baseColour.g + Random.Range(-variationAmount, variationAmount));
        float newB = Mathf.Clamp01(baseColour.b + Random.Range(-variationAmount, variationAmount));

        targetColour = new Color(newR, newG, newB, baseColour.a);

        delayTimer = treatModeManager.delayBeforeFade;
        fadeTimer = 0f;
        isFading = true;
    }


    public bool IsMaxed()
    {
        return tapCount >= treatModeManager.doseColours.Length;
    }
}


