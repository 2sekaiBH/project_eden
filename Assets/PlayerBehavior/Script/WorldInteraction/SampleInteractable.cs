using UnityEngine;

public class SampleInteractable : MonoBehaviour, IWorldInteractable
{
    // 
    // 1. IWorldInteractable 인터페이스 구현
    // 2. 아래 프로퍼티들 구현
    // 3. interact() 구현
    //

    public int InteractionId => 10001;

    public InteractionType InteractionType => InteractionType.PopUp;

    private bool _canInteract = true;
    public bool CanInteract => _canInteract;

    public void Interact()
    {
        Debug.Log("팝업등장");
        //_canInteract = false;
    }
}
