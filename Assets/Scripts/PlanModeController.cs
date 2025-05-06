using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlanModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;

    // Canvas Objects
    public GameObject planModeIntroObj;
    private ModeIntro planModeIntro;

    public GameObject progress;
    private PlanProgress planProgress;

    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;


    public bool planModeComplete;

    public GameObject planMode;

    public int timeRemaining;
    public float progressVal;
    public int onTargetTaps = 0;
    public int offTargetTaps = 0;

    void Awake()
    {
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        planProgress = progress.GetComponent<PlanProgress>();
        planModeIntro = planModeIntroObj.GetComponent<ModeIntro>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
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


    public void StartPlanModeIntro()
    {
        planModeIntroObj.SetActive(true);
        StartCoroutine(planModeIntro.RunFullIntro(StartPlanMode));
    }


    public void StartPlanMode()
    {
        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        planModeIntroObj.SetActive(false);
    }


    public void EndPlanMode()
    {
        countdownTimer.CancelCountdown();

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
        planMode.SetActive(false);
        progress.SetActive(false);
        countdownDisplay.SetActive(false);
    }


    public void UpdateTapStats(int onTarget, int offTarget)
    {
        onTargetTaps = onTarget;
        offTargetTaps = offTarget;
    }

    // Canvas Overlay Methods 
    private void SetUpProgressIndicator()
    {
        progress.SetActive(true);

    }

    private void SetUpCountdownIndicator()
    {
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();
    }


    public void UpdateProgress(float progress)
    {
        progressVal = progress;
        if (progress < 0.34)
        {
            planProgress.SetStage1();
        }

        if ((progress > 0.34) && (progress < 0.67))
        {
            planProgress.SetStage1();
            planProgress.SetStage2();
        }

        if (progress > 0.9)
        {
            planProgress.SetStage1();
            planProgress.SetStage2();
            planProgress.SetStage3();
        }
    }

}

