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

    // Mode Controllers 
    private PlanModeController planModeController;
    private TreatModeController treatModeController;



    // Transition Elper Script: move o PCS when I'm ready 
    public GameObject planCompleteScreen;
    public Image planCompleteBackground;
    public TextMeshProUGUI planCompleteText;
    public CanvasEffects canvasEffects;

    public GameObject statsBox;
    public StatBuilder statBuilder;


    // Stats at the end: 
    private int planOnTarget;
    private int planOffTarget;
    private float planTimeRemaining;
    private float planProgressVal;

    private int treatOnTarget;
    private int treatOffTarget;
    private float treatTimeRemaining;
    private float treatProgressVal;


    private void Awake()
    {
        planModeController = GetComponent<PlanModeController>();
        treatModeController = GetComponent<TreatModeController>();

    }


    private void Start()
    {
        //RunStartScreen();
        Debug.Log("In GameManager Start");




        planModeController.StartPlanModeIntro(); // works 
        //treatModeController.StartTreatModeIntro(); // works 

    }

    // we will pull in complete time, n taps on target, off target in both of these 
    // for a final summary I think

    public void EndOfPlanMode()
    {
        Debug.Log("In GameManager, end of Plan Mode");

        if (planModeController.planModeComplete)
        {
            Debug.Log("Show Congratulations Screen + button continue to treat");
            StartCoroutine(ShowPlanCompleteScreen());
        }
        else
        {
            Debug.Log("Show Planning Failed- return to medical school");
        }
    }

    public void EndOfTreatMode()
    {
        Debug.Log("In GameManager, end of Treat Mode");
    }

    private IEnumerator ShowPlanCompleteScreen()
    {
        planCompleteScreen.SetActive(true);

        // Set initial alpha
        var bgCol = planCompleteBackground.color;
        bgCol.a = 0;
        planCompleteBackground.color = bgCol;

        var txtCol = planCompleteText.color;
        txtCol.a = 0;
        planCompleteText.color = txtCol;

        // Fade in background and text
        yield return StartCoroutine(canvasEffects.FadeImage(planCompleteBackground, 0f, 1f, 1.5f));
        planModeController.DeactivatePlanMode();
        yield return StartCoroutine(canvasEffects.FadeText(planCompleteText, 0f, 1f, 2f));

        yield return new WaitForSeconds(10f);

        planCompleteScreen.SetActive(false);
        treatModeController.StartTreatModeIntro();
    }




    // Utils

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

