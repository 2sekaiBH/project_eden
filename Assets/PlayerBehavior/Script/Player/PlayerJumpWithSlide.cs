using System;
using System.Collections;
using UnityEngine;

public class PlayerJumpWithSlide : MonoBehaviour
{
    [Header("[Settings]")]
    [Header("[Jump]")]
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float jumpCoolTime = 2f;
    [Header("[Slide]")]
    [SerializeField] private float slideForce = 50f;
    [SerializeField] private float slideCoolTime = 2f;
    [SerializeField] private float slideDuration = 1f;
    [SerializeField] private Vector2 slideColliderOffset = new Vector2(1, 0.7f);
    [SerializeField] private Vector2 slideColliderSize = new Vector2(4.3f, 1.4f);

    [Header("[Reference]")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    public static event Action onJump;
    public static event Action onLand;
    public static event Action onSlide;
    public static event Action onSlideEnd;
    

    private bool isGrounded;
    private bool wasGrounded; // 이전 프레임 접지 상태

    private Rigidbody2D rb;
    private PlayerDefaultMove defaultMove;
    private BoxCollider2D playerCollider;

    private bool isSliding = false;

    private bool isJumpEnable = true;
    private bool isSlideEnable = true;
    private float sliderTimer = 0f;

    private Vector2 baseColliderOffset = default;
    private Vector2 baseColliderSize = default;

    private KeyCode jumpKeyCode = KeyCode.W;
    private KeyCode slideKeyCode = KeyCode.S;

    //움직이는 발판인지 체크
    private bool isOnPlatform = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultMove = GetComponent<PlayerDefaultMove>();
        playerCollider = GetComponent<BoxCollider2D>();

        baseColliderOffset = playerCollider.offset;
        baseColliderSize = playerCollider.size;
    }

    private void OnDisable()
    {
        if (KeyManager.Instance != null)
            KeyManager.Instance.OnKeyChanged -= UpdateKeyCode;
    }

    private void Start()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다. - 기본 키로 설정: Jump: W, Slide: S");
            return;
        }
        KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(
    groundCheck.position,
    new Vector2(0.3f, 0.3f),
    0f,
    groundLayer
);

        if (isGrounded && !wasGrounded) // 공중 -> 지상으로 바뀐 프레임
        {
            onLand?.Invoke();
            isSlideEnable = true;
            StartCoroutine(CoRunJumpCoolTime()); // 점프 end 시 cool 타임 처리
        }

        wasGrounded = isGrounded;


        if (isSliding) // slide 종료
        {
            sliderTimer -= Time.fixedDeltaTime;
            if (sliderTimer <= 0f)
            {
                playerCollider.offset = baseColliderOffset;
                playerCollider.size = baseColliderSize;

                isSliding = false;
                isJumpEnable = true;
                onSlideEnd?.Invoke();
                StartCoroutine(CoRunSlideRunTime());
            }
        }
    }

    /// <summary>
    /// 외부에서 slide, jump 제어 시 사용
    /// </summary>
    public void BlockJumpAndSlide()
    {
        isJumpEnable = false;
        isSlideEnable = false;
    }

    public void AllowJumpSlide()
    {
        isJumpEnable = true;
        isSlideEnable = true;
    }

    // ------ 플레이어 점프  ------ //
    private void OnJump()
    {
        if (!isJumpEnable) return;

        if (isGrounded || isOnPlatform)
        {
            transform.SetParent(null); //점프 중 부모 해제
            isSlideEnable = false; // 점프 중 슬라이드 막기

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); //속도 초기화
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);

            onJump?.Invoke();
        }
    }

    //발판 내려가는 도중 점프뛰기
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    transform.SetParent(col.transform);
                    isOnPlatform = true; //바꿔줌
                    break;
                }
            }
        }
    }

    //움직이는 발판 위에서 내려갔을 때
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            isOnPlatform = false; //발판에서 내려갔으니 다시 꺼줌

        }
    }




    // ------ 플레이어 슬라이드  ------ //
    private void OnSlide()
    {
        if (!isSlideEnable) return;

        float direction = defaultMove.LastInputDir;
        if (isGrounded)
        {
            isJumpEnable = false; // 슬라이드 중 점프 막기
            isSlideEnable = false; // 중복 슬라이드 방지
            isSliding = true;
            sliderTimer = slideDuration;

            onSlide?.Invoke(); // 애니메이션 적용
            playerCollider.offset = slideColliderOffset;
            playerCollider.size = slideColliderSize;
            rb.AddForceX(slideForce * direction, ForceMode2D.Impulse); // 대시
        }
    }   

    private IEnumerator CoRunJumpCoolTime()
    {
        yield return new WaitForSeconds(jumpCoolTime);
        isJumpEnable = true;
    }

    private IEnumerator CoRunSlideRunTime()
    {
        yield return new WaitForSeconds(slideCoolTime);
        isSlideEnable = true;
    }

    // isGrounded checking gizmos
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKeyCode))
            OnJump();
        else if(Input.GetKeyDown(slideKeyCode))
            OnSlide();
    }

    private void UpdateKeyCode()
    {
        jumpKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.PlayerJump);
        slideKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.PlayerSlide);
    }
}
