using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public event Action<IWorldInteractable> OnInteractableChanged;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IWorldInteractable>(out var interactable))
        {
            OnInteractableChanged?.Invoke(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnInteractableChanged?.Invoke(null);
    }
}
