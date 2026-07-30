using UnityEngine;
using UnityEngine.UI;

public class ItemAcquisitionUI : MonoBehaviour
{
    public static ItemAcquisitionUI Instance { get; private set; }

    [Header("UI 컴포넌트 연결")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image ItemDetailIcon;
    [SerializeField] private Text itemInfoText;

    private int currentAcquiredItemId;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (popupPanel != null) popupPanel.SetActive(false);
    }

    public void ShowAcquisitionPopup(int id)
    {
        if (ItemDatabase.Instance == null)
        {
            return;
        }

        ItemData targetData = ItemDatabase.Instance.GetItemByID(id);

        if (targetData == null)
        {
            return;
        }

        currentAcquiredItemId = id;

        itemInfoText.text = $"[{targetData.itemName}]";

        if (ItemDetailIcon != null)
        {
            if (targetData.ItemDetailIcon != null)
            {
                ItemDetailIcon.gameObject.SetActive(true);
                ItemDetailIcon.sprite = targetData.ItemDetailIcon;
                ItemDetailIcon.SetNativeSize();
            }
            else
            {
                ItemDetailIcon.gameObject.SetActive(false);
            }
        }

        if (popupPanel != null) popupPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickConfirmButton()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.AddItemByID(currentAcquiredItemId);
            Debug.Log("아이템이 인벤토리에 추가되었습니다.");
        }
        else
        {
            Debug.LogError("아이템을 찾을 수 없습니다");
        }

        if (popupPanel != null) popupPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public ItemData GetUIDataByID(int id)
    {
        if (ItemDatabase.Instance != null)
        {
            return ItemDatabase.Instance.GetItemByID(id);
        }
        return null;
    }

}