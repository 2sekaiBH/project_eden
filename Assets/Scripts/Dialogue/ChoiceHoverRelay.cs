using UnityEngine;
using UnityEngine.EventSystems;

public class ChoiceHoverRelay :
    MonoBehaviour,
    IPointerEnterHandler
{
    private IntroDialogueController controller;
    private int choiceIndex;

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
    }
}