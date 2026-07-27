using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.Rendering.GPUSort;

public class CardSelectOnPanelController : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private List<GameObject> cards = new List<GameObject>(); // 카드 게임 오브젝트
    [SerializeField] private List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay
    [SerializeField] private HandManager playerHand;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject cardPrefab;

    private Dictionary<int, CardData> selectedCard = new Dictionary<int, CardData>(); // <index, CardData>
    /// <summary>
    /// 선택한 card Index, CardData 딕셔너리
    /// </summary>
    public Dictionary<int, CardData> SelectedCard => selectedCard;

    private void OnEnable()
    {
        if(rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        cardDisplays.ForEach((display) => display.OnCardSelected += HandleSelectCard);

        cards.ForEach((card) => card.SetActive(true)); // 모든 카드 오브젝트 활성화
        Initialize(playerHand.AffordableCards); // 카드 데이터로 초기화
        cardDisplays.ForEach((display) => display.UpdateVisibleDisplay()); // data 없는 오브젝트 비활성화
    }
    private void OnDisable()
    {
        Initialize(Enumerable.Repeat<CardData>(null, cardDisplays.Count).ToList()); // 모든 오브젝트에서 카드 데이터 제거
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    private void Initialize(List<CardData> cardDatas)
    {
        // 상태 변수 초기화
        selectedCard.Clear();

        if (cardDatas.Count > cardDisplays.Count)
        {
            for (int i = 0; i < cardDatas.Count - cardDisplays.Count; i++) // 5개보다 더 많은 손패 존재 시 카드 오브젝트 새로 생성
            {
                GameObject extraCard = Instantiate(cardPrefab, rectTransform, false);
                cards.Add(extraCard);
                CardDisplay extraDisplay = extraCard.GetComponent<CardDisplay>();
                cardDisplays.Add(extraDisplay);
                extraDisplay.OnCardSelected += HandleSelectCard;
            }
        }
        for (int i = 0; i < cardDatas.Count; i++)
        {
            cardDisplays[i].SetCard(cardDatas[i]);
        }
    }

    private void HandleSelectCard(CardDisplay selectDisplay)
    {
        int index = cardDisplays.FindIndex(display => display.Equals(selectDisplay));
        if (index > -1)
        {
            selectedCard.Add(index, selectDisplay.CardData);
        }
    }

    public IEnumerator CoRunSelect()
    {
        yield return new WaitUntil(() => ( selectedCard.Count > 0 ));
    }
}
