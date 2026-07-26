using CardSystem.Runtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 덱에서 카드를 가져오는 effect 모듈
/// </summary>
[CreateAssetMenu(fileName = "CardDrawEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/CardDraw")]
public class CardDrawEffect : CardEffectData
{
    public int count = 1; // 가져오는 카드 갯수
    public override void Execute(CardContext context)
    {
       context.caster.DrawCards(count);

        Debug.Log($"{context.caster}: {count} 장을 드로우 했습니다");

    }
                 
}
