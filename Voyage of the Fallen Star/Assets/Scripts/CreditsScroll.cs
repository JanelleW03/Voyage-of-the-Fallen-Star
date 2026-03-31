using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed = 50f;
    public float endY = 1500f;

    private void OnEnable()
    {
        // Reset scroll position every time credits are opened
        creditsText.anchoredPosition = new Vector2(0, -500f);
    }

    private void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }
}