using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class KameraTeleport : MonoBehaviour
{
   [Header("Deteksi Obstacle")]
    public float detectionDistance = 10f; // Jarak obstacle terdeteksi
    public LayerMask obstacleLayer;       // Layer khusus Obstacle agar raycast akurat
    public string obstacleTag = "Obstacle"; // Tag obstacle (opsional, untuk double check)

    [Header("Slow Motion Settings")]
    public float slowMotionFactor = 0.1f; // Seberapa lambat waktu berjalan (0.1 = 10% speed)
    public GameObject teleportUIPanel;    // Panel UI yang berisi tombol Kiri/Kanan

    [Header("Teleport Settings")]
    public float teleportDistance = 5f;   // Seberapa jauh pindah ke samping
    public float transitionDuration = 0.3f; // Durasi animasi pindah (detik)

    private bool isSlowMotion = false;
    private bool isMoving = false;
    private float defaultFixedDeltaTime;

    [Header("Teleport Lane Settings")]
    public List<Transform> cameraLaneAnchors; // Masukkan 3 Transform (Kiri, Tengah, Kanan)
    public int currentLaneIndex = 1;

    void Start()
    {
        // Simpan default physics speed
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        
        // Pastikan UI mati saat mulai
        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(false);
    }

    void Update()
    {
        // Jangan deteksi jika sedang slow motion atau sedang bergerak
        if (!isSlowMotion && !isMoving)
        {
            DetectObstacle();
        }
    }

    // --- LOGIKA DETEKSI ---
    void DetectObstacle()
    {
       Vector3 origin = transform.position;
    Vector3 direction = transform.forward;

    RaycastHit hit;
    float sphereRadius = 3f; // Ukuran "bola" laser, perbesar jika masih tidak kena

    // Visualisasi laser di Scene View (Tekan Play untuk melihat)
    Debug.DrawRay(origin, direction * detectionDistance, Color.red);

    // Gunakan SphereCast agar area deteksi lebih gemuk/lebar
    if (Physics.SphereCast(origin, sphereRadius, direction, out hit, detectionDistance, obstacleLayer))
    {
        if (hit.collider.CompareTag(obstacleTag))
        {
            StartSlowMotion();
            Debug.Log("Obstacle Terdeteksi: " + hit.collider.name);
        }
    }
    }

    // --- LOGIKA SLOW MOTION ---
    void StartSlowMotion()
    {
        isSlowMotion = true;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale; // Agar physics tetap halus

        // Munculkan UI Tombol
        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(true);
    }

    public void StopSlowMotion()
    {
        isSlowMotion = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        // Sembunyikan UI Tombol
        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(false);
    }

    // --- FUNGSI TOMBOL UI ---
    // Pasang function ini di tombol UI "Kiri"
   public void OnClickTeleportLeft()
{
    if (isMoving || currentLaneIndex <= 0) return; // Jangan pindah jika sudah di paling kiri
    
    currentLaneIndex--; // Pindah index ke kiri
    StopSlowMotion();
    StartCoroutine(TransitionToAnchor(cameraLaneAnchors[currentLaneIndex].position));
}

public void OnClickTeleportRight()
{
    if (isMoving || currentLaneIndex >= cameraLaneAnchors.Count - 1) return; // Jangan jika sudah paling kanan
    
    currentLaneIndex++; // Pindah index ke kanan
    StopSlowMotion();
    StartCoroutine(TransitionToAnchor(cameraLaneAnchors[currentLaneIndex].position));
    }

    // --- LOGIKA TRANSISI GERAK (Smooth) ---
IEnumerator TransitionToAnchor(Vector3 targetPos)
{
    isMoving = true;
    Vector3 startPos = transform.position;
    
    // Kita hanya ingin pindah posisi X, posisi Y dan Z tetap ikuti kamera asal
    Vector3 finalTarget = new Vector3(targetPos.x, startPos.y, startPos.z);
    
    float elapsedTime = 0;
    while (elapsedTime < transitionDuration)
    {
        // Pindah secara halus
        transform.position = Vector3.Lerp(startPos, finalTarget, elapsedTime / transitionDuration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    transform.position = finalTarget;
    isMoving = false;
}
}
