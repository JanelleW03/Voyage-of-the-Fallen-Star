using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

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