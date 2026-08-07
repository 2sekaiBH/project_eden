using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger2D : MonoBehaviour
{
    [SerializeField] private IntroDialogueController dialogueController;
    [SerializeField] private string startNodeId;
    [SerializeField] private bool triggerOnlyOnce = true;

    [Header("Optional Event")]
    [SerializeField] private UnityEvent onTriggered;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnlyOnce && hasTriggered)
            return;

        Transform playerRoot = other.transform.root;

        if (!playerRoot.CompareTag("Player"))
            return;

        hasTriggered = true;

        onTriggered?.Invoke();

        if (dialogueController != null &&
            !string.IsNullOrWhiteSpace(startNodeId))
        {
            dialogueController.StartDialogue(startNodeId);
        }
    }
}