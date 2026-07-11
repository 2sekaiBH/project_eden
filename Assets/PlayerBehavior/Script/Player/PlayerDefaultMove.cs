using System;
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

    [HideInInspector] public bool isCanNotMoving = false;
    // 플레이어 이동 상태 프로퍼티 - 이동 가능 상태면 false, 가능이면 true
    // 외부 수정 가능.

    private Rigidbody2D rb;
    private SpriteRenderer spriteRender;
    private float lastInputDir = 0f;
    private bool leftHeld = false;
    private bool rightHeld = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
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
        if (isCanNotMoving) return;

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
