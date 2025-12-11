using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    // HANYA ADA SATU DEKLARASI INI:
    public Vector3 offset; 
    
    // Variabel BARU yang saya minta untuk ditambahkan (untuk tembok):
    public Collider2D leftBoundary; 

    private float minX;
    private float startY; 

    void Start()
    {
        // Simpan posisi X awal kamera
        minX = transform.position.x;
        
        // SIMPAN POSISI Y AWAL
        startY = transform.position.y; 
    }

    void LateUpdate()
    {
        // --- LOGIKA HORIZONTAL (X): Maju Saja ---
        float targetX = target.position.x + offset.x;
        float lockedX = Mathf.Max(targetX, minX);
        minX = lockedX;
        
        // --- LOGIKA VERTIKAL (Y): Kunci Y ---
        float fixedY = startY; 

        // Terapkan posisi baru SECARA LANGSUNG
        Vector3 desiredPosition = new Vector3(lockedX, fixedY, transform.position.z);
        
        transform.position = desiredPosition;
        
        // --- Pindahkan Tembok Tak Terlihat ---
        if (leftBoundary != null)
        {
            // Menghitung setengah lebar tampilan kamera
            float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            
            // Posisi X tembok = Posisi Tengah Kamera (minX/lockedX) - Setengah Lebar - Setengah Lebar Tembok
            // Diasumsikan Tembok memiliki lebar 1 unit untuk perhitungan ini
            float wallX = minX - halfWidth; 
            
            // Atur posisi tembok
            leftBoundary.transform.position = new Vector3(
                wallX, 
                leftBoundary.transform.position.y, 
                leftBoundary.transform.position.z
            );
        }
    }
}