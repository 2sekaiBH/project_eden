using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 방어 모듈
/// </summary>
[CreateAssetMenu(fileName = "ReduceDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/ReduceDamage")]
public class ReduceDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        context.caster.EnableHalfDamage();
    }
}

