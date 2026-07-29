using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlotUI[] slotUI;

    [SerializeField] private Image itemDetailIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false); //초기값, 인벤토리 UI는 꺼져 있음

        //초기값, 아이템 설명창의 모든 것들을 꺼둠
        itemDetailIcon.enabled = false;
        itemName.text = "";
        itemDescription.text = "";

    }

    // Update is called once per frame
    void Update()
     {
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    //인벤토리 UI키고 끄는 것 관리
    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        // 인벤토리가 켜질 때 최신 데이터로 슬롯을 싹 갱신해줍니다.
        if (isActive)
        {
            Refresh();
        }
    }

    // 인벤토리 내 슬롯 갱신
    public void Refresh()
    {
        // 싱글톤으로 존재하는 Inventory.Instance에서 데이터를 안전하게 가져옵니다.
        if (Inventory.Instance == null || slotUI == null) return;

        for (int i = 0; i < Inventory.Instance.slots.Length; i++)
        {
            if (i < slotUI.Length)
            {
                slotUI[i].SetItem(Inventory.Instance.slots[i].item);
            }
        }
    }

    //아이템을 설명창에 보여주도록 하는 함수
    public void ShowItem(ItemData item)
    {
        itemDetailIcon.sprite = item.ItemDetailIcon;
        itemDetailIcon.SetNativeSize();
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;

        itemDetailIcon.enabled = true;
    }

}
