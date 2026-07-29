using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : MonoBehaviour
{
    private InventoryUI inventoryUI;

    [SerializeField] private Image itemIcon;

    private ItemData currentItem; //ItemData를 불러옴

    //슬롯 프리펩이 알아서 버튼 컴포넌트를 찾게 해주는 함수
    private void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
    }

    //Slot에 아이템 이미지를 붙여줌
    public void SetItem(ItemData item)
    {

        currentItem = item;

        if(item == null)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = null;
                itemIcon.enabled = false;
            }
        }

        else
        {
            Debug.Log("데이터 장착 완료");
            if (itemIcon != null)
            { 
                itemIcon.sprite = item.itemIcon;
                itemIcon.enabled = true;
            }
        }
    }

    public void OnClick()
    {
        if (currentItem == null || inventoryUI == null)
            return;

        inventoryUI.ShowItem(currentItem);
    }
}
