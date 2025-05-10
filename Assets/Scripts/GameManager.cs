using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

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
    {   // Reset all transition UI screens
        //planSuccessScreen.SetActive(false);
        //planFailScreen.SetActive(false);
        //endGameScreen.SetActive(false);
        //Debug.Log("In GameManager Start");
        planModeController.StartPlanMode();
        //startScreen.SetActive(true);
        //treatModeController.StartTreatMode();

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
        GrabTreatStats();
        StartCoroutine(EndGameTransition());


    }

    private IEnumerator EndGameTransition()
    {
        ActivateAndFadeInUI(endGameScreen, 0.5f);
        yield return new WaitForSeconds(1.0f);
        treatModeController.DeactivateTreatMode();
    }


    private IEnumerator PlanToTreatTransition()
    {
        ActivateAndFadeInUI(planSuccessScreen, 0.5f);
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
        planOnTarget = treatModeController.onTargetTaps;
        planOffTarget = treatModeController.offTargetTaps;
        planTimeRemaining = treatModeController.timeRemaining;
        planProgressVal = treatModeController.progressVal;
    }

    public void EndSession()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //rPI

    public void TurnOnLED()
    {
        StartCoroutine(SendLEDONRequest());
    }

    IEnumerator SendLEDONRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:8000/on");
        yield return request.SendWebRequest();
        // Optional: Add error checking here if needed
    }


    public void TurnOffLED()
    {
        StartCoroutine(SendLEDOFFRequest());
    }

    IEnumerator SendLEDOFFRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:8000/off");
        yield return request.SendWebRequest();
        // Optional: Add error checking here if needed
    }


}

