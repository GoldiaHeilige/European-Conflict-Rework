using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHPDisplay : MonoBehaviour
{
    public EntityStats playerStats;
    public TextMeshProUGUI hpText;

    void Start()
    {
        if (playerStats != null)
            playerStats.OnHealthChanged += UpdateHPText;
        UpdateHPText(playerStats.CurrentHP);
    }

    void UpdateHPText(int currentHP)
    {
        hpText.text = $"{currentHP}";
    }
}
