using UnityEngine;

public class OrbWeaponController : WeaponController
{
    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedOrb = Instantiate(weaponData.Prefab);
        spawnedOrb.transform.position = transform.position; // Xuất hiện tại vị trí của player
        spawnedOrb.GetComponent<OrbWeaponBehaviour>(); // Gắn script để nó quay quanh player
    }
}
