using UnityEngine;

public class CircleWeaponController : WeaponController
{
    [SerializeField] int numberOfProjectiles = 8; // Số viên đạn tạo ra mỗi lần tấn công

    protected override void Attack()
    {
        base.Attack();

        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float projectileDirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float projectileDirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 projectileMoveDirection = new Vector3(projectileDirX, projectileDirY, 0f);

            GameObject projectile = Instantiate(weaponData.Prefab, transform.position, Quaternion.identity);
            projectile.GetComponent<CircleProjectileBehaviour>().SetDirection(projectileMoveDirection);

            angle += angleStep;
        }
    }
}
