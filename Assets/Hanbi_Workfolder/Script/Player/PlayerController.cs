using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Horizontal Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashValue = 1.5f;
    private float _moveInput;
    public float moveInput => _moveInput;
    // 이동 방향 프로퍼티

    private Rigidbody2D rb;
    private float lastInputDir = 0f;
    private bool leftHeld = false;
    private bool rightHeld = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 좌측 이동
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastInputDir = -1f;
            leftHeld = true;
        }
        else if (context.canceled)
            leftHeld = false;
    }

    //우측 이동
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lastInputDir = 1f;
            rightHeld = true;
        }
        else if (context.canceled)
            rightHeld = false;
    }

    // 달리기
    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
            moveSpeed *= dashValue;
        else if (context.canceled)
            moveSpeed /= dashValue;
    }

    public void FixedUpdate()
    {
        if (leftHeld && !rightHeld) 
            _moveInput = -1f;
        else if (!leftHeld && rightHeld)
            _moveInput = 1f;
        else if(leftHeld && rightHeld) // 키 동시 입력 시 마지막 입력 방향 유지
            _moveInput = lastInputDir;
        else
            _moveInput = 0f;

        rb.linearVelocityX = _moveInput * moveSpeed;
    }
}
