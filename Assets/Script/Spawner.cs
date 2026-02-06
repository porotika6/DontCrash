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

    
    private float timerSpawn;
    
    [Header("Events")]
    public UnityEvent OnRandom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerSpawn = delayAwal;
    }

    // Update is called once per frame
    void Update()
    {
         timerSpawn -= Time.deltaTime;

        if (timerSpawn <= 0)
        {
                SpawnObjectRandom();
                ResetTimer();
        }
        OnRandom?.Invoke();

    }
    void SpawnObjectRandom()
    {
       Vector3 random = new Vector3();
       random.x = Random.Range(-10, 10f);
       random.y = transform.position.y;

        var randomIndex = Random.Range(0, item.Count-1);
        var randomItem = item[randomIndex];
        GameObject objekBaru = Instantiate(randomItem, random, Quaternion.identity);
        objekBaru.transform.SetParent(this.transform);
    }

     void ResetTimer()
    {
        // Semakin tinggi skor, semakin kecil delay-nya
        float skorSekarang = highscore.timer;
        float delayBaru = delayAwal - (skorSekarang * faktorKesulitan);
        
        timerSpawn = Mathf.Clamp(delayBaru, delayMinimal, delayAwal);
    }

}
