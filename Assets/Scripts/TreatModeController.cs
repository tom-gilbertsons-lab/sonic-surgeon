using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TreatModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;

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
        treatModeIntroObj.SetActive(true);
        StartCoroutine(treatModeIntro.RunFullIntro(StartTreatMode));
    }

    public void StartTreatMode()
    {
        if (!treatMode.activeSelf)
        {
            treatMode.SetActive(true);
        }

        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        Debug.Log("In TreatModeController StartTreatMode");
        treatModeIntroObj.SetActive(false);
    }

    // this will go into stats at the end: 
    public void EndTreatMode()
    {
        countdownTimer.CancelCountdown();
        gameManager.EndTreatMode();
    }

    public void DeactivateTreatMode()
    {
        treatMode.SetActive(false);
        progress.SetActive(false);
        countdownDisplay.SetActive(false);
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
