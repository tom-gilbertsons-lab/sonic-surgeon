using UnityEngine;

public class VimDose : MonoBehaviour
{
    // cycles through colour change 

    public SpriteRenderer sr;
    private TreatModeManager treatModeManager;

    private int doseCount = 0;

    private Color startColour;
    private Color treatedColour;


    private float fadeDose = 0f;
    private float delayDose = 0f;
    private bool isHeating = false;


    private void Awake()
    {
        // on awake want them all transparent. 
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, 0f); // fully transparent


        // Find manager automatically in parent hierarchy:
        treatModeManager = GetComponentInParent<TreatModeManager>();
        startColour = sr.color;
        treatedColour = sr.color;
    }


    void Update()
    {
        if (isHeating)
        {
            if (delayDose > 0f)
            {
                delayDose -= Time.deltaTime;
                return;
            }

            fadeDose += Time.deltaTime;
            float t = fadeDose / treatModeManager.fadeDuration;
            sr.color = Color.Lerp(startColour, treatedColour, t);

            if (t >= 1f)

            {
                sr.color = treatedColour;
                isHeating = false;
            }
        }
    }


    public void AccumulateDose()
    {
        if (doseCount >= treatModeManager.doseColours.Length) return;

        doseCount++;
        Color baseColour = treatModeManager.doseColours[doseCount - 1];

        // Add v slight variation to the RGB 
        float dapple = 0.2f;

        float newR = Mathf.Clamp01(baseColour.r + Random.Range(-dapple, dapple));
        float newG = Mathf.Clamp01(baseColour.g + Random.Range(-dapple, dapple));
        float newB = Mathf.Clamp01(baseColour.b + Random.Range(-dapple, dapple));

        treatedColour = new Color(newR, newG, newB, baseColour.a);

        delayDose = treatModeManager.delayBeforeFade;
        fadeDose = 0f;
        isHeating = true;
    }


    public bool IsMaxed()
    {
        return doseCount >= treatModeManager.doseColours.Length;
    }
}


