using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ModeIntro : MonoBehaviour
{
    public GameObject promptObject;

    public GameObject canvasEffectsObj;
    private CanvasEffects canvasEffects;


    public Image backgroundImage;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI message1;
    public TextMeshProUGUI message2;
    public TextMeshProUGUI message3;


    string titleTextStr;
    string message1Str;
    string message2Str;
    string message3Str;



    private void Awake()
    {
        canvasEffects = canvasEffectsObj.GetComponent<CanvasEffects>();
        promptObject.SetActive(false);

        titleTextStr = GrabText(titleText);
        message1Str = GrabText(message1);
        message2Str = GrabText(message2);
        message3Str = GrabText(message3);

    }

    private string GrabText(TextMeshProUGUI myTMP)
    {
        string s = myTMP.text;
        myTMP.text = "";
        return s;
    }

    public IEnumerator RunFullIntro(System.Action onDone)
    {
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(canvasEffects.TypeText(titleText, titleTextStr, 0.04f));
        yield return StartCoroutine(canvasEffects.TypeText(message1, message1Str, 0.03f));
        yield return StartCoroutine(canvasEffects.TypeText(message2, message2Str, 0.03f));
        yield return StartCoroutine(canvasEffects.TypeText(message3, message3Str, 0.03f));

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(canvasEffects.FadeText(titleText, 1f, 0f, 0.3f));
        StartCoroutine(canvasEffects.FadeText(message1, 1f, 0f, 0.3f));
        StartCoroutine(canvasEffects.FadeText(message2, 1f, 0f, 0.3f));
        StartCoroutine(canvasEffects.FadeText(message3, 1f, 0f, 0.3f));

        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(canvasEffects.FadeImage(backgroundImage, 1f, 0.4f, 0.5f));

        yield return StartCoroutine(RunPrompt(promptObject));

        yield return StartCoroutine(canvasEffects.FadeImage(backgroundImage, 0.4f, 0f, 0.3f));

        onDone?.Invoke(); // <-- this is your callback
    }


    public IEnumerator RunPrompt(GameObject promptObject)
    {
        promptObject.SetActive(true);
        yield return StartCoroutine(promptObject.GetComponent<PromptCountdown>().PromptOpener());
        promptObject.SetActive(false);
    }
}
