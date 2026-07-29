using CardSystem.Runtime;
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
        Debug.Log($"{context.caster}: {count} get");
    }
}
