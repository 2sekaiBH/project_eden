using CardSystem.Effects;
using CardSystem.Runtime;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "RestoreHp", menuName = "Scriptable Objects/CardSystem/CardEffectData/RestoreHp")]
public class SystemRestore: CardEffectData
{
    public int AddHp = 5;

    //AddHp만큼 치유합니다.
    public override void Execute(CardContext context)
    {

        context.caster.Heal(AddHp);
        Debug.Log($"힐했음! {context.caster.CurrentHp}");

    }

}
