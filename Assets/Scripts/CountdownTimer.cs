using UnityEngine;
using System.Collections;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text countdownClock;

    //public Material standardCountdown;


    public int duration = 30;
    public bool complete = false;

    public int timeRemaining;

    public void StartCountdown()
    {
        complete = false;
        timeRemaining = duration;
        StartCoroutine(CountdownRoutine());
    }

    public void CancelCountdown()
    {
        StopAllCoroutines();
        complete = false;
    }

    private IEnumerator CountdownRoutine()
    {
        while (timeRemaining > 0 && !complete)
        {
            countdownClock.text = $"00:{timeRemaining:00}";
            // Add flash effect here if you want
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        if (!complete)
        {
            countdownClock.text = "00:00";
            complete = true;
        }

    }



}


