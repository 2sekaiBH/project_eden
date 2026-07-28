using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 공격 모듈 - 랜덤과 같이 구현하기 위해 IntRange 사용
/// </summary>
[CreateAssetMenu(fileName = "AttackEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Attack")]
public class DamageEffect : CardEffectData
{
    public IntRange amount = new IntRange(); // 데미지 값
    public override void Execute(CardContext context) 
    {
        context.target.TakeDamage(amount.GetValue(), context.caster);
        Debug.Log($"{context.caster}이 공격하여 {context.target}가 {amount.GetValue()} 데미지를 입었습니다");
    }
    
}
