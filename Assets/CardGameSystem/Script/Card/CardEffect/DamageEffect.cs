using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Attack")]
public class DamageEffect : CardEffectData
{
    public IntRange amount = new IntRange(); // 데미지 값
    public override void Execute(CardContext context) 
    {
        context.target.TakeDamage(amount.GetValue());
        Debug.Log($"{context.caster}이 공격하여 {context.target}가 {amount.GetValue()} 데미지를 입었습니다");
    }
    
}
