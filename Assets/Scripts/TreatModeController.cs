using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreatModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;

    // Canvas Objects
    // Canvas Objects
    public GameObject treatModeIntroObj;
    private ModeIntro treatModeIntro;


    public GameObject progress;
    private TreatProgress treatProgress;

    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;

    public GameObject treatMode;

    public int timeRemaining;
    public float progressVal;
    public int onTargetTaps = 0;
    public int offTargetTaps = 0;



    void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        treatProgress = progress.GetComponent<TreatProgress>();
        treatModeIntro = treatModeIntroObj.GetComponent<ModeIntro>();
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
        treatModeIntroObj.SetActive(true);
        treatMode.SetActive(true);
        StartCoroutine(treatModeIntro.RunFullIntro(StartTreatMode));
    }


    public void StartTreatMode()
    {
        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        Debug.Log("In TreatModeController StartTreatMode");
        treatModeIntroObj.SetActive(false);
    }


    // this will go into stats at the end: 
    public void EndTreatMode()
    {
        Debug.Log("Finished Treat:: ie Complete, do stuff, set off an 'end of plan mode' thing ");

        countdownTimer.CancelCountdown();
        Debug.Log("You had " + timeRemaining.ToString() + "seconds left");
        Debug.Log($"You completed {(int)(progressVal * 100f)} % of the treatment");
        Debug.Log("You had " + onTargetTaps.ToString() + " and " + offTargetTaps.ToString() + "off target");

        gameManager.EndOfTreatMode();

        //gameManager.EndO

    }

    public void OnTargetTaps()
    {
        onTargetTaps++;
    }

    public void OffTargetTaps()
    {
        offTargetTaps++;
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

    public void UpdateProgress(float progress, int dosesDelivered)
    {
        progressVal = progress;
        treatProgress.ShowProgress(dosesDelivered);
    }


}
