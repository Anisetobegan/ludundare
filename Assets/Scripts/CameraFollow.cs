using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    [SerializeField] Transform playerTransform;
    float smoothTime = 0.25f;
    Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
