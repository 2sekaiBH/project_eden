using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class InventorySlotUI : MonoBehaviour
{

    [SerializeField] private Image itemIcon;
    [SerializeField] private InventoryUI InventoryUI;

    private ItemData currentItem; //ItemData를 불러옴

    //테스트용 아이템!!- 코드 꼭 삭제할 것
    [SerializeField] private ItemData testItem;

    private void Start()
    {
        SetItem(testItem);
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

        InventoryUI.ShowItem(currentItem);
    }


}
