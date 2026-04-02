using UnityEngine;

public enum ItemType
{
    Letter,
    Potion
}

[CreateAssetMenu(fileName = "Item", menuName = "RumpledCode/Item", order = 1)]
public class Item : ScriptableObject
{
    public string id;
    public string description;
    public Sprite icon;
    public GameObject prefab;
    public ItemType itemType;

    [TextArea(3, 10)]
    public string content;
}