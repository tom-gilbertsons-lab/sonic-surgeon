using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlanModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;

    // Canvas Objects
    //public GameObject planModeIntro;
    // public GameObject planModePrompt;

    public GameObject progress;
    private PlanProgress planProgress;
    private float progressVal;

    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;
    private int timeRemaining;


    public GameObject planMode;


    void Awake()
    {
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        planProgress = progress.GetComponent<PlanProgress>();
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
        // get the backgrounds & initalise the scripts 
        planMode.SetActive(true);
        planMode.transform.Find("Round1").gameObject.SetActive(false);
        planMode.transform.Find("Round2").gameObject.SetActive(false);
        planMode.transform.Find("Round3").gameObject.SetActive(false);

    }


    public void StartPlanMode()
    {
        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        Debug.Log("In PlanModeController StartPlanMode");
        planMode.SetActive(true); // when this is active it activates the scripts in PlanMode
    }


    public void EndPlanMode()
    {
        Debug.Log("Finished Plan ");
        countdownTimer.CancelCountdown();
        Debug.Log("You had " + timeRemaining.ToString() + "seconds left");
        Debug.Log($"You completed {(int)(progressVal * 100f)} % of the plan");



    }

    // Canvas Overlay Methods 
    private void SetUpProgressIndicator()
    {
        progress.SetActive(true);

    }

    private void SetUpCountdownIndicator()
    {
        Debug.Log(countdownTimer);
        Debug.Log(countdownDisplay);
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

