using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public List<GameObject> item;
    public Highscore highscore;

    [Header("Pengaturan Kesulitan")]
    public float delayAwal = 2.0f;
    public float delayMinimal = 0.5f;
    public float faktorKesulitan = 0.05f;

    [Header("Anti-Overlap")]
    public float radiusProteksi = 2.0f; // Jarak minimal antar cone
    public int maksimalPercobaan = 5;

    private float timerSpawn;
    public UnityEvent OnRandom;

    void Start() => timerSpawn = delayAwal;

    void Update()
    {
        timerSpawn -= Time.deltaTime;
        if (timerSpawn <= 0)
        {
            SpawnObjectRandom();
            ResetTimer();
            OnRandom?.Invoke();
        }
    }

    void SpawnObjectRandom()
    {
        if (item.Count == 0) return;

        Vector3 spawnPos = Vector3.zero;
        bool posisiAman = false;
        int percobaan = 0;

        // Loop untuk mencari posisi yang tidak overlap
        while (!posisiAman && percobaan < maksimalPercobaan)
        {
            float randomX = Random.Range(-15f, 15f);
            spawnPos = new Vector3(randomX, transform.position.y, 30f);

            // Cek apakah ada Collider dalam radius tertentu di posisi spawnPos
            // Menggunakan LayerMask jika ingin lebih spesifik (opsional)
            Collider[] hitColliders = Physics.OverlapSphere(spawnPos, radiusProteksi);

            if (hitColliders.Length == 0)
            {
                posisiAman = true;
            }
            percobaan++;
        }

        // Hanya spawn jika ditemukan posisi yang aman
        if (posisiAman)
        {
            int randomIndex = Random.Range(0, item.Count);
            Instantiate(item[randomIndex], spawnPos, Quaternion.identity, transform);
            OnRandom?.Invoke();
        }
    }

    void ResetTimer()
    {
        float delayBaru = delayAwal - (highscore.timer * faktorKesulitan);
        timerSpawn = Mathf.Clamp(delayBaru, delayMinimal, delayAwal);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(0, transform.position.y, 30f), radiusProteksi);
    }
}