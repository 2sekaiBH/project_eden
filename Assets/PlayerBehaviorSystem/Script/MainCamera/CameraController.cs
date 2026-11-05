using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private float smoothing = 0.2f;
    [SerializeField] private Transform minCameraBoundary;
    [SerializeField] private Transform maxCameraBoundary;

    [Header("Axis Settings")]
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private bool followPlayerY = true;

    private Camera cam;
    private float camHalfWidth;
    private float camHalfHeight;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = CalculateTargetPosition();
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    // 목표 위치 계산 로직을 함수로 분리
    private Vector3 CalculateTargetPosition()
    {
        float targetX = player.position.x;
        float targetY = followPlayerY ? (player.position.y + yOffset) : transform.position.y;

        Vector3 targetPos = new Vector3(targetX, targetY, -10f);

        if (minCameraBoundary != null && maxCameraBoundary != null)
        {
            float minX = minCameraBoundary.position.x + camHalfWidth;
            float maxX = maxCameraBoundary.position.x - camHalfWidth;
            float minY = minCameraBoundary.position.y + camHalfHeight;
            float maxY = maxCameraBoundary.position.y - camHalfHeight;

            if (minX <= maxX) targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            if (minY <= maxY) targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);
        }

        return targetPos;
    }
}
