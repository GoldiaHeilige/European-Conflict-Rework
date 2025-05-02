using UnityEngine;

public class PlayerModelCtrl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bodyRenderer;

    [System.Serializable]
    public struct ModelMapping
    {
        public string weaponName;
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
            if (mapping.weaponName == weaponData.weaponName)
            {
                bodyRenderer.sprite = mapping.bodySprite;
                break;
            }
        }
    }
}
