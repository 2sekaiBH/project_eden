using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultMove : MonoBehaviour
{
    [Header("Player Horizontal Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashValue = 1.5f;

    private float _moveInput;
    public float moveInput => _moveInput;
    // 이동 방향 프로퍼티

    public static event Action <bool>OnWalk;
    public static event Action <bool>OnRun;

    // 플레이어 이동 상태 프로퍼티 - 이동 가능 상태면 false, 가능이면 true
    // 외부 수정 가능.

    private Rigidbody2D rb;
    private SpriteRenderer spriteRender;
    private float lastInputDir = 0f;
    private bool leftHeld = false;
    private bool rightHeld = false;

    private bool blockDash = false;
    private bool blockMoving = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerJumpWithSlide.onJump += BlockDash;
        PlayerJumpWithSlide.onLand += AllowDash;
    }


    private void OnDisable()
    {
        PlayerJumpWithSlide.onJump -= BlockDash;
        PlayerJumpWithSlide.onLand -= AllowDash;
    }

    public void BlockDash()
    {
        blockDash = true;
    }

    public void AllowDash()
    {
        blockDash = false;
    }

    // 플레이어 이동 비활성화 - 외부 스크립트에서 플레이어 이동 제어할 때 사용.
    public void BlockMove()
    {
        blockMoving = true;
    }

    public void AllowMove()
    {
        blockMoving = false;
    }


    // ------ 플레이어 좌측 이동  ------ //
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastInputDir = -1f;
            leftHeld = true;
        }
        else if (context.canceled)
        {
            leftHeld = false;
        }
    }

    // ------ 플레이어 우측 이동  ------ //
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastInputDir = 1f;
            rightHeld = true;
        }
        else if (context.canceled)
        {
            rightHeld = false;
        }
    }

    // ------ 플레이어 달리기  ------ //
    public void OnDash(InputAction.CallbackContext context)
    {
        // 점프 중일 때 dash 막기
        if (blockDash == true) return;

        if (context.performed)
        {
            moveSpeed *= dashValue;
            OnRun?.Invoke(true);
        }
        else if (context.canceled)
        {
            moveSpeed /= dashValue;
            OnRun?.Invoke(false);
        }
    }

    private bool isWalking = false;
    public void FixedUpdate()
    {
        // 이동 막기
        if (blockMoving) return;

        bool wantsToMove = leftHeld || rightHeld;

        if (leftHeld && !rightHeld)
        {
            spriteRender.flipX = true;
            _moveInput = -1f;
        }
        else if (!leftHeld && rightHeld)
        {
            spriteRender.flipX = false;
            _moveInput = 1f;
        }
        else if (leftHeld && rightHeld) // 키 동시 입력 시 마지막 입력 방향 유지
        {
            _moveInput = lastInputDir;
            if (lastInputDir == -1)
                spriteRender.flipX = true;
            else if(lastInputDir == 1)
                spriteRender.flipX = false;
        }
        else
        {
            _moveInput = 0f;
        }

        // 상태가 바뀔 때만 이벤트 발생
        if(wantsToMove != isWalking)
        {
            isWalking = wantsToMove;
            OnWalk?.Invoke(isWalking);
        }

        rb.linearVelocityX = _moveInput * moveSpeed;
    }
}
