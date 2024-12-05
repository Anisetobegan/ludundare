using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    [SerializeField] Transform playerTransform;
    float smoothTime = 3f;
    Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - playerTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.fixedDeltaTime);
    }
}
