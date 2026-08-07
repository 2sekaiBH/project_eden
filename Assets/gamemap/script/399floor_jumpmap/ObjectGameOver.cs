using UnityEngine;

public class GameOver : MonoBehaviour
{
    [Header("리스폰 위치 설정")]
    [SerializeField] private readonly Vector3 spawnPosition = new Vector3(-42.0f, -3.8f, 0.0f);

    private static bool isGameOverTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOverTriggered) return;

        // 닿은 오브젝트의 태그가 "Player"인지 확인
        if (other.CompareTag("Player"))
        {
            isGameOverTriggered = true;

            Debug.Log("게임오버");

            ResetPlayerPosition(other.gameObject);

        }
    }
    private void ResetPlayerPosition(GameObject playerObj)
    {
        // 2D 게임용 Rigidbody2D 속도/관성 초기화 (튕겨나감 방지)
        if (playerObj.TryGetComponent<Rigidbody2D>(out var rb2d))
        {
            rb2d.linearVelocity = Vector2.zero;
            rb2d.angularVelocity = 0f;
            rb2d.position = spawnPosition;
        }

        // Transform 위치 이동
        playerObj.transform.position = spawnPosition;
    }
}
