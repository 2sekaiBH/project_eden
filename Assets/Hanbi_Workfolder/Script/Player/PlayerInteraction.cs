using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Referneces")]
    [SerializeField] private Transform detector;

    void Start()
    {
        
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 상호작용 로직 구현
        }
    }

    void Update()
    {
        
    }

}
