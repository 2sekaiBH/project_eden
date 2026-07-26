using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 방어 모듈
/// </summary>
[CreateAssetMenu(fileName = "ReduceCost", menuName = "Scriptable Objects/CardSystem/CardEffectData/ReduceCost")]
public class ReduceCost : CardEffectData
{
    public override void Execute(CardContext context)
    {
        PendingEffectManager.Instance.ReduceCost(context.caster);
    }
}

