using CardSystem.Effects;
using CardSystem.Runtime;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Scriptable Objects/CardSystem/CardEffectData/Defense")]
public class DefenseEffect : CardEffectData
{
    public IntRange amount = new IntRange(); // ¹æ¾î °ª
    public override void Execute(CardContext context) => Debug.Log($"{context.target}: {amount} block");
}
