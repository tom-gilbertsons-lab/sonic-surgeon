using UnityEngine;

public class VimDose : MonoBehaviour
{
    // cycles through colour change 

    public SpriteRenderer sr;
    private TreatMode treatMode;

    private int doseCount = 0;

    private Color startColour;
    private Color treatedColour;


    private float fadeDose = 0f;
    private float delayDose = 0f;
    private bool isHeating = false;


    private void Awake()


    {
        Debug.Log("Awake");
        // on awake want them all transparent. 
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1f, 1f, 1f, 0f); // fully transparent


        // Find manager automatically in parent hierarchy:
        treatMode = GetComponentInParent<TreatMode>();
        startColour = sr.color;
        treatedColour = sr.color;


        Debug.Log(treatMode);
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
            float t = fadeDose / treatMode.fadeDuration;
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

        if (doseCount >= treatMode.doseColours.Length) return;

        doseCount++;
        Color baseColour = treatMode.doseColours[doseCount - 1];

        // Add v slight variation to the RGB 
        float dapple = 0.2f;

        float newR = Mathf.Clamp01(baseColour.r + Random.Range(-dapple, dapple));
        float newG = Mathf.Clamp01(baseColour.g + Random.Range(-dapple, dapple));
        float newB = Mathf.Clamp01(baseColour.b + Random.Range(-dapple, dapple));

        treatedColour = new Color(newR, newG, newB, baseColour.a);

        delayDose = treatMode.delayBeforeFade;
        fadeDose = 0f;
        isHeating = true;
    }


    public bool IsMaxed()
    {
        Debug.Log(treatMode);
        return doseCount >= treatMode.doseColours.Length;
    }
}


