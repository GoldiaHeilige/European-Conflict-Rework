using UnityEngine;
using System.Collections;

public class EntityDeathEffect : MonoBehaviour
{
    [Header("Death Visuals")]
    public GameObject[] deathAnimations; 
    public float waitAfterAnim = 3f;
    public float fadeDuration = 2f;

    private SpriteRenderer spriteRenderer;
    private EntityStats stats;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (stats != null)
            stats.OnDie += HandleDeath;
    }

    private void HandleDeath()
    {
        Debug.Log($"[DieEffect] {gameObject.name} chết → xử lý hiệu ứng");

        // Disable tất cả logic scripts
        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            if (comp != this) comp.enabled = false;
        }

        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }

        float angle = GetDeathAngle();

        if (deathAnimations.Length > 0)
        {
            int index = Random.Range(0, deathAnimations.Length);
            GameObject animObj = Instantiate(deathAnimations[index], transform.position, Quaternion.Euler(0, 0, angle), transform);
        }

        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(waitAfterAnim);

        float timer = 0f;
        CanvasGroup canvas = gameObject.AddComponent<CanvasGroup>(); 

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvas.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }

        Destroy(gameObject);
    }

    private float GetDeathAngle()
    {
        Transform reference = transform;

        var firePoint = transform.Find("UpperBody/FirePoint");
        if (firePoint != null)
            reference = firePoint;

        Vector2 dir = reference.right;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return angle;
    }
}
