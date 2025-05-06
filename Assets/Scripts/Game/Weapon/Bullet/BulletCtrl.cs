using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    protected Vector2 moveDirection;
    protected float moveSpeed;
    protected float lifeTime;

    protected GameObject owner;
    protected int damage;

    public virtual void Initialize(Vector2 direction, float speed, float lifeTime)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        this.lifeTime = lifeTime;

        Destroy(gameObject, lifeTime);
    }

    public virtual void SetOwnerAndDamage(GameObject owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    public virtual GameObject GetOwner() => owner;
    public virtual int GetDamage() => damage;

    protected virtual void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
