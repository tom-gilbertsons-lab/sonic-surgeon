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
        yield return StartCoroutine(canvasEffects.TypeTextFixed(titleText, titleTextStr, 0.1f));
        yield return StartCoroutine(canvasEffects.TypeTextFixed(instructionText, instructionTextStr, 0.1f));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PromptCountdown());
        onDone?.Invoke();
    }

    public IEnumerator PromptCountdown()
    {
        string[] countdown = { "3", "2", "1", "Go→" };
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
