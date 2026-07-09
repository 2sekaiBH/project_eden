using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[9];


    //처음 인벤토리 슬롯 9개를 로딩함 -> 아이템을 담을 준비
    public void awake()
    {
        for (int i=0; i < slots.Length; i++)
        {   
            //만들어 뒀던 인벤토리 슬롯 객체를 붙여줌!
            slots[i] = new InventorySlot();
        }

    }


    //아이템이 인벤토리에 존재하는지 확인하는 함수
    public bool HasItem(ItemData item)
    {
        foreach(InventorySlot slot in slots)
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
