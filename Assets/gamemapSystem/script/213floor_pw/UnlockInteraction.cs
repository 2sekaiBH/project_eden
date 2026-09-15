using UnityEngine;

public class UnlockInteraction : MonoBehaviour, IWorldInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("인터페이스 필수 설정")]
    [SerializeField] private int interactionId;
    [SerializeField] private InteractionType interactionType;
    public int InteractionId => interactionId;
    public InteractionType InteractionType => interactionType;

    public bool CanInteract => true;

    [Header("스프라이트 설정")]
    [Tooltip("평소 상태의 이미지")]
    [SerializeField] private Sprite sprite1;
    [Tooltip("플레이어가 접근했을 때의 이미지")]
    [SerializeField] private Sprite sprite2;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetHighlight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetHighlight(false);
        }
    }

    public void SetHighlight(bool isCurrentOverlapping)
    {
        if (spriteRenderer == null) return;

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
        Debug.Log("자물쇠 단말기와 상호작용");

        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }

        // 비밀번호 팝업 UI 켜기
        if (ButtonSequenceManager.Instance != null)
        {
            ButtonSequenceManager.Instance.OpenSequenceUI();
        }
        else
        {
            Debug.LogError("팝업창을 찾을 수 없습니다");
        }
    }

    // 필요 시 초기화용
    public void ResetToObject1()
    {
        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }
    }
}
