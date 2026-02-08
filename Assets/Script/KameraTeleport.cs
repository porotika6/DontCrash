using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraTeleportSystem : MonoBehaviour
{
    [Header("Deteksi Obstacle (SphereCast)")]
    public float detectionDistance = 15f; 
    public float sphereRadius = 2.0f;      // "Ketebalan" laser bola kamu
    public LayerMask obstacleLayer;       
    public string obstacleTag = "Obstacle"; 

    [Header("Slow Motion Settings")]
    [Range(0f, 1f)] 
    public float slowMotionFactor = 0.2f; // Pastikan di bawah 1 (0.2 = 20% kecepatan)
    public GameObject teleportUIPanel;    

    [Header("Teleport Lane Settings")]
    public List<Transform> cameraLaneAnchors; // Masukkan 3 Objek (Kiri, Tengah, Kanan)
    public int currentLaneIndex = 1;         // Index 1 adalah Tengah
    public float transitionDuration = 0.3f;

    private bool isSlowMotion = false;
    private bool isMoving = false;
    private float defaultFixedDeltaTime;

    void Start()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        
        // Atur posisi awal kamera ke anchor tengah saat start
        if (cameraLaneAnchors.Count > currentLaneIndex)
            transform.position = new Vector3(cameraLaneAnchors[currentLaneIndex].position.x, transform.position.y, transform.position.z);

        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(false);
    }

    void Update()
    {
        if (!isSlowMotion && !isMoving)
        {
            DetectObstacle();
        }
    }

    void DetectObstacle()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;
        RaycastHit hit;

        // Visualisasi Laser di Scene View (Biar kelihatan lasernya)
        Debug.DrawRay(origin, direction * detectionDistance, Color.red);

        // MENGGUNAKAN SPHERECAST agar tidak meleset
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, detectionDistance, obstacleLayer))
        {
            if (hit.collider.CompareTag(obstacleTag))
            {
                Debug.Log("Obstacle terdeteksi: " + hit.collider.name);
                StartSlowMotion();
            }
        }
    }

    void StartSlowMotion()
    {
        isSlowMotion = true;
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale; 

        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(true);
    }

    public void StopSlowMotion()
    {
        isSlowMotion = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;

        if (teleportUIPanel != null) 
            teleportUIPanel.SetActive(false);
    }

    // --- FUNGSI TOMBOL ---

    public void OnClickTeleportLeft()
    {
        if (isMoving || currentLaneIndex <= 0) return;
        
        currentLaneIndex--; 
        StopSlowMotion();
        StartCoroutine(TransitionToAnchor(cameraLaneAnchors[currentLaneIndex].position));
    }

    public void OnClickTeleportRight()
    {
        if (isMoving || currentLaneIndex >= cameraLaneAnchors.Count - 1) return;
        
        currentLaneIndex++; 
        StopSlowMotion();
        StartCoroutine(TransitionToAnchor(cameraLaneAnchors[currentLaneIndex].position));
    }

    IEnumerator TransitionToAnchor(Vector3 targetPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        
        // Kita hanya ambil X dari anchor, Y dan Z tetap posisi kamera sekarang
        Vector3 finalTarget = new Vector3(targetPos.x, startPos.y, startPos.z);
        
        float elapsedTime = 0;
        while (elapsedTime < transitionDuration)
        {
            // Pindah halus menggunakan Lerp
            // Gunakan Time.unscaledDeltaTime karena Time.timeScale mungkin sedang berubah
            transform.position = Vector3.Lerp(startPos, finalTarget, elapsedTime / transitionDuration);
            elapsedTime += Time.unscaledDeltaTime; 
            yield return null;
        }

        transform.position = finalTarget;
        isMoving = false;
    }

    // Gambar bola deteksi di editor agar kamu bisa lihat seberapa besar lasernya
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * detectionDistance, sphereRadius);
    }
}