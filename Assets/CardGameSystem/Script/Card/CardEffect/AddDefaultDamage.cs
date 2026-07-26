using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 평타 공격 강화
/// </summary>
[CreateAssetMenu(fileName = "AddDefaultDamage", menuName = "Scriptable Objects/CardSystem/CardEffectData/AddDefaultDamage")]
public class AddDefaultDamage : CardEffectData
{
    public override void Execute(CardContext context)
    {
        //평타 데미지 추가
        PendingEffectManager.Instance.AddExtraAttack(2); //다음 턴 평타에 추가되어야 하므로 pending에 추가
    }
}
