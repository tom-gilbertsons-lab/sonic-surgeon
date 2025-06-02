using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Canvas Objects 
    public GameObject startScreen;
    public CanvasEffects canvasEffects;

    // Mode Controllers 
    private PlanModeController planModeController;
    private TreatModeController treatModeController;

    // Transition Objects 
    public GameObject planSuccessScreen;
    public GameObject planFailScreen;
    public GameObject endGameScreen;

    // Stats at the end: 
    public int planOnTarget;
    public int planOffTarget;
    public int planTimeRemaining;
    public float planProgressVal;

    public int treatOnTarget;
    public int treatOffTarget;
    public int treatTimeRemaining;
    public float treatProgressVal;


    private void Awake()
    {
        planModeController = GetComponent<PlanModeController>();
        treatModeController = GetComponent<TreatModeController>();
    }


    private void Start()
    {
        startScreen.SetActive(true);
    }


    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        planModeController.planSceneObj.SetActive(true);
        //yield return canvasEffects.FadeOutRoutine(startScreen, 1.0f, fadeChildrenGraphics: true);
        StartCoroutine(canvasEffects.FadeOutRoutine(startScreen, 1.0f, fadeChildrenGraphics: true));
        yield return new WaitForSeconds(0.5f);
        planModeController.StartIntro();

    }

    public void EndPlanMode()
    {
        Debug.Log("In GameManager, end of Plan Mode");
        GrabPlanStats();

        if (planModeController.planModeComplete)
        {
            StartCoroutine(PlanToTreatTransition());
        }
        else
        {
            StartCoroutine(canvasEffects.FadeInRoutine(endGameScreen, 1.0f, fadeChildrenGraphics: true));

        }
    }

    private IEnumerator PlanToTreatTransition()
    {
        yield return canvasEffects.FadeInRoutine(planSuccessScreen, 1.0f, fadeChildrenGraphics: true);
        yield return new WaitForSeconds(1.0f);
        planModeController.DeactivatePlanMode();
        yield return new WaitForSeconds(2f);
        yield return canvasEffects.FadeOutRoutine(planSuccessScreen, 1.0f, fadeChildrenGraphics: true);
        treatModeController.treatSceneObj.SetActive(true);
        treatModeController.StartIntro();
    }

    public void EndTreatMode()
    {
        Debug.Log("back in GameManager post treat");
        GrabTreatStats();
        StartCoroutine(EndGameTransition());
    }

    private IEnumerator EndGameTransition()
    {
        yield return canvasEffects.FadeInRoutine(endGameScreen, 1.0f, fadeChildrenGraphics: true);
        treatModeController.DeactivateTreatMode();
        yield return new WaitForSeconds(1.0f);

    }

    private void GrabPlanStats()
    {
        planOnTarget = planModeController.onTargetTaps;
        planOffTarget = planModeController.offTargetTaps;
        planTimeRemaining = planModeController.timeRemaining;
        planProgressVal = planModeController.progressVal;
    }

    private void GrabTreatStats()
    {
        treatOnTarget = treatModeController.onTargetTaps;
        treatOffTarget = treatModeController.offTargetTaps;
        treatTimeRemaining = treatModeController.timeRemaining;
        treatProgressVal = treatModeController.progressVal;
    }

    public void EndSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}



