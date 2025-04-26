using UnityEngine;
using System.Collections;

public class TreatVFX : MonoBehaviour
{ 
    // the visualFX for a hotspot tap using the TreatShaderGraph
    private Material mat;

    void Awake()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_PulseProgress", 0f);
    }


    public IEnumerator AnimateHotSpot(float xPos, float yPos, float pulseDuration, float maxRadius)
    {
        float t = 0f;


        mat.SetFloat("_MaxRadius", maxRadius);
        mat.SetFloat("_PulseProgress", 0f);
        mat.SetVector("_TapPoint", new Vector4(xPos, yPos, 0, 0));

        while (t < pulseDuration)
        {

            float progress = t / pulseDuration;

            mat.SetFloat("_HeatProgress", progress);
            t += Time.deltaTime;
            yield return null;
        }
        mat.SetFloat("_HeatProgress", 0f);
    }
}

