using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterOverlay : MonoBehaviour
{
    public static LetterOverlay Instance { get; private set; }

    [Header("References")]
    [SerializeField] GameObject contentPanel;
    [SerializeField] TextMeshProUGUI contentText;

    [Header("Sprites")]
    [SerializeField] Sprite scrollDefault;
    [SerializeField] Sprite scrollHighlighted;

    private List<Item> _letters = new();
    private List<Image> _scrollIcons = new();
    private int _currentIndex = -1;

    private void Awake()
    {
        Instance = this;
        contentPanel.SetActive(false);
    }
    public void RegisterScrollIcon(Item item, Image icon)
    {
        if (!_letters.Contains(item))
        {
            _letters.Add(item);
            _scrollIcons.Add(icon);
        }
        icon.sprite = scrollDefault;
    }

    public void Show(Item item)
    {
        _currentIndex = _letters.IndexOf(item);
        if (_currentIndex < 0) _currentIndex = 0;
        DisplayCurrent();
    }

    private void DisplayCurrent()
    {
        if (_letters.Count == 0 || _currentIndex < 0) return;

        var item = _letters[_currentIndex];
        contentText.text = item.content;
        contentPanel.SetActive(true);

        for (int i = 0; i < _scrollIcons.Count; i++)
            _scrollIcons[i].sprite = i == _currentIndex ? scrollHighlighted : scrollDefault;
    }
}