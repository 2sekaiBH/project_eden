using UnityEngine;

[CreateAssetMenu(fileName = "NoaEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/NoaEffect")]
public class NoaEffect : NpcEffect
{
    [Header("Setting")]
    [SerializeField] private int extraEnergy;

    public override void Apply(NpcContext context)
    {
        context.playerActor.RefundEnergy(extraEnergy);
        UIUpdator.Instance.SetText($"노아 효과 적용 - 플레이어 에너지 {extraEnergy} 추가");
        Debug.Log($"노아 효과 적용 - 플레이어 에너지 {extraEnergy} 추가");
    }
}
