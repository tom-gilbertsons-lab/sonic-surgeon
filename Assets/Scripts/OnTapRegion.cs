using UnityEngine;

public class OnTapRegion : MonoBehaviour
{

    public bool isTarget = false;
    void OnMouseDown()
    {
        Debug.Log($"Tapped on {gameObject.name} is target? " + isTarget.ToString());
      
    }

}
