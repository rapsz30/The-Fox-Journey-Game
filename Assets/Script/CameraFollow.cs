using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset; 
    public Collider2D leftBoundary; 

    private float minX;
    private float startY; 

    void Start()
    {
        minX = transform.position.x;
        startY = transform.position.y; 
    }

    void LateUpdate()
    {
        float targetX = target.position.x + offset.x;
        float lockedX = Mathf.Max(targetX, minX);
        minX = lockedX;

        float fixedY = startY; 

        Vector3 desiredPosition = new Vector3(lockedX, fixedY, transform.position.z);
        
        transform.position = desiredPosition;

        if (leftBoundary != null)
        {
            float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;

            float wallX = minX - halfWidth; 

            leftBoundary.transform.position = new Vector3(
                wallX, 
                leftBoundary.transform.position.y, 
                leftBoundary.transform.position.z
            );
        }
    }
}