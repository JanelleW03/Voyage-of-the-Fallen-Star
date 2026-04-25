using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    InventoryUI ui;
    [SerializeField]
    PlayerHealthComponent playerHealth;
    [SerializeField]
    PlayerMovementController playerMovement;
    [SerializeField]
    PlayerCombatController playerCombat;

    [Header("Potion Settings")]
    [SerializeField]
    float potionHealAmount = 25f;

    [Header("Audio")]
    [SerializeField]
    AudioSource inventoryAudio;
    [SerializeField]
    AudioSource potionAudio;

    [Header("State")]
    [SerializeField]
    SerializedDictionary<string, Item> inventory = new();

    private readonly List<WorldItem> nearbyItems = new();

    private DialogueManager _dialogueManager;

    private void Start()
    {
        _dialogueManager = FindFirstObjectByType<DialogueManager>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Block inventory if dialogue is open
            if (_dialogueManager != null && _dialogueManager.IsDialogueActive()) return;

            bool isOpen = !ui.gameObject.activeSelf;
            ui.gameObject.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;
            playerMovement.SetMovementEnabled(!isOpen);
            playerCombat.enabled = !isOpen;
        }

        if (Input.GetKeyDown(KeyCode.E) && nearbyItems.Count > 0)
        {
            if (ui.gameObject.activeSelf) return;
            WorldItem closest = GetClosestItem();
            if (closest != null)
                PickUp(closest);
        }
    }


    // Returns true if the inventory UI is currently closed
    public bool IsInventoryClosed() => !ui.gameObject.activeSelf;

    // Returns true if the player has picked up a letter
    public bool HasLetter()
    {
        foreach (var item in inventory.Values)
            if (item.itemType == ItemType.Letter)
                return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<WorldItem>(out var worldItem))
            nearbyItems.Add(worldItem);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<WorldItem>(out var worldItem))
            nearbyItems.Remove(worldItem);
    }

    private void PickUp(WorldItem worldItem)
    {
        nearbyItems.Remove(worldItem);

        var item = worldItem.item;
        Destroy(worldItem.gameObject);

        switch (item.itemType)
        {
            case ItemType.Letter:
                inventoryAudio.Play();
                AddItem(item);
                break;
            case ItemType.Potion:
                potionAudio.Play();
                UsePotion();
                break;
        }
    }

    private void AddItem(Item item)
    {
        var inventoryId = Guid.NewGuid().ToString();
        inventory.Add(inventoryId, item);
        ui.AddUIItem(inventoryId, item);
    }

    private void UsePotion()
    {
        playerHealth.Heal(potionHealAmount);
    }

    public void RemoveItem(string inventoryId)
    {
        inventory.Remove(inventoryId);
        ui.RemoveUIItem(inventoryId);
    }

    private WorldItem GetClosestItem()
    {
        WorldItem closest = null;
        float closestDist = float.MaxValue;

        foreach (var worldItem in nearbyItems)
        {
            if (worldItem == null) continue;
            float dist = Vector3.Distance(transform.position, worldItem.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = worldItem;
            }
        }

        return closest;
    }
}