using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;

    public GameObject planModeIntro;


    public GameObject planModePrompt; 

    public GameObject planMode;
    public GameObject treatMode;


    public GameObject CountdownOverlay;
    public TMP_Text CountdownTimer;


    public GameObject brainPulse;




    public int planTime = 30;
    private bool inCountdown = true;

    private int timeRemaining;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
       planMode.SetActive(false);
       planModeIntro.SetActive(false);
    }

    public void PressStart()
    {
        StartCoroutine(StartGame());
;    }

    public IEnumerator StartGame()
    {
        Debug.Log("Pressed Start Game");
   
        yield return new WaitForSeconds(1.0f);
        
        startScreen.SetActive(false);

        planMode.SetActive(true);
        EnableAllColliders(planMode, false);
        planModeIntro.SetActive(true);
        yield return StartCoroutine(RunPrompt(planModePrompt));

        BeginPlanMode();
       // StartCoroutine(CountdownTimerRoutine(30));
    }


    public void BeginPlanMode()
    {
        TurnOffLED();
        CountdownOverlay.SetActive(true);
        CountdownTimer.text = "00:30";
        EnableAllColliders(planMode, true);
    }




    public void EndPlanMode()
    {
        planMode.SetActive(false);
        TurnOnLED();
        StartCoroutine(WaitForSecs(5.0f));
        BeginTreatMode();
    }

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



    public void BeginTreatMode()
    {
        //TurnOffLED();
        treatMode.SetActive(true);

    }


    public void EndTreatMode()
    {
        StartCoroutine(WaitForSecs(1.0f));
        treatMode.SetActive(false);
        EndSession();
    }

    public IEnumerator WaitForSecs(float secs)
    { 
        yield return new WaitForSeconds(secs);

    }



    private IEnumerator CountdownTimerRoutine(int duration)
    {
        timeRemaining = duration;
        while (timeRemaining > 0)
        {
            CountdownTimer.text = $"00:{timeRemaining:00}";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        CountdownTimer.text = "00:00";
    }

    // utilities 

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

    public IEnumerator RunPrompt(GameObject promptObject)
    {
        promptObject.SetActive(true);
        yield return StartCoroutine(promptObject.GetComponent<PromptCountdown>().PromptOpener());
        promptObject.SetActive(false);
    }

}

