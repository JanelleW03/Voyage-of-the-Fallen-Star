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

    private void Update()
    {
        //Debug.Log("Update running");

        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventoryAudio.Play();
            bool isOpen = !ui.gameObject.activeSelf;
            ui.gameObject.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;
        }

        if (Input.GetKeyDown(KeyCode.E) && nearbyItems.Count > 0)
        {
            WorldItem closest = GetClosestItem();
            if (closest != null)
                PickUp(closest);

        }
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