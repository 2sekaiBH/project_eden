using UnityEngine;

[CreateAssetMenu(fileName = "NoaEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/NoaEffect")]
public class NoaEffect : NpcEffect
{
    [Header("Setting")]
    [SerializeField] private int extraEnergy;

    public override void Apply(NpcContext context)
    {
        Debug.Log($"플레이어 에너지 {extraEnergy} 추가");
        context.playerActor.RefundEnergy(extraEnergy);
    }
}
