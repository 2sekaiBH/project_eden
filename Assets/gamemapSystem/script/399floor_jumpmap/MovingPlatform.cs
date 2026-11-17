using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("이동 범위 설정")]
    [SerializeField] private float minDistance = 1f; // 최소 이동 거리
    [SerializeField] private float maxDistance = 2f; // 최대 이동 거리

    [Header("속도 설정")]
    [SerializeField] private float minSpeed = 1.5f;   // 최소 속도
    [SerializeField] private float maxSpeed = 3.5f;   // 최대 속도

    private Rigidbody rb;
    private float randomOffset;
    private float moveDistance;
    private float moveSpeed;
    private Vector3 startPosition;

    private Vector3 previousPosition;
    private Vector3 currentVelocity;
    void Start()
    {
        startPosition = transform.position;
        previousPosition = startPosition;

        moveDistance = Random.Range(minDistance, maxDistance);
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        //모든 발판이 동시에 위로 올라가지 않고 제각각 움직이도록 함
        randomOffset = Random.Range(0f, 100f);
    }

    void FixedUpdate()
    {
        previousPosition = transform.position;

        float sinValue = Mathf.Sin((Time.time * moveSpeed) + randomOffset);
        float newY = startPosition.y + (sinValue * moveDistance);
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // 프레임당 발판의 이동 속도(벡터) 계산
        currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        CharacterController playerController = collision.gameObject.GetComponent<CharacterController>();
        if (playerController != null)
        {
            // 발판이 움직이는 만큼 플레이어를 함께 밀어줍니다.
            playerController.Move(currentVelocity * Time.deltaTime);
        }
    }
}