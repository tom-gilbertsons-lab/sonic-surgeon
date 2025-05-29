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

    public GameObject planComplete;


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

            planModeController.UpdateProgress(3);
            planModeController.StopCountdown();
            StartCoroutine(PlanModeSuccess());
        }
    }

    private IEnumerator PlanModeSuccess()
    {
        DeactivateAllRounds();
        Debug.Log("postDeactivate");
        yield return StartCoroutine(FadeBoth(targetRound1, targetRound2, 1.0f));
        Debug.Log("post yeilds");
        planModeController.HideCountdownAndProgress();
        planComplete.SetActive(true);
        yield return StartCoroutine(FadeOne(targetRound3, 1.0f));
        yield return StartCoroutine(sceneEffects.FadeOutSceneThen(1.0f, () =>
        {
            planModeController.EndPlanMode();
        }));
    }

    private IEnumerator FadeBoth(GameObject obj1, GameObject obj2, float duration)
    {
        Coroutine c1 = StartCoroutine(sceneEffects.FadeOutObject(obj1, duration));
        Coroutine c2 = StartCoroutine(sceneEffects.FadeOutObject(obj2, duration));
        yield return c1;
        yield return c2;
        obj1.SetActive(false);
        obj2.SetActive(false);
    }

    private IEnumerator FadeOne(GameObject obj1, float duration)
    {
        Coroutine c1 = StartCoroutine(sceneEffects.FadeOutObject(obj1, duration));
        yield return c1;
        obj1.SetActive(false);
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


}