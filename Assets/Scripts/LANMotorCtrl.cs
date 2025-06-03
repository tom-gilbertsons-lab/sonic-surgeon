using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LANMotorCtrl : MonoBehaviour
{
    //rPI

    public void crst4()
    {
        StartCoroutine(SendCRST4());
    }

    private IEnumerator SendCRST4()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/crst4");
        yield return request.SendWebRequest();
    }

    public void crst3()
    {
        StartCoroutine(SendCRST3());
    }

    private IEnumerator SendCRST3()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/crts3");
        yield return request.SendWebRequest();
    }

    public void crst2()
    {
        StartCoroutine(SendCRST2());
    }

    private IEnumerator SendCRST2()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/crts2");
        yield return request.SendWebRequest();
    }

    public void crst1()
    {
        StartCoroutine(SendCRST1());
    }

    private IEnumerator SendCRST1()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/crts2");
        yield return request.SendWebRequest();
    }


    public void crst0()
    {
        StartCoroutine(SendCRST0());
    }

    private IEnumerator SendCRST0()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://192.168.8.165:5000/crts2");
        yield return request.SendWebRequest();
    }



}
