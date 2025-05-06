using UnityEngine;
using System.Collections;
using TMPro;

public class ModeIntro : MonoBehaviour
{
    public GameObject sceneObjectBeingPrompted;
    [Header("Prompt + Effects")]
    public GameObject promptObject;
    public CanvasEffects canvasEffects;

    [Header("UI Text Fields")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI message1;
    public TextMeshProUGUI message2;
    public TextMeshProUGUI message3;


    [Header("Timing")]
    public float fadeDuration = 0.5f;
    public float letterDelay = 0.08f;

    private CanvasGroup cg;
    private string titleTextStr;
    private string message1Str;
    private string message2Str;
    private string message3Str;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        promptObject.SetActive(false);

        // Grab and clear text
        titleTextStr = GrabAndClear(titleText);
        message1Str = GrabAndClear(message1);
        message2Str = GrabAndClear(message2);
        message3Str = GrabAndClear(message3);

        // Ensure invisible at start
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = true;
    }

    private string GrabAndClear(TextMeshProUGUI tmp)
    {
        string s = tmp.text;
        tmp.text = "";
        return s;
    }

    public IEnumerator RunFullIntro(System.Action onDone)
    {
        // Fade in everything
        yield return StartCoroutine(canvasEffects.FadeCanvasGroup(cg, 0f, 1f, fadeDuration));

        // Type out text one at a time
        yield return StartCoroutine(canvasEffects.TypeText(titleText, titleTextStr, letterDelay));
        yield return StartCoroutine(canvasEffects.TypeText(message1, message1Str, letterDelay));
        yield return StartCoroutine(canvasEffects.TypeText(message2, message2Str, letterDelay));
        yield return StartCoroutine(canvasEffects.TypeText(message3, message3Str, letterDelay));

        yield return new WaitForSeconds(1f);

        sceneObjectBeingPrompted.SetActive(true);
        EnableAllColliders(sceneObjectBeingPrompted, false);
        yield return StartCoroutine(canvasEffects.FadeCanvasGroup(cg, 1f, 0f, fadeDuration));

        promptObject.SetActive(true);
        yield return StartCoroutine(promptObject.GetComponent<PromptCountdown>().PromptOpener());
        promptObject.SetActive(false);
        EnableAllColliders(sceneObjectBeingPrompted, true);


        // Callback to controller (GameManager, etc.)
        onDone?.Invoke();
    }


    public void EnableAllColliders(GameObject gameObject, bool enable)
    {
        BoxCollider2D[] colliders = gameObject.GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
        foreach (var col in colliders)
        {
            col.enabled = enable;
        }
    }
}
