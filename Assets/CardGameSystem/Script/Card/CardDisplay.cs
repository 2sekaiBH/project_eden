using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

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

    //카드 호버 시 크기 커지고 살짝 올라오게 구현 관려 변수
    [SerializeField] private float hoverScale = 0.7f; //호버 시 크기
    [SerializeField] private float hoverHeight = 10f; //호버 시 높이

    private Vector3 originalScale;
    private Vector2 originalPos;
    private int originalSiblingIndex;

    private RectTransform rt; //카드의 rectTransform을 들고옴

    //카드 뒤집기 효과를 위한 변수
    [SerializeField] private Sprite back;

    private bool isFront = false;
    private Vector3 baseScale;

    private bool isFlipping = false;


    private void Start()
    {
        ShowBack();

    }


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
        HandManager.OnCardSelect -= UpdateAffordableVisual;
    }

    /// <summary>
    /// 외부에서 카드를 바꿀 때는 반드시 이 함수를 통해서만.
    /// </summary>
    public void SetCard(CardData newCard, bool isUsingInPanel = false)
    {
        card = newCard;
        ShowBack();
        UpdateCardDisplay();

        if (isUsingInPanel) return;
        baseScale = transform.localScale;
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

         /*energyText.text = $"{card.energyCost.ToString()}";
         iconImage.sprite = iconSprite[(int)card.cardType];
         valueText.text = card.effect;
         descriptionText.text = card.description;*/
        image.sprite = card.cardImage; //카드 이미지 설정

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
        canvasGroup.alpha = active ? 1f : 0.5f;
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


    //기준 크기 설정
    public void SetBaseScale()
    {
        baseScale = transform.localScale;
    }

    //호버 구현
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isFlipping) return; 

        rt = GetComponent<RectTransform>();

        originalScale = rt.localScale;
        originalPos = rt.anchoredPosition;
        originalSiblingIndex = transform.GetSiblingIndex();

        transform.SetAsLastSibling();

        rt.localScale = originalScale * hoverScale;
        rt.anchoredPosition = originalPos + new Vector2(0, hoverHeight);
    }

    //마우스 포인터 뗌
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isFlipping) return;
        if (rt == null) return;

        rt.localScale = originalScale;
        rt.anchoredPosition = originalPos;

        transform.SetSiblingIndex(originalSiblingIndex);
    }

    //카드 뒷면 및 상태 저장
    public void ShowBack()
    {
        image.sprite = back;
        isFront = false;
    }

    //카드 앞면 및 상태 저장
    public void ShowFront()
    {
        image.sprite = card.cardImage;
        isFront = true;
    }

    //앞면으로 뒤집기
    public IEnumerator FlipToFront()
    {
        if (card == null || isFront) yield break;

        isFlipping = true;

        float duration = 0.15f;
        float time = 0;

        // 접기
        while (time < duration)
        {
            float t = time / duration;
            float scaleX = Mathf.Lerp(1, 0, t);
            transform.localScale = new Vector3(baseScale.x * scaleX, baseScale.y, baseScale.z);

            time += Time.deltaTime;
            yield return null;
        }

        // 무조건 앞면
        ShowFront();

        time = 0;

        // 펼치기
        while (time < duration)
        {
            float t = time / duration;
            float scaleX = Mathf.Lerp(0, 1, t);
            transform.localScale = new Vector3(baseScale.x * scaleX, baseScale.y, baseScale.z);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = baseScale;
        isFlipping = false;
    }

}
