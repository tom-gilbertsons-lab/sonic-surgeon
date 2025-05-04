using UnityEngine;

public class PlanVFX: MonoBehaviour
{
    public float pulseDuration = 0.7f;
    public bool target = false;


    private PlanMode planMode; 
    private Material mat;


    private void Awake()
    {
        planMode= GetComponentInParent<PlanMode>();
    }

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

        float uvX = (localClick.x / sr.sprite.bounds.size.x) + 0.5f;
        float uvY = (localClick.y / sr.sprite.bounds.size.y) + 0.5f;

        mat.SetVector("_TapPoint", new Vector4(uvX, uvY, 0, 0));

  
        StartCoroutine(AnimatePulse());
    }

    System.Collections.IEnumerator AnimatePulse()
    {
        float t = 0f;


        while (t < pulseDuration)
        {
          
            float progress = t / pulseDuration;
          
            mat.SetFloat("_PulseProgress", 1.5f* progress);
            t += Time.deltaTime;
            yield return null;
        }
        mat.SetFloat("_PulseProgress", 0f);
        if (target)
        {
            planMode.HitTarget();

        }
  
    }
}
