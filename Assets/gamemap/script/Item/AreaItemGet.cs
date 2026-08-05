using UnityEngine;

public class AreaItemGrant : MonoBehaviour
{
    [Header("아이템 ID")]
    [SerializeField] private int itemId;

    [Header("획득 여부")]
    [SerializeField] private bool isOneTimeOnly = true;

    private bool hasGranted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이미 얻었거나 플레이어가 아니라면 무시
        if (hasGranted && isOneTimeOnly) return;

        // 부딪힌 대상이 플레이어인지 확인 (Tag 설정 필요)
        if (other.CompareTag("Player"))
        {
            GrantItem();
        }
    }

    private void GrantItem()
    {
        hasGranted = true;
        isOneTimeOnly = true;

        Debug.Log($"[희귀 아이템] ID {itemId} 획득");

        if (ItemAcquisitionUI.Instance != null)
        {
            ItemAcquisitionUI.Instance.ShowAcquisitionPopup(itemId);
        }
    }
}
