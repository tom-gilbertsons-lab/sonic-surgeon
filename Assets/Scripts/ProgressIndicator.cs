using UnityEngine;

public class ProgressIndicator : MonoBehaviour
{
    public float progress = 0f;

    public Color colourOff = new Color(0.3f, 0.3f, 0.3f, 1f); // 'off' colour

    public Color colourOn = new Color(0f, 1f, 1f, 1f);       // target colour\

    public Color transducerColour = Color.white;


    public void Start()
    {
        Transform transducer = transform.Find("Outer");
        // Set outer colour

        foreach (Transform child in transducer)
        {
            Renderer transducerRend = child.GetComponent<Renderer>();
            transducerRend.material.color = transducerColour;
        }

    }

    public void ApplyProgress()
    {
        for (int i = 1; i <= 8; i++)
        {
            Transform lvl = transform.Find("Lvl" + i);
            foreach (Transform child in lvl)
            {
                Renderer rend = child.GetComponent<Renderer>();
                float localProgress = Mathf.Clamp01((progress * 8f - (i - 1)));
                rend.material.color = Color.Lerp(colourOff, colourOn, localProgress);
            }
        }

        // Show complete object only when progress is full
        transform.Find("Complete").gameObject.SetActive(progress >= 1f);
    }

}

