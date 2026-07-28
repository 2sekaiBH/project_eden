using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 카드의 UI를 관리하는 스크립트
/// </summary>
public class CardDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardData card = null;
    public CardData CardData => card;

    public int CardId => card.cardId;

    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Sprite[] iconSprite; // 0: Attack 1: Defense 2: Special 순서 맞춰서 - 자동화 필요..

    public event Action<CardDisplay> OnCardSelected; // HandManager, CardSelectOnPanelController에서 구독

    private bool isSelected = false;


    private void Awake()
    {
        if(image == null)
            image = GetComponent<Image>();
        if(canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        HandManager.OnCardSelect += UpdateAffordableVisual;
    }

    private void OnDisable()
    {
        HandManager.OnCardSelect += UpdateAffordableVisual;
    }

    /// <summary>
    /// 외부에서 카드를 바꿀 때는 반드시 이 함수를 통해서만.
    /// </summary>
    public void SetCard(CardData newCard)
    {
        card = newCard;
        UpdateCardDisplay();
    }
    
    /// <summary>
    /// 상태 초기화
    /// </summary>
    public void StateReset()
    {
        isSelected = false;
        SetSelectedVisual(false);
        SetActiveInput(false);
    }

    /// <summary>
    /// card data를 UI에 반영
    /// </summary>
    private void UpdateCardDisplay()
    {
        if (card == null) return;

        image.sprite = card.cardImage; //카드 이미지 설정

    }

    /// <summary>
    /// CardData 부재 시 오브젝트 비활성화
    /// </summary>
    public void UpdateVisibleDisplay()
    {
        if(card == null)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// 카드 제거를 UI에 반영
    /// </summary>
    public void UpdateActiveCard(bool active)
    {
        gameObject.SetActive(active);
    }

    /// <summary>
    /// 플레이어의 인풋을 받을지 여부를 제어
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveInput(bool active)
    {
        canvasGroup.blocksRaycasts = active;
    }

    /// <summary>
    /// 카드 선택 - 선택하자마자 energy cost 반영
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardSelected?.Invoke(this);
    }

    /// <summary>
    /// 선택된 카드의 UI 변경
    /// </summary>
    /// <param name="selected">선택 여부</param>
    public void SetSelectedVisual(bool selected)
    {
        isSelected = selected;
        image.color = isSelected ? Color.coral : Color.white;
    }

    /// <summary>
    /// 선택 가능 여부 UI 반영
    /// </summary>
    /// <param name="currentEnergy">PlayerActor의 currentEnergy</param>
    private void UpdateAffordableVisual(int currentEnergy)
    {
        if (card == null) return;
        bool affordable = isSelected || currentEnergy >= card.energyCost;
        canvasGroup.alpha = affordable ? 1f : 0.5f;
    }

    private void Hover()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 호버 구현
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 호버 구현
    }
}
