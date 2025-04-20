using UnityEngine;

public class OnTapPulse: MonoBehaviour
{
    public float pulseDuration = 0.5f;

    private Material mat;
    private Coroutine pulseRoutine;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat("_PulseProgress", 0f);
    }

    void OnMouseDown()
    {
        Vector3 worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 localClick = transform.InverseTransformPoint(worldClick);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        Debug.Log("click on SG");

        float uvX = (localClick.x / sr.sprite.bounds.size.x) + 0.5f;
        float uvY = (localClick.y / sr.sprite.bounds.size.y) + 0.5f;

        mat.SetVector("_TapPoint", new Vector4(uvX, uvY, 0, 0));

        // mat.SetFloat("_PulseProgress", 0.8f);

        Debug.Log("clicked on slice");
        //mat.SetVector("_TapPoint", new Vector4(0.5f, 0.5f, 0, 0));



        //if (pulseRoutine != null) StopCoroutine(pulseRoutine);
        pulseRoutine = StartCoroutine(AnimatePulse());
    }

    System.Collections.IEnumerator AnimatePulse()
    {
        float t = 0f;


        while (t < pulseDuration)
        {
          
            float progress = t / pulseDuration;
            Debug.Log(progress);
            mat.SetFloat("_PulseProgress", progress);
            t += Time.deltaTime;
            yield return null;
        }
        mat.SetFloat("_PulseProgress", 0f);
    }
}
