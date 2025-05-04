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

    private SpriteRenderer targetIndicator1;
    private SpriteRenderer targetIndicator2;
    private SpriteRenderer targetIndicator3;



    private int onTargetHitCount = 0;

 
    void Start()
    {
        planModeController = gameManagerObject.GetComponent<PlanModeController>();

        targetIndicator1 = targetRound1.GetComponent<SpriteRenderer>();
        targetIndicator2 = targetRound2.GetComponent<SpriteRenderer>();
        targetIndicator3 = targetRound3.GetComponent<SpriteRenderer>();
        StartRound1();

    }

    public void HitTarget()
    {
        Debug.Log("Hit Target");
        onTargetHitCount++;

        if (onTargetHitCount == 1)
        {
            targetIndicator1.color = new Color(treatGreen.r, treatGreen.g, treatGreen.b, 0.05f);
            planModeController.UpdateProgress(0.3f);
            StartRound2();
        }
        else if (onTargetHitCount == 2)
        {
            targetIndicator2.color = new Color(treatGreen.r, treatGreen.g, treatGreen.b, 0.1f);
            planModeController.UpdateProgress(0.6f);
            StartRound3();
        }
        else if (onTargetHitCount >= 3)
        {
            targetIndicator3.color = new Color(treatGreen.r, treatGreen.g, treatGreen.b, 1.0f);
            planModeController.UpdateProgress(0.9f);
            StartCoroutine(EndPlanMode());
        }
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

    private IEnumerator EndPlanMode()
    {
        Debug.Log("Finished Plan Mode, Do Stuff pass back to PMC ");

        yield return new WaitForSeconds(1.0f);
        planModeController.EndPlanMode();

    }
}