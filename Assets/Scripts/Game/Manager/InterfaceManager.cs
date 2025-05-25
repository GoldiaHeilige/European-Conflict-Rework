
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject overlayUI;
    [SerializeField] private InventoryUI inventoryUIHandler;

    private bool inventoryOpen = false;

    private void Start()
    {
        CloseInventory();
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
        PlayerRotation.allowMouseLook = false;

        if (inventoryUI != null) inventoryUI.SetActive(true);

        WeightDisplayHUD hud = inventoryUI.GetComponentInChildren<WeightDisplayHUD>(true);
        if (hud != null)
        {
            hud.ForceUpdate(); 
        }

        foreach (var armorSlot in FindObjectsOfType<ArmorSlotUI>(true))
        {
            armorSlot.UpdateSlot();
        }


        if (overlayUI != null) overlayUI.SetActive(true);
        if (hudUI != null) hudUI.SetActive(false);

        inventoryUIHandler.UpdateInventory(PlayerInventory.Instance.GetItems());

        Time.timeScale = 0f;
    }

    private void CloseInventory()
    {
        inventoryOpen = false;
        PlayerRotation.allowMouseLook = true;

        if (inventoryUI != null) inventoryUI.SetActive(false);
        if (overlayUI != null) overlayUI.SetActive(false);
        if (hudUI != null) hudUI.SetActive(true);

        if (inventoryUIHandler != null)
        {
            var stats = FindObjectOfType<EntityStats>();
            inventoryUIHandler.UpdateInventory(PlayerInventory.Instance.GetItems());
        }

        if (ItemContextMenu.currentOpenMenu != null)
            ItemContextMenu.currentOpenMenu.Close();

        var ammoPopup = FindObjectOfType<ChangeAmmoTypePopup>(true);
        if (ammoPopup != null && ammoPopup.gameObject.activeSelf)
            ammoPopup.gameObject.SetActive(false);


        if (PlayerWeaponCtrl.Instance != null)
            PlayerWeaponCtrl.Instance.ApplyPendingAmmoChange();

        if (PlayerWeaponCtrl.Instance != null && PlayerWeaponCtrl.Instance.runtimeItem != null)
        {
            FindObjectOfType<AmmoTextUI>()?.Bind(PlayerWeaponCtrl.Instance.runtimeItem); 
        }

        // Tắt toàn bộ popup UI nếu đang bật
        if (ItemContextMenu.currentOpenMenu != null)
            ItemContextMenu.currentOpenMenu.Close();

        if (DropAmountPopup.Instance != null)
            DropAmountPopup.Instance.Hide();

        if (ItemInspectUI.Instance != null)
            ItemInspectUI.Instance.Hide();

        // Có thể thêm các popup khác sau này

        Time.timeScale = 1f;

    }
}
