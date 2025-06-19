using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TremorSceneManager : MonoBehaviour
{


    [Header("CRST Traces")]
    public GameObject[] crstTraces;

    public int currentTremorLevel = 4;
    private int maxActiveTraces = 3;
    private Queue<TremorTraceRenderer> activeRenderers = new Queue<TremorTraceRenderer>();

    //void Start()
    //{
    //    StartCoroutine(Draw());
    //}

    private void OnEnable()
    {
        StartCoroutine(Draw());
        Debug.Log("DRAW");
    }

    private IEnumerator Draw()
    {
        int index = 0;

        while (true)
        {
            GameObject traceObj = crstTraces[index];
            var renderer = traceObj.GetComponent<TremorTraceRenderer>();

            // If too many active, fade and clear the oldest
            if (activeRenderers.Count >= maxActiveTraces)
            {
                TremorTraceRenderer oldest = activeRenderers.Dequeue();
                StartCoroutine(oldest.FadeOutAndClear());
            }

            renderer.DrawTrace();
            activeRenderers.Enqueue(renderer);

            yield return new WaitUntil(() => !renderer.IsDrawing);
            yield return new WaitForSeconds(1f); // optional gap

            index = (index + 1) % crstTraces.Length; // wrap around
        }
    }


    public int GetCurrentTremorLevel()
    {
        return currentTremorLevel;
    }
}

