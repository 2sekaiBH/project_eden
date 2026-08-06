using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 데미지 반사 모듈
/// </summary>
[CreateAssetMenu(fileName = "ReflectDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/ReflectDamage")]
public class ReflectDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        context.caster.EnableReflect(); //데미지 반사 상태를 킴
    }

}
