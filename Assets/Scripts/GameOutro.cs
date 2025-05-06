using UnityEngine;
using TMPro;



public class GameOutro : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public GameObject statsBoxLeft;
    public GameObject statsBoxRight;
    private StatBuilder statBuilderL;
    private StatBuilder statBuilderR;

    public Material greenMaterial;
    public Material amberMaterial;
    public Material redMaterial;

    public TextMeshProUGUI titleTMP;

    private float progress;


    void Awake()
    {
        statBuilderL = statsBoxLeft.GetComponent<StatBuilder>();
        statBuilderR = statsBoxRight.GetComponent<StatBuilder>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    public void OnEnable()
    {
        progress = gameManager.treatProgressVal;
        // build the stats boxes
        statsBoxLeft.SetActive(true);
        statBuilderL.SetPlanStats("Plan Report:", gameManager.planOnTarget, gameManager.planOffTarget, gameManager.planTimeRemaining, gameManager.planProgressVal);

        statsBoxRight.SetActive(true);
        statBuilderR.SetTreatStats("Plan Report:", gameManager.treatOnTarget, gameManager.treatOffTarget, gameManager.treatTimeRemaining, gameManager.treatProgressVal);

        if (gameManager.treatProgressVal > 0.99)
        {
            titleTMP.text = "Congratulations, Treatment Complete";
            titleTMP.fontSharedMaterial = greenMaterial;

        }
        else if (gameManager.treatProgressVal > 0.66)
        {
            titleTMP.text = "Partial Improvement- Retreatment Needed";
            titleTMP.fontSharedMaterial = amberMaterial;
        }
        else
        {
            titleTMP.text = "Treatment Fail- Retraining Needed";
            titleTMP.fontSharedMaterial = redMaterial;
        }

    }

}
