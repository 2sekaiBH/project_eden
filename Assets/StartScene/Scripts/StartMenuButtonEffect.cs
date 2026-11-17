using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuButtonEffect : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler,
    IPointerEnterHandler
{
    [Header("Button Visual")]
    [SerializeField] private Image buttonImage;

    [Range(0f, 1f)]
    [SerializeField] private float normalAlpha = 0.55f;

    [Range(0f, 1f)]
    [SerializeField] private float selectedAlpha = 1f;

    [SerializeField] private float selectedScale = 1.12f;

    [Header("Selection Cursor")]
    [SerializeField] private RectTransform selectionCursor;

    [SerializeField]
    private Vector2 cursorOffset = new Vector2(-35f, 0f);

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        if (buttonImage == null)
            buttonImage = GetComponent<Image>();

        SetSelectedVisual(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetSelectedVisual(true);
        MoveCursor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetSelectedVisual(false);
    }

    private void SetSelectedVisual(bool selected)
    {
        transform.localScale =
            selected
                ? originalScale * selectedScale
                : originalScale;

        if (buttonImage != null)
        {
            Color color = buttonImage.color;

            color.a =
                selected
                    ? selectedAlpha
                    : normalAlpha;

            buttonImage.color = color;
        }
    }

    private void MoveCursor()
    {
        if (selectionCursor == null)
            return;

        RectTransform buttonRect =
            GetComponent<RectTransform>();

        selectionCursor.SetParent(buttonRect, false);

        selectionCursor.anchorMin = new Vector2(0f, 0.5f);
        selectionCursor.anchorMax = new Vector2(0f, 0.5f);
        selectionCursor.pivot = new Vector2(0.5f, 0.5f);

        selectionCursor.anchoredPosition = cursorOffset;

        selectionCursor.gameObject.SetActive(true);
        selectionCursor.SetAsLastSibling();
    }
}