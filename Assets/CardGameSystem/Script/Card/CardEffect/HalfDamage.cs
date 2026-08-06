using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 데미지 반사 모듈
/// </summary>
[CreateAssetMenu(fileName = "HalfDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/HaflDamage")]
public class HaflDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        context.caster.EnableHalfDamage(); //데미지 50%상태를 킴

    }

}