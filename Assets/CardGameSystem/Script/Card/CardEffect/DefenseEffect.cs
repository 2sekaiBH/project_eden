using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

/// <summary>
/// 방어 모듈
/// </summary>
[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Defense")]
public class DefenseEffect : CardEffectData
{
    public int amount = 2; // 방어 값
    public override void Execute(CardContext context) { 

        context.caster.AddBlock(amount); //방어력 추가 함수 실행
        Debug.Log($"{context.caster}: {amount} block");

    }
}
