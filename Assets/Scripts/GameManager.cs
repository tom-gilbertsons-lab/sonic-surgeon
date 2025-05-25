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




    public GameObject progressIndicator;

    private void Awake()
    {
        planModeController = GetComponent<PlanModeController>();
        treatModeController = GetComponent<TreatModeController>();
    }


    private void Start()
    {
        // startScreen.SetActive(true);
        progressIndicator.SetActive(true);
    }
    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    public IEnumerator StartGameRoutine()
    {
        FadeOutAndDeactivateUI(startScreen, 2.0f);
        yield return new WaitForSeconds(2.0f);
        planModeController.StartPlanModeIntro();
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
            ActivateAndFadeInUI(planFailScreen, 0.5f);
        }
    }

    public void EndTreatMode()
    {
        Debug.Log("back in GameManager post treat");
        GrabTreatStats();
        StartCoroutine(EndGameTransition());


    }

    private IEnumerator EndGameTransition()
    {
        ActivateAndFadeInUI(endGameScreen, 1.0f);
        treatModeController.DeactivateTreatMode();
        yield return new WaitForSeconds(1.0f);

    }


    private IEnumerator PlanToTreatTransition()
    {
        ActivateAndFadeInUI(planSuccessScreen, 1.0f);
        yield return new WaitForSeconds(1.0f);
        planModeController.DeactivatePlanMode();
        yield return new WaitForSeconds(3f);
        FadeOutAndDeactivateUI(planSuccessScreen, 0.5f);
        treatModeController.StartTreatModeIntro();
    }

    // Utils

    public void ActivateAndFadeInUI(GameObject obj, float duration)
    {
        obj.SetActive(true);
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        StartCoroutine(canvasEffects.FadeCanvasGroup(cg, 0f, 1f, duration));
    }

    public void FadeOutAndDeactivateUI(GameObject obj, float duration)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        StartCoroutine(canvasEffects.FadeOutCanvasGroup(cg, duration, true));
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



