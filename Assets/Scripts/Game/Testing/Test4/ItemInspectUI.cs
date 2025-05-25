using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class ItemInspectUI : MonoBehaviour
{
    public static ItemInspectUI Instance;

    [Header("References")]
    public GameObject panel;
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI categoryTierText;
    public TextMeshProUGUI weightText;
    public TextMeshProUGUI line1Text;
    public TextMeshProUGUI line2Text;
    public TextMeshProUGUI line3Text;
    public Button closeButton;

    private void Awake()
    {
        Instance = this;
        Debug.Log("✅ ItemInspectUI Awake called, Instance set");
        closeButton.onClick.AddListener(Hide);
    }

    public void Show(InventoryItemRuntime item)
    {
        var data = item.itemData;

        nameText.text = $"Name: {data.itemName}";
        descriptionText.text = $"Description: {data.description}";
        weightText.text += $"\n<color=grey><i>Weight:</i> {data.weightPerUnit} kg</color>";

        // ✳ Mặc định ẩn các dòng
        categoryTierText.gameObject.SetActive(false);
        line1Text.gameObject.SetActive(false);
        line2Text.gameObject.SetActive(false);
        line3Text.gameObject.SetActive(false);

        iconImage.sprite = data.icon;

        // 👉 Tùy loại item mà hiện chi tiết phù hợp
        if (data is ArmorData armor)
        {
            categoryTierText.gameObject.SetActive(true);
            categoryTierText.text = $"Armor - Tier {armor.tier}";

            line1Text.gameObject.SetActive(true);
            line1Text.text = $"Armor Rating: {armor.armorRating}";

            // 🟡 Tìm runtime item để lấy current durability
            if (item is ArmorRuntimeItem armorRuntime)
            {
                line2Text.gameObject.SetActive(true);
                line2Text.text = $"Durability: {armorRuntime.durability} / {armor.maxDurability}";
            }
            else
            {
                line2Text.gameObject.SetActive(true);
                line2Text.text = $"Durability: ? / {armor.maxDurability}";
            }
        }

        else if (data is AmmoItemData ammoItem)
        {
            var ammo = ammoItem.linkedAmmoData;

            categoryTierText.gameObject.SetActive(true);
            categoryTierText.text = $"Ammo - Tier {ammo.tier}";

            line1Text.gameObject.SetActive(true);
            line1Text.text = $"Pen Power: {ammo.penetrationPower}";

            line2Text.gameObject.SetActive(true);
            line2Text.text = $"Base Damage: {ammo.baseDamage}";
        }

        else if (data is HealingItemData health)
        {
            categoryTierText.gameObject.SetActive(true);
            categoryTierText.text = "Health Item";

            line1Text.gameObject.SetActive(true);
            line1Text.text = $"Restore: +{health.healAmount} HP";
        }

        else if (data is WeaponData weapon)
        {
            categoryTierText.gameObject.SetActive(true);
            categoryTierText.text = "Weapon";

            line1Text.gameObject.SetActive(true);
            line1Text.text = $"Class: {weapon.weaponClass}";

            line2Text.gameObject.SetActive(true);
            line2Text.text = $"Fire Rate: {weapon.fireRate}/s";

            line3Text.gameObject.SetActive(true);
            line3Text.text = $"Reload: {weapon.reloadTime}s | Clip: {weapon.clipSize}";
        }

        // ✳ Nếu có thêm loại item khác thì thêm else if ở đây

        if (data.stackable && item.quantity > 1)
        {
            line3Text.gameObject.SetActive(true);
            line3Text.text = $"<color=#FFD700>Amount:</color> {item.quantity}";
        }

        panel.SetActive(true);
    }

    public static void InitIfNeeded()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<ItemInspectUI>(true);
            if (Instance == null)
            {
                Debug.LogError("❌ Không tìm thấy ItemInspectUI trong scene!");
            }
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
