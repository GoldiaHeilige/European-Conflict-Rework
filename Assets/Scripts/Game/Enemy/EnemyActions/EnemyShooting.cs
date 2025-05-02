using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private EnemyGunCtrl gunCtrl;
    private Transform player;
    public bool shouldShoot = false;

    private void Start()
    {
        gunCtrl = GetComponentInChildren<EnemyGunCtrl>(); 
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Debug.Log($"EnemyShooting setup | gunCtrl = {(gunCtrl != null ? "OK" : "NULL")} | player = {(player != null ? player.name : "NULL")}");
    }

    private void Update()
    {
        if (!shouldShoot || player == null || gunCtrl == null) return;

/*        Debug.LogWarning("Không thể bắn: thiếu điều kiện");*/


        Vector2 dirToPlayer = (player.position - gunCtrl.firePoint.position).normalized;
        gunCtrl.TryShoot(dirToPlayer, this.gameObject);
    }
}
