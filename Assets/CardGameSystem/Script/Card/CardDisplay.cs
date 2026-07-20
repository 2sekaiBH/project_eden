using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [SerializeField] private Sprite[] iconSprite; // 0: Attack 1: Defense 2: Special 순서 맞춰서 - 자동화 필요..

    public event Action<CardData> OnCardSelected;

    private Image image;
    private CanvasGroup canvasGroup;

    private bool isSelected = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
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
        UpdateClickedUI(false);
        SetActiveInput(false);
    }

    /// <summary>
    /// card data를 UI에 반영
    /// </summary>
    private void UpdateCardDisplay()
    {
        if(card != null) // cardData 존재 시 반영
        {
            canvasGroup.alpha = 1f;
            energyText.text = $"{card.energyCost.ToString()}";
            iconImage.sprite = iconSprite[(int)card.cardType];
            // valueText.text
            descriptionText.text = card.description;
        }
        else // cardData 부재 시 카드 투명화
        {
            canvasGroup.alpha = 0;
        }
    }

    /// <summary>
    /// 카드 제거를 UI에 반영
    /// </summary>
    public void UpdateDiscardCard()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이어의 인풋을 받을지 여부를 제어
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveInput(bool active)
    {
        canvasGroup.blocksRaycasts = active;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCardSelected?.Invoke(card);
        if (!isSelected) // 카드 선택
        {
            isSelected = true;
        }
        else // 카드 선택 해제
        {
            isSelected = false;
        }
        UpdateClickedUI(isSelected);
    }

    private void UpdateClickedUI(bool isSelected)
    {
        if(isSelected) 
            image.color = Color.blue;
        else
            image.color = Color.white;
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
