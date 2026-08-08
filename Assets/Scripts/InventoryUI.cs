using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] slotUI;
    public bool IsOpened => inventoryPanel != null && inventoryPanel.activeSelf;

    [SerializeField] private Image itemDetailIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Text itemDescription;

    private KeyCode inventoryKeyCode = KeyCode.I;


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
    }
    private void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        itemDetailIcon.enabled = false;
        itemName.text = "";
        itemDescription.text = "";

        if (KeyManager.Instance != null)
        {
            KeyManager.Instance.OnKeyChanged += UpdateKeyCode;
            UpdateKeyCode();
        }

    }
    private void UpdateKeyCode()
    {
        // KeyManager에서 설정된 Inventory 키를 받아옴
        inventoryKeyCode = KeyManager.Instance.GetKeyCode(KeyBindingName.Inventory);
    }

    // Update is called once per frame
    void Update()
    {
        // I키: 인벤토리 열기/닫기
        if (Keyboard.current != null && Input.GetKeyDown(inventoryKeyCode))
        {
            ToggleInventory();
        }
        // ESC키: 인벤토리 닫기
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (inventoryPanel != null && inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            }
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(ESfx.tallcase_C);
            }
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            Refresh();
        }
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(ESfx.tallcase_C);
        }
    }

    public void Refresh()
    {
        if (Inventory.Instance == null || slotUI == null) return;

        for (int i = 0; i < Inventory.Instance.slots.Length; i++)
        {
            if (i < slotUI.Length)
            {
                slotUI[i].SetItem(Inventory.Instance.slots[i].item);
            }
        }
    }

    public void ShowItem(ItemData item)
    {
        itemDetailIcon.sprite = item.ItemDetailIcon;
        itemDetailIcon.SetNativeSize();
        string nameToDisplay = item.itemName;
        string descriptionToDisplay = item.itemDescription;

        // 5번 아이템이고, 9번 아이템을 가지고 있다면 바뀐 설명 출력
        if (item.id == 5)
        {
            ItemData item9 = ItemDatabase.Instance != null ? ItemDatabase.Instance.GetItemByID(9) : null;

            bool hasItem9 = item9 != null && Inventory.Instance != null && Inventory.Instance.HasItem(item9);
            if (!string.IsNullOrWhiteSpace(item.changedName))
            {
                nameToDisplay = item.changedName;
            }

            if (hasItem9 && !string.IsNullOrWhiteSpace(item.changedDescription))
            {
                descriptionToDisplay = item.changedDescription;
            }
        }

        itemName.text = nameToDisplay;
        itemDescription.text = descriptionToDisplay;
        itemDetailIcon.enabled = true;
    }

    private void OnEnable()
    {
        OpenCloseSettingsWindow.OnEscPressed += HandleEscClose;
    }

    private void OnDisable()
    {
        OpenCloseSettingsWindow.OnEscPressed -= HandleEscClose;
    }

    // ESC가 눌렸을 때 실행될 함수
    private bool HandleEscClose()
    {
        // 인벤토리 패널이 실제로 켜져 있다면 닫고 true 반환
        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);

            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX(ESfx.tallcase_C);
            }

            return true; // "내가 ESC 입력을 처리했음!" 알림
        }

        return false; // 안 켜져 있으면 처리 안 함
    }
}
