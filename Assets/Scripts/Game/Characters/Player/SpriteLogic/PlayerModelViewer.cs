using UnityEngine;

public class PlayerModelViewer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bodyRenderer;

    [System.Serializable]
    public struct ModelMapping
    {
        public string weaponID;
        public Sprite bodySprite;
    }

    public ModelMapping[] modelMappings;

    private void OnEnable()
    {
        PlayerWeaponSwitcher.OnWeaponSwitched += HandleWeaponSwitched;
    }

    private void OnDisable()
    {
        PlayerWeaponSwitcher.OnWeaponSwitched -= HandleWeaponSwitched;
    }

    private void HandleWeaponSwitched(WeaponData weaponData)
    {
        foreach (var mapping in modelMappings)
        {
            if (mapping.weaponID == weaponData.weaponID)
            {
                bodyRenderer.sprite = mapping.bodySprite;
                break;
            }
        }
    }
}
