using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class InventorySlotUI : MonoBehaviour
{
    private InventoryUI inventoryUI;

    [SerializeField] private Image itemIcon;

    private ItemData currentItem; //ItemData를 불러옴

    //테스트용 아이템!!- 코드 꼭 삭제할 것
    [SerializeField] private ItemData testItem;

    private void Start()
    {
        SetItem(testItem);
    }

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
      
            itemIcon.enabled = false;
        }

        else
        {
            itemIcon.enabled = true;
            itemIcon.sprite = item.itemIcon;
        
        }


    }

    public void OnClick()
    {
        if (currentItem == null)
            return;

        inventoryUI.ShowItem(currentItem);
    }


}
