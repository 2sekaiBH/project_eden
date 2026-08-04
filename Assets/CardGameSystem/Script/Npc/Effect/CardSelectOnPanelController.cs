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
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private PlayerActor playerActor;

    private List<GameObject> cards = new List<GameObject>(); // 카드 게임 오브젝트
    private List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay
    private List<(int, CardData)> selectedCard = new List<(int, CardData)>(); // <index, CardData>
    /// <summary>
    /// 선택한 card Index, CardData 딕셔너리
    /// </summary>
    public List<(int, CardData)> SelectedCard => selectedCard;

    private void OnEnable()
    {
        if(rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        Initialize(playerActor.Hand); // 카드 데이터로 초기화
    }
    private void OnDisable()
    {
        cardDisplays.Clear();
        cards.ForEach((cardObj) => Destroy(cardObj));
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    private void Initialize(List<CardData> cardDatas)
    {
        // 상태 변수 초기화
        selectedCard.Clear();

        for (int i = 0; i < cardDatas.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, rectTransform, false);
            cards.Add(cardObject);
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            cardDisplays.Add(cardDisplay);
            cardDisplay.OnCardSelected += HandleSelectCard;

            cardDisplay.SetCard(cardDatas[i]);
            cardDisplay.SetActiveInput(true);
        }
    }

    private void HandleSelectCard(CardDisplay selectDisplay)
    {
        int index = cardDisplays.FindIndex(display => display.Equals(selectDisplay));
        if (index > -1)
        {
            selectedCard.Add((index, selectDisplay.CardData));
        }
    }

    public IEnumerator CoRunSelect()
    {
        yield return new WaitUntil(() => ( selectedCard.Count > 0 ));
    }
}
