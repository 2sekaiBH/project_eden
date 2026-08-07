using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Player의 손패 관리 매니저
/// PlayerActor와 CardDisplay 사이의 인터페이스 역할
/// </summary>
public class HandManager : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private List<GameObject> cards = new List<GameObject>(); // 카드 오브젝트들
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private PlayerActor player;
    [SerializeField] private RectTransform rectTransform;

    public List<CardDisplay> cardDisplays = new List<CardDisplay>(); // 카드 오브젝트에 부착된 CardDisplay

    private List<CardData> selectedCards = new List<CardData>(); // 선택된 카드 리스트

    private bool selectEndFlag = false; // 선택 종료 플래그  

    //UI용 세팅
    [SerializeField] private float cardSpacing = 120f; //카드 간경
    [SerializeField] private float curveHeight = 15f; //카드 높이
    [SerializeField] private float rotateAngle = 8f; //카드 각도

    //카드 제출 시 손패를 살짝 아래로 내림 관련 변수
    [SerializeField] private float moveDistance = 80f; // 얼마나 내려갈지
    [SerializeField] private float moveDuration = 0.25f; // 속도

    private Vector2 originalPos; // 원래 위치 저장



    public void HandleSelectEndFlag(bool value) // 제출 버튼에서 구독
    {
        selectEndFlag = value;
    }

    public static event Action<List<CardData>> OnSelectEnd;
    // 플레이어 선택 최종 종료 이벤트
    // PlayerActor에서 구독

    public static Action<int> OnCardSelect;
    // CardDisplay에서 구독



    private void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();

        originalPos = rectTransform.anchoredPosition; // 원래 위치 저장
    }

    // 이벤트 구독
    private void OnEnable()
    {
        RoundFlowManager.OnRoundEnd += DiscardAllCard;
        player.OnPlayerDrawCard += Initialize;
        TurnFlowManager.OnTurnStart += HandleTurnStart; //손패가 올라오도록 하기 위해 턴이 시작했다는 이벤트를 구독
    }
    // 이벤트 해제
    private void OnDisable()
    {
        cardDisplays.ForEach((display) => display.OnCardSelected -= HandleSelectCard);
        RoundFlowManager.OnRoundEnd -= DiscardAllCard;
        player.OnPlayerDrawCard -= Initialize;
        TurnFlowManager.OnTurnStart -= HandleTurnStart;
    }

    /// <summary>
    /// 손패 업데이트
    /// </summary>
    public void Initialize(List<CardData> cardDatas)
    {
        Debug.Log($"Initialize 호출! {cardDatas.Count}장");

        for (int i = 0; i < cardDatas.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab, rectTransform, false);
            cards.Add(cardObject);
            CardDisplay cardDisplay = cardObject.GetComponent<CardDisplay>();
            cardDisplays.Add(cardDisplay);
            cardDisplay.OnCardSelected += HandleSelectCard;

            cardDisplay.SetCard(cardDatas[i]);
        }

        UpdateHandUI();
    }

    public void ReplaceHand(int index = 0, CardData cardData = null)
    {
        cardDisplays[index].SetCard(cardData);
    }

    /// <summary>
    /// 카드 선택 시작
    /// </summary>
    /// <param name="handDatas">손패 데이터</param>
    public void StartSelect(List<CardData> handDatas)
    {
        // 상태 변수 초기화
        selectedCards.Clear();
        selectEndFlag = false;

        StartCoroutine(CoRunSelect());
    }

    /// <summary>
    /// 메인 카드 선택 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoRunSelect()
    {
        // 카드 선택 시작
        cardDisplays.ForEach((display) => display.SetActiveInput(true)); // input 활성화

        Debug.Log("플레이어 카드 제출 기다리는 중");
        yield return new WaitUntil(() => selectEndFlag); // 제출 버튼 누를 때까지 기다리기

        // 플레이어 카드 제출 완료
        OnSelectEnd?.Invoke(selectedCards);
        selectedCards.ForEach((selectedCard) => DiscardCard(selectedCard)); // 카드 버리기

        yield return StartCoroutine(MoveDown()); //제출 시 손패를 아래로 내려가게끔


        cardDisplays.ForEach((display) => display.StateReset()); // 카드 UI 상태 초기화
        ResetState(); // 상태 변수 초기화
    }

    /// <summary>
    /// 카드 선택 이벤트 핸들러
    /// </summary>
    /// <param name="selectedCard"></param>
    private void HandleSelectCard(CardDisplay display)
    {
        CardData card = display.CardData;
        if (card == null) return;

        //조건부 카드 미션 실패 시 아예 카드 선택이 안 됨
        if (card.isMissionCard && !MissionManager.Instance.IsMissionComplete(player))
        {
            UIUpdator.Instance.SetText("미션을 성공해야 사용할 수 있습니다.");
            return;
        }

        bool alreadySelected = selectedCards.Contains(card);

        if (!alreadySelected) // 카드 선택
        {
            if (!player.TrySpendEnergy(card.energyCost)) // currentEnergy와 비교, 판정
                return; // 에너지 부족 -> 무시, UI도 이미 흐려져 있어서 시각적으로 인지 가능

            // 선택된 카드 리스트에 추가
            selectedCards.Add(card);
            display.SetSelectedVisual(true);
        }
        else // 이미 클릭된 카드 선택 - 선택 카드 해제
        {
            player.RefundEnergy(card.energyCost);
            selectedCards.Remove(card);
            display.SetSelectedVisual(false);
        }
        OnCardSelect?.Invoke(player.CurrentEnergy);
    }

    /// <summary>
    /// 카드 제거
    /// </summary>
    /// <param name="card">제거할 카드</param>
    private void DiscardCard(CardData card)
    {
        player.DiscardCard(card);

        GameObject discardedCard = cardDisplays.Find((display) => (display.card.Equals(card))).gameObject;
        cardDisplays.Remove(discardedCard.GetComponent<CardDisplay>());
        cards.Remove(discardedCard);
        Destroy(discardedCard);

        UpdateHandUI();
    }

    /// <summary>
    /// 상태 초기화
    /// </summary>
    private void ResetState()
    {
        selectedCards.Clear();
        selectEndFlag = false;
    }

    /// <summary>
    /// 라운드 종료 시 모든 카드 오브젝트 삭제
    /// </summary>
    private void DiscardAllCard(int _)
    {
        cards.ForEach(card => Destroy(card));
        cards.Clear();
        cardDisplays.Clear();
    }

    //손패 UI갱신
    public void UpdateHandUI()
    {
        int count = cardDisplays.Count;

        for (int i = 0; i < count; i++)
        {
            RectTransform rt = cardDisplays[i].GetComponent<RectTransform>();


            float offset = i - (count - 1) / 2f;

            float x = offset * cardSpacing;
            float y = -offset * offset * curveHeight;
            float angle = -offset * rotateAngle;

            rt.anchoredPosition = new Vector2(x, y);
            rt.localRotation = Quaternion.Euler(0, 0, angle);

            //카드 크기 설정
            rt.localScale = Vector3.one * 0.6f;
        }
    }

    //손패 UIf를 부드럽게 밑으로 내림
    private IEnumerator MoveDown()
    {
        Vector2 start = rectTransform.anchoredPosition;
        Vector2 end = originalPos - new Vector2(0, moveDistance);

        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        rectTransform.anchoredPosition = end;
    }

    private IEnumerator MoveUp()
    {
        Vector2 start = rectTransform.anchoredPosition;
        Vector2 end = originalPos;

        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        rectTransform.anchoredPosition = end;
    }

    private void HandleTurnStart(int turn)
    {
        StartCoroutine(MoveUp()); //손패 올라오도록!
    }

}
