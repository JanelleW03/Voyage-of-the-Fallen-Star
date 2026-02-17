using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUi;
    public GameObject hud;
    public GameObject[] otherUis;
    
    private InputAction _openInventoryAction;

    private void Start()
    {
        _openInventoryAction = InputSystem.actions.FindAction("Open Inventory");
    }

    private void Update()
    {
        if (_openInventoryAction.WasPerformedThisFrame() && !otherUis.Any(ui => ui.activeSelf))
        {
            var shouldOpen = !inventoryUi.activeSelf;
            inventoryUi.SetActive(shouldOpen);
            hud.SetActive(!shouldOpen);
        }
    }
}
