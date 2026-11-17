using TMPro.Examples;
using UnityEngine;

public class FailGameOver : MonoBehaviour
{
    [Header("추락 한계 설정")]
    [SerializeField] private float fallLimitY = -6.0f;

    [Header("대상 플레이어")]
    [SerializeField] private Transform playerTransform;

    [Header("리스폰(초기) 위치")]
    [SerializeField] private Vector3 spawnPosition = new Vector3(-42.0f, -3.8f, 0.0f);

    [SerializeField] private CameraController cameraController;

    private bool isGameOverTriggered = false;

    private void Awake()
    {
        // 만약 인스펙터에서 플레이어를 연결하지 않았다면, 이 스크립트가 붙은 본인 오브젝트를 대상으로 잡음
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("[FailGameOver] 씬 내에 'Player' 태그를 가진 오브젝트가 없습니다! 플레이어 태그를 확인해주세요.");
            }
        }
    }

    private void Start()
    {
        isGameOverTriggered = false;
    }

    private void Update()
    {

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
        if (isGameOverTriggered) return;
        isGameOverTriggered = true;

        Debug.Log($"[게임오버] 플레이어가 추락했습니다!");

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(ESfx.error_glitch);
        }

        ResetPlayerPosition();
    }

    public void ResetPlayerPosition()
    {
        if (playerTransform == null) return;
        playerTransform.SetParent(null);
        // 1) CharacterController가 붙어있는 경우 (위치 강제 이동 시 끄고 켜야 함)
        if (playerTransform.TryGetComponent<CharacterController>(out var controller))
        {
            controller.enabled = false;
            playerTransform.position = spawnPosition;
            controller.enabled = true;
        }
        // 2) Rigidbody(3D)가 붙어있는 경우 (낙하 관성/속도 초기화)
        else if (playerTransform.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.position = spawnPosition;
            playerTransform.position = spawnPosition;
            rb.linearVelocity = Vector3.zero; // Unity 6 기준 (구버전은 rb.velocity = Vector3.zero;)
            rb.angularVelocity = Vector3.zero;

            // 만약 중력이 꺼져있거나 락이 걸렸다 풀린 경우를 대비해 확실히 켜줌
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        // 3) Rigidbody2D가 붙어있는 경우 (2D 게임용)
        else if (playerTransform.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            rb2d.position = spawnPosition;
            playerTransform.position = spawnPosition;
            rb2d.linearVelocity = Vector2.zero; // Unity 6 기준 (구버전은 rb2d.velocity = Vector2.zero;)
            rb2d.angularVelocity = 0f;
            rb2d.simulated = true;
        }
        // 4) 일반 Transform 이동
        else
        {
            playerTransform.position = spawnPosition;
        }
        isGameOverTriggered = false;
    }

}
