using UnityEngine;

public class EnemyAI_SimpleShooter : EnemyAI_Base
{
    protected override void HandleAI()
    {
        shooting.shouldShoot = true;

        reloading.shouldReload = true;
    }
}
