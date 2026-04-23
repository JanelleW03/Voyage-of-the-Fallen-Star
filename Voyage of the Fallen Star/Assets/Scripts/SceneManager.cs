using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        // show Credits automatically after winning
        if (PlayerPrefs.GetInt("ShowCredits", 0) == 1)
        {
            PlayerPrefs.SetInt("ShowCredits", 0);
            PlayerPrefs.Save();

            startPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }
    }

    public void OnStartPressed()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnControlsPressed()
    {
        startPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void OnCreditsPressed()
    {
        startPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnBackPressed()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        startPanel.SetActive(true);
    }
}