using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Header("Pengaturan Posisi")]
    public float yAwal = 4f;
    public float yTarget = 0f;
    public float kecepatan = 2f;

    [Header("Pengaturan Scale")]
    public float scaleAwal = 0.3f;
    public float scaleTarget = 1f;

    void Update()
    {
        // 1. Gerakkan posisi Y ke bawah
        float newY = Mathf.MoveTowards(transform.position.y, yTarget, kecepatan * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 2. Hitung progress (0 sampai 1) berdasarkan jarak yang sudah ditempuh
        // Kita hitung berapa persen perjalanan dari yAwal ke yTarget
        float totalJarak = yAwal - yTarget;
        float jarakSekarang = transform.position.y - yTarget;
        
        // InverseLerp akan menghasilkan angka 0 saat di Y=4, dan 1 saat di Y=0
        float t = Mathf.InverseLerp(yAwal, yTarget, transform.position.y);

        // 3. Terapkan Scale berdasarkan progress t
        float currentScale = Mathf.Lerp(scaleAwal, scaleTarget, t);
        transform.localScale = new Vector3(currentScale, currentScale, 1f);

        // 4. (Opsional) Hancurkan objek jika sudah sampai target agar tidak memenuhi memori
        if (transform.position.y <= yTarget)
        {
            // Tambahkan efek atau hancurkan
             //Destroy(gameObject); 
        }
    }
}