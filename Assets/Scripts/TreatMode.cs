using UnityEngine;
using System.Collections.Generic;

public class TreatMode : MonoBehaviour
{

    public GameObject gameManagerObject;
    private TreatModeController treatModeController;

    [Header("Target")]
    public GameObject target;
    public float delayBeforeFade = 0.7f;
    public float fadeDuration = 0.5f;

    [ColorUsage(true, true)]
    public Color[] doseColours = new Color[] {
        new Color(1f, 0.5f, 0f, 0.2f),
        new Color(1f, 0.2f, 0f, 0.6f),
        new Color(2f, 0f, 0f, 1f)
    };


    [Header("Hotspot vFX")]
    public float hotspotRadius = 0.25f;
    public GameObject vFXPrefab;
    public Transform vFXParent;
    public float vFXDuration = 1.0f;
    private List<GameObject> activeVFX = new List<GameObject>();


    private Vector3 worldClick;

    private int totalSubNuclei = 0;
    private int treatedSubNuclei = 0;
    private int dosesDelivered = 0;

    // to pass to TreatModeController
    public float proportionTreated = 0f;
    public bool treatmentComplete = false;


    private void Awake()
    {
        // counting the sub nuclei 
        foreach (Transform child in target.transform)
        {
            totalSubNuclei++;
        }
        Debug.Log("totalSubNuclei)" + totalSubNuclei.ToString());

        treatModeController = gameManagerObject.GetComponent<TreatModeController>();
    }


    private void OnMouseDown()
    {
        // onClick make hotspot and check if we are near target 
        worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        TargetOverlap();
        SpawnVFX();

    }


    private void TargetOverlap()
    {

        foreach (Transform child in target.transform)
        {

            VimDose vimSub = child.GetComponent<VimDose>();
            if (vimSub == null || vimSub.IsMaxed())
                continue;

            float targetRadius = vimSub.sr.bounds.extents.magnitude;
            float distance = Vector2.Distance(worldClick, child.position);

            if (distance <= targetRadius + hotspotRadius)
            {

                vimSub.AccumulateDose();
                dosesDelivered++;
                proportionTreated = (float)dosesDelivered / (3 * (float)totalSubNuclei);
                treatModeController.UpdateProgress(proportionTreated);

                if (vimSub.IsMaxed())
                {
                    treatedSubNuclei++;
                }
            }
        }

        if (treatedSubNuclei == totalSubNuclei)
        {
            treatmentComplete = true;
            treatModeController.EndTreatMode();

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

        //Debug.Log("correctedHotspotRadius = " + correctedHotspotRadius);

        treatvfx.StartCoroutine(treatvfx.AnimateHotSpot(xShaderPos, yShaderPos, vFXDuration, correctedHotspotRadius));

        activeVFX.Add(newVFX);

        if (activeVFX.Count > 5)
        {
            Destroy(activeVFX[0]);
            activeVFX.RemoveAt(0);
        }

        Destroy(newVFX, vFXDuration + 0.1f);
    }


}
