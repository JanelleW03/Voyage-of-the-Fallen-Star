using System.Collections;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        Instance = this;
        fadeCanvas.alpha = 1f;
        StartCoroutine(FadeIn());
    }
    private void Start()
    {
        fadeCanvas.alpha = 1f; // start black
        StartCoroutine(FadeIn()); // fade to clear
    }
    public IEnumerator FadeOut() // goes dark
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn() // clears up
    {
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        fadeCanvas.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = to;
    }
}