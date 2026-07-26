using CardSystem.Runtime;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EveEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/EveEffect")]
public class EveEffect : NpcEffect
{
    private CardData selectCard;
    private GameObject gameObject;
    private CardSelectOnPanelController panelController;

    private IEnumerator ExecuteRoutine()
    {
        gameObject.SetActive(true); // 선택 패널 활성화
        if(!panelController) panelController = gameObject.GetComponentInChildren<CardSelectOnPanelController>(); 

        yield return panelController.CoRunSelect(); // 카드 선택 시작

        PendingEffectManager.Instance.SetRoundPendingEffect(panelController.SelectedCard); // pendingEffect에 해당 카드 저장
        gameObject.SetActive(false); // 선택 패널 비활성화
    }

    public override void Apply(NpcContext context)
    {
        // 코루틴 사용이므로 비워두기
    }

    public override IEnumerator ApplyRoutine(NpcContext context)
    {
        Debug.Log("이브 효과 적용");
        gameObject = context.gameObject;
        yield return ExecuteRoutine();
    }

}
