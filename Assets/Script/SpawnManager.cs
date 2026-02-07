using UnityEngine;
using System.Collections.Generic; // Supaya bisa pakai List

public class SpawnManager : MonoBehaviour
{
    [Header("Daftar Obstacle (Bisa diisi Cone, Box, dll)")]
    public List<GameObject> obstaclePrefabs; 

    [Header("Daftar Jalur (Bisa ditambah/dikurang sesuka hati)")]
    public List<Transform> laneAnchors; 

    [Header("Pengaturan Spawn")]
    public float spawnInterval = 1.5f; // Jeda waktu muncul
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnSomething();
            timer = 0;
        }
    }

    void SpawnSomething()
    {
        // Cek dulu apakah list sudah diisi atau belum, biar tidak Error
        if (obstaclePrefabs.Count == 0 || laneAnchors.Count == 0)
        {
            Debug.LogError("List Obstacle atau Lane masih kosong! Isi dulu di Inspector.");
            return;
        }

        // 1. Pilih Obstacle secara acak dari list
        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
        GameObject chosenObstacle = obstaclePrefabs[randomObstacleIndex];

        // 2. Pilih Jalur secara acak dari list anchor
        int randomLaneIndex = Random.Range(0, laneAnchors.Count);
        Transform chosenAnchor = laneAnchors[randomLaneIndex];

        float minX, maxX;
        if (randomLaneIndex == 0)
        {
            minX= -54;
            maxX= -25;
        }
        else if (randomLaneIndex == 1)
        {
            minX= -22;
            maxX= 20;
        }
        else
        {
            minX= 22;
            maxX= 53;
        }

        var pos = chosenAnchor.position;
        float targetX = Random.Range(minX, maxX);

        // 3. Eksekusi pemunculan
       GameObject clone = Instantiate(chosenObstacle, chosenAnchor.position, chosenAnchor.rotation);

        // 2. Beritahu si klon ke arah mana dia harus bergerak
        ObjectMovement moveScript = clone.GetComponent<ObjectMovement>();
        if (moveScript != null)
        {
            moveScript.SetTargetDirection(targetX);
        }

        // 3. Hancurkan klon setelah 5 detik agar tidak menumpuk (Opsional)
        Destroy(clone, 16f);
    }
}