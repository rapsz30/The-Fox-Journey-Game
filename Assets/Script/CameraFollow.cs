using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float minX;

    void Start()
    {
        // Simpan posisi awal kamera
        minX = transform.position.x;
    }

    void LateUpdate()
    {
        Vector3 currentPos = transform.position;

        // Target X dari player
        float targetX = target.position.x + offset.x;

        // Batasi supaya kamera tidak pernah mundur
        float lockedX = Mathf.Max(targetX, minX);

        // Update minX agar kamera hanya bisa maju
        minX = lockedX;

        // Buat posisi baru
        Vector3 desiredPosition = new Vector3(lockedX, currentPos.y, currentPos.z);

        Vector3 smoothedPosition = Vector3.Lerp(currentPos, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
