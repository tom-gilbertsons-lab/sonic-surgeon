using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlanModeController : MonoBehaviour
{

    [Header("Game Manager")]
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public CanvasEffects canvasEffects;
    private LANMotorCtrl lANMotorCtrl;


    // Canvas Objects
    [Header("PlanModeUI")]
    public GameObject modeUI;
    public GameObject introCanvas;
    private Intro intro;
    public GameObject msg1;
    public GameObject msg2;
    public GameObject msg3;
    public GameObject progressDisplayObj;
    private ProgressIndicator progressDisplay;
    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;


    [Header("Report Stats")]
    public int timeRemaining;
    public float progressVal;
    public int onTargetTaps = 0;
    public int offTargetTaps = 0;


    public bool planModeComplete;

    public GameObject planSceneObj;
    private PlanMode planMode;


    void Awake()
    { // script refs 
        gameManager = gameManagerObject.GetComponent<GameManager>();
        progressDisplay = progressDisplayObj.GetComponent<ProgressIndicator>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
        intro = introCanvas.GetComponent<Intro>();
        planMode = planSceneObj.GetComponent<PlanMode>();
        lANMotorCtrl = gameManagerObject.GetComponent<LANMotorCtrl>();
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
        modeUI.SetActive(true);
        StartCoroutine(StartIntroRoutine());
    }

    private IEnumerator StartIntroRoutine()
    {
        yield return StartCoroutine(canvasEffects.FadeInRoutine(introCanvas, 1f, fadeChildrenGraphics: true));
        yield return StartCoroutine(intro.RunIntro(StartPlanMode));
    }


    public void StartPlanMode()
    {
        StartCoroutine(canvasEffects.FadeOutRoutine(introCanvas, 1.0f, fadeChildrenGraphics: true));
        SetUpOverlays();
        planMode.StartPlan();
        lANMotorCtrl.StartShake();
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
        modeUI.SetActive(false);

    }

    public void DeactivatePlanMode()
    {
        planSceneObj.SetActive(false);
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



    public void UpdateTapStats(int onTarget, int offTarget)
    {
        onTargetTaps = onTarget;
        offTargetTaps = offTarget;
    }

    // Canvas Overlay Methods 
    private void SetUpOverlays()
    {
        progressDisplayObj.SetActive(true);
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();

    }


    public void UpdateProgress(int stage)
    {
        switch (stage)
        {
            case 1:
                progressDisplay.progress = 0.33f;
                progressVal = 0.33f;
                StartCoroutine(InfoBox(msg1, false));
                break;
            case 2:
                progressDisplay.progress = 0.66f;
                progressVal = 0.66f;
                StartCoroutine(InfoBox(msg2, false));
                break;
            case 3:
                progressDisplay.progress = 1f;
                progressVal = 1f;
                StartCoroutine(InfoBox(msg3, true));
                break;
        }

        progressDisplay.ApplyProgress();
    }


    public IEnumerator InfoBox(GameObject obj, bool end)
    {

        if (!end)
        {
            yield return canvasEffects.FadeInRoutine(obj, 0.25f, fadeChildrenGraphics: true);
            yield return new WaitForSeconds(1f);
            yield return canvasEffects.FadeOutRoutine(obj, 0.25f, fadeChildrenGraphics: true);
        }
        else
        {
            yield return canvasEffects.FadeInRoutine(obj, 0.5f, fadeChildrenGraphics: true);
            yield return new WaitForSeconds(2f);
            yield return canvasEffects.FadeOutRoutine(obj, 0.25f, fadeChildrenGraphics: true);
        }
    }

}

