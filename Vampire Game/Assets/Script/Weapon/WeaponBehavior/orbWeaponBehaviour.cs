using UnityEngine;

public class OrbWeaponBehaviour : ProjectileWeaponBehaviour
{
    public float orbitRadius = 2f; // Bán kính quỹ đạo
    public float orbitSpeed = 100f; // Tốc độ quay (độ/giây)

    private Transform player;
    private float angle;

    void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            angle += orbitSpeed * Time.deltaTime; // Tăng góc theo thời gian
            float radian = angle * Mathf.Deg2Rad; // Chuyển đổi thành radian

            // Xác định vị trí mới quanh Player
            float x = player.position.x + Mathf.Cos(radian) * orbitRadius;
            float y = player.position.y + Mathf.Sin(radian) * orbitRadius;

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}
