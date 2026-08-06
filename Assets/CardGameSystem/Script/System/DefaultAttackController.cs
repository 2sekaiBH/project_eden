using System;
using UnityEngine;

public class DefaultAttackController : MonoBehaviour
{

    [Header("Setting")]
    [SerializeField] private int attackAmount;
    [SerializeField] private int blockAmount;

    [Header("Reference")]
    [SerializeField] private PlayerActor playerActor;
    [SerializeField] private OpponentActor opponentActor;

    private Action <int> onTurnStartHandler;
    /// <summary>
    /// 평타 업그레이드 카드 구현을 위한 필드 추가
    /// </summary>
    private int extraDamageAmount; 

    private void OnEnable()
    {
        onTurnStartHandler = (int _) => { DefaultAttack(_); DefaultAddBlock(_); };
        TurnFlowManager.OnTurnStart += onTurnStartHandler;
    }

    private void OnDisable()
    {
        TurnFlowManager.OnTurnStart -= onTurnStartHandler;
    }
    /// <summary>
    /// 평타 공격
    /// </summary>
    /// <param name="_"></param>
    private void DefaultAttack(int _)
    {
        extraDamageAmount = PendingEffectManager.Instance.ConsumeExtraAttack();

        if (extraDamageAmount > 0)
        {
            Debug.Log($"추가 평타 데미지 반영 : 공격 {attackAmount + extraDamageAmount}");
            opponentActor.TakeDamage(attackAmount + extraDamageAmount, null);
            ExtraAttackEnd();
            return;
        }
        Debug.Log($"평타 공격 {attackAmount}");
        opponentActor.TakeDamage(attackAmount, null);
    }

    /// <summary>
    /// 평타 방어
    /// </summary>
    /// <param name="_"></param>
    private void DefaultAddBlock(int _)
    {
        Debug.Log($"평타 방어 {blockAmount}");
        playerActor.AddBlock(blockAmount);
    }

    /// <summary>
    /// 평타 업그레이드 기능 구현
    /// </summary>
    /// <param name="extra">추가 데미지(2)</param>
    public void ExtraAttack(int extra)
    {
        Debug.Log($"추가 평타 데미지: {extra}");
        extraDamageAmount = extra;
    }

    /// <summary>
    /// 평타 업그레이드 기능 종료 - 초기화
    /// </summary>
    public void ExtraAttackEnd()
    {
        Debug.Log("추가 평타 종료");
        extraDamageAmount = 0;
    }

    void Start()
    {

    }
}
