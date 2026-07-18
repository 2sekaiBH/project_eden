using CardSystem.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDrawEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/CardDraw")]
public class CardDrawEffect : CardEffectData
{
    public int count; // 가져오는 카드 갯수
    public override void Execute(CardContext context) => Debug.Log($"{context.caster}: {count} get");
}
