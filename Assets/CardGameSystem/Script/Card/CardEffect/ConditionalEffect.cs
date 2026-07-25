using CardSystem.Effects;
using CardSystem.Runtime;
using System;
using UnityEngine;

/// <summary>
/// 공격 모듈 - 랜덤과 같이 구현하기 위해 IntRange 사용
/// </summary>
[CreateAssetMenu(fileName = "ConditionalEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Conditional")]
public class ConditinalEffect : CardEffectData
{
    public int threshold = 15;
    public int normalDamage = 3;
    public int BonusDamage = 8;


    public override void Execute(CardContext context)
    {
        int damage;

        if(context.target.CurrentHp <= threshold)
        {
            damage = BonusDamage;
            
 
        }

        else {
            damage = normalDamage;
        }
        
        context.target.TakeDamage(damage);
        Debug.Log($"공격했음 {damage}");

    }

}
