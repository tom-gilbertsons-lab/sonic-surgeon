using UnityEngine;

public class TreatModeManager : MonoBehaviour
{


    private Vector3 worldClick;
    private Vector2 localClick;

    public GameObject vFX;
    private TreatVFX treatvFX;

    private void Start()
    {
        treatvFX = vFX.GetComponent<TreatVFX>();
    }

    void OnMouseDown()
    {
        worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        localClick = transform.InverseTransformPoint(worldClick);



        Debug.Log("clicked on Background");

        treatvFX.CalledFrom();

        Debug.Log(worldClick.ToString());
        Debug.Log(localClick.ToString());



    }


}
