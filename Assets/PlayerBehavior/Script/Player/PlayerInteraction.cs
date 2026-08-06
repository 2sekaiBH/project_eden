using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referneces")]
    [SerializeField] private InteractionDetector detector;
    private IWorldInteractable currentInteraction = null;
    private int currentInteractionId = 0;
    private KeyCode interactionKeyCode = KeyCode.F;

    void Awake()
    {
        if(detector != null)
        {
            detector = GetComponentInChildren<InteractionDetector>();
        }
    }

    // OnInteractablechanged 구독
    private void OnEnable()
    {
        detector.OnInteractableChanged += HandleInteractableChanged;
    }

    private void OnDisable()
    {
        detector.OnInteractableChanged -= HandleInteractableChanged;
        if (KeyManager.Instance != null)
            KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
    }

    private void Start()
    {
        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("KeyManager가 없습니다. - 기본 키로 설정: 상호작용 - F ");
            return;
        }
        KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
    }

    // 상호작용 중인지 검사
    private bool isInteracting(int interactionId) { return currentInteractionId == interactionId; }

    private void HandleInteractableChanged(IWorldInteractable target)
    {
        currentInteraction = target;
        if (currentInteraction == null)
            currentInteractionId = -1;
    }

    // ----- 플레이어 상호작용  ------ //
    private void OnInteraction()
    {
        // detect된 오브젝트 없을 때
        if (currentInteraction == null)
            return;
        // 상호작용 중복 방지
        if (isInteracting(currentInteraction.InteractionId))
            return;

        // interact() 실행
        if (currentInteraction.CanInteract)
        {
            currentInteractionId = currentInteraction.InteractionId;
            currentInteraction.Interact();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactionKeyCode))
            OnInteraction();
    }

    private void UpdateKeyCode()
    {
        interactionKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.Interaction);
    }
}
