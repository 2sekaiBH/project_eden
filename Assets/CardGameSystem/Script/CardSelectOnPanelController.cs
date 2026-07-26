using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CardSelectOnPanelController : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay
    [SerializeField] private HandManager handManager;

    private CardData selectedCard = null;
    public CardData SelectedCard => selectedCard;

    private void OnEnable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected += HandleSelectCard);
        Initialize(handManager.ActiveCards);
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    private void Initialize(List<CardData> cardDatas)
    {
        // 상태 변수 초기화
        selectedCard = null;

        // 카드 초기화
        for (int i = 0; i < cardDatas.Count; i++)
        {
            cardDisplays[i].SetCard(cardDatas[i]);
        }
        
    }

    private void OnDisable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
    }

    private void HandleSelectCard(CardDisplay selectDisplay)
    {
        selectedCard = selectDisplay.CardData;
    }

    public IEnumerator CoRunSelect()
    {
        yield return new WaitUntil(() => ( selectedCard ));
    }
}
