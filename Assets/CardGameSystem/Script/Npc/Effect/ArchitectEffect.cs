using System.ComponentModel;
using UnityEngine;

/// <summary>
/// 아키텍트 npc effect - 적 턴 스킵
/// </summary>
/// <param name="context"></param>
[CreateAssetMenu(fileName = "ArchitectEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/ArchitectEffect")]
public class ArchitectEffect : NpcEffect
{
    public override void Apply(NpcContext context)
    {
        var opponent = context.opponentActor;
        opponent.SetActiveOnCurrentTurn(false);

        void Handler(int _)
        {   opponent.SetActiveOnCurrentTurn(true);
            TurnFlowManager.OnTurnEnd -= Handler;
        }
        TurnFlowManager.OnTurnEnd += Handler;
        UIUpdator.Instance.SetText("아키텍트: 이번 턴 적 스킵");
        Debug.Log("카인 효과 적용 - 이번 턴 적 스킵");
    }
}
