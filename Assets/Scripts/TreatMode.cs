using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class TreatMode : MonoBehaviour
{

    public GameObject gameManagerObject;
    private TreatModeController treatModeController;

    [Header("Target")]
    public GameObject target;
    public float delayBeforeFade = 0.7f;
    public float fadeDuration = 0.5f;
    public Material treatCompleteMaterial;
    public GameObject treatCompleteVim;

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
    public int dosesDelivered = 0;

    // to pass to TreatModeController
    public float proportionTreated = 0f;
    public bool treatmentComplete = false;

    private SceneEffects sceneEffects;



    private void Awake()
    {

        sceneEffects = GetComponent<SceneEffects>();
        // counting the sub nuclei 
        foreach (Transform child in target.transform)
        {
            totalSubNuclei++;
        }
        treatModeController = gameManagerObject.GetComponent<TreatModeController>();
        EnableTreatCollider(false);
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
        bool hitAnyTarget = false;

        foreach (Transform child in target.transform)
        {

            VimDose vimSub = child.GetComponent<VimDose>();
            if (vimSub == null || vimSub.IsMaxed())
                continue;

            float targetRadius = vimSub.sr.bounds.extents.magnitude;
            float distance = Vector2.Distance(worldClick, child.position);

            if (distance <= targetRadius + hotspotRadius)
            {
                hitAnyTarget = true;

                vimSub.AccumulateDose();
                dosesDelivered++;
                proportionTreated = (float)dosesDelivered / (3 * (float)totalSubNuclei);
                treatModeController.UpdateProgress(proportionTreated, dosesDelivered);

                if (vimSub.IsMaxed())
                {
                    treatedSubNuclei++;
                }
            }
        }

        if (hitAnyTarget)
        {
            Debug.Log("On target");
            treatModeController.OnTargetTaps();
        }
        else
        {
            Debug.Log("Off target");
            treatModeController.OffTargetTaps();
        }



        if (treatedSubNuclei == totalSubNuclei)
        {
            treatmentComplete = true;
            treatModeController.StopCountdown();
            StartCoroutine(TreatModeSuccess());
        }

    }

    private IEnumerator TreatModeSuccess()
    {
        Debug.Log("In treatMode success");
        EnableTreatCollider(false);

        yield return new WaitForSeconds(1.0f);
        treatModeController.HideOverlays();

        yield return StartCoroutine(TreatCompleteSequence());

        yield return StartCoroutine(sceneEffects.FadeOutSceneThen(1.0f, () =>
        {
            treatModeController.EndTreatMode();
        }));

    }

    private IEnumerator TreatCompleteSequence()
    {
        foreach (Transform child in target.transform)
        {

            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

            sr.material = new Material(treatCompleteMaterial);
            float dapple = Random.Range(0.1f, 0.4f);

            yield return new WaitForSeconds(dapple); // delay between each
        }

        yield return new WaitForSeconds(0.3f);

        SpriteRenderer vimSR = target.GetComponent<SpriteRenderer>();
        vimSR.enabled = true;
        Material mat = vimSR.material;

        // Phase 1: pulse 0.3 → 1 → 0.3 twice
        //for (int i = 0; i < 2; i++)
        //{
        yield return StartCoroutine(PulseGlow(mat, 0.3f, 1f, 1f));
        yield return StartCoroutine(PulseGlow(mat, 1f, 0.3f, 1f));
        // }

        // Phase 2: ramp to 3
        yield return StartCoroutine(PulseGlow(mat, 0.3f, 3f, 1f));

        // Phase 3: fade to 0
        //yield return StartCoroutine(PulseGlow(mat, 3f, 0f, 1f));
    }



    private IEnumerator PulseGlow(Material mat, float from, float to, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / dur);
            mat.SetFloat("_GlowStrength", val);
            yield return null;
        }
        mat.SetFloat("_GlowStrength", to);
    }






    private void EnableTreatCollider(bool enable)
    {
        BoxCollider2D treatCollider = GetComponent<BoxCollider2D>();
        treatCollider.enabled = enable;
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
