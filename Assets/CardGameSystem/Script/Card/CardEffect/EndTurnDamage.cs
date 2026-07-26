
using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 방어 모듈
/// </summary>
[CreateAssetMenu(fileName = "EndTurnDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/EndTurnDamage")]
public class EndTurnDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        PendingEffectManager.Instance.AddEndturnDamage(3, 2, context.target);
    }
}

