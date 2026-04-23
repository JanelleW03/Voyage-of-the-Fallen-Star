using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }

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
    }

    // Reloads MainScene from the beginning
    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    // Goes back to the main menu
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ScreensScene");
    }
}