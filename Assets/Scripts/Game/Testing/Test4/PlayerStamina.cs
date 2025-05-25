using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerInventory))]
public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;

    [SerializeField] private float regenDelay = 2f;
    private float regenDelayTimer = 0f;

    [Header("Drain Rates")]
    public float drainSoft = 10f;
    public float drainHard = 15f;
    public float drainOver = 25f;

    [Header("Regen Rates")]
    public float regenSoft = 20f;
    public float regenHard = 10f;
    public float regenOver = 0f;

    [Header("UI")]
    public Image barFill;
    public CanvasGroup uiGroup; // 🎯 gán object cha của UI thanh stamina

    private PlayerInventory inventory;
    private PlayerController controller;

    public bool IsStaminaEmpty => currentStamina <= 0f;
    public bool IsSprinting => controller != null && controller.IsSprinting;
    public bool IsMoving => controller != null && controller.IsMoving;

    private Coroutine fadeRoutine;
    private Coroutine hideDelayRoutine;
    private float lastStaminaValue = -1f;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        controller = GetComponent<PlayerController>();
        currentStamina = maxStamina;

        if (uiGroup != null) uiGroup.alpha = 0f;
    }

    private void Update()
    {
        float weight = inventory.TotalWeight;
        float delta = 0f;

#pragma warning disable CS0219
        bool draining = false;
        bool isDrainingNow = false;
#pragma warning restore CS0219

        if (IsSprinting && IsMoving)
        {
            draining = true;
            isDrainingNow = true;

            if (weight <= inventory.SoftLimit)
                delta = -drainSoft;
            else if (weight <= inventory.HardLimit)
                delta = -drainHard;
            else
                delta = -drainOver;

            regenDelayTimer = regenDelay; // ⏱ reset delay mỗi khi đang chạy
        }
        else
        {
            if (regenDelayTimer > 0f)
            {
                regenDelayTimer -= Time.deltaTime;
            }
            else
            {
                // ⏱ Sau khi chờ đủ thời gian, mới được hồi
                if (weight <= inventory.SoftLimit)
                    delta = regenSoft;
                else if (weight <= inventory.HardLimit)
                    delta = regenHard;
                else
                    delta = regenOver;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina + delta * Time.deltaTime, 0f, maxStamina);

        if (barFill != null)
        {
            barFill.fillAmount = currentStamina / maxStamina;
        }

        // 🎯 UI Logic
        if (Mathf.Abs(currentStamina - lastStaminaValue) > 0.05f)
        {
            FadeInUI();

            if (hideDelayRoutine != null)
            {
                StopCoroutine(hideDelayRoutine);
                hideDelayRoutine = null;
            }
        }
        else if (currentStamina >= maxStamina && hideDelayRoutine == null && uiGroup.alpha > 0f)
        {
            hideDelayRoutine = StartCoroutine(DelayedFadeOut(2f));
        }

        lastStaminaValue = currentStamina;
    }


    private void FadeInUI()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeUI(1f, 0.3f));
    }

    private void FadeOutUI()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeUI(0f, 0.3f));
    }

    private IEnumerator DelayedFadeOut(float delay)
    {
        yield return new WaitForSeconds(delay);
        FadeOutUI();
        hideDelayRoutine = null;
    }

    private IEnumerator FadeUI(float targetAlpha, float duration)
    {
        if (uiGroup == null) yield break;

        float start = uiGroup.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            uiGroup.alpha = Mathf.Lerp(start, targetAlpha, t);
            yield return null;
        }

        uiGroup.alpha = targetAlpha;
    }
}
