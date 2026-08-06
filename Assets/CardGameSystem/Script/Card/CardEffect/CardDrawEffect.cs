using CardSystem.Runtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 덱에서 카드를 가져오는 effect 모듈
/// </summary>
[CreateAssetMenu(fileName = "CardDrawEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/CardDraw")]
public class CardDrawEffect : CardEffectData
{
    public int count; // 가져오는 카드 갯수
    public override void Execute(CardContext context)
    {
        Debug.Log($"뽑기전 카드 : {context.caster.hand.Count}");
        var card = DeckManager.Instance.DrawExtrCard(1)[0]; //덱매니저 함수에서 카드를 한 장 뽑아옴
        card.energyCost = 0;
        Debug.Log($"뽑은 카드 : {card}");


        PendingEffectManager.Instance.AddExtraCard(context.caster, card); //손패에 카드 추가


        Debug.Log($"{context.caster}: {count} get");
    }
}
