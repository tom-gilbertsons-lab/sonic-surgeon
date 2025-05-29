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

    // CanvasObjects
    public GameObject planModeUI;

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
        planModeUI.SetActive(true);
    }


    private void Start()
    {
        startScreen.SetActive(true);
    }

    public void StartGame()
    {
        FadeOutAndDeactivateUI(startScreen, 0.5f, new[] { "TitleObject", "Button" });
        planModeController.StartIntro();
    }













    public void EndPlanMode()
    {
        Debug.Log("In GameManager, end of Plan Mode");
        EndSession();
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
        cg.alpha = 0f;
        StartCoroutine(canvasEffects.FadeCanvasGroup(cg, 0f, 1f, duration));
    }

    public void FadeOutAndDeactivateUI(GameObject obj, float duration, string[] childNamesToDeactivate = null)
    {
        // Deactivate named child objects if specified
        if (childNamesToDeactivate != null)
        {
            foreach (string childName in childNamesToDeactivate)
            {
                Transform child = obj.transform.Find(childName);
                if (child != null)
                    child.gameObject.SetActive(false);
                else
                    Debug.LogWarning($"Child '{childName}' not found under '{obj.name}'");
            }
        }

        // Fade out the primary object's canvas group
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            StartCoroutine(canvasEffects.FadeCanvasGroup(cg, 1f, 0f, duration, true));
        }
        else
        {
            Debug.LogWarning("No CanvasGroup found on object: " + obj.name);
            obj.SetActive(false);
        }
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

    public void EnableAllColliders(GameObject gameObject, bool enable)
    {
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
        foreach (var col in colliders)
        {
            col.enabled = enable;
        }
    }

}



