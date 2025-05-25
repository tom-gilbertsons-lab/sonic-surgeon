using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;


public class TreatModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;
    private LANMotorCtrl lANMotorCtrl;

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

    private float lastShakeLevel = -1f;


    void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        treatProgress = progress.GetComponent<TreatProgress>();
        treatModeIntro = treatModeIntroObj.GetComponent<ModeIntro>();
        lANMotorCtrl = gameManagerObject.GetComponent<LANMotorCtrl>();
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
        lANMotorCtrl.StartShake();
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
        gameManager.EndTreatMode();
    }

    public void DeactivateTreatMode()
    {
        treatMode.SetActive(false);
        progress.SetActive(false);
        countdownDisplay.SetActive(false);
    }

    public void StopCountdown()
    {
        countdownTimer.CancelCountdown();
    }

    public void HideCountdownAndProgress()
    {
        gameManager.FadeOutAndDeactivateUI(countdownDisplay, 0.5f);
        gameManager.FadeOutAndDeactivateUI(progress, 0.5f);
    }

    public void OnTargetTaps()
    {
        onTargetTaps++;
        Debug.Log(onTargetTaps.ToString());
    }

    public void OffTargetTaps()
    {
        offTargetTaps++;
        Debug.Log(offTargetTaps.ToString());
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
        Debug.Log(progressVal.ToString());
        treatProgress.ShowProgress(dosesDelivered);


        if (progressVal > 0.99f && lastShakeLevel != 0)
        {
            lANMotorCtrl.StopShake();
            lastShakeLevel = 0;
        }
        else if (progressVal > 0.5f && progressVal <= 0.99f && lastShakeLevel != 1)
        {
            lANMotorCtrl.ShakeLo();
            lastShakeLevel = 1;
        }
        else if (progressVal <= 0.5f && lastShakeLevel != 2)
        {
            lANMotorCtrl.ShakeMid();
            lastShakeLevel = 2;
        }
    }
}


