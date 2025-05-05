using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreatModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;

    // Canvas Objects
    //public GameObject treatModeIntro;
    // public GameObject planModePrompt;

    public GameObject progress;
    private TreatProgress treatProgress;
    private float progressVal;
    private int dosesDelivered;


    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;
    private int timeRemaining;


    public GameObject treatMode;



    void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        treatProgress = progress.GetComponent<TreatProgress>();
    }

    private void Update()
    {
        timeRemaining = countdownTimer.timeRemaining;
        if (countdownTimer != null && countdownTimer.complete)
        {
            countdownTimer.CancelCountdown();
            EndTreatMode();
        }
    }

    public void StartTreatModeIntro()
    {
        // get the backgrounds & initalise the scripts
        // disable the colliders 
        treatMode.SetActive(true);
        gameManager.EnableAllColliders(treatMode, false);
    }


    public void StartTreatMode()
    {
        treatMode.SetActive(true);
        gameManager.EnableAllColliders(treatMode, true);
        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        Debug.Log("In TreatModeController StartTreatMode");
    }

    // this will go into stats at the end: 
    public void EndTreatMode()
    {
        Debug.Log("Finished Treat:: ie Complete, do stuff, set off an 'end of plan mode' thing ");

        countdownTimer.CancelCountdown();
        Debug.Log("You had " + timeRemaining.ToString() + "seconds left");
        Debug.Log($"You completed {(int)(progressVal * 100f)} % of the treatment");



    }


    // Canvas Overlay Methods 
    private void SetUpProgressIndicator()
    {
        progress.SetActive(true);
        //progressIndicator = progress.transform.Find("Progress").GetComponent<Image>();
        //progressIndicator.fillAmount = 0.0f;

    }

    private void SetUpCountdownIndicator()
    {
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();
    }

    public void UpdateProgress(float progress, int dosesDelivered)
    {
        progressVal = progress;
        treatProgress.ShowProgress(dosesDelivered);
    }


}
