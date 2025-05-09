
using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject overlayUI;

    private bool inventoryOpen = false;

    private void Start()
    {
        CloseInventory(); // đảm bảo đúng trạng thái lúc đầu
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventoryUI();
        }
    }

    public void ToggleInventoryUI()
    {
        if (inventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        inventoryOpen = true;

        if (inventoryUI != null) inventoryUI.SetActive(true);
        if (overlayUI != null) overlayUI.SetActive(true);
        if (hudUI != null) hudUI.SetActive(false);

        Time.timeScale = 0f;
    }

    private void CloseInventory()
    {
        inventoryOpen = false;

        if (inventoryUI != null) inventoryUI.SetActive(false);
        if (overlayUI != null) overlayUI.SetActive(false);
        if (hudUI != null) hudUI.SetActive(true);

        Time.timeScale = 1f;
    }
}
