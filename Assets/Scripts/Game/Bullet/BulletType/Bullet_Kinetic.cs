using UnityEngine;

public class Bullet_Kinetic : BulletCtrl
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // Sau này kiểm tra tag enemy hoặc wall
        // Ví dụ: if (collision.CompareTag("Enemy")) { damage logic }

        base.OnTriggerEnter2D(collision); // Gọi lại destroy mặc định
    }
}
