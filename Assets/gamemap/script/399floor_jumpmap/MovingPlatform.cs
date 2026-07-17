using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("이동 범위 설정")]
    [SerializeField] private float minDistance = 1f; // 최소 이동 거리
    [SerializeField] private float maxDistance = 2f; // 최대 이동 거리

    [Header("속도 설정")]
    [SerializeField] private float minSpeed = 1.5f;   // 최소 속도
    [SerializeField] private float maxSpeed = 3.5f;   // 최대 속도

    private float randomOffset;
    private float moveDistance;
    private float moveSpeed;
    private Vector3 startPosition;

    void Start()
    {
        // 게임이 시작될 때의 최초 위치를 기억
        startPosition = transform.position;

        moveDistance = Random.Range(minDistance, maxDistance);
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        // 이 오프셋 덕분에 모든 발판이 동시에 위로 올라가지 않고 제각각 움직입니다.
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Time.time에 발판 고유의 randomOffset을 더해줍니다.
        float sinValue = Mathf.Sin((Time.time * moveSpeed) + randomOffset);

        float newY = startPosition.y + (sinValue * moveDistance);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 내 위에 부딪힌 오브젝트의 태그가 Player라면
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어를 내 자식으로 들여보냅니다. (발판과 함께 이동)
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어가 나가면 자식 관계를 끊어줍니다.
            collision.transform.SetParent(null);
        }
    }
}