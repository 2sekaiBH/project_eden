using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private CardData card;
    public CardData Card => card;

    [Header("Reference")]
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Sprite[] iconSprite; // 0: Attack 1: Defense 2: Special 순서 맞춰서
    

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

    private void UpdateCardDisplay()
    {
        energyText.text = $"<> {card.energyCost.ToString()}";
        iconImage.sprite = iconSprite[(int)card.cardType];
        // valueText.text
        descriptionText.text = card.description;
    }
}
