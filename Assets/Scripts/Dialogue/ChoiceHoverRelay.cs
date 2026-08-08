using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceHoverRelay :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private IntroDialogueController controller;
    private int choiceIndex;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;        
    }


    public void Initialize(
        IntroDialogueController targetController,
        int index)
    {
        controller = targetController;
        choiceIndex = index;
    }

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        if (controller == null)
        {
            return;
        }

        controller.SetChoiceIndex(choiceIndex);
        transform.localScale = originalScale * 1.1f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 원상복구
        transform.localScale = originalScale;
    }

}