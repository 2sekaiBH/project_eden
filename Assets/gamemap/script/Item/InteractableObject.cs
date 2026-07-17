using UnityEngine;
using UnityEngine.SceneManagement;

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
        // 이미 상호작용했다면 실행 방지 안전장치
        if (isInteracted) return;

        Debug.Log($"{gameObject.name} (ID: {InteractionId})과 상호작용을 실행합니다.");

        isInteracted = true;
        // 2. 상호작용이 끝났으므로 스프라이트는 기본 상태(sprite1)로 고정합니다.
        if (spriteRenderer != null && sprite1 != null)
        {
            spriteRenderer.sprite = sprite1;
        }

        // 3. [UI 연동] 화면에 획득 팝업 UI를 띄웁니다.
        if (ItemAcquisitionUI.Instance != null)
        {
            ItemAcquisitionUI.Instance.ShowAcquisitionPopup(interactionId);
        }

        // 4. [인벤토리 연동] 인벤토리 담당자의 스크립트로 ID를 토스합니다.
        /*
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItemByID(interactionId);
        }
        else
        {
            Debug.LogWarning($"[인벤토리] ID {interactionId} 전달됨 (테스트 중일 경우 Init 씬이 안 켜졌을 수 있음)");
        }
        */
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