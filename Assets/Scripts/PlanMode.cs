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



    void Start()
    {
        planModeController = gameManagerObject.GetComponent<PlanModeController>();
        StartRound1();

    }

    public void HitTarget()
    {
        Debug.Log("Hit Target");
        onTargetHitCount++;

        if (onTargetHitCount == 1)
        {
            targetRound1.SetActive(true);
            planModeController.UpdateProgress(0.33f);
            StartRound2();
        }
        else if (onTargetHitCount == 2)
        {
            targetRound2.SetActive(true);
            planModeController.UpdateProgress(0.66f);
            StartRound3();
        }
        else if (onTargetHitCount >= 3)
        {
            targetRound3.SetActive(true);

            planModeController.UpdateProgress(1f);
            planModeController.StopCountdown();
            StartCoroutine(PlanModeSuccess());
        }
    }

    private IEnumerator PlanModeSuccess()
    {
        Debug.Log("Plan Mode Success- send flag to stop countdown immediately ");
        DeactivateAllRounds();
        yield return new WaitForSeconds(1.0f);
        targetRound1.SetActive(false);
        targetRound2.SetActive(false);
        Debug.Log("Do a little display thing like update pogress brightlys");

        planModeController.HideCountdownAndProgress();
        planComplete.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        planModeController.EndPlanMode();

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