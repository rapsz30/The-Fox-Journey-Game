using UnityEngine;

public class FollowCameraWall : MonoBehaviour
{
    public Transform cam;
    public float xOffset = -5f;

    void LateUpdate()
    {
        transform.position = new Vector3(cam.position.x + xOffset, transform.position.y, transform.position.z);
    }
}
