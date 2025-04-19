using UnityEngine;

public class TapPulse : MonoBehaviour
{
    [SerializeField] private float maxPulseWidth = 0.5f;
    [SerializeField] private float pulseDuration = 1.5f;
    [SerializeField] private float pulseSoftness = 0.1f;

    private Material _material;
    private Coroutine _pulseRoutine;

    private void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
        _material.SetFloat("_PulseWidth", 0f);
        _material.SetFloat("_PulseSoftness", pulseSoftness);
    }

    private void OnMouseDown()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 localPoint = transform.InverseTransformPoint(worldPoint);
        Vector2 spriteSize = GetComponent<SpriteRenderer>().bounds.size;
        Vector2 uv = new Vector2((localPoint.x / spriteSize.x) + 0.5f, (localPoint.y / spriteSize.y) + 0.5f);

        Debug.Log("clicked Pulsed bit");

        _material.SetVector("_PulseCenter", new Vector4(uv.x, uv.y, 0, 0));

        if (_pulseRoutine != null)
            StopCoroutine(_pulseRoutine);

        _pulseRoutine = StartCoroutine(Pulse());
    }

    private System.Collections.IEnumerator Pulse()
    {
        float t = 0f;
        while (t < pulseDuration)
        {
            float progress = t / pulseDuration;
            float pulseValue = Mathf.Sin(progress * Mathf.PI); // rise and fall
            _material.SetFloat("_PulseWidth", pulseValue * maxPulseWidth);
            t += Time.deltaTime;
            yield return null;
        }

        _material.SetFloat("_PulseWidth", 0f);
    }
}
