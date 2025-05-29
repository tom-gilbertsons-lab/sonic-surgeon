using UnityEngine;
using TMPro;
using System.Collections;

public class Intro : MonoBehaviour
{
    public CanvasEffects canvasEffects;

    [Header("UI Text Fields")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI prompt;

    public float letterDelay = 0.15f;

    private string titleTextStr;
    private string instructionTextStr;

    private void Awake()
    {
        titleTextStr = GrabAndClear(titleText);
        instructionTextStr = GrabAndClear(instructionText);
        string s = GrabAndClear(prompt);
    }

    public IEnumerator RunIntro(System.Action onDone)
    {
        yield return StartCoroutine(canvasEffects.TypeText(titleText, titleTextStr, letterDelay));
        yield return StartCoroutine(canvasEffects.TypeText(instructionText, instructionTextStr, letterDelay));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PromptCountdown());
        onDone?.Invoke();
    }


    public IEnumerator PromptCountdown()
    {
        string[] countdown = { "3", "2", "1", "Go!" };
        foreach (string step in countdown)
        {
            prompt.text = step;
            yield return new WaitForSeconds(1f);
        }

    }


    private string GrabAndClear(TextMeshProUGUI tmp)
    {
        string s = tmp.text;
        tmp.text = "";
        return s;
    }

}
