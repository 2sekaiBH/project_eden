using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Reference")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    

    [Header("Setting")]
    // 점프맵에서만 점프 및 슬라이드 가능
    [SerializeField] private bool isJumpAndSlideAble = false;

    private float _moveInput;
    // 이동 방향 프로퍼티
    public float moveInput => _moveInput;

    private bool isGrounded;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // horizontal movement
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<float>();

        // flip 기능 추가 - animation, sprite 연동
    }

    // jump -> jump map에서만 action 할당
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log(isGrounded);
        if (context.performed && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }
    }

    public void FixedUpdate()
    {
        if(isJumpAndSlideAble)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        rb.linearVelocityX = moveInput * moveSpeed;
    }

    void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
    }
}
