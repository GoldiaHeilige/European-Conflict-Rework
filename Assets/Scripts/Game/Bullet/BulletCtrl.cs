using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    protected Vector2 moveDirection;
    protected float moveSpeed;
    protected float lifeTime;

    public virtual void Initialize(Vector2 direction, float speed, float lifeTime)
    {
        this.moveDirection = direction.normalized;
        this.moveSpeed = speed;
        this.lifeTime = lifeTime;

        Destroy(gameObject, this.lifeTime);
    }

    protected virtual void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
