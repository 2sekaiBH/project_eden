using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInputItemGet : MonoBehaviour
{
    [Header("희귀 아이템 ID")]
    [SerializeField] private int itemId;

    private bool isPlayerInside = false;
    private bool isGranted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && !isGranted && Keyboard.current.wKey.wasPressedThisFrame)
        {
            GrantItem();
        }
    }

    private void GrantItem()
    {
        isGranted = true;

        if (ItemAcquisitionUI.Instance != null)
        {
            ItemAcquisitionUI.Instance.ShowAcquisitionPopup(itemId);
        }
    }
}
