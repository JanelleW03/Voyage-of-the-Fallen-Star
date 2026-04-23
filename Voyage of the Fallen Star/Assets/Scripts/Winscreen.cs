using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// After showing for 3 seconds it automatically loads ScreensScene
// and passes a flag to show the Credits panel.

public class WinScreen : MonoBehaviour
{
    public static WinScreen Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        StartCoroutine(AutoTransition());
    }

    private IEnumerator AutoTransition()
    {
        yield return new WaitForSecondsRealtime(3f);

        // Pass a flag to ScreensScene so it knows to show Credits
        PlayerPrefs.SetInt("ShowCredits", 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("ScreensScene");
    }
}