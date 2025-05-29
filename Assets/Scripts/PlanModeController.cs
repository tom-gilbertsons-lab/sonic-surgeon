using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlanModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public CanvasEffects canvasEffects;


    // Canvas Objects
    public GameObject planIntroCanvas;
    private Intro planIntro;
    public GameObject msg1;
    public GameObject msg2;
    public GameObject msg3;


    public GameObject progressDisplayObj;
    private ProgressIndicator progressDisplay;

    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;


    public bool planModeComplete;

    public GameObject planSceneObj;
    private PlanMode planMode;



    public int timeRemaining;
    public float progressVal;
    public int onTargetTaps = 0;
    public int offTargetTaps = 0;

    void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        progressDisplay = progressDisplayObj.GetComponent<ProgressIndicator>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        planIntro = planIntroCanvas.GetComponent<Intro>();
        planMode = planSceneObj.GetComponent<PlanMode>();
    }

    private void Update()
    {
        timeRemaining = countdownTimer.timeRemaining;
        if (countdownTimer != null && countdownTimer.complete)
        {
            countdownTimer.CancelCountdown();
            EndPlanMode();
        }
    }

    public void StartIntro()
    {
        planSceneObj.SetActive(true);
        planIntroCanvas.SetActive(true);
        StartCoroutine(planIntro.RunIntro(StartPlanMode));

    }
    public void StartPlanMode()
    {
        if (!planSceneObj.activeSelf)
        {
            planSceneObj.SetActive(true);
        }

        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        FadeInfoBox(planIntroCanvas);
        planMode.StartPlan();

    }

    public void EndPlanMode()
    {
        if (onTargetTaps >= 3)
        {
            planModeComplete = true;
        }
        else
        {
            planModeComplete = false;
        }

        gameManager.EndPlanMode();

    }

    public void DeactivatePlanMode()
    {
        planSceneObj.SetActive(false);
    }

    public void StopCountdown()
    {
        countdownTimer.CancelCountdown();
    }

    public void HideCountdownAndProgress()
    {
        gameManager.FadeOutAndDeactivateUI(countdownDisplay, 0.5f);
        gameManager.FadeOutAndDeactivateUI(progressDisplayObj, 0.5f);
    }



    public void UpdateTapStats(int onTarget, int offTarget)
    {
        onTargetTaps = onTarget;
        offTargetTaps = offTarget;
    }

    // Canvas Overlay Methods 
    private void SetUpProgressIndicator()
    {
        progressDisplayObj.SetActive(true);

    }

    private void SetUpCountdownIndicator()
    {
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();
    }

    public void UpdateProgress(int stage)
    {
        switch (stage)
        {
            case 1:
                progressDisplay.progress = 0.33f;
                StartCoroutine(InfoBox(msg1));
                break;
            case 2:
                progressDisplay.progress = 0.66f;
                StartCoroutine(InfoBox(msg2));
                break;
            case 3:
                progressDisplay.progress = 1f;
                StartCoroutine(InfoBox(msg3));
                break;
        }

        progressDisplay.ApplyProgress();
    }


    public IEnumerator InfoBox(GameObject obj)
    {
        gameManager.ActivateAndFadeInUI(obj, 0.3f);
        yield return new WaitForSeconds(2f);
        FadeInfoBox(obj);
    }

    private void FadeInfoBox(GameObject obj)
    {
        // Before starting coroutine
        Outline outline = obj.GetComponentInChildren<Outline>();
        if (outline != null) outline.enabled = false;
        gameManager.FadeOutAndDeactivateUI(obj, 0.3f);
    }
}

