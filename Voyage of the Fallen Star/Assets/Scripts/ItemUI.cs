using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image image;
    [SerializeField] Button button;

    private Item _item;

    public void Initialize(string inventoryId, Item item, Action<string> removeItemAction)
    {
        _item = item;
        image.sprite = item.icon;

        if (item.itemType == ItemType.Letter)
        {
            // Register this icon with LetterOverlay so it can manage highlights
            LetterOverlay.Instance.RegisterScrollIcon(item, image);
            button.onClick.AddListener(() => LetterOverlay.Instance.Show(_item));
        }
        else
        {
            button.onClick.AddListener(() => removeItemAction.Invoke(inventoryId));
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}