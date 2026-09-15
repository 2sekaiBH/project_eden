using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [SerializeField] private InventoryUI inventoryUI;

    [SerializeField] private ItemDatabase itemDatabase;

    public InventorySlot[] slots = new InventorySlot[9];


    //처음 인벤토리 슬롯 9개를 로딩함 -> 아이템을 담을 준비
    public void Awake()
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
        if (itemDatabase != null) itemDatabase.Initialize();
        for (int i = 0; i < slots.Length; i++)
        {
            //만들어 뒀던 인벤토리 슬롯 객체를 붙여줌!
            slots[i] = new InventorySlot();
        }

    }

    // ItemAcquisitionUI에 저장해둔 데이터를 로드!
    public void AddItemByID(int id)
    {
        ItemData targetItem = ItemDatabase.Instance.GetItemByID(id);

        if (targetItem == null) return;

        AddItem(targetItem);

        // 무거운 Find 함수 없이 다이렉트로 리프레시 (연산 최적화)
        if (inventoryUI != null)
        {
            InventoryUI.Instance.Refresh();
        }
    }


    //아이템이 인벤토리에 존재하는지 확인하는 함수
    public bool HasItem(ItemData item)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                Debug.Log("아이템 있음");
                return true;
            }
        }

        return false;

    }

    //아이템을 인벤토리에 추가하는 함수
    public void AddItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                Debug.Log("아이템 추가됨");
                return;
            }
        }
    }
}
