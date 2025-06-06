﻿
using UnityEngine;

public class EnemyAI_SimpleShooter : EnemyAI_Base
{
    [SerializeField] private Transform fireTarget;
    [SerializeField] private float fireInterval = 2f;
    private float timer = 0f;

    protected override void Update()
    {
        if (controller?.weaponCtrl?.runtimeData == null) return;

        // ❌ Ngăn bắn khi đang reload
        if (controller.weaponCtrl.reload != null && controller.weaponCtrl.reload.IsReloading) return;

        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            controller.weaponCtrl.shooting.TryShootAt(fireTarget.position);
            timer = 0f;
        }
    }
}
