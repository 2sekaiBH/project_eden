using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 평타 데미지 추가 모듈
/// </summary>
[CreateAssetMenu(fileName = "AddDefaultDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/AddDefaultDamage")]
public class AddDefaultDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        if(context.caster is PlayerActor)
            PendingEffectManager.Instance.AddExtraAttack(2); //카드 시전자가 플레이어인 경우에만 평타 강화 실행

    }

}