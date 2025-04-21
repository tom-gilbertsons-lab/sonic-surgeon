using UnityEngine;

public class TargetOnTap : MonoBehaviour
{

    public GameObject vFX;
    private TreatVFX treatvFX;

    private void Start()
    {
        treatvFX = vFX.GetComponent<TreatVFX>();
    }

    void OnMouseDown()
    {
        Vector3 worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 localClick = transform.InverseTransformPoint(worldClick);
       
       

        Debug.Log("clicked on target sprite");

        Debug.Log(worldClick.ToString());
        Debug.Log(localClick.ToString());
        
    }
}
