using UnityEngine;

public class Floor67ExitTrigger : MonoBehaviour
{
    [SerializeField] private Floor67FlowController floorFlow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform playerRoot = other.transform.root;

        if (!playerRoot.CompareTag("Player"))
            return;

        if (floorFlow != null)
        {
            floorFlow.GoToNextFloor();
        }
    }
}