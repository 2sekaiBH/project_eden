using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referneces")]
    [SerializeField] private InteractionDetector detector;
    private IWorldInteractable currentInteraction = null;
    private int currentInteractionId = 0;

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
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    } 
}
