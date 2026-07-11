using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isMovingInput;
    private AnimState currentAnim = AnimState.idle;

    private enum AnimState
    {
        idle,
        walking,
        jumping,
        sliding,
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        PlayerDefaultMove.OnWalk += HandleWalk;
        PlayerDefaultMove.OnRun += HandleRun;
        PlayerJumpWithSlide.onLand += HandleLand;
        PlayerJumpWithSlide.onJump += HandleJump;
    }

    void OnDisable()
    {
        PlayerDefaultMove.OnWalk -= HandleWalk;
        PlayerDefaultMove.OnRun -= HandleRun;
        PlayerJumpWithSlide.onLand -= HandleLand;
        PlayerJumpWithSlide.onJump -= HandleJump;
    }

    // ------ 걷기 애니메이션  ------ //
    private void HandleWalk(bool isWalking)
    {
        isMovingInput = isWalking; // 점프 시 idle 상태인지, walk 상태인지 기록

        if (currentAnim == AnimState.jumping)
            return;

        currentAnim = isWalking ? AnimState.walking : AnimState.idle;
        animator.SetTrigger(isWalking ? "walkTrigger" : "idleTrigger");
    }

    // ------ 달리기 - speed 증감  ------ //
    private void HandleRun(bool isRunning)
    {
        if(isRunning)
            animator.speed += 0.2f;
        else
            animator.speed -= 0.2f;
    }

    // ------ 점프 애니메이션  ------ //
    private void HandleJump()
    {
        currentAnim = AnimState.jumping;
        animator.SetTrigger("jumpTrigger");
    }

    // ------ 착지 -> 점프 이전 state로 전환 ------ //
    private void HandleLand()
    {
        if (currentAnim != AnimState.jumping) // 점프 상태일 때만 검사
            return;
        // 착지 시점에 기억해둔 isMovingInput으로 복귀할 상태 결정
        currentAnim = isMovingInput ? AnimState.walking : AnimState.idle;
        animator.SetTrigger(isMovingInput ? "walkTrigger" : "idleTrigger");
    }
}
