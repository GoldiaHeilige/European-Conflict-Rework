using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    protected Vector2 moveDirection;
    protected float moveSpeed;
    protected float lifeTime;

    protected GameObject owner;
    protected AmmoData ammoData;

    [SerializeField] private string defaultTag;

    public virtual void SetAmmoInfo(GameObject owner, AmmoData ammo)
    {
        this.owner = owner;
        this.ammoData = ammo;
    }

    public virtual void Initialize(Vector2 direction, float speed, float lifeTime)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);
    }

    public virtual void SetBulletTag(string tag)
    {
        gameObject.tag = tag;
    }

    public virtual void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    protected virtual void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.GetComponentInParent<EntityStats>()?.gameObject ??
                            (collision.GetComponentInParent<IDamageable>() as Component)?.gameObject;

        if (target != null && ammoData != null)
        {
            DamageResolver.ApplyDamage(target, ammoData, owner);
        }

        Destroy(gameObject);
    }
}
