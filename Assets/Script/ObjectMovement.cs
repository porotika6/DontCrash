using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
[Header("Titik Acuan Z")]
    public float zMulai = 4f;    // Titik terjauh (saat spawn)
    public float zSelesai = -2f; // Titik terdekat/melewati kamera

    [Header("Pengaturan Skala (X & Y)")]
    public float scaleAwal = 0.3f;  // Skala kecil saat jauh
    public float scaleTarget = 2.5f; // Skala besar saat dekat

    [Header("Kecepatan")]
    public float speed = 3f;

    // Variabel untuk mengunci nilai yang ingin tetap
    private float fixedX;
    private float fixedY;
    private float fixedScaleZ;

    void Start()
    {
        // Simpan posisi X dan Y awal agar tidak berubah oleh script ini
        fixedX = transform.position.x;
        fixedY = transform.position.y;
        
        // Simpan skala Z awal agar tetap
        fixedScaleZ = transform.localScale.z;

        // Set posisi Z awal dan skala awal
        transform.position = new Vector3(fixedX, fixedY, zMulai);
        transform.localScale = new Vector3(scaleAwal, scaleAwal, fixedScaleZ);
    }

    void Update()
    {
        // 1. Hanya ubah posisi Z (bergerak maju/mendekat)
        float newZ = transform.position.z - (speed * Time.deltaTime);
        transform.position = new Vector3(fixedX, fixedY, newZ);

        // 2. Hitung progress (0.0 sampai 1.0) berdasarkan posisi Z
        float t = Mathf.InverseLerp(zMulai, zSelesai, transform.position.z);

        // 3. Hanya ubah Skala X dan Y, Skala Z tetap sesuai awal
        float currentScale = Mathf.Lerp(scaleAwal, scaleTarget, t);
        transform.localScale = new Vector3(currentScale, currentScale, fixedScaleZ);

        // 4. Hancurkan jika sudah melewati batas Z selesai
        if (transform.position.z <= zSelesai)
        {
            Destroy(gameObject);
        }
    }
}
