
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class CardSelectOnPanelController : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private PlayerActor playerActor;

    [Header("UI용 Setting")]
    [SerializeField] private float cardSpacing = 200f;      // 카드 간격 (기본값, 최대값으로 사용)
    [SerializeField] private float curveHeight = 13f;        // 카드 곡선 높이
    [SerializeField] private float cardScale = 0.6f;          // 카드 크기
    [SerializeField] private float maxHandWidth = 1200f;      // 손패가 차지할 수 있는 최대 폭

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

    private void Start()
    {
        float maxWidth = rectTransform.rect.width * 0.9f;
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

        UpdateHandUI();

        foreach(CardDisplay display in cardDisplays)
        {
            display.SetBaseScale();
            StartCoroutine(display.FlipToFront());
        }
    }

    public void UpdateHandUI()
    {
        int count = cardDisplays.Count;
        if (count == 0) return;

        // 카드 개수에 따라 간격 자동 조정
        // (count - 1) * spacing 이 maxHandWidth를 넘지 않도록 spacing을 줄임
        float spacing = cardSpacing;
        if (count > 1)
        {
            float requiredWidth = (count - 1) * cardSpacing;
            if (requiredWidth > maxHandWidth)
            {
                spacing = maxHandWidth / (count - 1);
            }
        }

        for (int i = 0; i < count; i++)
        {
            RectTransform rt = cardDisplays[i].GetComponent<RectTransform>();
            float offset = i - (count - 1) / 2f;
            float x = offset * spacing;
            float y = -offset * offset * curveHeight;

            rt.anchoredPosition = new Vector2(x, y);
            // 회전 제거 (필요하면 기본값으로 초기화)
            rt.localRotation = Quaternion.identity;

            // 카드 크기 설정 (인스펙터에서 조절 가능)
            rt.localScale = Vector3.one * cardScale;
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
