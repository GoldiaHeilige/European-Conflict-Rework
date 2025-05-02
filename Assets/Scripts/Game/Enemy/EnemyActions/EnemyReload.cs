using UnityEngine;

public class EnemyReload : MonoBehaviour
{
    private EnemyGunCtrl gunCtrl;
    public bool shouldReload = false;

    private void Start()
    {
        gunCtrl = GetComponentInChildren<EnemyGunCtrl>(); 
    }

    private void Update()
    {
        if (!shouldReload || gunCtrl == null) return;

        if (!gunCtrl.IsReloading() && gunCtrl.IsClipEmpty())

        {
            gunCtrl.Reload();
        }
    }
}
