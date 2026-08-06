using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 다음 턴 카드 코스트 -1
/// </summary>
[CreateAssetMenu(fileName = "ReduceCost", menuName = "Scriptable Objects/CardSystem/CardEffectData/ReduceCost")]
public class ReduceCost : CardEffectData
{
    public override void Execute(CardContext context)
    {
        PendingEffectManager.Instance.ReduceCost(context.caster); //에너지 코스트 -1을 Pending에 저장

    }

}