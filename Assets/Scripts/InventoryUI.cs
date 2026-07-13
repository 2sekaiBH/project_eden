using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] slotUI;

    [SerializeField] private Image itemDetailIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        inventoryPanel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
     {
        if (Keyboard.current.iKey.wasPressedThisFrame) //이번 프레임에서 i키를 한 번 눌렀는지 검사
            ToggleInventory();
    }

    //인벤토리 UI키고 끄는 것 관리
    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    //인벤토리 내 슬롯 갱신(아이템을 얻을 경우 불러와 슬롯을 새로고침함)
    public void Refresh()
    {
        for (int i=0; i< inventory.slots.Length; i++)
        {
            slotUI[i].SetItem(inventory.slots[i].item);
        }
    }

    //아이템을 설명창에 보여주도록 하는 함수
    public void ShowItem(ItemData item)
    {
        itemDetailIcon.sprite = item.ItemDetailIcon;
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
    }
}
