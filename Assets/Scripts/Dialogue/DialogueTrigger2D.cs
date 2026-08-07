using UnityEngine;

public class DialogueTrigger2D : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField]
    private IntroDialogueController dialogueController;

    [SerializeField]
    private string startNodeId;

    [Header("Trigger")]
    [SerializeField]
    private bool triggerOnlyOnce = true;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnlyOnce && hasTriggered)
        {
            return;
        }

        // 플레이어의 자식 Collider가 닿아도 루트의 Tag를 검사
        Transform playerRoot = other.transform.root;

        if (!playerRoot.CompareTag("Player"))
        {
            return;
        }

        if (dialogueController == null)
        {
            Debug.LogError(
                $"{name}: DialogueController가 연결되지 않았습니다."
            );
            return;
        }

        if (string.IsNullOrWhiteSpace(startNodeId))
        {
            Debug.LogError(
                $"{name}: 시작할 대사 ID가 비어 있습니다."
            );
            return;
        }

        hasTriggered = true;
        dialogueController.StartDialogue(startNodeId);
    }
}