using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Attack")]
public class DamageEffect : CardEffectData
{
    public IntRange amount = new IntRange(); // 데미지 값
    public override void Execute(CardContext context) => Debug.Log($"{context.target}: {amount} damage");
}
