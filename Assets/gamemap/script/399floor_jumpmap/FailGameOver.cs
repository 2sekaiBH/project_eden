using UnityEngine;

public class FailGameOver : MonoBehaviour
{
    [Header("추락 한계 설정")]
    [SerializeField] private float fallLimitY = -6.0f;

    [Header("대상 플레이어")]
    [SerializeField] private Transform playerTransform;

    private bool isGameOverTriggered = false;

    private void Awake()
    {
        // 만약 인스펙터에서 플레이어를 연결하지 않았다면, 이 스크립트가 붙은 본인 오브젝트를 대상으로 잡음
        if (playerTransform == null)
        {
            playerTransform = transform;
        }
    }

    private void Start()
    {
        isGameOverTriggered = false;
    }

    private void Update()
    {
        // 이미 게임오버가 한 번 실행되었다면 더 이상 검사하지 않는다
        if (isGameOverTriggered) return;

        if (playerTransform != null)
        {
            // 플레이어의 현재 Y좌표가 설정한 한계치보다 낮아졌는지 실시간 검사
            if (playerTransform.position.y < fallLimitY)
            {
                TriggerFallGameOver();
            }
        }
    }

    private void TriggerFallGameOver()
    {
        isGameOverTriggered = true;

        Debug.Log($"[게임오버] 플레이어가 추락했습니다!");

        // 가시 트랩과 동일한 게임오버 매니저를 호출하여 일관되게 처리합니다.
        
    }

}
