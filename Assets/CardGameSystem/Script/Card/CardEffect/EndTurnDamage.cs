using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 평타 데미지 추가 모듈
/// </summary>
[CreateAssetMenu(fileName = "EndTurnDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/EndTurnDamage")]
public class EndTurntDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        PendingEffectManager.Instance.AddEndturnDamage(3, 2, context.target);

    }

}