using CardSystem.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이브 효과 - 현재 손패에서 카드 1장 복사하여 다음 라운드 때 재사용
/// </summary>
[CreateAssetMenu(fileName = "EveEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/EveEffect")]
public class EveEffect : NpcEffect
{
    private CardSelectOnPanelController panelController;

    public override void Apply(NpcContext context)
    {
        // 코루틴 사용이므로 비워두기
    }

    public override IEnumerator ApplyRoutine(NpcContext context)
    {
        GameObject cardSelectUIPanel = context.cardSelectUIPanel;
        List<CardData> selectdCard = new List<CardData>();

        IEnumerator ExecuteRoutine()
        {
            cardSelectUIPanel.SetActive(true); // 선택 패널 활성화
            if (!panelController) panelController = cardSelectUIPanel.GetComponentInChildren<CardSelectOnPanelController>();

            yield return panelController.CoRunSelect(); // 카드 선택 시작

            selectdCard = panelController.SelectedCard.Select(cardData => cardData.Item2).ToList();

            PendingEffectManager.Instance.SetRoundPendingEffect(selectdCard); // pendingEffect에 해당 카드 저장
            cardSelectUIPanel.SetActive(false); // 선택 패널 비활성화
        }
        yield return ExecuteRoutine();

        UIUpdator.Instance.SetText($"이브: 선택한 {selectdCard[0].name}을(를) 다음 라운드 손패에 추가");
        Debug.Log($"이브 효과 적용 - 선택한 {selectdCard[0].name}을(를) 다음 라운드 손패에 추가합니다.");

    }

}
