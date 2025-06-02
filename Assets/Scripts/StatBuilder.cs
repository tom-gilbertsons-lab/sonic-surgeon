using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBuilder : MonoBehaviour
{
    public GameObject onTargetGroup;
    public GameObject offTargetGroup;
    public GameObject timeRemainingGroup;
    public GameObject progressGroup;

    public CanvasEffects canvasEffects;

    public Material greenMaterial;
    public Material amberMaterial;
    public Material redMaterial;

    public void SetPlanStats(int onTarget, int offTarget, int timeRemaining, float progressVal)
    {
        // ON TARGET
        Material onTargetMat;
        if (onTarget >= 3)
        {
            onTargetMat = greenMaterial;
        }
        else
        {
            onTargetMat = redMaterial;
        }
        ApplyStatGroupStyle(onTargetGroup, onTargetMat, onTarget.ToString());

        // OFF TARGET
        Material offTargetMat;
        if (offTarget == 0)
        {
            offTargetMat = greenMaterial;
        }
        else if (offTarget < 8)
        {
            offTargetMat = amberMaterial;
        }
        else
        {
            offTargetMat = redMaterial;
        }
        ApplyStatGroupStyle(offTargetGroup, offTargetMat, offTarget.ToString());

        // TIME REMAINING
        Material timeMat;
        if (timeRemaining >= 15)
        {
            timeMat = greenMaterial;
        }
        else if (timeRemaining > 0)
        {
            timeMat = amberMaterial;
        }
        else
        {
            timeMat = redMaterial;
        }
        ApplyStatGroupStyle(timeRemainingGroup, timeMat, timeRemaining + "s");

        // PROGRESS
        Material progMat;
        if (progressVal >= 0.9f)
        {
            progMat = greenMaterial;
        }
        else
        {
            progMat = redMaterial;
        }
        ApplyStatGroupStyle(progressGroup, progMat, $"{progressVal * 100f:F0}%");
    }


    public void SetTreatStats(string title, int onTarget, int offTarget, int timeRemaining, float progressVal)
    {
        Debug.Log("Set Treat unset");
    }

    private void ApplyStatGroupStyle(GameObject group, Material mat, string text)
    {
        TextMeshProUGUI[] tmps = group.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in tmps)
        {
            tmp.fontSharedMaterial = mat;
            if (tmp.gameObject.name == "Val")
            {
                tmp.text = text;
            }
        }

        // Safely fetch face color for TMP shader
        if (mat.HasProperty(ShaderUtilities.ID_FaceColor))
        {
            Color faceColor = mat.GetColor(ShaderUtilities.ID_FaceColor);
            Image img = group.GetComponentInChildren<Image>(true);
            if (img != null)
            {
                img.color = faceColor;
            }
        }
    }


}
