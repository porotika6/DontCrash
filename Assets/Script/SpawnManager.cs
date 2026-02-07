using UnityEngine;
using System.Collections.Generic; // Supaya bisa pakai List
using UnityEngine.Events;
public class FlexibleSpawner : MonoBehaviour
{
    [Header("Daftar Objek & Jalur")]
    public List<GameObject> item; // List prefab (Cone, Box, dll)
    public List<Transform> laneAnchors; // List titik jalur (Empty GameObjects)

    [Header("Referensi")]
    public Highscore highscore;

    [Header("Pengaturan Kesulitan")]
    public float delayAwal = 2.0f;
    public float delayMinimal = 0.5f;
    public float faktorKesulitan = 0.05f;

    [Header("Anti-Overlap")]
    public float radiusProteksi = 2.0f; 
    public int maksimalPercobaan = 5;

    private float timerSpawn;
    public UnityEvent OnRandom;

    void Start() => timerSpawn = delayAwal;

    void Update()
    {
        timerSpawn -= Time.deltaTime;
        if (timerSpawn <= 0)
        {
            SpawnObjectDiJalur();
            ResetTimer();
        }
    }

    void SpawnObjectDiJalur()
    {
        // Validasi agar tidak error jika list kosong
        if (item.Count == 0 || laneAnchors.Count == 0) return;

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;
        bool posisiAman = false;
        int percobaan = 0;

        // Loop untuk mencari jalur yang sedang kosong (tidak overlap)
        while (!posisiAman && percobaan < maksimalPercobaan)
        {
            // PENGGANTI RANDOM RANGE: Mengambil dari list jalur
            int randomLaneIndex = Random.Range(0, laneAnchors.Count);
            Transform selectedAnchor = laneAnchors[randomLaneIndex];
            
            spawnPos = selectedAnchor.position;
            spawnRot = selectedAnchor.rotation;

            // Cek apakah ada objek lain di jalur tersebut
            Collider[] hitColliders = Physics.OverlapSphere(spawnPos, radiusProteksi);

            if (hitColliders.Length == 0)
            {
                posisiAman = true;
            }
            percobaan++;
        }

        // Hanya spawn jika ditemukan jalur yang aman
        if (posisiAman)
        {
            int randomIndex = Random.Range(0, item.Count);
            Instantiate(item[randomIndex], spawnPos, spawnRot, transform);
            
            // Invoke dipanggil hanya saat berhasil spawn
            OnRandom?.Invoke();
        }
    }

    void ResetTimer()
    {
        // Menghitung delay berdasarkan timer di script Highscore
        float delayBaru = delayAwal - (highscore.timer * faktorKesulitan);
        timerSpawn = Mathf.Clamp(delayBaru, delayMinimal, delayAwal);
    }

    // Visualisasi radius proteksi di Editor
    void OnDrawGizmosSelected()
    {
        if (laneAnchors == null) return;
        
        Gizmos.color = Color.red;
        foreach (Transform lane in laneAnchors)
        {
            if (lane != null) Gizmos.DrawWireSphere(lane.position, radiusProteksi);
        }
    }
}