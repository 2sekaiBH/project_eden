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
    [SerializeField] private Text itemDescription;

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
        if (inventoryPanel != null) inventoryPanel.SetActive(false); //�ʱⰪ, �κ��丮 UI�� ���� ����

        //�ʱⰪ, ������ ����â�� ��� �͵��� ����
        itemDetailIcon.enabled = false;
        itemName.text = "";
        itemDescription.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        // I키: 인벤토리 열기/닫기
        if (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
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
        }
    }

    //�κ��丮 UIŰ�� ���� �� ����
    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        // �κ��丮�� ���� �� �ֽ� �����ͷ� ������ �� �������ݴϴ�.
        if (isActive)
        {
            Refresh();
        }
    }

    // �κ��丮 �� ���� ����
    public void Refresh()
    {
        // �̱������� �����ϴ� Inventory.Instance���� �����͸� �����ϰ� �����ɴϴ�.
        if (Inventory.Instance == null || slotUI == null) return;

        for (int i = 0; i < Inventory.Instance.slots.Length; i++)
        {
            if (i < slotUI.Length)
            {
                slotUI[i].SetItem(Inventory.Instance.slots[i].item);
            }
        }
    }

    //�������� ����â�� �����ֵ��� �ϴ� �Լ�
    public void ShowItem(ItemData item)
    {
        itemDetailIcon.sprite = item.ItemDetailIcon;
        itemDetailIcon.SetNativeSize();
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;

        itemDetailIcon.enabled = true;
    }

}
