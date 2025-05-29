using UnityEngine;


public class PlanOutro : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    public GameObject statsBox;
    private StatBuilder statBuilder;

    void Awake()
    {
        statBuilder = statsBox.GetComponent<StatBuilder>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    public void OnEnable()
    {
        statsBox.SetActive(true);
        statBuilder.SetPlanStats(gameManager.planOnTarget, gameManager.planOffTarget, gameManager.planTimeRemaining, gameManager.planProgressVal);
    }

}
