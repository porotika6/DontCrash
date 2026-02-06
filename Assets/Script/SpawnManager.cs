using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Daftar Objek (Klik + untuk tambah)")]
    public List<GameObject> daftarObjek; 

    [Header("Referensi")]
    public Highscore highscoreScript;
    public BoxCollider2D spawnArea;

    [Header("Pengaturan Kesulitan")]
    public float delayAwal = 2.0f;
    public float delayMinimal = 0.5f;
    public float faktorKesulitan = 0.05f;

    private float timerSpawn;

    void Start()
    {
        timerSpawn = delayAwal;
    }

    void Update()
    {
        timerSpawn -= Time.deltaTime;

        if (timerSpawn <= 0)
        {
            SpawnAcak();
            ResetTimer();
        }
    }

    void SpawnAcak()
    {
        if (daftarObjek.Count == 0) return;

        // 1. Pilih objek acak dari List (+)
        int indexAcak = Random.Range(0, daftarObjek.Count);
        GameObject objekTerpilih = daftarObjek[indexAcak];

        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        
         Instantiate(objekTerpilih, new Vector2(x, y), Quaternion.identity);
    
       
    }

    void ResetTimer()
    {
        // Semakin tinggi skor, semakin kecil delay-nya
        float skorSekarang = highscoreScript.timer;
        float delayBaru = delayAwal - (skorSekarang * faktorKesulitan);
        
        timerSpawn = Mathf.Clamp(delayBaru, delayMinimal, delayAwal);
    }

}