using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerHPDisplay : MonoBehaviour
{
    public EntityStats playerStats;
    public TextMeshProUGUI hpText;

    [Header("Popup Effect")]
    [SerializeField] private Vector2 popupIntensity = new Vector2(0.2f, 0.2f);
    [SerializeField] private float popupDuration = 0.2f;

    void Start()
    {
        if (playerStats != null)
            playerStats.OnHealthChanged += UpdateHPText;

        UpdateHPText(playerStats.CurrentHP);
    }

    void UpdateHPText(int currentHP)
    {
        if (hpText == null || playerStats == null) return;

        hpText.text = $"{currentHP}";
        float percent = (float)currentHP / playerStats.MaxHP;

        if (percent <= 0.1f)
            hpText.color = Color.red;
        else if (percent <= 0.25f)
            hpText.color = new Color(1f, 0.5f, 0f); // cam
        else if (percent <= 0.5f)
            hpText.color = Color.yellow;
        else
            hpText.color = Color.white;

        hpText.transform.DOKill();
        hpText.transform.localScale = Vector3.one;

        hpText.transform
            .DOPunchScale(popupIntensity, popupDuration)
            .OnComplete(() => hpText.transform.DORewind());
    }
}
