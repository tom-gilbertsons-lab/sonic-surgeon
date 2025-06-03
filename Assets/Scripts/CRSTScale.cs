using UnityEngine;

public class CRSTScale : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;
    private LANMotorCtrl lANMotorCtrl;

    private void Awake()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        lANMotorCtrl = gameManagerObject.GetComponent<LANMotorCtrl>();
    }


    public void CRST4()
    {
        Debug.Log("crst4");
        if (gameManager.lanMotor)
        {
            lANMotorCtrl.crst4();
        }

    }

    public void CRST3()
    {
        Debug.Log("crst3");

        if (gameManager.lanMotor)
        {
            lANMotorCtrl.crst3();
        }
    }

    public void CRST2()
    {
        Debug.Log("crst2");

        if (gameManager.lanMotor)
        {
            lANMotorCtrl.crst2();
        }
    }

    public void CRST1()
    {
        Debug.Log("crst1");

        if (gameManager.lanMotor)
        {
            lANMotorCtrl.crst1();
        }
    }

    public void CRST0()
    {
        Debug.Log("crst0");

        if (gameManager.lanMotor)
        {
            lANMotorCtrl.crst0();
        }
    }


}
