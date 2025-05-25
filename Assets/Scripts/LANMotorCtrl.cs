using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LANMotorCtrl : MonoBehaviour
{
    //rPI

    public void StartShake()
    {
        Debug.Log("Shake Begin");
        StartCoroutine(SendStartShake());
    }

    private IEnumerator SendStartShake()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/begin");
        yield return request.SendWebRequest();
    }

    public void StopShake()
    {
        Debug.Log("Shake Stop");
        StartCoroutine(SendStopShake());
    }

    private IEnumerator SendStopShake()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/stop");
        yield return request.SendWebRequest();
        // Optional: Add error checking here if needed
    }

    public void ShakeMid()
    {
        Debug.Log("Shake Mid");
        StartCoroutine(SendMidShake());
    }

    private IEnumerator SendMidShake()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/mid");
        yield return request.SendWebRequest();
        // Optional: Add error checking here if needed
    }

    public void ShakeLo()
    {
        StartCoroutine(SendLoShake());
    }

    private IEnumerator SendLoShake()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/lo");
        yield return request.SendWebRequest();
        // Optional: Add error checking here if needed
    }



}
