
using UnityEngine;

public class AutoReturnToPool : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    private ObjectPool pool;

    public void Init(ObjectPool poolRef)
    {
        pool = poolRef;
        CancelInvoke();
        Invoke(nameof(Return), lifetime);
    }

    private void Return()
    {
        pool.ReturnToPool(gameObject);
    }
}
