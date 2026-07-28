using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 카인 효과 - 현재 손패에서 카드 1장 교체
/// </summary>
[CreateAssetMenu(fileName = "KainEffect", menuName = "Scriptable Objects/CardSystem/NpcEffect/KainEffect")]
public class KainEffect : NpcEffect
{
    private CardData selectCard;
    private GameObject gameObject;
    private CardSelectOnPanelController panelController;

    public override void Apply(NpcContext context)
    {
        // 코루틴 사용이므로 비워두기
    }

    public override IEnumerator ApplyRoutine(NpcContext context)
    {
        Debug.Log("카인 효과 적용");
        gameObject = context.cardSelectUIPanel;

        IEnumerator ExecuteRoutine()
        {
            gameObject.SetActive(true); // 선택 패널 활성화
            if (!panelController) panelController = gameObject.GetComponentInChildren<CardSelectOnPanelController>();

            yield return panelController.CoRunSelect(); // 카드 선택 시작

            Dictionary<int, CardData> selectedCards = panelController.SelectedCard;
            foreach (var (index,cardData) in selectedCards)
            {
                context.playerActor.ReplaceCard(index);
            }

            gameObject.SetActive(false); // 선택 패널 비활성화
        }
        yield return ExecuteRoutine();
    }

}
