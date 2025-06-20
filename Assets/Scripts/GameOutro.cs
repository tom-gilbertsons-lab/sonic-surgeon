using UnityEngine;
using TMPro;



public class GameOutro : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public GameObject statsBox;
    private StatBuilder statBuilder;


    public Material greenMaterial;
    public Material amberMaterial;
    public Material redMaterial;

    public TextMeshProUGUI titleTMP;


    void Awake()
    {
        statBuilder = statsBox.GetComponent<StatBuilder>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    public void OnEnable()
    {


        statsBox.SetActive(true);
        statBuilder.SetTreatStats("Operation Note:", gameManager.treatOnTarget, gameManager.treatOffTarget, gameManager.treatTimeRemaining, gameManager.treatProgressVal);

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
