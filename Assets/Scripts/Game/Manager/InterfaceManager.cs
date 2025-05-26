
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject hudUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject overlayUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private InventoryUI inventoryUIHandler;

    private bool inventoryOpen = false;

    public static bool IsInventoryOpen { get; private set; } = false;

    private void Start()
    {
        StartCoroutine(InitInventoryUI());
    }

    private IEnumerator InitInventoryUI()
    {
        inventoryUI.SetActive(false);
        overlayUI.SetActive(false);
        hudUI.SetActive(true);
        pauseMenu.SetActive(false);

        yield return null;

/*        inventoryUI.SetActive(false);
        overlayUI.SetActive(false);*/
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !PauseMenu.IsGamePaused)
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
        IsInventoryOpen = true;
        inventoryOpen = true;
        PlayerRotation.allowMouseLook = false;

        if (inventoryUI != null) inventoryUI.SetActive(true);
/*        UIStackClose.Push(inventoryUI);*/

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
        IsInventoryOpen = false;
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
