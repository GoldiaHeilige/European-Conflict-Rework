using UnityEngine;

public class PlayerModelViewer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private WeaponVisualProfileDB visualDB;

    public void UpdateSprite(WeaponRuntimeItem runtimeWeapon)
    {
        var sprite = visualDB.GetSpriteFor(runtimeWeapon);
        bodyRenderer.sprite = sprite;
    }
}