using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Daftar Obstacle")]
    public List<GameObject> obstaclePrefabs;

    [Header("Daftar Jalur (Titik Awal Spawn)")]
    // Pastikan Anchor ini posisinya ada di Z JAUH (misal Z = 100) dan X = 0 (Tengah)
    // Atau X sesuai jalur jika ingin jalur sejajar.
    public List<Transform> laneAnchors;

    [Header("Pengaturan Target Jalur (X Akhir)")]
    // Ini menggantikan if-else hardcoded kamu. Isi di Inspector!
    // Element 0 = Jalur Kiri, Element 1 = Tengah, Element 2 = Kanan
    public List<Vector2> laneTargetRanges; 
    // Contoh isi di Inspector untuk 3 jalur:
    // Element 0 (Kiri): X min -54, X max -25
    // Element 1 (Tengah): X min -22, X max 20
    // Element 2 (Kanan): X min 22, X max 53

    [Header("Pengaturan Spawn")]
    public float spawnInterval = 1.5f;
    private float timer;

    [Header("Anti-Overlap")]
    public float jarakMinimal = 5.0f;
    public int maksimalPercobaan = 3;

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
        if (obstaclePrefabs.Count == 0 || laneAnchors.Count == 0) return;

        bool berhasilSpawn = false;
        int percobaan = 0;

        while (!berhasilSpawn && percobaan < maksimalPercobaan)
        {
            int randomLaneIndex = Random.Range(0, laneAnchors.Count);
            Transform chosenAnchor = laneAnchors[randomLaneIndex];

            // Cek Overlap
            Collider[] hitColliders = Physics.OverlapSphere(chosenAnchor.position, jarakMinimal);

            if (hitColliders.Length == 0)
            {
                ExecuteSpawn(chosenAnchor, randomLaneIndex);
                berhasilSpawn = true;
            }
            
            percobaan++;
        }
    }

    void ExecuteSpawn(Transform chosenAnchor, int laneIndex)
    {
        int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
        GameObject chosenObstacle = obstaclePrefabs[randomObstacleIndex];

        // Instantiate di posisi Anchor (Posisi Awal)
        GameObject clone = Instantiate(chosenObstacle, chosenAnchor.position, Quaternion.identity);

        // Hitung Target X (Posisi Akhir di depan kamera)
        float targetX = 0;
        
        // Mengambil data range dari List laneTargetRanges agar lebih rapi
        if (laneIndex < laneTargetRanges.Count)
        {
            Vector2 range = laneTargetRanges[laneIndex];
            targetX = Random.Range(range.x, range.y);
        }

        // Setup Script Pergerakan
        ObjectMovement moveScript = clone.GetComponent<ObjectMovement>();
        if (moveScript != null)
        {
            // Kita kirim Posisi Awal (Anchor X) dan Posisi Akhir (Target X)
            moveScript.InitializeMovement(chosenAnchor.position.x, targetX);
        }

        Destroy(clone, 15f); // Hancurkan setelah lama
    }

    void OnDrawGizmosSelected()
    {
        if (laneAnchors == null) return;
        Gizmos.color = Color.yellow;
        foreach (var anchor in laneAnchors)
        {
            if (anchor != null) Gizmos.DrawWireSphere(anchor.position, jarakMinimal);
        }
    }
}