using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbWeaponSpawner : MonoBehaviour
{
    public GameObject orbPrefab; // Gán Prefab Orb vào Inspector
    public Transform playerTransform; // Lưu vị trí của Player
    public int maxOrbs = 3; // Số lượng Orb tối đa
    private List<GameObject> activeOrbs = new List<GameObject>(); // Danh sách các Orb hiện tại
    public float spawnInterval = 5f; // Thời gian giữa mỗi lần sinh Orb

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        StartCoroutine(SpawnOrbsOverTime()); // Bắt đầu vòng lặp sinh Orb
    }

    private IEnumerator SpawnOrbsOverTime()
    {
        while (true) // Lặp vô hạn (hoặc có thể đặt điều kiện dừng nếu cần)
        {
            yield return new WaitForSeconds(spawnInterval); // Đợi trước khi sinh Orb

            if (activeOrbs.Count < maxOrbs) // Chỉ sinh nếu chưa đạt số lượng tối đa
            {
                SpawnOrb();
            }
        }
    }

    private void SpawnOrb()
    {
        GameObject orb = Instantiate(orbPrefab, playerTransform.position, Quaternion.identity);
        orb.transform.SetParent(playerTransform); // Gán Orb vào Player để nó di chuyển theo
        activeOrbs.Add(orb);
    }

    public void IncreaseMaxOrbs(int amount)
    {
        maxOrbs += amount; // Tăng giới hạn Orb tối đa
    }
}
