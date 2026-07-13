using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpWithSlide : MonoBehaviour
{
    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce = 10f;

    [Header("Reference")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    public static event Action onJump;
    public static event Action onLand;

    private bool isGrounded;
    private bool wasGrounded; // 이전 프레임 접지 상태

    private bool isSliding;
    private Rigidbody2D rb;
    private PlayerDefaultMove defaultMove;
    private Collider2D playerCollider;

    private bool jumpSlideBlock = false;

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
        }

        wasGrounded = isGrounded;
    }

    // 플레이어 점프, 슬라이드 비활성화
    public void BlockJumpAndSlide()
    {
        jumpSlideBlock = true;
    }

    public void AllowJumpSlide()
    {
        jumpSlideBlock = false;
    }

    // ------ 플레이어 점프  ------ //
    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpSlideBlock) return;

        if (context.performed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
            onJump?.Invoke();
        }
    }

    // ------ 플레이어 슬라이드  ------ //
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (jumpSlideBlock) return;

        float direction = defaultMove.moveInput;
        if (context.performed && isGrounded && isSliding)
        {
            // playerCollider 조정 및 애니메이션 적용
        }
    }   

    // isGrounded checking gizmos
    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
