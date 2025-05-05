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

    public Material greenMaterial;
    public Material redMaterial;

    public void SetStats(string title, int onTarget, int offTarget, float timeRemaining, float progressVal)
    {
        // Set title (always same material)
        titleTMP.text = title;

        // Update main stat lines
        onTargetHitTMP.text = $"On Target Hits:    {onTarget}";
        offTargetHitTMP.text = $"Off Target Hits:   {offTarget}";
        timeRemainingTMP.text = $"Time Remaining:    {timeRemaining:F1}s";
        progressTMP.text = $"Progress:          {(progressVal * 100f):F0}%";

        //// Logic for colour/material
        //Material mat = (progressVal < 1f || onTarget == 0) ? redAlertMaterial : normalMaterial;

        //onTargetHitTMP.fontSharedMaterial = mat;
        //offTargetHitTMP.fontSharedMaterial = mat;
        //timeRemainingTMP.fontSharedMaterial = mat;
        //progressTMP.fontSharedMaterial = mat;
    }
}
