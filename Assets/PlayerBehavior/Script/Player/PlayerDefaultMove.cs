using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultMove : MonoBehaviour
{
    [Header("Player Horizontal Movement Settings")]
    [SerializeField] private float baseMoveSpeed = 5f;
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
    public float LastInputDir => lastInputDir;
    private bool leftHeld = false;
    private bool rightHeld = false;
    private bool isDashHeld = false; // 대시 키가 물리적으로 눌려있는지

    private bool blockDash = false;
    private bool blockMoving = false;

    // 실제 이동에 사용할 속도는 항상 이 프로퍼티로 계산
    private float CurrentMoveSpeed => (isDashHeld && !blockDash) ? baseMoveSpeed * dashValue : baseMoveSpeed; // 실시간으로 달리기 상태 감지

    private KeyCode moveLeftKeyCode = KeyCode.A;
    private KeyCode moveRIghtKeyCode = KeyCode.D;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerJumpWithSlide.onJump += BlockDash;
        PlayerJumpWithSlide.onLand += AllowDash;
        PlayerJumpWithSlide.onSlide += BlockMove;
        PlayerJumpWithSlide.onSlideEnd += AllowMove;
    }


    private void OnDisable()
    {
        PlayerJumpWithSlide.onJump -= BlockDash;
        PlayerJumpWithSlide.onLand -= AllowDash;
        PlayerJumpWithSlide.onSlide -= BlockMove;
        PlayerJumpWithSlide.onSlideEnd -= AllowMove;
        if (KeyManager.Instance != null)
            KeyManager.Instance.OnKeyChanged -= UpdateKeyCode;
    }

    private void Start()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다. - 기본 키로 설정: LeftMove: A, RightMove: D");
            return;
        }
        KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
    }

    public void BlockDash()
    {
        blockDash = true;
        UpdateDashState();
    }

    public void AllowDash()
    {
        blockDash = false;
        UpdateDashState();
    }

    // 플레이어 이동 비활성화 - 외부 스크립트에서 플레이어 이동 제어할 때 사용.
    public void BlockMove()
    {
        blockMoving = true;
    }

    public void AllowMove()
    {
        blockMoving = false;
        isWalking = false;
    }


    // ------ 플레이어 좌측 이동  ------ //
    private void OnMoveLeft()
    {
        lastInputDir = -1f;
        leftHeld = true;
    }

    private void OnMoveLeftEnd()
    {
        leftHeld = false;
    }

    // ------ 플레이어 우측 이동  ------ //
    private void OnMoveRight()
    {

        lastInputDir = 1f;
        rightHeld = true;

    }

    private void OnMoveRightEnd()
    {
        rightHeld = false;
    }

    // ------ 플레이어 달리기  ------ //
    private void OnDash()
    {
        isDashHeld = true;

        UpdateDashState();
    }

    private void OnDashEnd()
    {
        isDashHeld = false;
        UpdateDashState();
    }

    // ------ 달리기 상태 갱신 ------ //
    private bool wasDashing = false;
    private void UpdateDashState()
    {
        bool isDashingNow = isDashHeld && !blockDash;
        if(isDashingNow != wasDashing) // 상태가 변할 떄 실행
        {
            wasDashing = isDashingNow;
            OnRun.Invoke(isDashingNow);
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

        rb.linearVelocityX = _moveInput * CurrentMoveSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(moveLeftKeyCode))
            OnMoveLeft();
        if (Input.GetKeyUp(moveLeftKeyCode))
            OnMoveLeftEnd();
        if (Input.GetKeyDown(moveRIghtKeyCode))
            OnMoveRight();
        if(Input.GetKeyUp(moveRIghtKeyCode))
            OnMoveRightEnd();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            OnDash();
        if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            OnDashEnd();
    }

    private void UpdateKeyCode()
    {
        moveLeftKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.PlayerLeft);
        moveRIghtKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.PlayerRight);
    }
}
