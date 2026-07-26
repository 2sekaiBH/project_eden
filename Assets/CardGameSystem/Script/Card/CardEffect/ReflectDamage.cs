using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 방어 모듈
/// </summary>
[CreateAssetMenu(fileName = "ReflectDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/ReflectDamage")]
public class ReflectDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        context.caster.EnableReflect();
    }
}
