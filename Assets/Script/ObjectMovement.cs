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
    public float risingSpeed = 2f;
    private Vector3 targetDirection;

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
    public void SetTargetDirection(float targetX)
    {
        // Target posisi di depan (misal Z jauh di depan) dengan variasi X
        Vector3 targetPos = new Vector3(targetX, transform.position.y, transform.position.z - 50f);
        targetDirection = (targetPos - transform.position).normalized;
    }

    void Update()
    {
        transform.Translate(targetDirection * speed * Time.deltaTime * risingSpeed, Space.World);

        float progress = Mathf.InverseLerp(zMulai, zSelesai, transform.position.z);
        
        float currentScale = Mathf.Lerp(scaleAwal, scaleTarget, progress);
        transform.localScale = new Vector3(currentScale, currentScale, transform.localScale.z);
        // Opsional: Agar objek menghadap ke arah jalannya
        if (targetDirection != Vector3.zero)
        {
            transform.forward = targetDirection;
        }
    }
    
}
