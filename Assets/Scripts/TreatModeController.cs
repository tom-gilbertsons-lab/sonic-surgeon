using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;


public class TreatModeController : MonoBehaviour
{
    [Header("Game Manager")]
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public CanvasEffects canvasEffects;
    private CRSTScale crstScale;


    // Canvas Objects
    [Header("TreatModeUI")]
    public GameObject modeUI;
    public GameObject introCanvas;
    private Intro intro;
    public GameObject crst4;
    public GameObject crst3;
    public GameObject crst2;
    public GameObject crst1;
    public GameObject crst0;
    public GameObject progressDisplayObj;
    private ProgressIndicator progressDisplay;
    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;

    public GameObject treatSceneObj;

    public int timeRemaining;
    public float progressVal;
    public int onTargetTaps = 0;
    public int offTargetTaps = 0;


    void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        progressDisplay = progressDisplayObj.GetComponent<ProgressIndicator>();
        intro = introCanvas.GetComponent<Intro>();
        treatSceneObj.GetComponent<BoxCollider2D>().enabled = false;
        crstScale = gameManagerObject.GetComponent<CRSTScale>();
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


    public void StartIntro()
    {
        modeUI.SetActive(true);
        StartCoroutine(StartIntroRoutine());
    }

    private IEnumerator StartIntroRoutine()
    {
        yield return StartCoroutine(canvasEffects.FadeInRoutine(introCanvas, 1f, fadeChildrenGraphics: true));
        yield return StartCoroutine(intro.RunIntro(StartTreatMode));
    }

    public void StartTreatMode()
    {
        StartCoroutine(canvasEffects.FadeOutRoutine(introCanvas, 1.0f, fadeChildrenGraphics: true));
        SetUpOverlays();
        treatSceneObj.GetComponent<BoxCollider2D>().enabled = true;
    }



    // this will go into stats at the end: 
    public void EndTreatMode()
    {
        gameManager.EndTreatMode();
    }

    public void DeactivateTreatMode()
    {
        treatSceneObj.SetActive(false);
    }

    public void StopCountdown()
    {
        countdownTimer.CancelCountdown();
    }

    public void HideOverlays()
    {
        canvasEffects.FadeOut(countdownDisplay, 0.5f, fadeChildrenGraphics: true);
        canvasEffects.FadeOut(progressDisplayObj, 0.5f, fadeChildrenGraphics: true);
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



    private void SetUpOverlays()
    {
        progressDisplayObj.SetActive(true);
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();

    }




    public void UpdateProgress(float progress, int dosesDelivered)
    {
        // update progress display
        progressVal = progress;
        progressDisplay.progress = progress;
        progressDisplay.ApplyProgress();

        if (progressVal > 0.99f)
        {
            crstScale.CRST0();
            SetMessage(crst0);
        }
        else if (progressVal > 0.75f)
        {
            crstScale.CRST1();
            SetMessage(crst1);
        }
        else if (progressVal > 0.50f)
        {
            crstScale.CRST2();
            SetMessage(crst2);
        }
        else if (progressVal > 0.25f)
        {
            crstScale.CRST3();
            SetMessage(crst3);
        }
        else
        {
            crstScale.CRST4();
            SetMessage(crst4);
        }

    }

    private void SetMessage(GameObject liveMessage)
    {
        crst4.SetActive(false);
        crst3.SetActive(false);
        crst2.SetActive(false);
        crst1.SetActive(false);
        crst0.SetActive(false);

        liveMessage.SetActive(true);

    }
}






