using UnityEngine;

[CreateAssetMenu(fileName = "NoaEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/NoaEffect")]
public class NoaEffect : NpcEffect
{
    [Header("Setting")]
    [SerializeField] private int extraEnergy;

    public override void Apply(NpcContext context)
    {
        context.playerActor.RefundEnergy(extraEnergy); // currentEnergy 추가
        context.playerActor.SetMaxEnergy(extraEnergy); // maxEnergy 추가
        UIUpdator.Instance.SetText($"노아: <sprite=0> + {extraEnergy} 추가");
        Debug.Log($"노아 효과 적용 - 플레이어 에너지 {extraEnergy} 추가");

        void ResetNoaEffect(int _)
        {
            context.playerActor.RefundEnergy(-extraEnergy);
            context.playerActor.SetMaxEnergy(-extraEnergy);
            //UIUpdator.Instance.SetText($"노아 효과 해제");
            Debug.Log($"노아 효과 해제");
            TurnFlowManager.OnTurnEnd -= ResetNoaEffect;
        }

        TurnFlowManager.OnTurnEnd += ResetNoaEffect;
    }
}
