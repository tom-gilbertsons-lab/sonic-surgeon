using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StatBuilder : MonoBehaviour
{
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI onTargetHitTMP;
    public TextMeshProUGUI offTargetHitTMP;
    public TextMeshProUGUI timeRemainingTMP;
    public TextMeshProUGUI progressTMP;

    public CanvasEffects canvasEffects;

    public Material greenMaterial;
    public Material amberMaterial;
    public Material redMaterial;

    public void SetPlanStats(string title, int onTarget, int offTarget, int timeRemaining, float progressVal)
    {
        // Set text values
        titleTMP.text = title;



        onTargetHitTMP.text = $"On Target Hits:    {onTarget}";

        if (onTarget >= 3)
        {
            onTargetHitTMP.fontSharedMaterial = greenMaterial;
        }
        else
        {
            onTargetHitTMP.fontSharedMaterial = redMaterial;
        }


        offTargetHitTMP.text = $"Off Target Hits:   {offTarget}";
        if (offTarget >= 15)
        {
            onTargetHitTMP.fontSharedMaterial = redMaterial;
        }
        else if (offTarget >= 12)
        {
            onTargetHitTMP.fontSharedMaterial = amberMaterial;
        }
        else
        {
            onTargetHitTMP.fontSharedMaterial = greenMaterial;
        }


        timeRemainingTMP.text = $"Time Remaining:    {timeRemaining}s";

        if (timeRemaining >= 15)
        {
            timeRemainingTMP.fontSharedMaterial = greenMaterial;
        }
        else if (timeRemaining > 0)
        {
            timeRemainingTMP.fontSharedMaterial = amberMaterial;
        }
        else
        {
            timeRemainingTMP.fontSharedMaterial = redMaterial;
        }

        progressTMP.text = $"Progress:         {progressVal * 100f:F0}%";

        if (progressVal >= 0.999)
        {
            progressTMP.fontSharedMaterial = greenMaterial;
        }
        else
        {
            progressTMP.fontSharedMaterial = redMaterial;
        }

    }


    public void SetTreatStats(string title, int onTarget, int offTarget, int timeRemaining, float progressVal)
    {
        // Set text values
        titleTMP.text = title;


        Debug.Log("In Treat Stats");



        onTargetHitTMP.text = $"On Target Hits:    {onTarget}";

        if (onTarget >= 23)
        {
            onTargetHitTMP.fontSharedMaterial = greenMaterial;
        }
        else
        {
            onTargetHitTMP.fontSharedMaterial = redMaterial;
        }


        offTargetHitTMP.text = $"Off Target Hits:   {offTarget}";
        if (offTarget >= 15)
        {
            onTargetHitTMP.fontSharedMaterial = redMaterial;
        }
        else if (offTarget >= 10)
        {
            onTargetHitTMP.fontSharedMaterial = amberMaterial;
        }
        else
        {
            onTargetHitTMP.fontSharedMaterial = greenMaterial;
        }


        timeRemainingTMP.text = $"Time Remaining:    {timeRemaining}s";

        if (timeRemaining >= 15)
        {
            timeRemainingTMP.fontSharedMaterial = greenMaterial;
        }
        else if (timeRemaining > 0)
        {
            timeRemainingTMP.fontSharedMaterial = amberMaterial;
        }
        else
        {
            timeRemainingTMP.fontSharedMaterial = redMaterial;
        }

        progressTMP.text = $"Progress:         {progressVal * 100f:F0}%";

        if (progressVal >= 0.999)
        {
            progressTMP.fontSharedMaterial = greenMaterial;
        }
        else
        {
            progressTMP.fontSharedMaterial = redMaterial;
        }

    }
}