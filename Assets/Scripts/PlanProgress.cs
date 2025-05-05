using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanProgress : MonoBehaviour
{
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;
    public GameObject stage4;

    private void Awake()
    {
        ResetAll();
    }

    public void SetStage1()
    {
        StartCoroutine(FadeIn(stage1));
    }

    public void SetStage2()
    {
        StartCoroutine(FadeIn(stage2));
    }

    public void SetStage3()
    {
        StartCoroutine(FadeIn(stage3));
        StartCoroutine(FadeIn(stage4));
    }

    public void ResetAll()
    {
        stage1.SetActive(false);
        stage2.SetActive(false);
        stage3.SetActive(false);
        stage4.SetActive(false);
    }

    private IEnumerator FadeIn(GameObject obj)
    {
        obj.SetActive(true);

        Image img = obj.GetComponent<Image>();
        Color c = img.color;
        c.a = 0f;
        img.color = c;

        float duration = 0.2f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / duration);
            img.color = c;
            yield return null;
        }

        // Ensure final alpha is fully visible
        c.a = 1f;
        img.color = c;
    }
}


//using UnityEngine;

//public class PlanProgress : MonoBehaviour
//{
//    public GameObject stage1;
//    public GameObject stage2;
//    public GameObject stage3;
//    public GameObject stage4;

//    private void Awake()
//    {
//        stage1.SetActive(false);
//        stage2.SetActive(false);
//        stage3.SetActive(false);
//        stage4.SetActive(false);
//    }

//    public void SetStage1()
//    {
//        stage1.SetActive(true);
//    }

//    public void SetStage2()
//    {
//        stage2.SetActive(true);
//    }

//    public void SetStage3()
//    {
//        stage3.SetActive(true);
//        stage4.SetActive(true);
//    }

//    public void ResetAll()
//    {
//        stage1.SetActive(false);
//        stage2.SetActive(false);
//        stage3.SetActive(false);
//        stage4.SetActive(false);
//    }
//}
