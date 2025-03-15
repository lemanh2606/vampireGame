using UnityEngine;

public class CircleProjectileBehaviour : ProjectileWeaponBehaviour
{
    void Update()
    {
        // Di chuyển theo hướng đã được thiết lập
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }
}
