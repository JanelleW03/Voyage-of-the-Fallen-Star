using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashLoader : MonoBehaviour
{
    public Image fadeImage;
    public float duration = 2f;
    public string nextSceneName = "MainScene";

    void Start()
    {
        StartCoroutine(FadeAndLoad());
    }

    System.Collections.IEnumerator FadeAndLoad()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (t / duration));
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextSceneName);
    }
}