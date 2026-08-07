using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FinalChoice : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("Choice Info")]
    [SerializeField] private string characterId;

    [Header("Visual")]
    [SerializeField] private Image choiceImage;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private FC_Popup_Controller popup;

    // Noa의 히든 엔딩 조건 등에 따라 설정
    private bool isUnlocked = true;

    // 현재 이 카드가 클릭되어 선택 확정된 상태인지
    private bool isSelected = false;

    public string CharacterId => characterId;
    public bool IsUnlocked => isUnlocked;
    public bool IsSelected => isSelected;

    public void Initialize(FC_Popup_Controller owner, bool unlocked)
    {
        popup = owner;
        isUnlocked = unlocked;
        isSelected = false;

        RefreshVisual();
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        RefreshVisual();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 잠긴 Noa는 오버해도 활성화 이미지가 되지 않음
        if (!isUnlocked)
            return;

        // 이미 다른 카드가 선택된 상태면 Hover 효과를 주지 않음
        if (popup != null && popup.HasSelectedCard && !isSelected)
            return;

        choiceImage.sprite = activeSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isUnlocked)
            return;

        // 클릭하여 선택 확정된 카드는 활성화 이미지 유지
        if (isSelected)
            return;

        choiceImage.sprite = inactiveSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 잠긴 Noa는 클릭도 무시
        if (!isUnlocked)
            return;

        popup.SelectCard(this);
    }

    private void RefreshVisual()
    {
        // 잠긴 경우: 무조건 비활성화 이미지
        if (!isUnlocked)
        {
            choiceImage.sprite = inactiveSprite;
            return;
        }

        // 선택된 경우: 활성화 이미지 고정
        choiceImage.sprite = isSelected
            ? activeSprite
            : inactiveSprite;
    }
}
