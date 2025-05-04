using UnityEngine;
using System.Collections;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownClock;

    //public Material standardCountdown;


    public int duration = 30;
    public bool complete = false;

    private int timeRemaining;

    public void StartCountdown()
    {
        complete = false;
        timeRemaining = duration;
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        while (timeRemaining > 0)
        {
            countdownClock.text = $"00:{timeRemaining:00}";
            // Add flash effect here if you want
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        countdownClock.text = "00:00";
        complete = true;
    }
}


