using UnityEngine;

public class PlanModeManager : MonoBehaviour
{
    public GameObject targetSlice;
    public GameObject targetIndicator;

    private SpriteRenderer targetIndicatorSR;

    private Color baseGreen = new Color(0f, 1f, 0f, 0f);
    private int hitCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetIndicatorSR= targetIndicator.GetComponent<SpriteRenderer>();
        targetIndicatorSR.color = baseGreen; // fully transparent
    }

   public void HitTarget()
    {
        Debug.Log("Hit Target");
        hitCount++;

        float newAlpha = 0f;

        if (hitCount == 1)
        {
            newAlpha = 0.1f;
        }
        else if (hitCount == 2)
        {
            newAlpha = 0.3f;
        }
        else if (hitCount >= 3)
        {
            newAlpha = 0.92f;
        }

        // Update only the alpha, keep the green
        targetIndicatorSR.color = new Color(baseGreen.r, baseGreen.g, baseGreen.b, newAlpha);
       
   
    }
}

