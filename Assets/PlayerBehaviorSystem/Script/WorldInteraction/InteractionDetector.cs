using System;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public event Action<IWorldInteractable> OnInteractableChanged;

    private InteractableObject currentObject; // 스크립트 추가
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IWorldInteractable>(out var interactable))
        {
            OnInteractableChanged?.Invoke(interactable);

            //스크립트 내용 추가
            if (collision.TryGetComponent<InteractableObject>(out var obj))
            {
                currentObject = obj;
                currentObject.SetHighlight(true);
            } 
            //스크립트 내용 추가 끝
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnInteractableChanged?.Invoke(null);

        //스크립트 내용 추가
        if (currentObject != null)
        {
            currentObject.SetHighlight(false);
            currentObject = null;
        }//스크립트 내용 추가 끝
    }
}
