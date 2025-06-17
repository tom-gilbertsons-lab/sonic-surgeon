using UnityEngine;

public class TremorClickManager : MonoBehaviour
{
    public int tremorLevel;
    public GameObject tremorSceneObj;
    private TremorSceneManager tremorSceneManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        tremorSceneManager = tremorSceneObj.GetComponent<TremorSceneManager>();
    }

    private void OnMouseDown()
    {
        tremorSceneManager.ClickedButton(tremorLevel);
    }
}
