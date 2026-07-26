using CardSystem.Runtime;
using System.Collections;
using UnityEngine;

/// <summary>
/// Npc의 효과를 정의하는 추상클래스
/// </summary>
public abstract class NpcEffect: ScriptableObject
{
    public abstract void Apply(NpcContext context);

    /// <summary>
    /// 코루틴 필요한 이펙트만 override
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual IEnumerator ApplyRoutine(NpcContext context)
    {
        Apply(context);
        yield break;
    }
}
