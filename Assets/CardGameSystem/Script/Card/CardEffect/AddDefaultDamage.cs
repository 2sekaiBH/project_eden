using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 평타 데미지 추가 모듈
/// </summary>
[CreateAssetMenu(fileName = "AddDefaultDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/AddDefaultDamage")]
public class AddDefaultDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        PendingEffectManager.Instance.AddExtraAttack(2); //평타 공격 강화 설정

    }

}