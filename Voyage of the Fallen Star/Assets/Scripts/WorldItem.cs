using UnityEngine;

// Attach to every item GameObject placed in the world.
// Assign the matching Item ScriptableObject in the Inspector.
[RequireComponent(typeof(Collider))]
public class WorldItem : MonoBehaviour
{
    public Item item;
}