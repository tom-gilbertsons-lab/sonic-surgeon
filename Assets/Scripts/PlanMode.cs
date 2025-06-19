using UnityEngine;
using System.Collections;


public class PlanMode : MonoBehaviour
{
    public GameObject gameManagerObject;
    private PlanModeController planModeController;


    public Color treatGreen = new Color(0f, 1f, 0f, 0f);

    public GameObject round1;
    public GameObject targetRound1;
    public GameObject round2;
    public GameObject targetRound2;
    public GameObject round3;
    public GameObject targetRound3;
    public GameObject targetComplete;


    public int onTargetHitCount = 0;
    public int offTargetTaps = 0;

    private SceneEffects sceneEffects;



    private void Awake()
    {
        sceneEffects = GetComponent<SceneEffects>();
        planModeController = gameManagerObject.GetComponent<PlanModeController>();
        DeactivateAllRounds();
    }



    public void StartPlan()
    {

        StartRound1();

    }

    public void HitTarget()
    {
        Debug.Log("Hit Target");
        onTargetHitCount++;

        if (onTargetHitCount == 1)
        {
            targetRound1.SetActive(true);
            planModeController.UpdateProgress(1);
            StartRound2();
        }
        else if (onTargetHitCount == 2)
        {
            targetRound2.SetActive(true);
            planModeController.UpdateProgress(2);
            StartRound3();
        }
        else if (onTargetHitCount >= 3)
        {
            targetRound3.SetActive(true);
            StartCoroutine(PlanModeSuccess());
        }
    }


    private IEnumerator PlanModeSuccess()
    {
        planModeController.StopCountdown();
        planModeController.UpdateProgress(3);
        DeactivateAllRounds();
        yield return new WaitForSeconds(1f);
        StartCoroutine(GrowInX(targetComplete, 1.5f));
        //planModeController.HideOverlays();
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(sceneEffects.FadeOutSceneThen(1.0f, () =>
        {
            planModeController.EndPlanMode();
        }));
    }

    public void ReportTapsToController()
    {
        planModeController.UpdateTapStats(onTargetHitCount, offTargetTaps);
    }

    private void StartRound1()
    {
        round1.SetActive(true);

        round2.SetActive(false);
        round3.SetActive(false);
    }

    private void StartRound2()
    {
        round2.SetActive(true);
        round1.SetActive(false);
        round3.SetActive(false);
    }

    private void StartRound3()
    {
        round3.SetActive(true);
        round1.SetActive(false);
        round2.SetActive(false);
    }

    private void DeactivateAllRounds()
    {
        round1.SetActive(false);
        round2.SetActive(false);
        round3.SetActive(false);
    }


    public IEnumerator GrowInX(GameObject obj, float duration = 0.5f)
    {
        obj.SetActive(true);

        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale = new Vector3(0f, originalScale.y, originalScale.z);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float newX = Mathf.Lerp(0f, originalScale.x, smoothT);
            obj.transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = originalScale; // ensure final size
    }


}