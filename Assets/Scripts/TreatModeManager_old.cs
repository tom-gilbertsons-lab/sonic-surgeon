using UnityEngine;

public class TreatModeManager_old : MonoBehaviour
{

    public float vFXDuration = 1.3f;

    public float hotspotRadius = 0.075f;

    private Vector3 worldClick;
    private Vector2 localClick;

    public GameObject target;
    public GameObject vFX;

    private SpriteRenderer treatSR;
    private TreatVFX treatvFX;


    private float xPos;
    private float yPos;

   
    public float catchEnergyRadius;

    private void Start()
    {
        treatvFX = vFX.GetComponent<TreatVFX>();
        treatSR = GetComponent<SpriteRenderer>();



    }

   

    void OnMouseDown()
    {
        worldClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        localClick = transform.InverseTransformPoint(worldClick);



        xPos = (localClick.x / treatSR.sprite.bounds.size.x) + 0.5f;
        yPos = (localClick.y / treatSR.sprite.bounds.size.y) + 0.5f;

        Debug.Log("Click");

   

        StartCoroutine(treatvFX.AnimateHotSpot(xPos, yPos, vFXDuration, hotspotRadius));

        CheckOverlap();

    }


    private void CheckOverlap()
    {
        foreach (Transform child in target.transform) // target = TargetParent
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            Vector3 targetPos = child.position;
            float targetRadius = sr.bounds.extents.magnitude; // estimate radius
            float distance = Vector2.Distance(worldClick, targetPos);

            if (distance <= targetRadius + catchEnergyRadius)
            {
                Debug.Log("Overlap with " + child.name);
                sr.color = Color.red; // Change to whatever effect you want
            }
        }
    }


    //private void CheckOverlap()
    //{
    //    float distance = Vector2.Distance(worldClick, targetPosition);

    //    //Debug.Log("Target Pos: " + targetPosition.ToString());
    //    //Debug.Log("Distance: "+ distance);
    //    //Debug.Log("Target Radius: " + targetRadius);

    //    if (distance <= catchEnergyRadius)
    //    {
    //        Debug.Log("GO ACCUMULATE");
    //        ChangeSpriteColour();
    //    }
    //    else
    //    {
    //        Debug.Log("no accumulate");
    //    }

    //}
}




