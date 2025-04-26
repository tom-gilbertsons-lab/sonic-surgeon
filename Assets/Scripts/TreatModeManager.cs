using UnityEngine;
using System.Collections.Generic;

public class TreatModeManager : MonoBehaviour
{

    //Treat Mode Play


    [Header("Target Colour Settings")]
    public Color[] doseColours = new Color[] {
        new Color(1f, 0f, 0f, 0.4f),  
        new Color(1f, 0.5f, 0f, 0.6f),
        new Color(0f, 1f, 0f, 1f)     
    };

    [Header("Target Settings")]
    public GameObject target;
    public float delayBeforeFade = 1.0f;
    public float fadeDuration = 1.0f;

    [Header("VFX Settings")]
    public float hotspotRadius = 1.0f;
    public GameObject vFXPrefab;
    public Transform vFXParent;
    public float vFXDuration = 1.3f;
    private List<GameObject> activeVFX = new List<GameObject>();


    private Vector3 worldClick;
   


    private void OnMouseDown()
    {
        Debug.Log("Clicked Collider");

        worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        TargetOverlap();
        SpawnVFX();
        
    }


    private void TargetOverlap()
    {
        foreach (Transform child in target.transform)
        {
            VimSubColourCycle vimSub = child.GetComponent<VimSubColourCycle>();
            if (vimSub == null || vimSub.IsMaxed()) continue;

            float targetRadius = vimSub.sr.bounds.extents.magnitude;
            float distance = Vector2.Distance(worldClick, child.position);

            if (distance <= targetRadius + hotspotRadius)
            {
                Debug.Log("Click overlapped " + child.name);
                vimSub.AccumulateDose();
            }
        }
    }


    private void SpawnVFX()
    {
        GameObject newVFX = Instantiate(vFXPrefab, Vector3.zero, Quaternion.identity, vFXParent);

        SpriteRenderer vfxSR = newVFX.GetComponent<SpriteRenderer>();
        TreatVFX treatvfx = newVFX.GetComponent<TreatVFX>();

        Vector2 localClick = vfxSR.transform.InverseTransformPoint(worldClick);

        float xShaderPos = (localClick.x / vfxSR.sprite.bounds.size.x) + 0.5f;
        float yShaderPos = (localClick.y / vfxSR.sprite.bounds.size.y) + 0.5f;

   
        float width = vfxSR.sprite.bounds.size.x * vfxSR.transform.lossyScale.x;
        float height = vfxSR.sprite.bounds.size.y * vfxSR.transform.lossyScale.y;

        float uvRadiusX = hotspotRadius / width;
        float uvRadiusY = hotspotRadius / height;
        float correctedHotspotRadius = (uvRadiusX + uvRadiusY) / 2f;

        Debug.Log("correctedHotspotRadius = " + correctedHotspotRadius);

        treatvfx.StartCoroutine(treatvfx.AnimateHotSpot(xShaderPos, yShaderPos, vFXDuration, correctedHotspotRadius));

        activeVFX.Add(newVFX);

        if (activeVFX.Count > 4)
        {
            Destroy(activeVFX[0]);
            activeVFX.RemoveAt(0);
        }

        Destroy(newVFX, vFXDuration + 0.1f);
    }


}
