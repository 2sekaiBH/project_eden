using UnityEngine;

public class Floor218ExitTrigger : MonoBehaviour
{
    [SerializeField] private Floor218FlowController floorFlow;

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