using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpWithSlide : MonoBehaviour
{
    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private float slideForce = 50f;
    [SerializeField] private float jumpCoolTime = 2f;
    [SerializeField] private float slideCoolTime = 2f;
    [SerializeField] private float slideDuration = 1f;

    [Header("Reference")]
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
    private Collider2D playerCollider;

    private bool isSliding = false;

    private bool isJumpEnable = true;
    private bool isSlideEnable = true;
    private float sliderTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        defaultMove = GetComponent<PlayerDefaultMove>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if(isGrounded && !wasGrounded) // 공중 -> 지상으로 바뀐 프레임
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
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isJumpEnable) return;

        if (context.performed && isGrounded)
        {
            isSlideEnable = false; // 점프 중 슬라이드 막기
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            onJump?.Invoke();
        }
    }

    // ------ 플레이어 슬라이드  ------ //
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (!isSlideEnable) return;

        float direction = defaultMove.LastInputDir;
        if (context.performed && isGrounded)
        {
            isJumpEnable = false; // 슬라이드 중 점프 막기
            isSlideEnable = false; // 중복 슬라이드 방지
            isSliding = true;
            sliderTimer = slideDuration;

            onSlide?.Invoke(); // 애니메이션 적용
            rb.AddForceX(slideForce * direction, ForceMode2D.Impulse); 
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
}
