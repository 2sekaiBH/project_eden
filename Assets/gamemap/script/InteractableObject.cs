using UnityEngine;

public class InteractableObject : MonoBehaviour, IWorldInteractable
{
    [Header("인터페이스 필수 설정")]
    [SerializeField] private int interactionId;
    [SerializeField] private InteractionType interactionType;
    public int InteractionId => interactionId;
    public InteractionType InteractionType => interactionType;

    public bool CanInteract => !isInteracted;

    [Header("스프라이트 설정")]
    [Tooltip("평소 상태의 이미지")]
    [SerializeField] private Sprite sprite1;
    [Tooltip("플레이어가 접근했을 때의 이미지")]
    [SerializeField] private Sprite sprite2;

    private SpriteRenderer spriteRenderer;
    private bool isInteracted = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    public void SetHighlight(bool isCurrentOverlapping)
    {
        if (spriteRenderer == null) return;
        if (isInteracted) return; // 이미 상호작용이 완료되었다면 이미지를 바꾸지 않음

        if (isCurrentOverlapping && sprite2 != null)
        {
            spriteRenderer.sprite = sprite2;
        }
        else if (!isCurrentOverlapping && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    public void Interact()
    {
        Debug.Log($"{gameObject.name} (ID: {InteractionId})과 상호작용을 실행합니다.");

        isInteracted = true;
        ResetToObject1();
    }

    public void ResetToObject1()
    {
        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
        isInteracted = false;
    }
}