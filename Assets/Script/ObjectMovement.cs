using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Header("Titik Acuan Z")]
    public float zMulai = 100f;   // Z tempat spawn (Jauh di sana)
    public float zSelesai = -10f; // Z tempat hilang (Di belakang kamera)

    [Header("Pengaturan Skala")]
    public float scaleAwal = 0.1f;   // Kecil banget pas jauh
    public float scaleTarget = 2.5f; // Besar pas nabrak kamera

    [Header("Kecepatan")]
    public float speed = 20f; // Kecepatan gerak di sumbu Z

    // Variabel posisi
    private float startX; // X saat spawn
    private float targetX; // X saat sampai di depan kamera
    private float fixedY;
    
    private bool isInitialized = false;

    // Fungsi ini dipanggil oleh SpawnManager
    public void InitializeMovement(float _startX, float _targetX)
    {
        startX = _startX;
        targetX = _targetX;
        fixedY = transform.position.y;
        
        // Pastikan posisi Z diset ke zMulai saat inisialisasi
        transform.position = new Vector3(startX, fixedY, zMulai);
        
        isInitialized = true;
    }

    void Update()
    {
        // Jangan jalan kalau belum di-setup oleh Spawner
        if (!isInitialized) return;

        // 1. Gerakkan Z mundur (Mendekati kamera)
        // Kita gerakkan posisi Z objek secara manual
        Vector3 currentPos = transform.position;
        currentPos.z -= speed * Time.deltaTime;
        
        // 2. Hitung "Progress" perjalanan (0.0 = di Start, 1.0 = di Selesai)
        // InverseLerp menghitung persentase posisi Z saat ini di antara zMulai dan zSelesai
        float progress = Mathf.InverseLerp(zMulai, zSelesai, currentPos.z);

        // 3. Interpolasi X (Agar geraknya miring mengikuti perspektif)
        // Saat progress 0 (jauh), X = startX. Saat progress 1 (dekat), X = targetX.
        currentPos.x = Mathf.Lerp(startX, targetX, progress);

        // Terapkan posisi baru
        transform.position = currentPos;

        // 4. Interpolasi Skala (Membesar saat mendekat)
        float currentScale = Mathf.Lerp(scaleAwal, scaleTarget, progress);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale); // Z scale juga ikut membesar agar proporsional

        // Opsional: Jika objek sudah lewat jauh di belakang kamera, hancurkan
        if (currentPos.z < zSelesai)
        {
            Destroy(gameObject);
        }
    }
}