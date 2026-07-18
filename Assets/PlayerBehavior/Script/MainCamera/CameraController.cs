using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("플레이어 연결")]
    [SerializeField] Transform player;
    [Tooltip("카메라 이동 속도")]
    [SerializeField] float smoothing = 0.2f;
    [Tooltip("카메라 경계 설정 - CameraBoundary object 연결")]
    [SerializeField] Transform minCameraBoundary;
    [SerializeField] Transform maxCameraBoundary;

    // 스크립트 추가
    [Header("Y Axis Settings")]
    [Tooltip("플레이어 중심에서 Y축으로 얼마나 떨어질지 설정 (예: 2를 넣으면 플레이어보다 살짝 위를 비춤)")]
    [SerializeField] float yOffset = 1.5f;
    [Tooltip("true면 플레이어를 따라가되 오프셋만 적용, false면 플레이어 무시하고 고정된 Y값 사용")]
    [SerializeField] bool followPlayerY = true;
    [Tooltip("followPlayerY가 false일 때 카메라가 고정될 절대적인 Y 좌표")]
    [SerializeField] float fixedYPosition = 0f;

    private Camera cam;
    private float camHalfWidth;
    private float camHalfHeight; // Y축 제한을 위해 높이 절반 값도 추가
    // 스크립트 추가 끝

    void Start()
    {
        cam = GetComponent<Camera>(); //스크립트 추가

        if (player == null)
        {
            Debug.LogError("CameraController에 player assign해주세요");
            player = GameObject.FindWithTag("Player").transform; // 스크립트 추가

            Debug.LogError("CameraController에 CameraBoundary assign해주세요.");
        }

        // 스ㅋ립트 추가
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
        // 스크립트 추가 끝
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.position.x, maxCameraBoundary.position.x);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);

        // 스크립트 추가
        if (player == null || minCameraBoundary == null || maxCameraBoundary == null) return;

        // 1. 기본 타겟 X 좌표 계산
        float targetX = player.position.x;

        // 2. 타겟 Y 좌표 계산 (설정에 따라 분기)
        float targetY = 0f;
        if (followPlayerY)
        {
            // 플레이어 중심이 아닌 플레이어 위치 + 오프셋 적용
            targetY = player.position.y + yOffset;
        }
        else
        {
            // 플레이어의 점프/낙하를 무시하고 지정된 특정 높이에 고정
            targetY = fixedYPosition;
        }

        // 3. Lerp를 이용해 부드러운 목적지 계산
        Vector3 currentPos = transform.position;
        Vector3 desiredPos = Vector3.Lerp(currentPos, new Vector3(targetX, targetY, currentPos.z), smoothing);

        // 4. X축 카메라 경계 제한 (기존 한비님 코드 로직 유지)
        float minX = minCameraBoundary.position.x + camHalfWidth;
        float maxX = maxCameraBoundary.position.x - camHalfWidth;

        if (minX > maxX)
        {
            desiredPos.x = (minCameraBoundary.position.x + maxCameraBoundary.position.x) / 2f;
        }
        else
        {
            desiredPos.x = Mathf.Clamp(desiredPos.x, minX, maxX);
        }

        // 5. Y축 카메라 경계 제한 (화면이 맵 밖으로 나가는 것 방지)
        float minY = minCameraBoundary.position.y + camHalfHeight;
        float maxY = maxCameraBoundary.position.y - camHalfHeight;

        if (minY > maxY)
        {
            desiredPos.y = (minCameraBoundary.position.y + maxCameraBoundary.position.y) / 2f;
        }
        else
        {
            desiredPos.y = Mathf.Clamp(desiredPos.y, minY, maxY);
        }

        // 6. 최종 위치 적용
        transform.position = desiredPos;
        // 스크립트 추가 끝
    }
}
