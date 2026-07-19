using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardData card;
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

    private bool isSelected = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        UpdateCardDisplay();
    }

    /// <summary>외부에서 카드를 바꿀 때는 반드시 이 함수를 통해서만.</summary>
    public void SetCard(CardData newCard)
    {
        card = newCard;
        UpdateCardDisplay();
    }

    public void StateReset()
    {
        isSelected = false;
        UpdateClickedUI(false);
        SetActiveInput(false);
    }

    private void UpdateCardDisplay()
    {
        energyText.text = $"{card.energyCost.ToString()}";
        iconImage.sprite = iconSprite[(int)card.cardType];
        // valueText.text
        descriptionText.text = card.description;
    }

    public void DiscardCard()
    {
        Debug.Log(CardId);
        gameObject.SetActive(false);
    }

    public void SetActiveInput(bool active)
    {
        image.raycastTarget = active;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 호버 구현
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 호버 구현
    }
}
