using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlanModeController : MonoBehaviour
{
    // Game Mananger Objects 
    public GameObject gameManagerObject;
    private GameManager gameManager;

    // Canvas Objects
    public GameObject planModeIntro;
    public GameObject planModePrompt;

    public GameObject progress;
    private Image progressIndicator;

    public GameObject countdownDisplay;
    private CountdownTimer countdownTimer;

    public int duration = 30;
    private int timeRemaining;


    public GameObject planMode;


    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        countdownTimer = countdownDisplay.GetComponent<CountdownTimer>();
    }


    private void Update()
    {
        if (countdownTimer != null && countdownTimer.complete)
        {
            countdownTimer.complete = false; //to stop another execution
            StartCoroutine(EndPlanMode());
        }
    }

    public void StartPlanModeIntro()
    {
        // get the backgrounds & initalise the scripts 
        planMode.SetActive(true);
        planMode.transform.Find("Round1").gameObject.SetActive(false);
        planMode.transform.Find("Round2").gameObject.SetActive(false);
        planMode.transform.Find("Round3").gameObject.SetActive(false);

    }


    public void StartPlanMode()
    {
        SetUpProgressIndicator();
        SetUpCountdownIndicator();
        Debug.Log("In PlanModeController StartPlanMode");
        planMode.SetActive(true); // when this is active it activates the scripts in PlanMode
    }


    public IEnumerator EndPlanMode()
    {
        Debug.Log("Finished Plan:: ie Complete, do stuff, set off an 'end of plan mode' thing ");

        yield return new WaitForSeconds(1.0f);

    }

    // Canvas Overlay Methods 
    private void SetUpProgressIndicator()
    {
        progress.SetActive(true);
        progressIndicator = progress.transform.Find("Progress").GetComponent<Image>();
        progressIndicator.fillAmount = 0.0f;

    }

    private void SetUpCountdownIndicator()
    {
        countdownDisplay.SetActive(true);
        countdownTimer.StartCountdown();
    }

    public void UpdateProgress(float progress)
    {
        progressIndicator.fillAmount = progress;
    }

}

