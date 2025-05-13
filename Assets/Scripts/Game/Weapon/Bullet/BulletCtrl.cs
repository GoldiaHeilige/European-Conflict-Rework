using UnityEngine;

public enum BulletType
{
    Kinetic,
    Explosive
}

public class BulletCtrl : MonoBehaviour
{
    protected Vector2 moveDirection;
    protected float moveSpeed;
    protected float lifeTime;

    protected GameObject owner;
    protected int damage;

    protected BulletType bulletType = BulletType.Kinetic;
    protected ArmorPenetration penetrationLevel = ArmorPenetration.APLight;

    [SerializeField] private string defaultTag;

    public void SetArmorPenetration(ArmorPenetration ap)
    {
        penetrationLevel = ap;
    }

    public ArmorPenetration GetArmorPenetration()
    {
        return penetrationLevel;
    }

    public void SetBulletTag(string tag)
    {
        gameObject.tag = tag;
    }

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

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

    public virtual void SetBulletType(BulletType type)
    {
        bulletType = type;
/*        Debug.Log($"[BulletCtrl] Set bullet type = {type}");*/
    }

    public virtual BulletType GetBulletType() => bulletType;

    protected virtual void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
